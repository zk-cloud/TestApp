using Quartz;
using Quartz.Impl;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using XFramework.XInject;
using XFramework.XInject.MVC;

namespace TestApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static ApplicationContext applicationContext { get; set; }
        private static IScheduler scheduler;
        protected async void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Ӧ�ó���������
            applicationContext = new ClassPathXmlApplicationContext(
                Path.Combine(HttpRuntime.AppDomainAppPath,
                    Path.Combine("bin", "Bean_Config.xml")
                )
            );
            // ��дControllerFactory�Ա���ע��
            ControllerBuilder.Current.SetControllerFactory(
            new XInjectControllerFactory()
                {
                    applicationContext = applicationContext
                });

            //IWindsorContainer container = new WindsorContainer();
            //ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));
            //ComponentRegistrar.AddComponentsTo(container);

            #region ��Web��Ŀ��ʹ��Quartz�������Ҫ���ǵ�վ����̱����պ����´��������
            if (scheduler != null)
            {
                try
                {
                    _ = scheduler.Shutdown(false);
                }
                catch { }
            }
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            scheduler = await schedulerFactory.GetScheduler();
            _ = scheduler.Start();
            #endregion
        }

        protected void Application_End()
        {
            _ = scheduler.Shutdown(false);
        }
    }
}
