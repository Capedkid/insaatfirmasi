using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsaatFirmasi.Migrations
{
    /// <inheritdoc />
    public partial class AddSliderButtons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Button1Type",
                table: "Sliders",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Button2Type",
                table: "Sliders",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Button1Type",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "Button2Type",
                table: "Sliders");
        }
    }
}
