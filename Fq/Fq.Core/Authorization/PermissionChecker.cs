using Abp.Authorization;
using Fq.Authorization.Roles;
using Fq.Authorization.Users;
using Fq.MultiTenancy;

namespace Fq.Authorization
{
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
