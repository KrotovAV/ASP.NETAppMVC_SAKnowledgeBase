using Microsoft.EntityFrameworkCore;
using SAKnowledgeBase.DataBase;
using SAKnowledgeBase.DataBase.Entities;
using SAKnowledgeBase.Repositories.Interfaces;
using System;

namespace SAKnowledgeBase.Repositories
{
    public class ThemeRepository : DbRepository<Theme>
    {
        public ThemeRepository(AppDbContext db) : base(db) { }
        public override IQueryable<Theme> Items => base.Items.Include(x => x.Questions);

    }
}
