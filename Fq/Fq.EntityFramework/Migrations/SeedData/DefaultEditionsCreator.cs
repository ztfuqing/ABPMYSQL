using System.Linq;
using Abp.Application.Editions;
using Fq.Editions;
using Fq.EntityFramework;

namespace Fq.Migrations.SeedData
{
    public class DefaultEditionsCreator
    {
        private readonly FqDbContext _context;

        public DefaultEditionsCreator(FqDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateEditions();
        }

        private void CreateEditions()
        {
            var defaultEdition = _context.Editions.FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
            if (defaultEdition == null)
            {
                defaultEdition = new Edition { Name = EditionManager.DefaultEditionName, DisplayName = EditionManager.DefaultEditionName };
                _context.Editions.Add(defaultEdition);
                _context.SaveChanges();

            }   
        }
    }
}