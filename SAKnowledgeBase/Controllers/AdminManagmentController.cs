using Microsoft.AspNetCore.Mvc;

namespace SAKnowledgeBase.Controllers
{
    public class AdminManagmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
