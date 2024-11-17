using SAKnowledgeBase.DataBase.Entities;
using System.ComponentModel.DataAnnotations;

namespace SAKnowledgeBase.Models.ViewModel
{
    public class UserCreateViewModel
    {
        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Password")]
        [RegularExpression("(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9])\\S{1,16}$", ErrorMessage = "Please enter password")]
        public string Password { get; set; }
        [Required]
        public Role Role { get; set; }
    }
}
