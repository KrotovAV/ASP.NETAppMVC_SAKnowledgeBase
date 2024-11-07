using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAKnowledgeBase.DataBase.Entities;
using SAKnowledgeBase.Models.ViewModel;
using SAKnowledgeBase.Repositories.Interfaces;

namespace SAKnowledgeBase.Controllers
{
    public class InfoController : Controller
    {
        private IRepository<Info> _infoRepo;
        private IRepository<Question> _questionRepo;
        private IRepository<Theme> _themeRepo;
        private IRepository<TextFormat> _textFormatRepo;
        private IWebHostEnvironment _environment;

        public InfoController(
            IRepository<Info> infoRepo, 
            IRepository<Question> questionRepo, 
            IRepository<Theme> themeRepo, 
            IRepository<TextFormat> textFormatRepo, 
            IWebHostEnvironment environment)
        {
            _infoRepo = infoRepo;
            _questionRepo = questionRepo;
            _themeRepo = themeRepo;
            _textFormatRepo = textFormatRepo;
            _environment = environment;
        }
        
        public async Task<IActionResult> Index(int searchTheme, string searchFor)
        {
            InfoViewModel infoViewModel = new InfoViewModel();

            if (searchTheme != 0 & searchFor != null)
            {
                IQueryable<Info> infos = _infoRepo.Items;
                infos = infos.Where(x => x.Question.ThemeId == searchTheme);

                infos = infos.Where(ser =>ser.Text.ToLower().Contains(searchFor.ToLower()));

                infoViewModel.Infos = infos
                    .OrderBy(x => x.SequenceNum)
                    .OrderBy(x => x.Question.SequenceNum)
                    .OrderBy(x => x.Question.Theme.SequenceNum)
                    .ToList();
            }
            else if (searchTheme != 0)
            {
                IQueryable<Info> infos = _infoRepo.Items;
                infos = infos.Where(x => x.Question.ThemeId == searchTheme);
                infoViewModel.Infos = infos
                    .OrderBy(x => x.SequenceNum)
                    .OrderBy(x => x.Question.SequenceNum)
                    .OrderBy(x => x.Question.Theme.SequenceNum)
                    .ToList();
            }
            else if (searchFor != null)
            {
                IQueryable<Info> infos = _infoRepo.Items;
                infos = infos.Where(ser => ser.Text.ToLower().Contains(searchFor.ToLower()));

                infoViewModel.Infos = infos
                    .OrderBy(x => x.SequenceNum)
                    .OrderBy(x => x.Question.SequenceNum)
                    .OrderBy(x => x.Question.Theme.SequenceNum)
                    .ToList();
            }
            else
            {
                infoViewModel.Infos = await _infoRepo.Items
                   .OrderBy(x => x.SequenceNum)
                   .OrderBy(x => x.Question.SequenceNum)
                   .OrderBy(x => x.Question.Theme.SequenceNum)
                   .ToListAsync();
            }
            

            //var themesData = await _themeRepo.Items.OrderBy(x => x.SequenceNum).ToListAsync();
            //infoViewModel.ThemesItems = themesData
            //         .Select(i => new SelectListItem
            //         {
            //             Value = i.Id.ToString(),
            //             Text = i.ThemeName
            //         }).ToList();

            //IQueryable<Question> questions = _questionRepo.Items;
            //if (searchTheme != null)
            //{
            //    questions = questions.Where(x => x.ThemeId == searchTheme);
            //}

            //var questionsData = questions.OrderBy(x => x.SequenceNum).ToList();

            //infoViewModel.QuestionsItems = questionsData
            //         .Select(i => new SelectListItem
            //         {
            //             Value = i.Id.ToString(),
            //             Text = i.QuestionName
            //         }).ToList();

            await LoadDropdownListIndex();
            return View(infoViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var info = await _infoRepo.GetAsync(id);

            return View(info);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDropdownList();
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(InfoCreateViewModel infoCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                Info info = new Info //Text, QuestionId, SequenceNum, FormatId, ? PhotoPath, Level, ? Link 
                {
                    Text = infoCreateViewModel.Text,
                    QuestionId = infoCreateViewModel.QuestionId,
                    SequenceNum = infoCreateViewModel.SequenceNum,
                    FormatId = infoCreateViewModel.FormatId,
                    Level = infoCreateViewModel.Level,
                    Link = infoCreateViewModel.Link ?? null
                };
                if (infoCreateViewModel.UploadFile != null)
                {
                    info.PhotoPath = UploadFile(infoCreateViewModel.UploadFile);
                }
                try
                {
                    await _infoRepo.AddAsync(info);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Что-то пошло не так {ex.Message}");
                }
            }
            ModelState.AddModelError(string.Empty, $"Что-то пошло не так, недопустимая модель");
            await LoadDropdownList();
            return View(infoCreateViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var info = await _infoRepo.GetAsync(id);
            var themeId = info.Question.ThemeId;
            await LoadDropdownList();
            return View(info);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Info info)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var infoToEdit = await _infoRepo.GetAsync(info.Id);

                    if (infoToEdit != null)
                    {
                        infoToEdit.Text = info.Text;
                        infoToEdit.QuestionId = info.QuestionId;
                        infoToEdit.SequenceNum = info.SequenceNum;
                        infoToEdit.FormatId = info.FormatId;
                        infoToEdit.PhotoPath = info.PhotoPath;
                        infoToEdit.Level = info.Level;
                        infoToEdit.Link = info.Link;
                        await _infoRepo.UpdateAsync(infoToEdit);

                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Что-то пошло не так {ex.Message}");
                }
            }

            ModelState.AddModelError(string.Empty, $"Что-то пошло не так, недопустимая модель");

            return View(info);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var info = await _infoRepo.GetAsync(id);
            return View(info);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var infoToDelete = await _infoRepo.GetAsync(id);

            if (infoToDelete.QuestionId != 0) //РАЗОБРАТЬСЯ для чего это
            {
                return RedirectToAction("Warning", new { id = infoToDelete.Id });
            }

            await _infoRepo.RemoveAsync(infoToDelete.Id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Warning(int id)
        {
            var info = await _infoRepo.GetAsync(id);
            return View(info);
        }


        private string UploadFile(IFormFile formFile)
        {
            
            string TargetPath = Path.Combine(_environment.WebRootPath, "img", formFile.FileName);
            using (var stream = new FileStream(TargetPath, FileMode.Create))
            {
                formFile.CopyTo(stream);
            }
            return formFile.FileName;
        }

        private async Task LoadDropdownListIndex()
        {
            var themesData = await _themeRepo.Items.OrderBy(x => x.SequenceNum).ToListAsync();
            ViewBag.Themes = themesData
                     .Select(i => new SelectListItem
                     {
                         Value = i.Id.ToString(),
                         Text = i.ThemeName
                     }).ToList();
        }
        private async Task LoadDropdownList()
        {
            var themesData = await _themeRepo.Items.OrderBy(x => x.SequenceNum).ToListAsync();
            ViewBag.Themes = themesData
                     .Select(i => new SelectListItem
                     {
                         Value = i.Id.ToString(),
                         Text = i.ThemeName
                     }).ToList();


            //themeId = 2;
            var questionsData = await _questionRepo.Items.OrderBy(x => x.SequenceNum).ToListAsync();
            //var questionsData = await _questionRepo.Items.Where(x => x.ThemeId == themeId).OrderBy(x => x.SequenceNum).ToListAsync();
            ViewBag.Questions = questionsData
                     .Select(i => new SelectListItem
                     {
                         Value = i.Id.ToString(),
                         Text = i.QuestionName
                     }).ToList();

            var textFormatsData = await _textFormatRepo.Items.ToListAsync();
            ViewBag.TextFormats = textFormatsData
                     .Select(i => new SelectListItem
                     {
                         Value = i.Id.ToString(),
                         Text = i.FormatName
                     }).ToList();

            
        //Level
        //ViewBag.PriorityTypesRadio = Enum.GetValues(typeof(PriorityType)).Cast<PriorityType>().ToArray();
        }
    }
}
