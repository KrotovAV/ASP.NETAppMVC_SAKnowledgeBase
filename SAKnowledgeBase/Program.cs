using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using SAKnowledgeBase.Authentication;
using SAKnowledgeBase.DataBase;
using SAKnowledgeBase.DataBase.Entities;
using SAKnowledgeBase.Models.ViewModel;
using SAKnowledgeBase.Repositories;
using SAKnowledgeBase.Repositories.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SAKnowledgeBase
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Services.AddControllersWithViews();

            builder.Services.AddSingleton<AppDbContext>();
            builder.Services.AddTransient<IRepository<Theme>, ThemeRepository>();
            builder.Services.AddTransient<IRepository<Question>, QuestionRepository>();
            builder.Services.AddTransient<IRepository<Info>, InfoRepository>(); 
            builder.Services.AddTransient<IRepository<TextFormat>, TextFormatRepository>();
            builder.Services.AddTransient<IRepository<User>, UserRepository>();
            builder.Configuration.GetConnectionString("Connection");

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // указывает, будет ли валидироваться издатель при валидации токена
                        ValidateIssuer = true,
                        // строка, представляющая издателя
                        ValidIssuer = AuthOptions.ISSUER,
                        // будет ли валидироваться потребитель токена
                        ValidateAudience = true,
                        // установка потребителя токена
                        ValidAudience = AuthOptions.AUDIENCE,
                        // будет ли валидироваться время существования
                        ValidateLifetime = true,
                        // установка ключа безопасности
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        // валидация ключа безопасности
                        ValidateIssuerSigningKey = true,
                    };
                });

            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            //{
            //    opt.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = builder.Configuration["Jwt:Issuer"],
            //        ValidAudience = builder.Configuration["Jwt:Audience"],
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            //    };
            //});




            builder.Services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
            );

            var app = builder.Build();

            
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.Map("/login/{username}", (string username) =>
            //{
            //    var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
            //    var jwt = new JwtSecurityToken(
            //            issuer: AuthOptions.ISSUER,
            //            audience: AuthOptions.AUDIENCE,
            //            claims: claims,
            //            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(60)), // время действия 60 минуты
            //            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            //    return new JwtSecurityTokenHandler().WriteToken(jwt);
            //});

            //app.Map("/login", (SignInViewModel signInViewModel) =>
            //{
            //    // находим пользователя 
            //    //Person? person = people.FirstOrDefault(p => p.Email == loginData.Email && p.Password == loginData.Password);
            //    // если пользователь не найден, отправляем статусный код 401
            //    //if (person is null) return Results.Unauthorized();

            //    var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Email) };
            //    // создаем JWT-токен
            //    var jwt = new JwtSecurityToken(
            //            issuer: AuthOptions.ISSUER,
            //            audience: AuthOptions.AUDIENCE,
            //            claims: claims,
            //            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            //            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            //    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            //    // формируем ответ
            //    var response = new
            //    {
            //        access_token = encodedJwt,
            //        username = person.Email
            //    };

            //    return Results.Json(response);
            //});



            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
