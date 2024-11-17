using SAKnowledgeBase.DataBase.Entities;
using System.ComponentModel.DataAnnotations;

namespace SAKnowledgeBase.Models.ViewModel
{
    public class UserEditViewModel
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string Name { get; set; }


        [Display(Name = "NewPassword")]
        [RegularExpression("(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9])\\S{1,16}$", ErrorMessage = "Please enter new password")]
        public string? NewPassword { get; set; } = null;

        public Role Role { get; set; }
    }
}
