using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Fq.Authorization.Roles.Dto;

namespace Fq.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task UpdateRolePermissions(UpdateRolePermissionsInput input);

        Task<ListResultOutput<RoleListDto>> GetRoles();

        Task<GetRoleForEditOutput> GetRoleForEdit(NullableIdInput input);

        Task CreateOrUpdateRole(CreateOrUpdateRoleInput input);

        Task DeleteRole(EntityRequestInput input);
    }
}
