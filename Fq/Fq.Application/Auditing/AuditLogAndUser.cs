using Abp.Auditing;
using Fq.Authorization.Users;

namespace Fq.Auditing
{
    public class AuditLogAndUser
    {
        public AuditLog AuditLog { get; set; }

        public User User { get; set; }
    }
}