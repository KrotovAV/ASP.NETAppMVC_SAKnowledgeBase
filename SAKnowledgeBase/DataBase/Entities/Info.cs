namespace SAKnowledgeBase.DataBase.Entities
{
    public class Info : Entity
    {
        public string Text { get; set; }
        public int QuestionId { get; set; }// внешний ключ
        public virtual Question Question { get; set; }//навигационное свойство
        public int SequenceNum { get; set; }
        public int FormatId { get; set; }// внешний ключ
        public virtual TextFormat TextFormat { get; set; }//навигационное свойство
        public string? PhotoPath { get; set; }
        public int Level { get; set; }
        public string? Link { set; get; }
    }
}
