using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class MakeInlineQualityResultComputed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Result",
                table: "InlineQualities",
                type: "nvarchar(450)",
                nullable: true,
                computedColumnSql: "CAST((CASE WHEN [CheckedQty] = 0 THEN 'Pending' WHEN ([CriticalDefects] + [MajorDefects] + [MinorDefects]) / CAST([CheckedQty] AS decimal(18,4)) * 100 > [MaxAllowed] THEN 'Failed' ELSE 'Passed' END) AS nvarchar(50))",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Result",
                table: "InlineQualities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true,
                oldComputedColumnSql: "CAST((CASE WHEN [CheckedQty] = 0 THEN 'Pending' WHEN ([CriticalDefects] + [MajorDefects] + [MinorDefects]) / CAST([CheckedQty] AS decimal(18,4)) * 100 > [MaxAllowed] THEN 'Failed' ELSE 'Passed' END) AS nvarchar(50))");
        }
    }
}
