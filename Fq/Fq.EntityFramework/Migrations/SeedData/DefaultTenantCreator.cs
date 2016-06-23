using System.Linq;
using Fq.EntityFramework;
using Fq.MultiTenancy;

namespace Fq.Migrations.SeedData
{
    public class DefaultTenantCreator
    {
        private readonly FqDbContext _context;

        public DefaultTenantCreator(FqDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateUserAndRoles();
        }

        private void CreateUserAndRoles()
        {
            //Default tenant

            var defaultTenant = _context.Tenants.FirstOrDefault(t => t.TenancyName == "Default");
            if (defaultTenant == null)
            {
                _context.Tenants.Add(new Tenant {TenancyName = "Default", Name = "Default"});
                _context.SaveChanges();
            }
        }
    }
}