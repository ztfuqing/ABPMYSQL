using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Fq.Authorization;
using Fq.Authorization.Roles;
using Fq.Authorization.Roles.Dto;
using Fq.Dto;

namespace Fq.Roles
{
    [AbpAuthorize(AppPermissions.Admin_Roles)]
    public class RoleAppService : FqAppServiceBase, IRoleAppService
    {
        private readonly RoleManager _roleManager;

        public RoleAppService(RoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task UpdateRolePermissions(UpdateRolePermissionsInput input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.RoleId);

            var grantedPermissions = PermissionManager.GetPermissionsFromNamesByValidating(input.GrantedPermissionNames);
            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
        }

        public async Task<ListResultOutput<RoleListDto>> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return new ListResultOutput<RoleListDto>(roles.MapTo<List<RoleListDto>>());
        }

        [AbpAuthorize(AppPermissions.Admin_Roles_Create, AppPermissions.Admin_Roles_Edit)]
        public async Task<GetRoleForEditOutput> GetRoleForEdit(NullableIdInput input)
        {
            var permissions = PermissionManager.GetAllPermissions();
            var grantedPermissions = new Permission[0];
            RoleEditDto roleEditDto;

            if (input.Id.HasValue)
            {
                var role = await _roleManager.GetRoleByIdAsync(input.Id.Value);
                grantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).ToArray();
                roleEditDto = role.MapTo<RoleEditDto>();
            }
            else
            {
                roleEditDto = new RoleEditDto();
            }

            return new GetRoleForEditOutput
            {
                Role = roleEditDto,
                Permissions = permissions.MapTo<List<FlatPermissionDto>>().OrderBy(p => p.DisplayName).ToList(),
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList()
            };
        }

        public async Task CreateOrUpdateRole(CreateOrUpdateRoleInput input)
        {
            if (input.Role.Id.HasValue)
            {
                await UpdateRoleAsync(input);
            }
            else
            {
                await CreateRoleAsync(input);
            }
        }

        [AbpAuthorize(AppPermissions.Admin_Roles_Delete)]
        public async Task DeleteRole(EntityRequestInput input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.Id);
            CheckErrors(await _roleManager.DeleteAsync(role));
        }


        [AbpAuthorize(AppPermissions.Admin_Roles_Edit)]
        protected virtual async Task UpdateRoleAsync(CreateOrUpdateRoleInput input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.Role.Id.Value);
            role.DisplayName = input.Role.DisplayName;
            role.IsDefault = input.Role.IsDefault;

            await UpdateGrantedPermissionsAsync(role, input.GrantedPermissionNames);
        }

        [AbpAuthorize(AppPermissions.Admin_Roles_Create)]
        protected virtual async Task CreateRoleAsync(CreateOrUpdateRoleInput input)
        {
            var role = new Role(AbpSession.TenantId, input.Role.DisplayName) { IsDefault = input.Role.IsDefault };
            CheckErrors(await _roleManager.CreateAsync(role));
            await CurrentUnitOfWork.SaveChangesAsync();
            await UpdateGrantedPermissionsAsync(role, input.GrantedPermissionNames);
        }

        private async Task UpdateGrantedPermissionsAsync(Role role, List<string> grantedPermissionNames)
        {
            var grantedPermissions = PermissionManager.GetPermissionsFromNamesByValidating(grantedPermissionNames);
            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
        }
    }
}