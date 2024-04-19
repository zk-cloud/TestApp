using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestApp.Windsor
{
    public class ComponentRegistrar
    {
        public static void AddComponentsTo(IWindsorContainer container)
        {
            AddCustomRepositoriesToService(container);
            AddCustomRepositoriesToDao(container);
            AddAllControllerTo(container);
            //AddSessionHelperTo(container);
        }
        private static void AddCustomRepositoriesToService(IWindsorContainer container)
        {
            container.Register(
                Classes.FromAssemblyNamed("TestApp.Services")
                .Where(type => type.Name.EndsWith("Services"))
                .WithService.DefaultInterfaces());
        }
        private static void AddCustomRepositoriesToDao(IWindsorContainer container)
        {
            container.Register(
                Classes.FromAssemblyNamed("TestApp.Dao")
                .Where(type => type.Name.EndsWith("Dao"))
                .WithService.DefaultInterfaces());
        }
        private static void AddAllControllerTo(IWindsorContainer container)
        {
            container .Register(Classes.FromAssemblyNamed("TestApp")
                .Where(typeof(IController).IsAssignableFrom)
                .If(c => c.Name.EndsWith("Controller"))
                .LifestyleTransient());
        }
    }
}