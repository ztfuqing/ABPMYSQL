using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace Fq.EntityFramework.Repositories
{
    public abstract class FqRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<FqDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected FqRepositoryBase(IDbContextProvider<FqDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class FqRepositoryBase<TEntity> : FqRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected FqRepositoryBase(IDbContextProvider<FqDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
