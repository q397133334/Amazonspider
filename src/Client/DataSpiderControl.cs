using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Amazonspider.Core;
using Amazonspider.Core.EntityFramework;
using Amazonspider.Core.PlugIns;
using SKGL;

namespace Amazonspider.Client
{
    public partial class DataSpiderControl : UserControl
    {

        Bootstrap Bootstrap;
        public DataSpiderControl()
        {
            InitializeComponent();
        }

        private void btsSaveSetting_Click(object sender, EventArgs e)
        {
            try
            {
                //Setting.Proxy = txtProxy.Text;
                //Setting.ThreadNum = (int)txtThreadNum.Value;
                Setting.RegisterCode = txtRegisterCode.Text;
                Setting.Save();
                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {

            }
        }

        private void DataSpiderControl_Load(object sender, EventArgs e)
        {
            Setting.Init(() =>
            {
                //txtThreadNum.Value = Setting.ThreadNum;
                //txtProxy.Text = Setting.Proxy;
                txtSoftCode.Text = Setting.SoftCode;
                txtRegisterCode.Text = Setting.RegisterCode;
            });

            CheckRegister();
            Bootstrap = new Bootstrap();
            Bootstrap.WriteLog += (msg, consoleLogStatus) =>
            {
                listboxLog.Invoke(new Action(() =>
                {
                    LogListBoxItem item = new LogListBoxItem() { Msg = $"{msg}------" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ConsoleLogStatus = consoleLogStatus };
                    listboxLog.Items.Insert(0, item);
                    while (listboxLog.Items.Count > 1000)
                    {
                        listboxLog.Items.RemoveAt(listboxLog.Items.Count - 1);
                    }
                }
                ));
            };
        }

        private void listboxLog_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            ListBox listBox = sender as ListBox;
            if (e.Index == -1)
                return;


            switch ((listBox.Items[e.Index] as LogListBoxItem).ConsoleLogStatus)
            {
                case ConsoleLogStatus.Nomral:
                    e.Graphics.DrawString((((ListBox)sender).Items[e.Index] as LogListBoxItem).Msg, e.Font, new SolidBrush(Color.Black), e.Bounds);
                    break;
                case ConsoleLogStatus.Success:
                    e.Graphics.DrawString((((ListBox)sender).Items[e.Index] as LogListBoxItem).Msg, e.Font, new SolidBrush(Color.Green), e.Bounds);
                    break;
                case ConsoleLogStatus.Error:
                    e.Graphics.DrawString((((ListBox)sender).Items[e.Index] as LogListBoxItem).Msg, e.Font, new SolidBrush(Color.Red), e.Bounds);
                    break;
                case ConsoleLogStatus.Exption:
                    e.Graphics.DrawString((((ListBox)sender).Items[e.Index] as LogListBoxItem).Msg, e.Font, new SolidBrush(Color.Yellow), e.Bounds);
                    break;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            var playerList = new List<PlugInSource>();
            playerList.Add(new PlugInSource()
            {
                Assemblie = new ProductDownload.Download().GetType().Assembly,
                PlugInName = "亚马逊商品采集插件",
                PlugInType = PlugInType.Player
            });

            Bootstrap.Start(playerList);
            timer1.Start();
            runInfoBindingSource.DataSource = Bootstrap.RunInfos;
            btnStart.Enabled = false;
            btnStop.Enabled = true;

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Bootstrap.Stop();
            timer1.Stop();
            btnStop.Enabled = false;
            btnStart.Enabled = true;

        }

        private void btnAddAsin_Click(object sender, EventArgs e)
        {
            var product = Product.AddOrUpdate(new Product()
            {
                Asin = txtAsin.Text,
                Status = 0,
                Desc = "",
                Title = "",
                Price = 0,
                Time = DateTime.Now.GetTimestamp()
            });
            if (product.Id > -1)
            {
                TaskSchedule.AddOrUpdate(new TaskSchedule()
                {
                    PlayerAccountId = product.Id,
                    PlayerStep = "download",
                    RunDateTime = DateTime.Now.GetTimestamp(),
                    PlayerType = new Amazonspider.ProductDownload.Download().GetType().ToString()
                });
            }
        }

        private long timerCount = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            timerCount++;
            runInfoBindingSource.ResetBindings(true);
        }

        public void CheckRegister()
        {
            //MIXSQ-MQHMY-DPXZH-YHNLH
            // 1.创建key验证对象
            //    var ValidateAKey = new Validate();
            //    //2.设置密钥
            //    ValidateAKey.secretPhase = "Camouflage";
            //    //3.设置要验证的key，注意，这是上面代码生成的key1内容
            //    ValidateAKey.Key = txtRegisterCode.Text;

            //    if (!ValidateAKey.IsValid || ValidateAKey.IsExpired)
            //    {
            //        MessageBox.Show("软件验证失败，暂时无法使用");
            //        btnStart.Enabled = false;
            //        btnStop.Enabled = false;
            //    }
            //    else
            //    {
            //        this.ParentForm.Text += $"--剩余使用期限：{ValidateAKey.DaysLeft}天";
            //    }
        }
        WebBrowser webBrowser;
        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser = new WebBrowser();
            webBrowser.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var cookies = webBrowser.GetCookieEvent();
        }
    }

    /// <summary>
    /// Listbox的Item
    /// </summary>
    public class LogListBoxItem
    {
        public string Msg { get; set; }
        public ConsoleLogStatus ConsoleLogStatus { get; set; }
    }
}
