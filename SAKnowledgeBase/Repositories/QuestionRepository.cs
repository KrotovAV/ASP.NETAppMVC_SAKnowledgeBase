using Microsoft.EntityFrameworkCore;
using SAKnowledgeBase.DataBase;
using SAKnowledgeBase.DataBase.Entities;

namespace SAKnowledgeBase.Repositories
{
    public class QuestionRepository : DbRepository<Question>
    {
        public QuestionRepository(AppDbContext db) : base(db) { }
        public override IQueryable<Question> Items => base.Items.Include(x => x.Infos);
    }
}
