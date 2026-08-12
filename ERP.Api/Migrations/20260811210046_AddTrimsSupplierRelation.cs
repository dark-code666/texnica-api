using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTrimsSupplierRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "TrimsControls");

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "TrimsControls",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrimsControls_SupplierId",
                table: "TrimsControls",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrimsControls_Suppliers_SupplierId",
                table: "TrimsControls",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrimsControls_Suppliers_SupplierId",
                table: "TrimsControls");

            migrationBuilder.DropIndex(
                name: "IX_TrimsControls_SupplierId",
                table: "TrimsControls");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "TrimsControls");

            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "TrimsControls",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
