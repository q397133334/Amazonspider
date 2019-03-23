using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazonspider.Core.EntityFramework
{
    [Table("Products")]
    public class Product
    {
        public Int64 Id { get; set; }

        public string Asin { get; set; }

        public string Title { get; set; }

        public string Desc { get; set; }

        public decimal Price { get; set; }

        public string Url { get; set; }

        public Int64 Time { get; set; }

        public Int64 Status { get; set; }

        [NotMapped]
        public string StatusText
        {
            get
            {
                switch (Status)
                {
                    case 1:
                        return "采集成功";
                    case 0:
                        return "待采集";
                    case -1:
                        return "采集失败，等待重试";
                    default:
                        return "无效数据";
                }

            }
        }


        public static Product Get(long id)
        {
            using (AmazonspiderDbContext context = new AmazonspiderDbContext())
            {
                return context.Products.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static Product Get(string asin)
        {
            using (AmazonspiderDbContext context = new AmazonspiderDbContext())
            {
                return context.Products.Where(q => q.Asin == asin).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="status">商品状态</param>
        /// <returns></returns>
        public static Tuple<int, List<Product>> GetProducts(int pageIndex, int pageSize, int status = 1)
        {
            using (AmazonspiderDbContext context = new AmazonspiderDbContext())
            {
                var query = context.Products.AsQueryable();
                if (status != -100)
                {
                    query = query.Where(q => q.Status == status);
                }
                int count = query.Count();
                var list = query.OrderByDescending(q => q.Id).Skip(pageIndex * pageSize - pageSize).Take(pageSize).ToList();

                return new Tuple<int, List<Product>>(count, list);
            }
        }


        public static Product AddOrUpdate(Product product)
        {

            using (AmazonspiderDbContext context = new AmazonspiderDbContext())
            {
                lock (AmazonspiderDbContext.sqlLiteLock)
                {
                    var p = context.Products.Where(q => q.Asin == product.Asin).FirstOrDefault();
                    if (p != null)
                    {
                        if (product.Id > -2)
                        {
                            p.Price = product.Price;
                            p.Status = product.Status;
                            p.Title = product.Title;
                            p.Desc = product.Desc;
                        }
                    }
                    else
                    {
                        product = context.Products.Add(product);
                    }
                    try
                    {
                        context.SaveChanges();
                        return product;
                    }
                    catch (Exception ex)
                    {
                        product.Id = -1;
                        return product;
                    }
                    finally
                    {

                    }
                }
            }
        }


        public static void Remove(int id)
        {
            using (AmazonspiderDbContext context = new AmazonspiderDbContext())
            {
                var product = context.Products.Where(q => q.Id == id).FirstOrDefault();
                if (product != null)
                {
                    var images = context.ProductImages.Where(q => q.Asin == product.Asin).ToList();
                    foreach (var image in images)
                    {
                        context.ProductImages.Remove(image);
                    }
                    context.Products.Remove(product);
                    try
                    {
                        context.SaveChanges();
                    }
                    catch(Exception ex)
                    {
                        log4net.ILog log = log4net.LogManager.GetLogger("testApp.Logging");//获取一个日志记录器 
                        log.Error("移除商品异常--", ex);
                    }
                }
            }
        }

        public static Tuple<int, int, int, int> GetCount()
        {
            using (AmazonspiderDbContext context = new AmazonspiderDbContext())
            {
                return new Tuple<int, int, int, int>(
                    context.Products.Count(),
                    context.Products.Where(q => q.Status == 1).Count(),
                    context.Products.Where(q => q.Status == 0).Count(),
                    context.Products.Where(q => q.Status == -1).Count());
            }
        }
    }
}
