using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using Fq.EntityFramework;

namespace Fq
{
    [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(FqCoreModule))]
    public class FqDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<FqDbContext>());

            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
