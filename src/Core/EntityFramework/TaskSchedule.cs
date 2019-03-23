using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Amazonspider.Core.EntityFramework
{
    [Table("TaskSchedules")]
    public class TaskSchedule
    {

        public Int64 Id { get; set; }

        public Int64 PlayerAccountId { get; set; }

        public string PlayerStep { get; set; }

        public Int64 RunDateTime { get; set; }

        public string PlayerType { get; set; }


        public static object lockTaskSchedule = new object();

        public static TaskSchedule Get()
        {
            lock (lockTaskSchedule)
            {
                using (AmazonspiderDbContext context = new AmazonspiderDbContext())
                {
                    var time = DateTime.Now.GetTimestamp();
                    var TaskSchedule = context.TaskSchedules.Where(q => q.RunDateTime <= time && q.PlayerAccountId > 0).OrderBy(q => q.PlayerStep).FirstOrDefault();
                    if (TaskSchedule != null)
                    {
                        context.TaskSchedules.Remove(TaskSchedule);
                        lock (AmazonspiderDbContext.sqlLiteLock)
                        {
                            context.SaveChanges();
                        }
                    }

                    return TaskSchedule;
                }
            }
        }

        public static void AddOrUpdate(TaskSchedule taskSchedule)
        {
            using (AmazonspiderDbContext context = new AmazonspiderDbContext())
            {
                try
                {
                    if (taskSchedule.Id > 0)
                    {
                        var t = context.TaskSchedules.Where(q => q.Id == taskSchedule.Id).FirstOrDefault();
                        if (t != null)
                        {
                            t.PlayerAccountId = taskSchedule.PlayerAccountId;
                            t.PlayerStep = taskSchedule.PlayerStep;
                            t.PlayerType = taskSchedule.PlayerType;
                            t.RunDateTime = taskSchedule.RunDateTime;
                        }
                        else
                        {
                            if (taskSchedule.PlayerAccountId > 0)
                            {
                                taskSchedule.Id = 0;
                                context.TaskSchedules.Add(taskSchedule);
                            }
                        }
                    }
                    else
                    {
                        if (taskSchedule.PlayerAccountId > 0)
                            context.TaskSchedules.Add(taskSchedule);
                    }
                    lock (AmazonspiderDbContext.sqlLiteLock)
                    {
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        public static void Delete(TaskSchedule taskSchedule)
        {
            using (AmazonspiderDbContext context = new AmazonspiderDbContext())
            {
                try
                {
                    taskSchedule = context.TaskSchedules.Where(q => q.Id == taskSchedule.Id).FirstOrDefault();
                    if (taskSchedule != null)
                    {
                        context.TaskSchedules.Remove(taskSchedule);
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

    }
}
