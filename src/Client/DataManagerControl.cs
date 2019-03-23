using Amazonspider.Core;
using Amazonspider.Core.EntityFramework;
using Camouflage.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Amazonspider.Client
{
    public partial class DataManagerControl : UserControl
    {
        private Product product = new Product();

        public DataManagerControl()
        {
            InitializeComponent();
        }



        private void DataManagerControl_Load(object sender, EventArgs e)
        {


            List<Tuple<string, int>> tuples = new List<Tuple<string, int>>();
            tuples.Add(new Tuple<string, int>("全部", -100));
            tuples.Add(new Tuple<string, int>("等待采集", 0));
            tuples.Add(new Tuple<string, int>("采集成功", 1));
            tuples.Add(new Tuple<string, int>("采集失败", -2));
            tuples.Add(new Tuple<string, int>("已经导出", 2));


            cmbStatus.DisplayMember = "Item1";
            cmbStatus.ValueMember = "Item2";
            cmbStatus.DataSource = tuples;
            cmbStatus.SelectedIndex = 0;

            loadData();
        }

        private void pagerControl1_OnPageChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void loadData()
        {

            var selectStatus = cmbStatus.SelectedValue.ToString();

            var list = Product.GetProducts(pagerControl1.PageIndex, pagerControl1.PageSize, int.Parse(selectStatus));
            pagerControl1.DrawControl(list.Item1);
            dataGridView1.DataSource = list.Item2;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            pagerControl1.PageIndex = 1;
            loadData();
        }

        private string exportFilePath = "";
        private string exportImagePath = "";

        private async void btnExport_Click(object sender, EventArgs e)
        {
            btnImoort.Enabled = false;
            btnClear.Enabled = false;
            btnExport.Enabled = false;

            SaveFileDialog savefile = new SaveFileDialog();

            savefile.Filter = "CSV文件(*.csv)|*.csv";

            savefile.RestoreDirectory = true;

            string timeStr = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            savefile.DefaultExt = "csv";
            if (savefile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                exportFilePath = savefile.FileName.ToString(); //获得文件路径 
                exportImagePath = exportFilePath.Substring(0, exportFilePath.LastIndexOf("."));
                await Export();
            }
            btnImoort.Enabled = true;
            btnClear.Enabled = true;
            btnExport.Enabled = true;
        }

        private Task Export()
        {
            return Task.Run(() =>
            {
                using (AmazonspiderDbContext context = new AmazonspiderDbContext())
                {
                    var list = context.Products.Where(q => q.Status == 1).Take(5000).ToList();
                    int canExportCount = 0;
                    int errorExportCount = 0;

                    if (!System.IO.Directory.Exists(exportImagePath))
                    {
                        Directory.CreateDirectory(exportImagePath);
                    }
                    CSVUtil csvUtin = new CSVUtil(exportFilePath);

                    foreach (var item in list)
                    {
                        var titles = item.Title.Split(' ').ToList();
                        foreach (var title in titles)
                        {
                            var l = Setting.Key.Where(q => q != "").Where(q => title.Trim() == q.Trim()).ToList();
                            if (l.Count > 0)
                            {
                                item.Status = 2;//.Update(model.Id, 2);
                                errorExportCount++;
                                continue;
                            }
                        }

                        var img = context.ProductImages.Where(q => q.Asin == item.Asin).FirstOrDefault();
                        var imgName = "";
                        if (img != null)
                        {
                            item.Status = 2;
                            if (File.Exists(Environment.CurrentDirectory + $"\\Images\\{img.Asin}_{img.Number}.tbi"))
                            {
                                System.IO.File.Copy(Environment.CurrentDirectory + $"\\Images\\{img.Asin}_{img.Number}.tbi", $"{exportImagePath}\\{img.Asin}_{img.Number}.tbi", true);
                                imgName = $"{img.Asin}_{img.Number}";
                            }

                            if (item.Price > 0.01M)
                            {
                                csvUtin.AddPorductInfo(item.Title, item.Price.ToString(), "0", "<SPAN style='COLOR: #ffffff; BACKGROUND-COLOR: #ffffff'>" + item.Asin + "</SPAN> " + descFilter(item.Desc), imgName, "", "", "", "", "");
                                canExportCount++;
                            }
                        }
                        else
                        {
                            errorExportCount++;
                            context.Products.Remove(item);
                        }

                        label2.Invoke(new Action(() =>
                        {
                            label2.Text = $"正在导出导出{list.Count},已导出{canExportCount},失败{errorExportCount}";
                        }));
                    }
                    context.SaveChanges();
                    csvUtin.WriteCSV5();
                    label2.Invoke(new Action(() =>
                    {
                        label2.Text = $"导出成功,一共导出{list.Count},已导出{canExportCount},失败{errorExportCount}";
                        MessageBox.Show("导出成功");
                    }));

                }
            });
        }

        private string importDatabasePath = "";

        private async void btnImoort_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "数据库文件（.db）|*.db|所有文件|*.*";//文件扩展名
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                btnImoort.Enabled = false;
                btnClear.Enabled = false;
                btnExport.Enabled = false;
                //MessageBox.Show(fileDialog.FileName);
                importDatabasePath = fileDialog.FileName;
                await Import();
            }
            label2.Text = "无操作";
            btnClear.Enabled = true;
            btnExport.Enabled = true;
            btnImoort.Enabled = true;
        }

        private Task Import()
        {
            return Task.Run(() =>
            {
                try
                {
                    SQLiteConnection sQLiteConnection = new SQLiteConnection($"data source ={importDatabasePath}");
                    AmazonspiderDbContext contextImport = new AmazonspiderDbContext(sQLiteConnection);
                    var waitImportList = contextImport.Products.Where(q => q.Status == 1).ToList();
                    AmazonspiderDbContext context = new AmazonspiderDbContext();
                    int importCount = 0;
                    int errorCount = 0;
                    foreach (var item in waitImportList)
                    {
                        var images = contextImport.ProductImages.Where(q => q.Asin == item.Asin && item.Status != 0).ToList();
                        if (images.Count == 0)
                        {
                            errorCount++;
                            continue;
                        }
                        var product = context.Products.Where(q => q.Asin == item.Asin).FirstOrDefault();
                        if (product != null)
                        {
                            //处理图片信息

                            if (images.Count != 0)
                            {
                                if (product.Status == 0)
                                {
                                    product.Status = 1;
                                    product.Title = item.Title;
                                    product.Price = item.Price;
                                    product.Url = item.Url;
                                    product.Desc = item.Desc;

                                    foreach (var image in images)
                                    {
                                        context.ProductImages.Add(new ProductImage()
                                        {
                                            Asin = image.Asin,
                                            Number = image.Number,
                                            Status = image.Status,
                                            Url = image.Url,
                                        });
                                    }
                                }
                                importCount++;
                            }
                            else
                            {
                                errorCount++;
                            }
                        }
                        else
                        {
                            context.Products.Add(new Product()
                            {
                                Title = item.Title,
                                Price = item.Price,
                                Asin = item.Asin,
                                Desc = item.Desc,
                                Status = item.Status,
                                Time = item.Time
                            });
                            foreach (var image in images)
                            {
                                context.ProductImages.Add(new ProductImage()
                                {
                                    Asin = image.Asin,
                                    Number = image.Number,
                                    Status = image.Status,
                                    Url = image.Url,
                                });
                            }
                            importCount++;
                        }
                        label2.Invoke(new Action(() =>
                        {
                            label2.Text = $"一共导入{waitImportList.Count},导入成功：{importCount},导入失败：{errorCount}";
                        }));
                    }
                    label2.Invoke(new Action(() =>
                    {
                        label2.Text = $"导入完毕,{waitImportList.Count},导入成功：{importCount},导入失败：{errorCount}";
                    }));
                    context.SaveChanges();
                    MessageBox.Show("导入成功");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导入失败");
                    log4net.ILog log = log4net.LogManager.GetLogger("testApp.Logging");//获取一个日志记录器 
                    log.Error("导入失败--", ex);
                }
            });
        }

        private string descFilter(string desc)
        {
            return desc.Replace("P.when(\"ReplacementPartsBulletLoader\").execute(function(module){ module.initializeDPX(); })", "").Replace("P.when('ReplacementPartsBulletLoader').execute(function(module){ module.initializeDPX(); })", "").Replace("\n", "").Replace("\t", "").Replace(",", "，");
        }

        private async void btnClear_Click(object sender, EventArgs e)
        {
            btnClear.Enabled = false;
            btnExport.Enabled = false;
            btnImoort.Enabled = false;
            using (AmazonspiderDbContext context = new AmazonspiderDbContext())
            {
                label2.Text = "正在清理输入请稍后。。。";

                Int32 count = 0;

                var list = context.Products.Where(q => q.Status != 1 && q.Status != 2).ToList();
                await Task.Run(() =>
                {
                    var t1query = from t in context.TaskSchedules
                                  join p in context.Products on t.PlayerAccountId equals p.Id
                                  where p.Status != 1 && p.Status != 2 && t.PlayerType == "Amazonspider.ProductDownload.Download"
                                  select t;

                    context.TaskSchedules.RemoveRange(t1query);


                    var t2query = from t in context.TaskSchedules
                                  join pi in context.ProductImages on t.PlayerAccountId equals pi.Id
                                  join p in context.Products on pi.Asin equals p.Asin 

                                  where p.Status != 1 && p.Status != 2 && t.PlayerType == "Amazonspider.ProductImageDownload.Download"
                                  select t;

                    context.TaskSchedules.RemoveRange(t2query);

                    var query = from pi in context.ProductImages
                                join p in context.Products on pi.Asin equals p.Asin
                                where p.Status != 1 && p.Status != 2
                                select pi;

                    context.ProductImages.RemoveRange(query);
                    context.Products.RemoveRange(context.Products.Where(q => q.Status != 1 && q.Status != 2));

                });
                await context.SaveChangesAsync();

   
            }

            using (AmazonspiderDbContext context = new AmazonspiderDbContext())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

                context.Database.ExecuteSqlCommand("VACUUM");

                await context.Database.ExecuteSqlCommandAsync("REINDEX 'Asin'");
            }
                MessageBox.Show("清理成功");
            label2.Text = "无操作";
            btnClear.Enabled = true;
            btnExport.Enabled = true;
            btnImoort.Enabled = true;
        }
    }
}
