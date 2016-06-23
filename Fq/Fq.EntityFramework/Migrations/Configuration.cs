using System.Data.Entity.Migrations;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;
using Fq.Migrations.SeedData;
using EntityFramework.DynamicFilters;

namespace Fq.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<Fq.EntityFramework.FqDbContext>, IMultiTenantSeed
    {
        public AbpTenantBase Tenant { get; set; }

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Fq";
            SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());
          
            //  SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());
            //  SetHistoryContextFactory("MySql.Data.MySqlClient", (conn, schema) => new MySql.Data.Entity.MySqlHistoryContext(conn, schema));

        }

        protected override void Seed(Fq.EntityFramework.FqDbContext context)
        {
            context.DisableAllFilters();

            if (Tenant == null)
            {
                //Host seed
                new InitialHostDbBuilder(context).Create();

                //Default tenant seed (in host database).
                //new DefaultTenantCreator(context).Create();
                //new TenantRoleAndUserBuilder(context, 1).Create();
            }
            else
            {
                //You can add seed for tenant databases and use Tenant property...
            }

            context.SaveChanges();
        }
    }
}
