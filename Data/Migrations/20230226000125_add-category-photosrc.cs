using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirBnB.Data.Migrations
{
    public partial class addcategoryphotosrc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Categoryphotosrc",
                table: "Categoraies",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categoryphotosrc",
                table: "Categoraies");
        }
    }
}
