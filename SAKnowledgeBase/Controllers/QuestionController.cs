using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAKnowledgeBase.DataBase.Entities;
using SAKnowledgeBase.Models.ViewModel;
using SAKnowledgeBase.Repositories.Interfaces;

namespace SAKnowledgeBase.Controllers
{
    public class QuestionController : Controller
    {
        private IRepository<Question> _questionRepo;
        private IRepository<Theme> _themeRepo;

        public QuestionController(IRepository<Question> questionRepo, IRepository<Theme> themeRepo)
        {
            _questionRepo = questionRepo;
            _themeRepo = themeRepo;
        }
        public async Task<IActionResult> Index(int searchTheme, string searchFor)
        {
            List<Question> questions = new List<Question>();
            if (searchTheme != 0 & searchFor != null)
            {
                IQueryable<Question> questionsData = _questionRepo.Items;

                questionsData = questionsData.Where(x => x.ThemeId == searchTheme);

                questionsData = questionsData.Where(x => x.QuestionName.ToLower().Contains(searchFor.ToLower()));

                questions = questionsData
                    .OrderBy(x => x.SequenceNum)
                    .OrderBy(x => x.Theme.SequenceNum)
                    .ToList();
            }
            else if (searchTheme != 0)
            {
                IQueryable<Question> questionsData = _questionRepo.Items;

                questionsData = questionsData.Where(x => x.ThemeId == searchTheme);

                questions = questionsData
                   .OrderBy(x => x.SequenceNum)
                   .OrderBy(x => x.Theme.SequenceNum)
                   .ToList();
            }
            else if (searchFor != null)
            {
                IQueryable<Question> questionsData = _questionRepo.Items;

                questionsData = questionsData.Where(x => x.QuestionName.ToLower().Contains(searchFor.ToLower()));

                questions = questionsData
                   .OrderBy(x => x.SequenceNum)
                   .OrderBy(x => x.Theme.SequenceNum)
                   .ToList();
            }
            else
            {
                questions = await _questionRepo.Items
                    .OrderBy(x => x.SequenceNum)
                    .OrderBy(x => x.Theme.SequenceNum)
                    .ToListAsync();
            }

            await LoadDropdownListIndex();
            return View(questions);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var question = await _questionRepo.GetAsync(id);

            return View(question);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Question question)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _questionRepo.AddAsync(question);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Что-то пошло не так {ex.Message}");
                }
            }
            ModelState.AddModelError(string.Empty, $"Что-то пошло не так, недопустимая модель");

            return View(question);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var question = await _questionRepo.GetAsync(id);
            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Question question)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var questionToEdit = await _questionRepo.GetAsync(question.Id);

                    if (questionToEdit != null)
                    {
                        questionToEdit.QuestionName = question.QuestionName;
                        await _questionRepo.UpdateAsync(questionToEdit);

                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Что-то пошло не так {ex.Message}");
                }
            }

            ModelState.AddModelError(string.Empty, $"Что-то пошло не так, недопустимая модель");

            return View(question);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var question = await _questionRepo.GetAsync(id);
            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var questionToDelete = await _questionRepo.GetAsync(id);

            if (questionToDelete.Infos.Count() != 0)
            {
                return RedirectToAction("Warning", new { id = questionToDelete.Id });
            }

            await _questionRepo.RemoveAsync(questionToDelete.Id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Warning(int id)
        {
            var question = await _questionRepo.GetAsync(id);
            return View(question);
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
    }
}

