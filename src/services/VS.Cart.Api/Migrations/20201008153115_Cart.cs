using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VS.Cart.Api.Migrations
{
    public partial class Cart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerCarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerCarts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerCartProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true),
                    Image = table.Column<string>(type: "varchar(100)", nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    CustomerCartId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerCartProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerCartProducts_CustomerCarts_Id",
                        column: x => x.Id,
                        principalTable: "CustomerCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_Customer",
                table: "CustomerCarts",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerCartProducts");

            migrationBuilder.DropTable(
                name: "CustomerCarts");
        }
    }
}
