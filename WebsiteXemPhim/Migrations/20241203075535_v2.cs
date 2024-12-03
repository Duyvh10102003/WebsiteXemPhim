using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebsiteXemPhim.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Linkdb",
                table: "TapPhim");

            migrationBuilder.RenameColumn(
                name: "Linkdb",
                table: "PhimLe",
                newName: "Trailer");

            migrationBuilder.AddColumn<string>(
                name: "Trailer",
                table: "PhimBo",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Trailer",
                table: "PhimBo");

            migrationBuilder.RenameColumn(
                name: "Trailer",
                table: "PhimLe",
                newName: "Linkdb");

            migrationBuilder.AddColumn<string>(
                name: "Linkdb",
                table: "TapPhim",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
