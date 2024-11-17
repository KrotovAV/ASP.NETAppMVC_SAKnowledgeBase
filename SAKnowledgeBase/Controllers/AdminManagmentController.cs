using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAKnowledgeBase.DataBase.Entities;
using SAKnowledgeBase.Models.ViewModel;
using SAKnowledgeBase.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SAKnowledgeBase.Controllers
{

    [Authorize]
    public class AdminManagmentController : Controller
    {
        private IRepository<User> _userRepo;
        public AdminManagmentController(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        //[Authorize]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepo.Items.ToListAsync();
            return View(users);
        }

        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            await LoadDropdownList();
            return View();
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(UserCreateViewModel userCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User();
                user.Name = userCreateViewModel.Name;
                user.Role = userCreateViewModel.Role;    

                user.Salt = new byte[16];

                new Random().NextBytes(user.Salt);
                var data = Encoding.ASCII.GetBytes(userCreateViewModel.Password).Concat(user.Salt).ToArray();
                SHA512 shaM = new SHA512Managed();
                user.Password = shaM.ComputeHash(data);

                try
                {
                    await _userRepo.AddAsync(user);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Что-то пошло не так {ex.Message}");
                }
            }
            ModelState.AddModelError(string.Empty, $"Что-то пошло не так, недопустимая модель");
            await LoadDropdownList();
            return View(userCreateViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userRepo.GetAsync(id);

            UserEditViewModel userEditViewModel = new UserEditViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Role = user.Role
            };

            await LoadDropdownList();
            return View(userEditViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel userEditViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userToEdit = await _userRepo.GetAsync(userEditViewModel.Id);

                    if (userToEdit != null)
                    {
                        userToEdit.Id = userEditViewModel.Id;
                        userToEdit.Name = userEditViewModel.Name;
                        userToEdit.Role = userEditViewModel.Role;

                        if (userEditViewModel.NewPassword != null)
                        {
                            userToEdit.Salt = new byte[16];

                            new Random().NextBytes(userToEdit.Salt);
                            var data = Encoding.ASCII.GetBytes(userEditViewModel.NewPassword).Concat(userToEdit.Salt).ToArray();
                            SHA512 shaM = new SHA512Managed();
                            userToEdit.Password = shaM.ComputeHash(data);
                        }
                        await _userRepo.UpdateAsync(userToEdit);

                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Что-то пошло не так {ex.Message}");
                }
            }

            ModelState.AddModelError(string.Empty, $"Что-то пошло не так, недопустимая модель");
            await LoadDropdownList();
            return View(userEditViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepo.GetAsync(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var infoToDelete = await _userRepo.GetAsync(id);

            await _userRepo.RemoveAsync(infoToDelete.Id);

            return RedirectToAction("Index");
        }

        private async Task LoadDropdownList()
        {
            
            ViewBag.RoleTypesRadio = Enum.GetValues(typeof(Role)).Cast<Role>().ToArray();
        }
    }
}
