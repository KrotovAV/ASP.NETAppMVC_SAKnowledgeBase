using Microsoft.EntityFrameworkCore;
using SAKnowledgeBase.DataBase.Entities;
using SAKnowledgeBase.Repositories.Interfaces;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace SAKnowledgeBase.DataBase
{
    public static class MigrationExtensions
    {
        //private static IRepository<User>? _userRepo;
        //public static MigrationExtensions(IRepository<User> userRepo)
        //{
        //    _userRepo = userRepo;
        //}

        public static void ApplyMigrations(this IApplicationBuilder app)
        //public static async Task ApplyMigrations(this IApplicationBuilder app)
        {

            //if (await _userRepo.Items.AnyAsync()) return;
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using AppDbContext appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            appDbContext.Database.Migrate();
        }

    }
}
