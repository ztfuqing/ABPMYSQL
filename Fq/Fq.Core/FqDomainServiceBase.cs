using Abp.Domain.Services;

namespace Fq
{
    public abstract class FqDomainServiceBase : DomainService
    {
        protected FqDomainServiceBase()
        {
            LocalizationSourceName = FqConsts.LocalizationSourceName;
        }
    }
}
