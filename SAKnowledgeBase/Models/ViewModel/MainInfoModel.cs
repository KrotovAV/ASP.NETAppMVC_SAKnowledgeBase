using SAKnowledgeBase.DataBase.Entities;

namespace SAKnowledgeBase.Models.ViewModel
{
    public class MainInfoModel
    {
        public List<Theme> Themes { get; set; }
        public List<Question> Questions { get; set; }
        public List<Info> Infos { get; set; }
    }
}
