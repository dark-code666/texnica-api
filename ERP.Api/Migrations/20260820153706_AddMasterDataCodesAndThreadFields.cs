using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMasterDataCodesAndThreadFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Sizes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThreadQuality",
                table: "Fabrics",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThreadTitle",
                table: "Fabrics",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AlternateCode",
                table: "Colors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorCode",
                table: "Colors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Sizes");

            migrationBuilder.DropColumn(
                name: "ThreadQuality",
                table: "Fabrics");

            migrationBuilder.DropColumn(
                name: "ThreadTitle",
                table: "Fabrics");

            migrationBuilder.DropColumn(
                name: "AlternateCode",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "ColorCode",
                table: "Colors");
        }
    }
}
