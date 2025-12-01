using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Politico.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddArticlesModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    PriorityScore = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArticleLocales",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<long>(type: "bigint", nullable: false),
                    Culture = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortSummary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleLocales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleLocales_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleLocales_ArticleId_Culture",
                table: "ArticleLocales",
                columns: new[] { "ArticleId", "Culture" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleLocales_Culture_Slug",
                table: "ArticleLocales",
                columns: new[] { "Culture", "Slug" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Category_PublishDate",
                table: "Articles",
                columns: new[] { "Category", "PublishDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_IsActive_PublishDate",
                table: "Articles",
                columns: new[] { "IsActive", "PublishDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_IsFeatured_PriorityScore",
                table: "Articles",
                columns: new[] { "IsFeatured", "PriorityScore" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleLocales");

            migrationBuilder.DropTable(
                name: "Articles");
        }
    }
}
