using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Authorization.Users;
using Abp.Extensions;
using Abp.Organizations;
using Microsoft.AspNet.Identity;

namespace Fq.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123qwe";

        public const int MinPlainPasswordLength = 6;

        public virtual bool ShouldChangePasswordOnNextLogin { get; set; }

        public virtual string Mobile { get; set; }

        public virtual string LoginCode { get; set; }

        public virtual long OrgId { get; set; }

        [ForeignKey("OrgId")]
        public virtual OrganizationUnit Organization { get; set; }

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress, string password)
        {
            return new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Password = new PasswordHasher().HashPassword(password)
            };
        }
    }
}