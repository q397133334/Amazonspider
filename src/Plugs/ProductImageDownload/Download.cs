using Amazonspider.Core;
using Amazonspider.Core.EntityFramework;
using Amazonspider.Core.Player;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Amazonspider.ProductImageDownload
{
    public class Download : BasePlayer, IPlayer
    {

        public Download()
        {
            Timeout = 1000 * 120;
        }

        private ProductImage productImage = null;

        protected override void ThreadRun()
        {
            productImage = ProductImage.Get(TaskSchedule.PlayerAccountId);
            if (productImage != null)
            {
                productImage.Status = 1;
                HttpRequest httpRequest = new HttpRequest(productImage.Url);


                try
                {
                    WriteLog($"开始下载图片--{productImage.Url}");
                    Image image = httpRequest.GetImage();
                    var imagePath = createImagepath();
                    image.Save($"{imagePath}/{productImage.Asin}_{productImage.Number}.tbi", ImageFormat.Jpeg);
                    ImageHelper.JoinImage($"{imagePath}/{productImage.Asin}_{productImage.Number}.tbi", Environment.CurrentDirectory + "\\Config\\shuiyin.png", "C");
                    WriteLog($"{productImage.Asin}_{productImage.Number}商品图片下载成功", Core.ConsoleLogStatus.Success);
                    Complete?.Invoke(null, productImage, null, this);
                }
                catch
                {
                    TaskSchedule.RunDateTime = DateTime.Now.AddMinutes(1).GetTimestamp();
                }
            }
            else
            {
                Complete(null, null, null, this);
                return;
            }
        }

        protected override void ThreadAbout()
        {
            TaskSchedule.RunDateTime = DateTime.Now.AddMinutes(1).GetTimestamp();
            base.ThreadAbout();
        }

        private string createImagepath()
        {
            string imagePath = "";
            imagePath = Environment.CurrentDirectory + "\\Images";
            if (System.IO.Directory.Exists(imagePath) == false)
            {
                try
                {
                    System.IO.Directory.CreateDirectory(imagePath);
                }
                catch
                {

                }
            }
            return imagePath;
        }
    }
}
