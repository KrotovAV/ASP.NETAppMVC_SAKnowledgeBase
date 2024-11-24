using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SAKnowledgeBase.DataBase.Entities;
using SAKnowledgeBase.Models.ViewModel;
using SAKnowledgeBase.Repositories.Interfaces;

namespace SAKnowledgeBase.Controllers
{
    [AllowAnonymous]
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
        public async Task<IActionResult> Index(string searchFor)
        {
            var mainInfos = new MainInfoModel();
            mainInfos.Themes = await _themeRepo.Items.OrderBy(x => x.SequenceNum).ToListAsync();
            mainInfos.Questions = new List<Question>();
            mainInfos.Infos = new List<Info>();

            if (searchFor == null)
            {
                foreach (var theme in mainInfos.Themes)
                {
                    List<Question> themeQuestions = new List<Question>();
                    foreach (var question in theme.Questions)
                    {
                        themeQuestions.Add(question);
                    }
                    themeQuestions.OrderBy(x => x.SequenceNum).ToList();
                    mainInfos.Questions.AddRange(themeQuestions);
                }
            }
            else
            {
                List<Question> searhQuestions = new List<Question>();

                IQueryable<Info> infos = _infoRepo.Items.Where(x => x.Text.ToLower().Contains(searchFor.ToLower())).Distinct();
                foreach (var info in infos)
                {
                    if(!searhQuestions.Contains(info.Question)) searhQuestions.Add(info.Question);
                }
                searhQuestions.OrderBy(x => x.SequenceNum).ToList();
                
                mainInfos.Questions.AddRange(searhQuestions);
            }

            foreach (var question in mainInfos.Questions)
            {
                List<Info> questionInfos = new List<Info>();
                foreach (var info in question.Infos)
                {
                    questionInfos.Add(info);
                }
                questionInfos.OrderBy(x => x.SequenceNum).ToList();
                mainInfos.Infos.AddRange(questionInfos);
            }


            return View(mainInfos);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var mainInfos = new MainInfoModel();

            mainInfos.Themes = await _themeRepo.Items.OrderBy(x => x.SequenceNum).ToListAsync();

            var theme = await _themeRepo.GetAsync(id);
            if (theme != null) {  
                mainInfos.Questions = theme.Questions.OrderBy(x => x.SequenceNum).ToList();
            }

            mainInfos.Infos = new List<Info>();
            foreach (var question in mainInfos.Questions)
            {
                List<Info> questionInfos = new List<Info>();
                foreach (var info in question.Infos)
                {
                    questionInfos.Add(info);
                }
                questionInfos.OrderBy(x => x.SequenceNum).ToList();
                mainInfos.Infos.AddRange(questionInfos);
            }

            return View(mainInfos);
        }

        [HttpGet]
        public async Task<IActionResult> Question(int id)
        {
            var mainInfos = new MainInfoModel();

            mainInfos.Themes = await _themeRepo.Items.OrderBy(x => x.SequenceNum).ToListAsync();

            var question = await _questionRepo.GetAsync(id);

            mainInfos.Questions = _questionRepo.Items.Where(x => x.Theme == question.Theme).OrderBy(x => x.SequenceNum).ToList();

            mainInfos.Infos = question.Infos.OrderBy(x => x.SequenceNum).ToList();

            return View(mainInfos);
        }

    }
}
