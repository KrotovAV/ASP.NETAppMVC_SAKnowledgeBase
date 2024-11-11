using System.ComponentModel.DataAnnotations;

namespace SAKnowledgeBase.Models.ViewModel
{
    public class QuestionCreateViewModel
    {
        [Required]
        [Display(Name = "Question Name")]
        [StringLength(50, MinimumLength = 3)]
        public string QuestionName { get; set; }

        [Required]
        [Display(Name = "Theme")]
        [RegularExpression("^\\d+$", ErrorMessage = "Please select a Theme")]
        public int ThemeId { get; set; }

        [Required]
        [Display(Name = "Sequence Num")]
        [Range(0, 10000000000)]
        public int SequenceNum { get; set; }

    }
}
