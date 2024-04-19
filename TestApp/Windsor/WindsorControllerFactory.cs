using Castle.MicroKernel;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TestApp.Windsor
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IWindsorContainer _container;
        public WindsorControllerFactory(IWindsorContainer container)
        {
            if(container == null)
            {
                throw new ArgumentNullException("container");
            }
            this._container = container;
        }
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(0x192, string.Format("当前对{0}的请求不存在", requestContext.HttpContext.Request.Path));
            }
            return (IController)_container.Resolve(controllerType);
        }
        public override void ReleaseController(IController controller)
        {
            var dispossable = controller as IDisposable;
            if (dispossable != null)
            {
                dispossable.Dispose();
            }
            this._container.Release(controller);
        }
    }
}