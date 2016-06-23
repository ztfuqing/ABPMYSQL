using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Notifications;
using Abp.Organizations;
using Abp.Runtime.Session;
using Abp.UI;
using Fq.Authorization.Roles;
using Fq.Authorization.Users.Dto;
using Fq.Notifications;
using Microsoft.AspNet.Identity;

namespace Fq.Authorization.Users
{
    [AbpAuthorize(AppPermissions.Admin_Users)]
    public class UserAppService : FqAppServiceBase, IUserAppService
    {
        private readonly RoleManager _roleManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly IPermissionManager _permissionManager;
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IAppNotifier _appNotifier;

        public UserAppService(RoleManager roleManager, IRepository<User, long> userRepository,
            OrganizationUnitManager organizationUnitManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IPermissionManager permissionManager,
            INotificationSubscriptionManager notificationSubscriptionManager,
            IAppNotifier appNotifier)
        {
            _roleManager = roleManager;
            _userRepository = userRepository;
            _permissionManager = permissionManager;
            _organizationUnitManager = organizationUnitManager;
            _organizationUnitRepository = organizationUnitRepository;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _appNotifier = appNotifier;
        }


        public async Task RemoveFromRole(long userId, string roleName)
        {
            CheckErrors(await UserManager.RemoveFromRoleAsync(userId, roleName));
        }

        public async Task<PagedResultOutput<UserListDto>> GetUsers(GetUsersInput input)
        {
            var query = UserManager.Users
                           .Include(u => u.Roles)
                           .WhereIf(
                               !input.Filter.IsNullOrWhiteSpace(),
                               u =>
                                   u.Surname.Contains(input.Filter) ||
                                   u.UserName.Contains(input.Filter)
                           );

            var userCount = await query.CountAsync();
            var users = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var userListDtos = users.MapTo<List<UserListDto>>();

            return new PagedResultOutput<UserListDto>(
                userCount,
                userListDtos
                );
        }


        public async Task UpdateUserPermissions(UpdateUserPermissionsInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            var grantedPermissions = PermissionManager.GetPermissionsFromNamesByValidating(input.GrantedPermissionNames);
            await UserManager.SetGrantedPermissionsAsync(user, grantedPermissions);
        }
        [AbpAuthorize(AppPermissions.Admin_Users_Create, AppPermissions.Admin_Users_Edit)]
        public async Task<GetUserForEditOutput> GetUserForEdit(NullableIdInput<long> input)
        {
            var userRoleDtos = (await _roleManager.Roles
               .OrderBy(r => r.DisplayName)
               .Select(r => new UserRoleDto
               {
                   RoleId = r.Id,
                   RoleName = r.Name,
                   RoleDisplayName = r.DisplayName
               })
               .ToArrayAsync());

            var output = new GetUserForEditOutput
            {
                Roles = userRoleDtos
            };

            if (!input.Id.HasValue)
            {
                output.User = new UserEditDto { IsActive = true, ShouldChangePasswordOnNextLogin = true };

                output.Organizations = (await _organizationUnitRepository.GetAllListAsync()).Select(a => new ComboboxItemDto
                {
                    DisplayText = a.DisplayName,
                    Value = a.Id.ToString()
                }).ToArray();

                foreach (var defaultRole in await _roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
                {
                    var defaultUserRole = userRoleDtos.FirstOrDefault(ur => ur.RoleName == defaultRole.Name);
                    if (defaultUserRole != null)
                    {
                        defaultUserRole.IsAssigned = true;
                    }
                }
            }
            else
            {
                var user = await UserManager.GetUserByIdAsync(input.Id.Value);
                output.Organizations = (await _organizationUnitRepository.GetAllListAsync()).Select(a => new ComboboxItemDto
                {
                    DisplayText = a.DisplayName,
                    Value = a.Id.ToString(),
                    IsSelected = a.Id == user.OrgId
                }).ToArray();
                output.User = user.MapTo<UserEditDto>();
                //output.ProfilePictureId = user.ProfilePictureId;

                foreach (var userRoleDto in userRoleDtos)
                {
                    userRoleDto.IsAssigned = await UserManager.IsInRoleAsync(input.Id.Value, userRoleDto.RoleName);
                }
            }

            //await _appNotifier.SendMessageToAllUser(AbpSession.TenantId, "test", "测试群发信息");

            return output;
        }

        public async Task CreateOrUpdateUser(CreateOrUpdateUserInput input)
        {
            if (input.User.Id.HasValue)
            {
                await UpdateUserAsync(input);
            }
            else
            {
                await CreateUserAsync(input);
            }
        }

        public async Task DeleteUser(IdInput<long> input)
        {
            if (input.Id == AbpSession.GetUserId())
            {
                throw new UserFriendlyException("不能删除自己");
            }

            var user = await UserManager.GetUserByIdAsync(input.Id);
            CheckErrors(await UserManager.DeleteAsync(user));
        }

        [AbpAuthorize(AppPermissions.Admin_Users_Edit)]
        protected virtual async Task UpdateUserAsync(CreateOrUpdateUserInput input)
        {
            // Debug.Assert(input.User.Id != null, "input.User.Id should be set.");

            var user = await UserManager.FindByIdAsync(input.User.Id.Value);

            input.User.MapTo(user);
            user.Name = user.Surname;
            if (!input.User.Password.IsNullOrEmpty())
            {
                CheckErrors(await UserManager.ChangePasswordAsync(user, input.User.Password));
            }

            CheckErrors(await UserManager.UpdateAsync(user));

            CheckErrors(await UserManager.SetRoles(user, input.AssignedRoleNames));
        }

        [AbpAuthorize(AppPermissions.Admin_Users_Create)]
        protected virtual async Task CreateUserAsync(CreateOrUpdateUserInput input)
        {
            var user = input.User.MapTo<User>();
            user.TenantId = AbpSession.TenantId;
            user.Name = user.Surname;
            user.IsEmailConfirmed = true;
            if (!input.User.Password.IsNullOrEmpty())
            {
                CheckErrors(await UserManager.PasswordValidator.ValidateAsync(input.User.Password));
            }
            else
            {
                input.User.Password = User.DefaultPassword;// User.CreateRandomPassword();
            }
            user.Password = new PasswordHasher().HashPassword(input.User.Password);
            user.ShouldChangePasswordOnNextLogin = input.User.ShouldChangePasswordOnNextLogin;

            user.Roles = new Collection<UserRole>();
            foreach (var roleName in input.AssignedRoleNames)
            {
                var role = await _roleManager.GetRoleByNameAsync(roleName);
                user.Roles.Add(new UserRole { RoleId = role.Id });
            }

            CheckErrors(await UserManager.CreateAsync(user));
            await CurrentUnitOfWork.SaveChangesAsync();

            //await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());
            await _appNotifier.WelcomeToTheApplicationAsync(user);
        }

    }
}