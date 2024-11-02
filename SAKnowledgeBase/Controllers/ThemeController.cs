using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAKnowledgeBase.DataBase.Entities;
using SAKnowledgeBase.Repositories.Interfaces;

namespace SAKnowledgeBase.Controllers
{
    public class ThemeController : Controller
    {
        private IRepository<Theme> _themeRepo;
        private IWebHostEnvironment _environment;
        public ThemeController(IRepository<Theme> themeRepo, IWebHostEnvironment environment)
        {
            _themeRepo = themeRepo;
            _environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            var themes = await _themeRepo.Items.OrderBy(x => x.SequenceNum).ToListAsync();
            return View(themes);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var theme = await _themeRepo.GetAsync(id);

            return View(theme);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Theme theme)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _themeRepo.AddAsync(theme);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Что-то пошло не так {ex.Message}");
                }
            }
            ModelState.AddModelError(string.Empty, $"Что-то пошло не так, недопустимая модель");

            return View(theme);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var theme = await _themeRepo.GetAsync(id);
            return View(theme);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Theme theme)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var themeToEdit = await _themeRepo.GetAsync(theme.Id);

                    if (themeToEdit != null)
                    {
                        themeToEdit.ThemeName = theme.ThemeName;
                        await _themeRepo.UpdateAsync(themeToEdit);

                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Что-то пошло не так {ex.Message}");
                }
            }

            ModelState.AddModelError(string.Empty, $"Что-то пошло не так, недопустимая модель");

            return View(theme);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var theme = await _themeRepo.GetAsync(id);
            return View(theme);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var themeToDelete = await _themeRepo.GetAsync(id);

            if (themeToDelete.Questions.Count() != 0)
            {
                return RedirectToAction("Warning", new { id = themeToDelete.Id });
            }

            await _themeRepo.RemoveAsync(themeToDelete.Id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Warning(int id)
        {
            var theme = await _themeRepo.GetAsync(id);
            return View(theme);
        }
    }
}
