using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using SAKnowledgeBase.DataBase.Entities;
using System.Collections.Generic;
using static System.Collections.Specialized.BitVector32;

namespace SAKnowledgeBase.DataBase
{
    public class AppDbContext : DbContext
    {
        public DbSet<TextFormat> Formats { get; set; }
        public DbSet<Info> Infos { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<User> Users { get; set; }

        //    dotnet ef migrations add InitialMigration 
        //    dotnet ef database update

        public AppDbContext(){ }
        public AppDbContext(DbContextOptions dbc) : base(dbc) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .Build();

            optionsBuilder
                .UseLazyLoadingProxies()
                    .UseSqlServer(config.GetConnectionString("Connection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Info>().HasOne(u => u.Question).WithMany(c => c.Infos).HasForeignKey(u => u.QuestionId);
            modelBuilder.Entity<Info>().HasOne(u => u.TextFormat).WithMany(c => c.Infos).HasForeignKey(u => u.FormatId);
            modelBuilder.Entity<Question>().HasOne(u => u.Theme).WithMany(c => c.Questions).HasForeignKey(u => u.ThemeId);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            //modelBuilder.Entity<Theme>().HasData(
            //    new Theme[]{
            //        new Theme { Id=1, ThemeName="BA/SA Agile", SequenceNum = 1},
            //        new Theme { Id=2, ThemeName="Требования", SequenceNum = 2},
            //        new Theme { Id=3, ThemeName="Документация",SequenceNum = 3},
            //        new Theme { Id=4, ThemeName="Фазы проекта",SequenceNum = 4},
            //        new Theme { Id=5, ThemeName="Прототипирование",SequenceNum = 5},
            //        new Theme { Id=6, ThemeName="Моделирование",SequenceNum = 6},
            //        new Theme { Id=7, ThemeName="Базы Данных",SequenceNum = 8},
            //        new Theme { Id=8, ThemeName="Интеграции",SequenceNum = 9},
            //        new Theme { Id=9, ThemeName="Тестирование",SequenceNum = 10},
            //        new Theme { Id=10, ThemeName="SQL",SequenceNum = 11},
            //        new Theme { Id=11, ThemeName="Web сервисы / API",SequenceNum = 7}
            //    }
            //);

            //modelBuilder.Entity<Question>().HasData(
            //    new Question[]{
            //        new Question { Id=1, QuestionName="Виды требований", ThemeId = 2},
            //        new Question { Id=2, QuestionName="Эстимация", ThemeId = 2},
            //        new Question { Id=3, QuestionName="Управление требованиями", ThemeId = 2},
            //        new Question { Id=4, QuestionName="V&S", ThemeId = 3},
            //        new Question { Id=5, QuestionName="SRS", ThemeId = 3},
            //        new Question { Id=6, QuestionName="ТЗ, ЧТЗ", ThemeId = 3}
            //    }
            //);


            modelBuilder.Entity<TextFormat>().HasData(
                new TextFormat[]{
                    new TextFormat { Id=1, FormatName="Раздел",  TextSize = 7, Bold = true, Tilt = false},
                    new TextFormat { Id=2, FormatName="Вопрос",  TextSize = 6, Bold = true, Tilt = false},
                    new TextFormat { Id=3, FormatName="Подраздел",  TextSize = 5, Bold = true, Tilt = false},
                    new TextFormat { Id=4, FormatName="Текст",  TextSize = 4, Bold = false, Tilt = false},
                    new TextFormat { Id=5, FormatName="Текст жирный",  TextSize = 4, Bold = true, Tilt = false},
                    new TextFormat { Id=6, FormatName="Перечисления",  TextSize = 4, Bold = false, Tilt = false},
                    new TextFormat { Id=7, FormatName="Пояснение",  TextSize = 3, Bold = false, Tilt =true }

                }
            );

            //modelBuilder.Entity<Info>().HasData(
            //    new Info[]{
            //        new Info { Id=1, 
            //            Text = "Виды требований ",
            //            QuestionId = 1,
            //            SequenceNum = 1,
            //            FormatId = 2,
            //            Level = 1},
            //        new Info { Id=2,
            //            Text = "По Вигерсу выделяют три уровня требований:",
            //            QuestionId = 1,
            //            SequenceNum = 2,
            //            FormatId = 4,
            //            Level = 1},
            //        new Info { Id=3,
            //            Text = "1. Бизнес-требования ",
            //            QuestionId = 1,
            //            SequenceNum = 3,
            //            FormatId = 4,
            //            Level = 1},
            //        new Info { Id=4,
            //            Text = "(описывают высокоуровневые цели организации или заказчиков системы. Например, после внедрения СДБО планируется рост выручки на 20%, уменьшение операционных расходов банка на 10%) ",
            //            QuestionId = 1,
            //            SequenceNum = 4,
            //            FormatId = 4,
            //            Level = 1},
            //        new Info { Id=5,
            //            Text = "2. Требования пользователей ",
            //            QuestionId = 1,
            //            SequenceNum = 5,
            //            FormatId = 4,
            //            Level = 1},
            //        new Info { Id=6,
            //            Text = "(описывают цели и задачи пользователя, которые позволит решить система. Use cases и User Stories описываются на этом уровне. Например, возможность осуществления банковских операций удаленно) ",
            //            QuestionId = 1,
            //            SequenceNum = 6,
            //            FormatId = 4,
            //            Level = 1},
            //        new Info { Id=7,
            //            Text = "3. Функциональные требования ",
            //            QuestionId = 1,
            //            SequenceNum = 7,
            //            FormatId = 4,
            //            Level = 1},
            //        new Info { Id=8,
            //            Text = "(описывают функциональность ПО, которую разработчики должны построить, чтобы пользователи смогли выполнить свои задачи в рамках бизнес-требований, описываются в спецификации требований к ПО (SRS). Например, открытие вклада, осуществление автоплатежей.)",
            //            QuestionId = 1,
            //            SequenceNum = 8,
            //            FormatId = 4,
            //            Level = 1}

            //    }
            //);

        }

    }
}
