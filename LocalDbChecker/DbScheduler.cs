using Quartz;
using Quartz.Impl;
using System;

namespace LocalDbChecker
{
    public class DbScheduler
    {
        public static async void Start(string connectionString, string uri)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            //Create job and add some context data
            IJobDetail job = JobBuilder.Create<GetCurrencyRatesJob>()
               .UsingJobData("connnectionString", connectionString)
               .UsingJobData("uri", uri)
               .UsingJobData("date", DateTime.Today.ToShortDateString()).Build();

            ITrigger trigger = TriggerBuilder.Create()
          .WithIdentity("trigger", "group1")
          .StartNow()
          .Build();
            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
