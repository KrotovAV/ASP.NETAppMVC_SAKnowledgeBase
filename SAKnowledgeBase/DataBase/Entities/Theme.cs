namespace SAKnowledgeBase.DataBase.Entities
{
    public class Theme : Entity
    {
        //public int Id { get; set; }
        public string ThemeName { get; set; }
        public int SequenceNum { get; set; }
        public virtual List<Question> Questions { get; set; } = new List<Question>();
    }
}

