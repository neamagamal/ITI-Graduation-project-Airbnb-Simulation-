using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirBnB.Data.Migrations
{
    public partial class addAcceptedColInProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Accepted",
                table: "Properties",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accepted",
                table: "Properties");
        }
    }
}
