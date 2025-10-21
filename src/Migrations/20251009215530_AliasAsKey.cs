using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.Migrations
{
    /// <inheritdoc />
    public partial class AliasAsKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(name: "PK_ShortUrls", table: "ShortUrls");

            migrationBuilder.DropIndex(name: "IX_ShortUrls_Alias", table: "ShortUrls");

            migrationBuilder.DropColumn(name: "Id", table: "ShortUrls");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShortUrls",
                table: "ShortUrls",
                column: "Alias"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(name: "PK_ShortUrls", table: "ShortUrls");

            migrationBuilder
                .AddColumn<int>(
                    name: "Id",
                    table: "ShortUrls",
                    type: "int",
                    nullable: false,
                    defaultValue: 0
                )
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(name: "PK_ShortUrls", table: "ShortUrls", column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ShortUrls_Alias",
                table: "ShortUrls",
                column: "Alias",
                unique: true
            );
        }
    }
}
