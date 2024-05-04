using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETL_test.Migrations
{
    /// <inheritdoc />
    public partial class Updatebool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "store_and_fwd_flag",
                table: "ObjectModels",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "store_and_fwd_flag",
                table: "ObjectModels",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
