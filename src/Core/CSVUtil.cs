using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Camouflage.Common
{
    public class CSVUtil
    {

        List<String[]> list;
        StreamWriter fileWriter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePathName">保存路径和文件名</param>
        public CSVUtil(string filePathName)
        {
            list = new List<string[]>();
            fileWriter = new StreamWriter(filePathName, false, Encoding.Default);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="goodsName">宝贝名称</param>
        /// <param name="goodsPrice">宝贝价格</param>
        /// <param name="expressPrice">快递费用</param>
        /// <param name="goodsDescription">宝贝描述</param>
        /// <param name="img1">图片1</param>
        /// <param name="img2">图片2</param>
        /// <param name="img3">图片3</param>
        /// <param name="img4">图片4</param>
        /// <param name="img5">图片5，以上5个图片没用图的话，就用空字符串</param>
        /// <param name="goodsShortDescription">宝贝卖点</param>

        public void AddPorductInfo(string goodsName, string goodsPrice, string expressPrice, string goodsDescription, string img1, string img2, string img3, string img4, string img5, string goodsShortDescription)
        {
            // 根据图片参数，拼写图片字段
            string img = "\"";
            if (!string.IsNullOrWhiteSpace(img1))
            {
                img += img1 + ":1:0:|;";
            }
            if (!string.IsNullOrWhiteSpace(img2))
            {
                img += img2 + ":1:1:|;";
            }
            if (!string.IsNullOrWhiteSpace(img3))
            {
                img += img3 + ":1:2:|;";
            }
            if (!string.IsNullOrWhiteSpace(img4))
            {
                img += img4 + ":1:3:|;";
            }
            if (!string.IsNullOrWhiteSpace(img5))
            {
                img += img5 + ":1:4:|;";
            }
            img += "\"";

            goodsDescription = CleanHtml(goodsDescription);
            goodsDescription = goodsDescription.Replace("\"", "'").Replace(",", "，");
            goodsShortDescription = CleanHtml(goodsShortDescription);
            goodsShortDescription = goodsShortDescription.Replace("\"", "'").Replace(",", "，");

            String[] s2 = new String[] { "\"" + Filter(goodsName) + "\"", "", "\"\"", "\"\"", "\"\"", "\"\"", "", Filter(goodsPrice), "\"\"", "", "", "", "", "", Filter(expressPrice), "", "", "", "", "\"\"", "\"" + Filter(goodsDescription) + "\"", "\"\"", "", "", "\" \"", "\" \"", "\"\"", "", img, "\"\"", "\"\"", "\"\"", "\"\"", "\"\"", "\"\"", "", "", "", "", "", "", "", "", "\"\"", "", "", "", "", "", "", "", "", "", "\"\"", "", "", "", "\"" + Filter(goodsShortDescription) + "\"", "\"\"", "\"\"", "\"\"", "", "" };
            list.Add(s2);
        }

        /// <summary>
        /// 去掉HTML中的所有标签,只留下纯文本
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>

        public static string CleanHtml(string strHtml)
        {
            if (string.IsNullOrEmpty(strHtml)) return strHtml;
            //删除脚本
            //Regex.Replace(strHtml, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase)
            strHtml = Regex.Replace(strHtml, @"(\<script(.+?)\</script\>)|(\<style(.+?)\</style\>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            //删除标签
            //var r = new Regex(@"</?[^>]*>", RegexOptions.IgnoreCase);
            //Match m;
            //for (m = r.Match(strHtml); m.Success; m = m.NextMatch())
            //{
            //    strHtml = strHtml.Replace(m.Groups[0].ToString(), "");
            //}
            return strHtml.Trim();
        }

        public static string Filter(string str)
        {
            str = str.Replace("\"", "\"\"");//替换英文冒号 英文冒号需要换成两个冒号
            str = str.Replace(",", "，");
            str = str.Replace("\"", "");
            str = str.Replace("\r", "");
            str = str.Replace("\n", "");
            //if (str.Contains(',') || str.Contains('"')
            //    || str.Contains('\r') || str.Contains('\n')) //含逗号 冒号 换行符的需要放到引号中
            //{
            //    str = string.Format("\"{0}\"", str);

            //}
            return str;
        }

            public void WriteCSV5()
        {
            // 写标题
            fileWriter.WriteLine("version 1.00");
            fileWriter.WriteLine("title,cid,seller_cids,stuff_status,location_state,location_city,item_type,price,auction_increment,num,valid_thru,freight_payer,post_fee,ems_fee,express_fee,has_invoice,has_warranty,approve_status,has_showcase,list_time,description,cateProps,postage_id,has_discount,modified,upload_fail_msg,picture_status,auction_point,picture,video,skuProps,inputPids,inputValues,outer_id,propAlias,auto_fill,num_id,local_cid,navigation_type,user_name,syncStatus,is_lighting_consigment,is_xinpin,foodparame,features,buyareatype,global_stock_type,global_stock_country,sub_stock_type,item_size,item_weight,sell_promise,custom_design_flag,wireless_desc,barcode,sku_barcode,newprepay,subtitle,cpv_memo,input_custom_cpv,qualification,add_qualification,o2o_bind_service");
            fileWriter.WriteLine("宝贝名称,宝贝类目,店铺类目,新旧程度,省,城市,出售方式,宝贝价格,加价幅度,宝贝数量,有效期,运费承担,平邮,EMS,快递,发票,保修,放入仓库,橱窗推荐,开始时间,宝贝描述,宝贝属性,邮费模版ID,会员打折,修改时间,上传状态,图片状态,返点比例,新图片,视频,销售属性组合,用户输入ID串,用户输入名-值对,商家编码,销售属性别名,代充类型,数字ID,本地ID,宝贝分类,用户名称,宝贝状态,闪电发货,新品,食品专项,尺码库,采购地,库存类型,国家地区,库存计数,物流体积,物流重量,退换货承诺,定制工具,无线详情,商品条形码,sku 条形码,7天退货,宝贝卖点,属性值备注,自定义属性值,商品资质,增加商品资质,关联线下服务,,,,");

            // 写数据
            foreach (String[] strArr in list)
            {
                fileWriter.WriteLine(String.Join(",", strArr));
            }
            fileWriter.Flush();
            fileWriter.Close();
        }

    }
}
