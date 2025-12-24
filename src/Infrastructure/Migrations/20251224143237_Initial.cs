using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.src.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShortUrls",
                columns: table => new
                {
                    Alias = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Accesses = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortUrls", x => x.Alias);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShortUrls");
        }
    }
}
