using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirBnB.Data.Migrations
{
    public partial class addCityImg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CityImg",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityImg",
                table: "Cities");
        }
    }
}
