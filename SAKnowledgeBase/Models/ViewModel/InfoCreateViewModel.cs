using SAKnowledgeBase.DataBase.Entities;
using System.ComponentModel.DataAnnotations;

namespace SAKnowledgeBase.Models.ViewModel
{
    public class InfoCreateViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 3)]
        public string Text { get; set; }

        [Required]
        [Display(Name = "Theme")]
        [RegularExpression("^\\d+$", ErrorMessage = "Please select a Theme")]
        public int ThemeId { get; set; }

        [Required]
        [Display(Name = "Question")]
        [RegularExpression("^\\d+$", ErrorMessage = "Please select a Question")]
        public int QuestionId { get; set; }

        [Required]
        [Range(0, 10000000000)]
        public int SequenceNum { get; set; }
        
        [Required]
        [Range(0, 10)]
        public int FormatId { get; set; }
        
        //public string? PhotoPath { get; set; }
        public IFormFile? UploadFile { get; set; }

        [Required]
        [Range(0, 4)]
        public int Level { get; set; }
        
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 3)]
        public string? Link { set; get; }
       
    }
}
