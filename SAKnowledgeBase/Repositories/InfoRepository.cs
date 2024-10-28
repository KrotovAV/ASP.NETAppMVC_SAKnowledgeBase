using Microsoft.EntityFrameworkCore;
using SAKnowledgeBase.DataBase.Entities;
using SAKnowledgeBase.DataBase;

namespace SAKnowledgeBase.Repositories
{
    public class InfoRepository : DbRepository<Info>
    {
        public InfoRepository(AppDbContext db) : base(db) { }
        public override IQueryable<Info> Items => base.Items.Include(x => x.Question).Include(x => x.TextFormat);
    }
}
