using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirBnB.Data.Migrations
{
    public partial class add_imgArea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AreaImg",
                table: "Areas",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaImg",
                table: "Areas");
        }
    }
}
