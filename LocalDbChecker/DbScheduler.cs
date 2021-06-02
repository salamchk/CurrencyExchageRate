using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication2
{
    public class DbScheduler
    {
        public static async void Start(string connectionString, string uri)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();
            int triggerId = 0;
            for (DateTime date = DateTime.Today.AddDays(-30); date < DateTime.Today; date = date.AddDays(1))
            {
                //Create job and add some context data
                IJobDetail job = JobBuilder.Create<DbParser>()
                    .UsingJobData("connnectionString", connectionString)
                    .UsingJobData("uri", uri)
                    .UsingJobData("date",date.ToShortDateString()).Build();
                
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity($"trigger{triggerId++}", "group1")
                    .StartNow().WithDailyTimeIntervalSchedule(x=>x
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0,0)))
                    .Build();

                await scheduler.ScheduleJob(job, trigger);
            }
        }
    }
}
