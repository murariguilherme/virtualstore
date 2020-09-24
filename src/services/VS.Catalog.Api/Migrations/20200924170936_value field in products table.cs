using Microsoft.EntityFrameworkCore.Migrations;

namespace VS.Catalog.Api.Migrations
{
    public partial class valuefieldinproductstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "Products",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Products");
        }
    }
}
