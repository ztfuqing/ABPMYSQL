using Abp.Authorization;
using Abp.Localization;

namespace Fq.Authorization
{
    public class FqAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var dashboard = context.GetPermissionOrNull(AppPermissions.Dashboard);
            if (dashboard == null)
            {
                dashboard = context.CreatePermission(AppPermissions.Dashboard, L("工作台"));
            }

            var system = context.GetPermissionOrNull(AppPermissions.Admin_System);

            if (system == null)
            {
                system = context.CreatePermission(AppPermissions.Admin_System, L("系统管理"));
            }

            var organizationUnits = system.CreateChildPermission(AppPermissions.Admin_OrganizationUnits, L("组织机构管理"));

            var roles = system.CreateChildPermission(AppPermissions.Admin_Roles, L("角色管理"));
            roles.CreateChildPermission(AppPermissions.Admin_Roles_Create, L("创建角色"));
            roles.CreateChildPermission(AppPermissions.Admin_Roles_Delete, L("删除角色"));
            roles.CreateChildPermission(AppPermissions.Admin_Roles_Edit, L("编辑角色"));

            var users = system.CreateChildPermission(AppPermissions.Admin_Users, L("用户管理"));
            users.CreateChildPermission(AppPermissions.Admin_Users_Create, L("创建用户"));
            users.CreateChildPermission(AppPermissions.Admin_Users_Delete, L("删除用户"));
            users.CreateChildPermission(AppPermissions.Admin_Users_Edit, L("编辑用户"));
            users.CreateChildPermission(AppPermissions.Admin_Users_ChangePermissions, L("用户授权"));

            var auditLogs = system.CreateChildPermission(AppPermissions.Admin_AuditLogs, L("日志管理"));
            var settings = system.CreateChildPermission(AppPermissions.Admin_Settings, L("系统设置"));
        }

        private static ILocalizableString L(string name)
        {
            return new FixedLocalizableString(name);
        }
    }
}
