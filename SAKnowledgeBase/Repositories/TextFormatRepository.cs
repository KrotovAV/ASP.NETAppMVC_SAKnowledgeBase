using SAKnowledgeBase.DataBase.Entities;
using SAKnowledgeBase.DataBase;

namespace SAKnowledgeBase.Repositories
{
    public class TextFormatRepository : DbRepository<TextFormat>
    {
        public TextFormatRepository(AppDbContext db) : base(db) { }
        public override IQueryable<TextFormat> Items => base.Items;
    }
}
