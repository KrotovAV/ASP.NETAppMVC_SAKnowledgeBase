namespace SAKnowledgeBase.DataBase.Entities
{
    public class TextFormat : Entity
    {
        //public int Id { get; set; }
        public string FormatName { get; set; }
        public int TextSize { get; set; }
        public bool Bold { get; set; }
        public bool Tilt { get; set; }
        public virtual List<Info> Infos { get; set; } = new List<Info>();
    }
}
