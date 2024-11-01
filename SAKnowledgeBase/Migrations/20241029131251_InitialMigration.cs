using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SAKnowledgeBase.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Formats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormatName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextSize = table.Column<int>(type: "int", nullable: false),
                    Bold = table.Column<bool>(type: "bit", nullable: false),
                    Tilt = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Themes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThemeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SequenceNum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SequenceNum = table.Column<int>(type: "int", nullable: false),
                    ThemeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Themes_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Infos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    SequenceNum = table.Column<int>(type: "int", nullable: false),
                    FormatId = table.Column<int>(type: "int", nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Infos_Formats_FormatId",
                        column: x => x.FormatId,
                        principalTable: "Formats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Infos_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Formats",
                columns: new[] { "Id", "Bold", "FormatName", "TextSize", "Tilt" },
                values: new object[,]
                {
                    { 1, true, "Раздел", 9, false },
                    { 2, true, "Вопрос", 8, false },
                    { 3, true, "Подраздел", 7, false },
                    { 4, false, "Текст", 6, false },
                    { 5, true, "Текст жирный", 6, false },
                    { 6, false, "Перечисления", 6, false },
                    { 7, false, "Пояснение", 5, true }
                });

            migrationBuilder.InsertData(
                table: "Themes",
                columns: new[] { "Id", "SequenceNum", "ThemeName" },
                values: new object[,]
                {
                    { 1, 1, "BA/SA Agile" },
                    { 2, 2, "Требования" },
                    { 3, 3, "Документация" },
                    { 4, 4, "Фазы проекта" },
                    { 5, 5, "Прототипирование" },
                    { 6, 6, "Моделирование" },
                    { 7, 8, "Базы Данных" },
                    { 8, 9, "Интеграции" },
                    { 9, 10, "Тестирование" },
                    { 10, 11, "SQL" },
                    { 11, 7, "Web сервисы / API" }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "QuestionName", "SequenceNum", "ThemeId" },
                values: new object[,]
                {
                    { 1, "Виды требований", 0, 2 },
                    { 2, "Эстимация", 0, 2 },
                    { 3, "Управление требованиями", 0, 2 },
                    { 4, "V&S", 0, 3 },
                    { 5, "SRS", 0, 3 },
                    { 6, "ТЗ, ЧТЗ", 0, 3 }
                });

            migrationBuilder.InsertData(
                table: "Infos",
                columns: new[] { "Id", "FormatId", "Level", "Link", "PhotoPath", "QuestionId", "SequenceNum", "Text" },
                values: new object[,]
                {
                    { 1, 2, 1, null, null, 1, 1, "Виды требований " },
                    { 2, 4, 1, null, null, 1, 2, "По Вигерсу выделяют три уровня требований:" },
                    { 3, 4, 1, null, null, 1, 3, "1. Бизнес-требования " },
                    { 4, 4, 1, null, null, 1, 4, "(описывают высокоуровневые цели организации или заказчиков системы. Например, после внедрения СДБО планируется рост выручки на 20%, уменьшение операционных расходов банка на 10%) " },
                    { 5, 4, 1, null, null, 1, 5, "2. Требования пользователей " },
                    { 6, 4, 1, null, null, 1, 6, "(описывают цели и задачи пользователя, которые позволит решить система. Use cases и User Stories описываются на этом уровне. Например, возможность осуществления банковских операций удаленно) " },
                    { 7, 4, 1, null, null, 1, 7, "3. Функциональные требования " },
                    { 8, 4, 1, null, null, 1, 8, "(описывают функциональность ПО, которую разработчики должны построить, чтобы пользователи смогли выполнить свои задачи в рамках бизнес-требований, описываются в спецификации требований к ПО (SRS). Например, открытие вклада, осуществление автоплатежей.)" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Infos_FormatId",
                table: "Infos",
                column: "FormatId");

            migrationBuilder.CreateIndex(
                name: "IX_Infos_QuestionId",
                table: "Infos",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ThemeId",
                table: "Questions",
                column: "ThemeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Infos");

            migrationBuilder.DropTable(
                name: "Formats");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Themes");
        }
    }
}
