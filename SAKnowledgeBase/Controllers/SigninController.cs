using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Mistake()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Check()
        {
            return View();
        }

        //[AllowAnonymous]
        [HttpPost]
        public IActionResult Check(SignInViewModel signInViewModel)
        {
        
            var user = _userRepo.Items.FirstOrDefault(x => x.Name == signInViewModel.UserName);

            var data = Encoding.ASCII.GetBytes(signInViewModel.Password).Concat(user.Salt).ToArray();
            SHA512 shaM = new SHA512Managed();
            var password = shaM.ComputeHash(data);

            //bool pas = user.Password.SequenceEqual(password);

            if (user.Password.SequenceEqual(password))
            {
                //var token = GenerateToken(user);

                //if (user is null) return Results.Unauthorized();
                List<Claim> claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                    };
                // создаем JWT-токен
                JwtSecurityToken jwt = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        claims: claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                // формируем ответ
                var response = new
                {
                    access_token = encodedJwt,
                    username = user.Name,
                    userrole = user.Role.ToString()
                };

                //return Results.Json(response);

            }
            else
            {
                return RedirectToAction("Mistake");
            }



            return RedirectToAction("Index", "Main");
            //return View();
        }


        //private string GenerateToken(User user)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        //    var claims = new[] {
        //    new Claim(ClaimTypes.NameIdentifier, user.Name),
        //    new Claim(ClaimTypes.Role, user.Role.ToString())};

        //    var token = new JwtSecurityToken(_config["Jwt:Issuer"],
        //        _config["Jwt:Audience"],
        //        claims,
        //        expires: DateTime.Now.AddMinutes(60),
        //        signingCredentials: credentials);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
    }
}
