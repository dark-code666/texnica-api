using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class FilterActiveFgpoLineUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FgpoLines_FgpoId_StyleId_ColorId_SizeId",
                table: "FgpoLines");

            migrationBuilder.CreateIndex(
                name: "IX_FgpoLines_FgpoId_StyleId_ColorId_SizeId",
                table: "FgpoLines",
                columns: new[] { "FgpoId", "StyleId", "ColorId", "SizeId" },
                unique: true,
                filter: "[Active] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FgpoLines_FgpoId_StyleId_ColorId_SizeId",
                table: "FgpoLines");

            migrationBuilder.CreateIndex(
                name: "IX_FgpoLines_FgpoId_StyleId_ColorId_SizeId",
                table: "FgpoLines",
                columns: new[] { "FgpoId", "StyleId", "ColorId", "SizeId" },
                unique: true);
        }
    }
}
