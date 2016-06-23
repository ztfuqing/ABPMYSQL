using Abp;

namespace Fq
{
    public abstract class FqServiceBase : AbpServiceBase
    {
        protected FqServiceBase()
        {
            LocalizationSourceName = FqConsts.LocalizationSourceName;
        }
    }
}
