using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsaatFirmasi.Migrations
{
    /// <inheritdoc />
    public partial class AddKickerToSlider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Kicker",
                table: "Sliders",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kicker",
                table: "Sliders");
        }
    }
}
