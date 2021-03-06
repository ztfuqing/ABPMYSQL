﻿using Fq.EntityFramework;
using EntityFramework.DynamicFilters;

namespace Fq.Migrations.SeedData
{
    public class InitialHostDbBuilder
    {
        private readonly FqDbContext _context;

        public InitialHostDbBuilder(FqDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new DefaultEditionsCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
        }
    }
}
