using Microsoft.AspNetCore.Mvc.Rendering;
using SAKnowledgeBase.DataBase.Entities;
using System.ComponentModel.DataAnnotations;

namespace SAKnowledgeBase.Models.ViewModel
{
    public class InfoViewModel
    {
        public List<Info> Infos { get; set; } = new List<Info>();
        public List<SelectListItem> ThemesSelectListItem { get; set; } = new List<SelectListItem>();
        public int? SelectedThemeId { get; set; }

    }
}
