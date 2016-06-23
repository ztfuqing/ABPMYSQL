using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Fq.MultiTenancy.Dto;

namespace Fq.MultiTenancy
{
    public interface ITenantAppService : IApplicationService
    {
        ListResultOutput<TenantListDto> GetTenants();

        Task CreateTenant(CreateTenantInput input);
    }
}
