using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using Fq.Authorization;

namespace Fq
{
    [DependsOn(typeof(FqCoreModule), typeof(AbpAutoMapperModule))]
    public class FqApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<FqAuthorizationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            CustomDtoMapper.CreateMappings();
        }
    }
}
