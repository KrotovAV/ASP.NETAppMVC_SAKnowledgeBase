using System.ComponentModel.DataAnnotations;

namespace SAKnowledgeBase.Models.ViewModel
{
    public class ThemeEditViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Question Name")]
        [StringLength(50, MinimumLength = 3)]
        public string ThemeName { get; set; }

        [Required]
        [Display(Name = "Sequence Num")]
        [Range(0, 10000000000)]
        public int SequenceNum { get; set; }
    }
}
