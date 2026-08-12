using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSewingSizeAndSupervisorRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "SewingProductions");

            migrationBuilder.DropColumn(
                name: "Supervisor",
                table: "SewingProductions");

            migrationBuilder.AddColumn<int>(
                name: "SizeId",
                table: "SewingProductions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SupervisorId",
                table: "SewingProductions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SewingProductions_SizeId",
                table: "SewingProductions",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_SewingProductions_SupervisorId",
                table: "SewingProductions",
                column: "SupervisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewingProductions_Sizes_SizeId",
                table: "SewingProductions",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewingProductions_Users_SupervisorId",
                table: "SewingProductions",
                column: "SupervisorId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewingProductions_Sizes_SizeId",
                table: "SewingProductions");

            migrationBuilder.DropForeignKey(
                name: "FK_SewingProductions_Users_SupervisorId",
                table: "SewingProductions");

            migrationBuilder.DropIndex(
                name: "IX_SewingProductions_SizeId",
                table: "SewingProductions");

            migrationBuilder.DropIndex(
                name: "IX_SewingProductions_SupervisorId",
                table: "SewingProductions");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "SewingProductions");

            migrationBuilder.DropColumn(
                name: "SupervisorId",
                table: "SewingProductions");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "SewingProductions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Supervisor",
                table: "SewingProductions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
