using Castle.Windsor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TestApp.Jobs;
using TestApp.Windsor;

namespace TestApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            IWindsorContainer container = new WindsorContainer();
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));
            ComponentRegistrar.AddComponentsTo(container);

            MainAsync();
        }

        static async Task MainAsync()
        {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = await schedulerFactory.GetScheduler();
            await scheduler.Start();
            Console.WriteLine($"任务调度器已启动");

            //var host = Host.CreateDefaultBuilder()
            //.ConfigureServices(services =>
            //{
            //    services.AddTransient<TestJob>();
            //    services.AddQuartz(opt =>
            //    {
            //        // 主键
            //        opt.SchedulerId = "";
            //        // 调度名称
            //        opt.SchedulerName = "";
            //        // 最大并发（一次运行Job的最大数）
            //        opt.MaxBatchSize = 1;
            //        // 可中断作业
            //        opt.InterruptJobsOnShutdown = true;
            //        // 关机时中断作业可等待
            //        opt.InterruptJobsOnShutdownWithWait = true;
            //        // 批量设置触发器的执行的提前时间
            //        opt.BatchTriggerAcquisitionFireAheadTimeWindow = TimeSpan.Zero;
            //    });
            //    // Quartz.Extensions.Hosting 托管
            //    services.AddQuartzHostedService(options =>
            //    {
            //        // 关闭时，我们希望作业优雅地完成
            //        options.WaitForJobsToComplete = true;
            //    });
            //})
            //.Build();

            //创建作业
            var jobDetail = JobBuilder.Create<TestJob>()
                            .StoreDurably(true)
                            .RequestRecovery(true)
                            .WithIdentity("SayHelloJob-Tom", "DemoGroup")
                            .WithDescription("Say hello to Tom job")
                            .Build();
            var trigger = TriggerBuilder.Create()
                            .WithCronSchedule("0 */1 * * * ?")
                            .Build();
            //添加调度
            await scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}
