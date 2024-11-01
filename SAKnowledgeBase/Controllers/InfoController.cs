using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAKnowledgeBase.DataBase.Entities;
using SAKnowledgeBase.Repositories.Interfaces;

namespace SAKnowledgeBase.Controllers
{
    public class InfoController : Controller
    {
        private IRepository<Info> _infoRepo;

        public InfoController(IRepository<Info> infoRepo)
        {
            _infoRepo = infoRepo;
        }
        public async Task<IActionResult> Index()
        {
            var infos = await _infoRepo.Items.OrderBy(x => x.SequenceNum).ToListAsync();
            return View(infos);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var info = await _infoRepo.GetAsync(id);

            return View(info);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Info info)
        {
            if (ModelState.IsValid)
            {
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

            return View(info);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var info = await _infoRepo.GetAsync(id);
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
    }
}
