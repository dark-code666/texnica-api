using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class FabricPOFgpoEntityRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FabricPOFgpos",
                table: "FabricPOFgpos");

            migrationBuilder.DropColumn(
                name: "DataOwner",
                table: "FabricPOs");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "FabricPOFgpos",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FabricPOFgpos",
                table: "FabricPOFgpos",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_FabricPOFgpos_FabricPOId_FGPOId",
                table: "FabricPOFgpos",
                columns: new[] { "FabricPOId", "FGPOId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FabricPOFgpos",
                table: "FabricPOFgpos");

            migrationBuilder.DropIndex(
                name: "IX_FabricPOFgpos_FabricPOId_FGPOId",
                table: "FabricPOFgpos");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "FabricPOFgpos");

            migrationBuilder.AddColumn<string>(
                name: "DataOwner",
                table: "FabricPOs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FabricPOFgpos",
                table: "FabricPOFgpos",
                columns: new[] { "FabricPOId", "FGPOId" });
        }
    }
}
