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
                name: "TextFormats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormatName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextSize = table.Column<int>(type: "int", nullable: false),
                    Boid = table.Column<bool>(type: "bit", nullable: false),
                    Tilt = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextFormats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThemeSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThemeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThemeSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThemeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_ThemeSections_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "ThemeSections",
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
                        name: "FK_Infos_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Infos_TextFormats_FormatId",
                        column: x => x.FormatId,
                        principalTable: "TextFormats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TextFormats",
                columns: new[] { "Id", "Boid", "FormatName", "TextSize", "Tilt" },
                values: new object[,]
                {
                    { 1, true, "Раздел", 20, false },
                    { 2, true, "Вопрос", 16, false },
                    { 3, true, "Подраздел", 14, false },
                    { 4, false, "Текст", 12, false },
                    { 5, true, "Текст жирный", 12, false },
                    { 6, false, "Перечисления", 12, false },
                    { 7, false, "Пояснение", 10, true }
                });

            migrationBuilder.InsertData(
                table: "ThemeSections",
                columns: new[] { "Id", "ThemeName" },
                values: new object[,]
                {
                    { 1, "Общее" },
                    { 2, "Требования" },
                    { 3, "Документация" }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "QuestionName", "ThemeId" },
                values: new object[,]
                {
                    { 1, "Виды требований", 2 },
                    { 2, "Эстимация", 2 },
                    { 3, "Управление требованиями", 2 },
                    { 4, "V&S", 3 },
                    { 5, "SRS", 3 },
                    { 6, "ТЗ, ЧТЗ", 3 }
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
                name: "Questions");

            migrationBuilder.DropTable(
                name: "TextFormats");

            migrationBuilder.DropTable(
                name: "ThemeSections");
        }
    }
}
