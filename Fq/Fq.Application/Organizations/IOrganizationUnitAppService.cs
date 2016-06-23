using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Fq.Organizations.Dto;

namespace Fq.Organizations
{
    public interface IOrganizationUnitAppService : IApplicationService
    {
        Task<OrganizationTreeDto> OrganizationForTree();

        Task<ListResultOutput<OrganizationUnitDto>> GetOrganizationUnits();

        Task<OrganizationUnitDto> CreateOrganizationUnit(CreateOrganizationUnitInput input);

        Task<OrganizationUnitDto> UpdateOrganizationUnit(UpdateOrganizationUnitInput input);

        Task<OrganizationUnitDto> MoveOrganizationUnit(MoveOrganizationUnitInput input);

        Task DeleteOrganizationUnit(IdInput<long> input);
    }
}
