using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAKnowledgeBase.DataBase.Entities;
using SAKnowledgeBase.Models.ViewModel;
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
        public async Task<IActionResult> Create(ThemeCreateViewModel themeCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                Theme theme = new Theme
                {
                    ThemeName = themeCreateViewModel.ThemeName,
                    SequenceNum = themeCreateViewModel.SequenceNum,
                };
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

            return View(themeCreateViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var theme = await _themeRepo.GetAsync(id);

            ThemeEditViewModel themeEditViewModel = new ThemeEditViewModel
            {
                Id = theme.Id,
                ThemeName = theme.ThemeName,
                SequenceNum = theme.SequenceNum,
            };
            return View(themeEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ThemeEditViewModel themeEditViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var themeToEdit = await _themeRepo.GetAsync(themeEditViewModel.Id);

                    if (themeToEdit != null)
                    {
                        themeToEdit.Id = themeEditViewModel.Id;
                        themeToEdit.ThemeName = themeEditViewModel.ThemeName;
                        themeToEdit.SequenceNum = themeEditViewModel.SequenceNum;
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

            return View(themeEditViewModel);
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
