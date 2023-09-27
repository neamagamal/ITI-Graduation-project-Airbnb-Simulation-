using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirBnB.Data.Migrations
{
    public partial class addamenityImgSrc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AmenityImgSrc",
                table: "Amenities",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmenityImgSrc",
                table: "Amenities");
        }
    }
}
