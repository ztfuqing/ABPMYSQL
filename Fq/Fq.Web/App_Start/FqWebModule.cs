using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.IO;
using Abp.Modules;
using Abp.Web.Mvc;
using Abp.Web.SignalR;
using Abp.Zero.Configuration;
using Castle.MicroKernel.Registration;
using Fq.Api;
using Fq.Web.App.Startup;
using Fq.Web.Bundling;
using Fq.Web.Front.Startup;
using Hangfire;
using Microsoft.Owin.Security;

namespace Fq.Web
{
    [DependsOn(
        typeof(FqDataModule),
        typeof(FqApplicationModule),
        typeof(FqWebApiModule),
        typeof(AbpWebSignalRModule),
        typeof(AbpWebMvcModule))]
    public class FqWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Enable database based localization
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<AppNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            IocManager.IocContainer.Register(
                Component
                    .For<IAuthenticationManager>()
                    .UsingFactoryMethod(() => HttpContext.Current.GetOwinContext().Authentication)
                    .LifestyleTransient()
                );

            //Areas
            
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleTable.Bundles.IgnoreList.Clear();
            AppBundleConfig.RegisterBundles(BundleTable.Bundles);
            FrontBundleConfig.RegisterBundles(BundleTable.Bundles);
            CommonBundleConfig.RegisterBundles(BundleTable.Bundles);
            //BundleTable.EnableOptimizations = true;
            
        }

        public override void PostInitialize()
        {
            var server = HttpContext.Current.Server;
            var appFolders = IocManager.Resolve<AppFolders>();

            appFolders.SampleProfileImagesFolder = server.MapPath("~/Common/Images/SampleProfilePics");
            appFolders.TempFileDownloadFolder = server.MapPath("~/Temp/Downloads");

            try { DirectoryHelper.CreateIfNotExists(appFolders.TempFileDownloadFolder); } catch { }
        }
    }
}
