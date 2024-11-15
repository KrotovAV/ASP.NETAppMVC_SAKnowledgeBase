using Microsoft.Extensions.FileProviders;
using SAKnowledgeBase.DataBase;
using SAKnowledgeBase.DataBase.Entities;
using SAKnowledgeBase.Repositories;
using SAKnowledgeBase.Repositories.Interfaces;

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
            //builder.Services.AddTransient<IRepository<User>, UserRepository>();
            builder.Configuration.GetConnectionString("Connection");

            builder.Services.AddSingleton<IFileProvider>(
            new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
