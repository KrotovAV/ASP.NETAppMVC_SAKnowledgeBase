namespace SAKnowledgeBase.DataBase.Entities
{
    public class Question : Entity
    {
       // public int Id { get; set; } 
        public string QuestionName { get; set; }
        public int ThemeId { get; set; }// внешний ключ
        public virtual Theme Theme { get; set; }//навигационное свойство
        public virtual List<Info> Infos { get; set; } = new List<Info>();
    }
}
