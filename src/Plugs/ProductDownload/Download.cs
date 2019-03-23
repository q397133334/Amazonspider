using Amazonspider.Core;
using Amazonspider.Core.EntityFramework;
using Amazonspider.Core.Player;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Amazonspider.ProductDownload
{
    public class Download : BasePlayer, IPlayer
    {


        private Product product = new Product();
        private static object proxyLock = new object();
        private static object insertData = new object();
        private static int errorCount = 0;
        private static object errorLock = new object();
        bool isRemove = false;
        HtmlDocument htmlDocument = null;
        protected override void ThreadRun()
        {
            ///设置超时时间
            Timeout = 1000 * 120;

            //ScrapingBrowser scrapingBrowser = null;
            product = Product.Get(TaskSchedule.PlayerAccountId);
            if (product == null)
            {
                Complete(null, null, null, this);
                return;
            }
            product.Status = 1;
            //初始化
            var uri = new Uri($"http://www.amazon.com/dp/{product.Asin}?language=en_US&th=1");
            //scrapingBrowser = new ScrapingBrowser();

            //设置代理信息
            var html = "";
            lock (proxyLock)
            {
                WriteLog("加载网页信息1");
                try
                {
                    HttpRequest httpRequest = new HttpRequest($"http://www.amazon.com/dp/{product.Asin}?language=en_US&th=1");
                    html = httpRequest.GetHtml();
                    //Host = httpRequest.Host;
                    htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(html);
                    System.Threading.Thread.Sleep(1000);
                }
                catch (Exception ex)
                {

                }
            }
            htmlDocument = new HtmlDocument();
            WriteLog("解析网页信息");
            htmlDocument.LoadHtml(html);
            //加载类型
            GetTypes();
            //加载商品名称
            GetTitle();
            //加载商品价格
            GetPrice();
            //加载商品详情
            GetDesc();
            //加载商品图片
            GetImages();
            //加载更多商品
            GetMoreProducts();

            if (product.Status == 0)
            {
                product.Status = -2;
                product.Desc = html;
                WriteLog($"商品{product.Asin}下载失败", ConsoleLogStatus.Error);

                //404
                if (html.IndexOf("cookies") >= 0)
                {
                    WriteLog($"失败中子数--{errorCount}");
                    errorCount++;

                }
                else
                {
                    Product.Remove(Convert.ToInt32(product.Id));
                    product.Id = -100;
                    isRemove = true;
                }

                if (errorCount >= 20)
                {
                    lock (errorLock)
                    {
                        try
                        {
                            WriteLog("断开宽带连接，准备重新拨号");
                            new Ras().Disconnect();
                        }
                        catch
                        {

                        }
                        finally
                        {
                            errorCount = 0;
                        }
                    }
                }

                if (isRemove)
                {
                    Complete?.Invoke(null, null, null, this);
                    return;
                }
                else
                {
                    TaskSchedule.RunDateTime = DateTime.Now.AddMinutes(2).GetTimestamp();
                    Complete?.Invoke(null, null, TaskSchedule, this);
                }
            }
            else
            {
                if (isRemove == false)
                {
                    WriteLog($"商品{product.Asin}下载成功", ConsoleLogStatus.Success);
                    //清楚任务
                    Complete?.Invoke(product, null, null, this);
                }
                else
                {
                    WriteLog($"商品{product.Asin}已被过滤", ConsoleLogStatus.Success);
                    Complete?.Invoke(null, null, null, this);
                }
            }
        }

        private void GetTypes()
        {
            List<string> xpaths = new List<string>()
            {
                "//*[@id='wayfinding-breadcrumbs_feature_div']/ul/li[not(@class='a-breadcrumb-divider')]/span/a"
            };
            foreach (var item in xpaths)
            {
                var types = htmlDocument.DocumentNode.SelectNodes(item);
                if (types != null && types.Count > 0)
                {
                    foreach (var t in types)
                    {
                        var tt = Setting.ProductTypes.Where(q => q != "").Where(q => t.InnerHtml.Trim() == q.Trim()).ToList();
                        if (tt != null)
                        {
                            isRemove = true;
                            Product.Remove(Convert.ToInt32(product.Id));
                            break;
                        }
                    }
                    if (isRemove)
                        break;
                }
                if (types == null || types.Count == 0)
                {
                    Product.Remove(Convert.ToInt32(product.Id));
                }
            }
        }

        protected override void ThreadException()
        {
            Product.Remove(Convert.ToInt32(product.Id));
            WriteLog($"商品{product.Asin}下载异常", ConsoleLogStatus.Error);
            Complete?.Invoke(null, null, null, this);
        }

        protected override void ThreadAbout()
        {
            base.ThreadAbout();
            TaskSchedule.RunDateTime = DateTime.Now.AddMinutes(1).GetTimestamp();
        }

        private void GetTitle()
        {
            WriteLog("解析商品标题");
            var title = htmlDocument.DocumentNode.CssSelect("#productTitle").FirstOrDefault();
            if (title != null)
            {
                product.Title = title.InnerText.Replace("\n", "").Trim();
                if (product.Title.Length > 60)
                {
                    product.Title = product.Title.Substring(0, 60);
                }
                WriteLog($"商品标题解析成功{product.Title}");
            }
            else
            {
                product.Status = 0;
                WriteLog($"商品标题解析失败");
            }
        }

        private void GetPrice()
        {
            WriteLog("获取商品价格");
            List<string> xpaths = new List<string>()
            {
                "//*[@id='priceblock_ourprice']",
                "//*[@id='priceblock_saleprice']",
                "//*[@id='mbc']/div[1]/div/h5[1]/span/span",
                "/html/body/div[2]/div[4]/div[7]/div[7]/div/div[2]/ul/li/span/span[1]/span/a/span[2]/span",
                "/html/body/div[2]/div[4]/div[7]/div[7]/div/div[2]/ul/li/span/span[1]/span/a/span[2]",
                "//*[@id='priceblock_dealprice']",
                "id('a-autoid-3-announce')/span[2]/span",
                "id('mediaTab_heading_0')/a/span/div[2]/span",
                "//*[@id='unqualified']/div[1]/span",
                "//*[@id='olp_feature_div']/div/span/a",
                "//*[@id='unqualified']/div[1]/span",
                "//*[@id='buyNewSection']/h5/div/div[2]/div/span[2]"
            };
            product.Price = -1;
            foreach (var path in xpaths)
            {
                var htmlNode = htmlDocument.DocumentNode.SelectSingleNode(path);

                if (htmlNode == null) continue;
                try
                {
                    if (htmlNode.InnerText.IndexOf('-') > -1)
                    {
                        string str = htmlNode.InnerText.Split('-')[0];
                        product.Price = Decimal.Parse(str.Trim().Replace("$", ""));// * Config.decExchangeRate;
                        break;
                    }
                    else if (htmlNode.InnerText.IndexOf("from") > -1)
                    {
                        string str = htmlNode.InnerText.Substring(htmlNode.InnerText.IndexOf('$') - 1, (htmlNode.InnerText.Length - htmlNode.InnerText.IndexOf('$')) + 1);
                        if (str.IndexOf("$") > -1)
                        {
                            str = str.Replace("shipping", "");
                            var prices = str.Split('+');
                            foreach (var price in prices)
                            {
                                try
                                {
                                    product.Price += decimal.Parse(price.Trim().Replace("$", ""));// * Config.decExchangeRate;
                                }
                                catch { }
                            }
                        }
                    }
                    else
                    {
                        product.Price = decimal.Parse(htmlNode.InnerText.Trim().Replace("$", ""));// * Config.decExchangeRate;
                        break;
                    }
                }
                catch
                {
                    try
                    {
                        product.Price = decimal.Parse(htmlNode.InnerText.Trim().Replace("$", ""));
                        break;
                    }
                    catch
                    {
                        try
                        {
                            product.Price = GetNumber(htmlNode.InnerText.Trim().Replace("$", ""));
                            break;
                        }
                        catch (Exception ex1)
                        {
                            product.Price = 0.01M;
                            //AmazonInternationalSpider.Helper.WriteLog("价格获取失败:" + this.url + "   " + ex1.Message);
                        }
                    }
                }
            }

            try
            {
                MSScriptControl.ScriptControl sc = new MSScriptControl.ScriptControlClass();
                sc.Language = "JavaScript";
                var weight = getProductWeight();
                product.Price = decimal.Parse(sc.Eval(Setting.PriceFunction + "Price(" + product.Price.ToString() + "," + weight.ToString() + ")").ToString());
            }
            catch (Exception ex)
            {
                WriteLog("商品价格计算出错" + ex.Message, ConsoleLogStatus.Exption);
            }

            if (product.Price == -1)
            {
                WriteLog($"商品价格{product.Asin}获取异常", ConsoleLogStatus.Exption);
            }
        }

        private decimal getProductWeight()
        {
            WriteLog("获取商品重量");
            decimal dec = 2;
            List<string> xpaths = new List<string>()
            {
                ".//table[@id='productDetailsTable']/tr/td/div/ul/li[2]",
                ".//*[@id='HLCXComparisonWidget_feature_div']/table/tr[8]/td[2]/span",
                "id('detail-bullets')/table/tr/td/div/ul/li[2]",
                "id('detail-bullets')/table/tr/td/div/ul/li[1]",
                "id('productDetails_detailBullets_sections1')/tr[3]"
            };
            try
            {
                foreach (var xpath in xpaths)
                {
                    HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode(xpath);
                    if (node != null && node.InnerText.ToLower().Contains("shipping weight"))
                    {
                        dec = GetNumber(node.InnerText);
                        if (node.InnerText.IndexOf("ounces") > -1 || node.InnerText.IndexOf("oz") > -1)
                        {
                            dec = dec / 16;
                            break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                WriteLog("重量获取失败:" + ex.Message, ConsoleLogStatus.Exption);
            }

            return dec;
        }


        private void GetDesc()
        {
            WriteLog("获取商品详情");
            product.Desc = "<h2>" + product.Title + "</h2>";
            //商品简单介绍
            List<string> xpaths = new List<string>()
            {
                //".//*[@id='feature-bullets']/ul",
                ".//*[@id='rich-product-description']",
            };
            foreach (var path in xpaths)
            {
                var htmlNode = htmlDocument.DocumentNode.SelectSingleNode(path);
                if (htmlNode != null)
                {
                    product.Desc = "<hr/>" + htmlNode.InnerText;
                    //Product.Desc = Regex.Replace(Product.Desc, @"<img.*?src=(['""]?)(?<url>[^'"" ]+)(?=\1)[^>]*>", delegate (Match m)
                    // {
                    //     return "";
                    // });
                    break;
                }
            }

            HtmlAgilityPack.HtmlDocument doc1 = new HtmlAgilityPack.HtmlDocument();
            string iframeHtml = System.Web.HttpUtility.UrlDecode(Search_string(htmlDocument.DocumentNode.InnerHtml, "var iframeContent = \"", "\";"));//查询脚本中存储的框架HTML代码，读取解码
            doc1.LoadHtml(iframeHtml);//载入html文档中
            HtmlNode desc2 = doc1.DocumentNode.SelectSingleNode(".//*[@id='productDescription']");
            if (desc2 == null)
            {
                desc2 = htmlDocument.DocumentNode.SelectSingleNode(".//*[@id='productDescription']");
            }

            if (desc2 != null)
            {
                product.Desc += "<hr/>" + desc2.InnerHtml;
            }

            //试用介绍 importantInformation
            HtmlNode desc3 = htmlDocument.DocumentNode.SelectSingleNode(".//*[@id='importantInformation']");
            if (desc3 != null)
            {
                product.Desc += "<hr/>" + desc3.InnerHtml;
            }

            product.Desc += "<hr/>" + Setting.ProductDesc;

            if (product.Desc == null || product.Desc == "")
            {
                WriteLog($"商品介绍{{{product.Asin}}}获取异常", ConsoleLogStatus.Exption);
            }
        }

        private void GetImages()
        {
            WriteLog("获取商品图片");
            List<string> xpaths = new List<string>()
            {
                ".//*[@id='altImages']/ul/li/span/span/span/span/img",
                ".//*[@id='altImages']/ul/li/span/span/span/span/span/img"
            };

            Regex RegexImage = new Regex(@"L\..*?.jpg");
            string imageSize = "L._SX450_SY450_CR,0,0,450,450_.jpg";

            foreach (string xpath in xpaths)
            {
                HtmlNodeCollection htmlNodes = htmlDocument.DocumentNode.SelectNodes(xpath);
                if (htmlNodes != null && htmlNodes.Count > 0)
                {
                    var count = 1;
                    lock (insertData)
                    {
                        foreach (var htmlNode in htmlNodes)
                        {
                            if (count > 2)
                                break;
                            string imageSrc = htmlNode.GetAttributeValue("src", "").Trim();
                            imageSrc = RegexImage.Replace(imageSrc, imageSize);

                            var pi = ProductImage.AddOrUpdate(new ProductImage()
                            {
                                Asin = product.Asin,
                                Number = count,
                                Status = 0,
                                Url = imageSrc
                            });

                            if (pi.Id > 0)
                            {
                                TaskSchedule.AddOrUpdate(new TaskSchedule()
                                {
                                    PlayerAccountId = pi.Id,
                                    RunDateTime = DateTime.Now.GetTimestamp(),
                                    PlayerType = "Amazonspider.ProductImageDownload.Download",
                                    PlayerStep = "1"
                                });
                            }
                            count++;
                        }
                    }
                    break;
                }
            }
        }

        private void GetMoreProducts()
        {
            WriteLog("获取更多商品");
            var descNodes = htmlDocument.GetElementbyId("sp_detail");  //GetHtmlNodesByCss("div[data-a-carousel-options*='ajax']");

            if (descNodes != null)
            {
                //foreach (var item in descNodes)
                //{
                string more = descNodes.GetAttributeValue("data-a-carousel-options", "").Replace("&quot;", "\"").Trim() + "\r\n";
                JObject jb = (JObject)JsonConvert.DeserializeObject(more);
                JArray jarr = null;
                if (more.IndexOf("initialSeenAsins") > -1)
                {
                    jarr = jb["initialSeenAsins"].ToObject<JArray>();
                }
                else if (more.IndexOf("id_list") > -1 && more.IndexOf("ajax") > -1)
                {
                    jarr = jb["ajax"]["id_list"].ToObject<JArray>();
                }

                if (jarr != null)
                {
                    lock (insertData)
                    {
                        for (int i = 0; i < jarr.Count; i++)
                        {
                            string id = jarr[i].Value<string>().Trim(':');
                            var p = Product.AddOrUpdate(new Product()
                            {
                                Id = -2,
                                Asin = id,
                                Price = 0,
                                Desc = "",
                                Title = "",
                                Time = DateTime.Now.GetTimestamp(),
                            });
                            if (p.Id > -1)
                            {
                                TaskSchedule.AddOrUpdate(new TaskSchedule()
                                {
                                    PlayerAccountId = p.Id,
                                    PlayerStep = "2",
                                    PlayerType = this.GetType().ToString(),
                                    RunDateTime = DateTime.Now.GetTimestamp()
                                });
                            }
                        }
                    }
                    //}
                }
            }
        }

        private HtmlNode GetHtmlNodeByCss(string cssSelect)
        {
            return htmlDocument.DocumentNode.CssSelect(cssSelect).FirstOrDefault();
        }

        private List<HtmlNode> GetHtmlNodesByCss(string cssSelect)
        {
            return htmlDocument.DocumentNode.CssSelect(cssSelect).ToList();
        }

        public decimal GetNumber(string str)
        {
            decimal result = 0;
            if (str != null && str != string.Empty)
            {
                // 正则表达式剔除非数字字符（不包含小数点.）
                str = Regex.Match(str, @"([1-9]\d*\.?\d*)|(0\.\d*[1-9])").ToString();
                // 如果是数字，则转换为decimal类型
                try
                {
                    result = decimal.Parse(str);
                }
                catch (Exception ex)
                {
                    WriteLog(ex.Message + ex.StackTrace);
                }
            }
            return result;
        }

        public string Search_string(string s, string s1, string s2)  //获取搜索到的数目  
        {
            int n1, n2;
            if (s == null)
                return "";
            if (s1 == null)
                return "";
            if (s2 == null)
                return "";

            n1 = s.IndexOf(s1, 0) + s1.Length;   //开始位置  
            if (s.Length < n1)
                return "";
            n2 = s.IndexOf(s2, n1);
            if (((n2 - n1) + n1) > s.Length)//结束位置
                return "";
            try
            {
                return s.Substring(n1, n2 - n1);   //取搜索的条数，用结束的位置-开始的位置,并返回  
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message + ex.StackTrace);
                return "";
            }
        }
    }
}
