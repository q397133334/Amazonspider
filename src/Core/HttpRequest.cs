using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Camouflage.Common;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Drawing;

namespace Amazonspider.Core
{
    /// <summary>
    /// Http请求类
    /// </summary>
    public class HttpRequest
    {
        #region 公共属性

        /// <summary>
        /// Cookice
        /// </summary>
        public string Cookice { get; set; }

        /// <summary>
        /// Cookies
        /// </summary>
        public CookieCollection Cookies { get; set; }

        /// <summary>
        /// Host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Http请求参考
        /// </summary>
        private HttpItem HttpItem { get; set; }

        /// <summary>
        /// Http 结果
        /// </summary>
        private HttpResult HttpResult { get; set; }

        /// <summary>
        /// Http帮助类
        /// </summary>
        private HttpHelper HttpHelper { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 证书
        /// </summary>
        public X509CertificateCollection Certificates { get; set; }

        #endregion

        #region 公共方法

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="cookice">Cookice</param>
        /// <param name="cookices">Cookices</param>
        /// <param name="host">Host</param>
        public HttpRequest(string url/*, string cookice, CookieCollection cookices, string host, X509CertificateCollection certificates = null*/)
        {
            Url = url;
            //Cookice = cookice;
            //Cookies = cookices;
            //Host = Host;
            //Certificates = certificates;
            
            httpItemInit();
        }

        /// <summary>
        /// 获取Html
        /// </summary>
        /// <returns></returns>
        public string GetHtml()
        {
            HttpHelper = new HttpHelper();
            HttpItem.ResultType = ResultType.String;
            HttpResult = HttpHelper.GetHtml(HttpItem);
            setCookice();
            return HttpResult.Html;
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <returns></returns>
        public Image GetImage()
        {
            HttpHelper = new HttpHelper();
            HttpItem.ResultType = ResultType.Byte;
            HttpResult = HttpHelper.GetHtml(HttpItem);
            setCookice();
            Image image = Image.FromStream(new MemoryStream(HttpResult.ResultByte, 0, HttpResult.ResultByte.Length));
            return image;
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 设置返回的Cookie
        /// </summary>
        private void setCookice()
        {
            //Cookice = HttpResult.Cookie;
            //Cookies = HttpResult.CookieCollection;
        }

        /// <summary>
        /// HttpItem初始化（IE）
        /// </summary>
        private void httpItemInit()
        {
            HttpItem = new HttpItem()
            {
                Allowautoredirect = true,
                Accept = "text/html,application/xhtml + xml,application/xml;q=0.9,*/*;q=0.8",
                UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0)",
                Host = Host,
                CookieCollection = Cookies,
                Cookie = Cookice,
                URL = Url,
                Encoding = Encoding.UTF8
            };
            HttpItem.Header.Add("Accept-Language", "zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3");
            HttpItem.Header.Add("Accept-Encoding", "gzip, deflate, br");
            HttpItem.Header.Add("Upgrade-Insecure-Requests", "1");
            HttpItem.Header.Add("Cache-Control", "max-age=0");

            if (Url.Contains("https") && Certificates != null)
            {
                HttpItem.ClentCertificates = Certificates;
            }
        }
        #endregion
    }
}
