using System.Data;

namespace SAKnowledgeBase.DataBase.Entities
{
    public class User : Entity
    {
        //public int Id { get; set; }

        public string Name { get; set; }

        public byte[] Password { get; set; }

        public byte[] Salt { get; set; }

        public Role Role { get; set; }

        
    }
}
