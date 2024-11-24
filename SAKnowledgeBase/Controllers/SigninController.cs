using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using SAKnowledgeBase.Authentication;
using SAKnowledgeBase.DataBase.Entities;
using SAKnowledgeBase.Models.ViewModel;
using SAKnowledgeBase.Repositories;
using SAKnowledgeBase.Repositories.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SAKnowledgeBase.Controllers
{
    public class SigninController : Controller
    {
        private readonly  IRepository<User> _userRepo;
        private readonly IConfiguration _config;

        public SigninController(IRepository<User> userRepo, IConfiguration config) 
        {
            _userRepo = userRepo;
            _config = config;
            
        }

        public IActionResult IndexMistake()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Check(SignInViewModel signInViewModel)
        {
        
            var user = _userRepo.Items.FirstOrDefault(x => x.Name == signInViewModel.UserName);
            if (user == null)
            {
                return RedirectToAction("IndexMistake", "Signin");
            }
            var data = Encoding.ASCII.GetBytes(signInViewModel.Password).Concat(user.Salt).ToArray();
            SHA512 shaM = new SHA512Managed();
            var password = shaM.ComputeHash(data);

            if (user.Password.SequenceEqual(password))
            {
                // создаем список с клаймами
                List<Claim> claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                    };

                // создаем JWT-токен
                JwtSecurityToken jwtToken= new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        claims: claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(240)), // время жизни токена 4 часа
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);

                // добавляем токен в куки
                var context = ControllerContext.HttpContext;

                context.Response.Cookies.Append("tasty-cookies", encodedJwt);
                //context.Response.Cookies.Append("r-o", user.Name);
                //context.Response.Cookies.Append("n-a", user.Role.ToString());

            }
            else
            {
                //return RedirectToAction("Index", "Main");
                return RedirectToAction("IndexMistake", "Signin");
            }

            return RedirectToAction("Index", "Info");
        }

        public async Task<IActionResult> Logout()
        {
            var context = ControllerContext.HttpContext;

            //context.Cookies.Delete["tasty-cookies"];

           // Request.Cookies
            //await HttpContext.SignOutAsync();
            if (context.Request.Cookies.ContainsKey("tasty-cookies"))
            {
                context.Response.Cookies.Delete("tasty-cookies");
            }

            return RedirectToAction("Index", "Main");
        }

    }
}
