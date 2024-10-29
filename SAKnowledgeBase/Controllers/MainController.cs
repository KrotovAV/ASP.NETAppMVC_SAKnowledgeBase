using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAKnowledgeBase.DataBase.Entities;
using SAKnowledgeBase.Models.ViewModel;
using SAKnowledgeBase.Repositories.Interfaces;

namespace SAKnowledgeBase.Controllers
{
    public class MainController : Controller
    {
        private IRepository<Theme> _themeRepo;
        private IRepository<Question> _questionRepo;
        private IRepository<Info> _infoRepo;
        
        public MainController(IRepository<Theme> themeRepo, IRepository<Question> questionRepo, IRepository<Info> infoRepo)
        {
            _themeRepo = themeRepo;
            _questionRepo = questionRepo;
            _infoRepo = infoRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var mainInfos = new MainInfoModel();
            mainInfos.Themes = await _themeRepo.Items.ToListAsync();
            mainInfos.Questions = await _questionRepo.Items.ToListAsync(); 
            mainInfos.Infos = await _infoRepo.Items.ToListAsync();

            return View(mainInfos);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var mainInfos = new MainInfoModel();
            mainInfos.Themes = await _themeRepo.Items.ToListAsync();
            var theme = await _themeRepo.GetAsync(id);
            mainInfos.Questions = theme.Questions.ToList();

            mainInfos.Infos = new List<Info>();
            foreach (var question in theme.Questions)
            {
                foreach (var info in question.Infos)
                {
                    mainInfos.Infos.Add(info);
                }
            }
            return View(mainInfos);
        }

        [HttpGet]
        public async Task<IActionResult> Question(int id)
        {
            var mainInfos = new MainInfoModel();

            mainInfos.Themes = await _themeRepo.Items.ToListAsync();

                var question = await _questionRepo.GetAsync(id);
                var theme = question.Theme;

            mainInfos.Questions = theme.Questions.ToList();

            mainInfos.Infos = new List<Info>();

            foreach (var info in question.Infos)
            {
                mainInfos.Infos.Add(info);
            }

            return View(mainInfos);
        }


    }
}
