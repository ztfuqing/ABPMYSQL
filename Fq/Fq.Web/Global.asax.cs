using System;
using Abp.Dependency;
using Abp.Reflection;
using Abp.Web;
using Castle.Facilities.Logging;

namespace Fq.Web
{
    public class MvcApplication : AbpWebApplication
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            AbpBootstrapper.IocManager.RegisterIfNot<IAssemblyFinder, CurrentDomainAssemblyFinder>();

            AbpBootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(f => f.UseLog4Net().WithConfig("log4net.config"));
            base.Application_Start(sender, e);
        }
    }
}
