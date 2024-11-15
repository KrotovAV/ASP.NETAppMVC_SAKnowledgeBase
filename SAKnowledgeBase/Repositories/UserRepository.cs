using SAKnowledgeBase.DataBase.Entities;
using SAKnowledgeBase.DataBase;

namespace SAKnowledgeBase.Repositories
{
    public class UserRepository : DbRepository<User>
    {
        public UserRepository(AppDbContext db) : base(db) { }
        public override IQueryable<User> Items => base.Items;
    }
}
