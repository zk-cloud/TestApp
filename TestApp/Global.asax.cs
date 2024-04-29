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
            Console.WriteLine($"���������������");

            //var host = Host.CreateDefaultBuilder()
            //.ConfigureServices(services =>
            //{
            //    services.AddTransient<TestJob>();
            //    services.AddQuartz(opt =>
            //    {
            //        // ����
            //        opt.SchedulerId = "";
            //        // ��������
            //        opt.SchedulerName = "";
            //        // ��󲢷���һ������Job���������
            //        opt.MaxBatchSize = 1;
            //        // ���ж���ҵ
            //        opt.InterruptJobsOnShutdown = true;
            //        // �ػ�ʱ�ж���ҵ�ɵȴ�
            //        opt.InterruptJobsOnShutdownWithWait = true;
            //        // �������ô�������ִ�е���ǰʱ��
            //        opt.BatchTriggerAcquisitionFireAheadTimeWindow = TimeSpan.Zero;
            //    });
            //    // Quartz.Extensions.Hosting �й�
            //    services.AddQuartzHostedService(options =>
            //    {
            //        // �ر�ʱ������ϣ����ҵ���ŵ����
            //        options.WaitForJobsToComplete = true;
            //    });
            //})
            //.Build();

            //������ҵ
            var jobDetail = JobBuilder.Create<TestJob>()
                            .StoreDurably(true)
                            .RequestRecovery(true)
                            .WithIdentity("SayHelloJob-Tom", "DemoGroup")
                            .WithDescription("Say hello to Tom job")
                            .Build();
            var trigger = TriggerBuilder.Create()
                            .WithCronSchedule("0 */1 * * * ?")
                            .Build();
            //��ӵ���
            await scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}
