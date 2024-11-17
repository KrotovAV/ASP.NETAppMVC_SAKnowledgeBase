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
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Salt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
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
                    { 1, true, "Раздел", 7, false },
                    { 2, true, "Вопрос", 6, false },
                    { 3, true, "Подраздел", 5, false },
                    { 4, false, "Текст", 4, false },
                    { 5, true, "Текст жирный", 4, false },
                    { 6, false, "Перечисления", 4, false },
                    { 7, false, "Пояснение", 3, true }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "Password", "Role", "Salt" },
                values: new object[] { 1, "admin@ad.min", new byte[] { 90, 134, 225, 217, 40, 237, 44, 134, 95, 104, 34, 83, 248, 217, 204, 173, 235, 111, 189, 196, 96, 130, 24, 13, 214, 172, 39, 103, 141, 240, 177, 244, 41, 252, 47, 217, 75, 241, 148, 60, 38, 241, 245, 167, 71, 234, 107, 191, 237, 61, 38, 67, 238, 44, 67, 109, 68, 232, 34, 163, 134, 88, 159, 125 }, 0, new byte[] { 37, 6, 204, 204, 32, 31, 160, 71, 178, 188, 87, 118, 38, 90, 251, 5 } });

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

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                table: "Users",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Infos");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Formats");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Themes");
        }
    }
}
