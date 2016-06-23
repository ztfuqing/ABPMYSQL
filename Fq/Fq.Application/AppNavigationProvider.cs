using Abp.Application.Navigation;
using Abp.Localization;
using Fq.Authorization;

namespace Fq
{
    public class AppNavigationProvider : NavigationProvider
    {
        public const string MenuName = "AdminMenu";

        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(new MenuItemDefinition(
                   "dashboard",
                   L("工作台"),
                   url: "dashboard",
                   icon: "icon-home",
                   order: 0,
                   requiredPermissionName: AppPermissions.Dashboard
                   )
               ).AddItem(new MenuItemDefinition(
                   "admin",
                   L("系统管理"),
                   icon: "icon-wrench",
                   order: 1
                   ).AddItem(new MenuItemDefinition(
                       "admin.organizationUnits",
                       L("组织机构管理"),
                       url: "organizationUnits",
                       icon: "icon-layers",
                       requiredPermissionName: AppPermissions.Admin_OrganizationUnits
                       )
                   ).AddItem(new MenuItemDefinition(
                       "admin.roles",
                       L("角色管理"),
                       url: "roles",
                       icon: "icon-briefcase",
                       requiredPermissionName: AppPermissions.Admin_Roles
                       )
                   ).AddItem(new MenuItemDefinition(
                       "admin.users",
                       L("用户管理"),
                       url: "users",
                       icon: "icon-users",
                       requiredPermissionName: AppPermissions.Admin_Users
                       )
                   ).AddItem(new MenuItemDefinition(
                       "admin.auditLogs",
                       L("审计日志"),
                       url: "auditLogs",
                       icon: "icon-lock",
                       requiredPermissionName: AppPermissions.Admin_AuditLogs
                       )
                   ).AddItem(new MenuItemDefinition(
                        "admin.settings",
                        L("系统设置"),
                        url: "settings",
                        icon: "icon-settings",
                        requiredPermissionName: AppPermissions.Admin_Settings
                   ))
               );
        }

        //private static ILocalizableString L(string name)
        //{
        //    return new LocalizableString(name, FakirConsts.LocalizationSourceName);
        //}

        private static ILocalizableString L(string name)
        {
            return new FixedLocalizableString(name);
        }
    }
}
