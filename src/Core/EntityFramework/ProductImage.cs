using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Amazonspider.Core.EntityFramework
{
    [Table("ProductImages")]
    public class ProductImage
    {
        public Int64 Id { get; set; }

        public string Asin { get; set; }

        public string Url { get; set; }

        public Int64 Number { get; set; }

        public int Status { get; set; }

        public static ProductImage Get(long id)
        {
            using (AmazonspiderDbContext context = new AmazonspiderDbContext())
            {
                return context.ProductImages.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static ProductImage Get(string asin)
        {
            using (AmazonspiderDbContext context = new AmazonspiderDbContext())
            {
                return context.ProductImages.Where(q => q.Asin == asin).FirstOrDefault();
            }
        }

        public static ProductImage AddOrUpdate(ProductImage productImage)
        {
            using (AmazonspiderDbContext context = new AmazonspiderDbContext())
            {
                if (productImage.Id > 0)
                {
                    var p = context.ProductImages.Where(q => q.Id == productImage.Id).FirstOrDefault();
                    if (p != null)
                    {
                        p.Status = productImage.Status;
                    }
                }
                else
                {
                    productImage = context.ProductImages.Add(productImage);
                }
                try
                {
                    lock (AmazonspiderDbContext.sqlLiteLock)
                    {
                        context.SaveChanges();
                        return productImage;
                    }
                }
                catch (Exception ex)
                {
                    productImage.Id = -1;
                    return productImage;
                }
                finally
                {

                }
            }
        }
    }
}

