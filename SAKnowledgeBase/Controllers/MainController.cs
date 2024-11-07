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
            mainInfos.Themes = await _themeRepo.Items.OrderBy(x => x.SequenceNum).ToListAsync();

            //mainInfos.Questions = await _questionRepo.Items.OrderBy(x => x.SequenceNum).ToListAsync();
            //mainInfos.Questions = await _questionRepo.Items
            //.OrderBy(x => x.SequenceNum)
            //.OrderBy(x => x.Theme.SequenceNum)
            //.ToListAsync();

            mainInfos.Questions = new List<Question>();
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


            //mainInfos.Infos = await _infoRepo.Items.OrderBy(x => x.SequenceNum).ToListAsync();
            //mainInfos.Infos = await _infoRepo.Items
            //    .OrderBy(x => x.SequenceNum)
            //    .OrderBy(x => x.Question.SequenceNum)
            //    .OrderBy(x => x.Question.Theme.SequenceNum)
            //    .ToListAsync();

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
            //else
            //{
            //    mainInfos.Questions = new List<Question>();
            //    foreach (var th in mainInfos.Themes)
            //    {
            //        List<Question> themeQuestions = new List<Question>();
            //        foreach (var question in th.Questions)
            //        {
            //            themeQuestions.Add(question);
            //        }
            //        themeQuestions.OrderBy(x => x.SequenceNum).ToList();
            //        mainInfos.Questions.AddRange(themeQuestions);
            //    }
            //}

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
