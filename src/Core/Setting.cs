using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Text;

namespace Amazonspider.Core
{
    [Serializable]
    public static class Setting
    {
        public static string Path { get; set; }

        public static string ProductDesc { get; set; }

        public static string PriceFunction { get; set; }

        /// <summary>
        /// 进程数量
        /// </summary>
        public static int ThreadNum { get; set; } = 3;

        /// <summary>
        /// 代理字符串
        /// </summary>
        public static string Proxy { get; set; } = "";

        /// <summary>
        /// 机器码
        /// </summary>
        public static string SoftCode { get; set; } = new SKGL.SerialKeyConfiguration().MachineCode.ToString();

        public static List<string> Key { get; set; } = new List<string>();


        public static List<string> ProductTypes { get; set; } = new List<string>();

        /// <summary>
        /// 注册码
        /// </summary>
        public static string RegisterCode { get; set; } = "";

        /// <summary>
        /// 初始化配置
        /// </summary>
        public static void Init(Action action)
        {
            //初始配置信息
            var Configuration = new ConfigurationBuilder()
                .AddJsonFile($"{Path}\\appsetting.json")
                .Build();

            ThreadNum = int.Parse(Configuration.GetSection("ThreadNum").Value);
            Proxy = Configuration.GetSection("Proxy").Value;
            RegisterCode = Configuration.GetSection("RegisterCode").Value;
            Configuration.GetSection("ThreadNum").Value = ThreadNum.ToString();

            //获取价格计算函数
            {
                string path = Path + "\\Config\\Price.js";
                if (File.Exists(path))
                {
                    StreamReader sr = File.OpenText(path);
                    StringBuilder jsonArrayText_tmp = new StringBuilder();

                    string txt = sr.ReadToEnd().ToString();
                    sr.Close();
                    try
                    {
                        PriceFunction = txt;
                    }
                    catch { }
                }
            }

            //获取商品详情
            {
                string path = Path + "\\Config\\ProductDesc.txt";
                if (File.Exists(path))
                {
                    StreamReader sr = File.OpenText(path);
                    StringBuilder jsonArrayText_tmp = new StringBuilder();

                    string txt = sr.ReadToEnd().ToString();
                    sr.Close();
                    try
                    {
                        ProductDesc = txt;
                    }
                    catch { }
                }
            }
            //获取关键词
            {
                string path = Path + "\\Config\\key.txt";
                if (File.Exists(path))
                {
                    StreamReader sr = File.OpenText(path);
                    StringBuilder jsonArrayText_tmp = new StringBuilder();

                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Trim() != "")
                        {
                            Key.Insert(0, line);
                        }
                    }
                }
            }

            //获取分类
            {
                string path = Path + "\\Config\\ProductTypes.txt";
                if (File.Exists(path))
                {
                    StreamReader sr = File.OpenText(path);
                    StringBuilder jsonArrayText_tmp = new StringBuilder();

                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Trim() != "")
                        {
                            ProductTypes.Insert(0, line);
                        }
                    }
                }
            }
            action?.Invoke();
        }

        public static void Save()
        {

            string jsonText = JsonConvert.SerializeObject(
                new
                {
                    ThreadNum,
                    Proxy,
                    RegisterCode
                }, Formatting.Indented);
            // 获取当前程序所在路径，并将要创建的文件命名为info.json 
            string fp = Path + "\\appsetting.json";
            if (!File.Exists(fp))  // 判断是否已有相同文件 
            {
                FileStream fs1 = new FileStream(fp, FileMode.Create, FileAccess.ReadWrite);
                fs1.Close();
            }
            File.WriteAllText(fp, jsonText);
        }
    }
}
