using System.Data.Common;
using System.Data.Entity;
using Abp.Zero.EntityFramework;
using Fq.Authorization.Roles;
using Fq.Authorization.Users;
using Fq.Game;
using Fq.MultiTenancy;

namespace Fq.EntityFramework
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class FqDbContext : AbpZeroDbContext<Tenant, Role, User>
    {

        public virtual IDbSet<TradingRecord> TradingRecords { get; set; }

        public virtual IDbSet<Transition> Transition { get; set; }

        public virtual IDbSet<Inventory> Inventories { get; set; }

        //TODO: Define an IDbSet for your Entities...

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public FqDbContext()
            : base("Data Source=localhost;user id=root;password=123;port=3306;Initial Catalog=fq;Character Set=utf8;Allow User Variables=True;")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in FqDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of FqDbContext since ABP automatically handles it.
         */
        public FqDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public FqDbContext(DbConnection connection)
            : base(connection, true)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //if (Database.Connection.Database.ToUpper().Contains("MySQL"))
            //{
            //    modelBuilder.Properties<string>().Configure(c => c.HasColumnType("longtext"));
            //}
        }
    }
}
