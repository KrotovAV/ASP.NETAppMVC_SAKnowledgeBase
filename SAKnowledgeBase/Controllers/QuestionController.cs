using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAKnowledgeBase.DataBase.Entities;
using SAKnowledgeBase.Repositories.Interfaces;

namespace SAKnowledgeBase.Controllers
{
    public class QuestionController : Controller
    {
        private IRepository<Question> _questionRepo;

        public QuestionController(IRepository<Question> questionRepo)
        {
            _questionRepo = questionRepo;
        }
        public async Task<IActionResult> Index()
        {
            var questions = await _questionRepo.Items
                .OrderBy(x => x.SequenceNum)
                .OrderBy(x => x.Theme.SequenceNum)
                .ToListAsync();
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
    }
}

