using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class AddedOffers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Offer",
                columns: table => new
                {
                    OfferedProductId = table.Column<int>(nullable: false),
                    ReceiverProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offer", x => new { x.OfferedProductId, x.ReceiverProductId });
                    table.ForeignKey(
                        name: "FK_Offer_Products_OfferedProductId",
                        column: x => x.OfferedProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Offer_Products_ReceiverProductId",
                        column: x => x.ReceiverProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Offer_ReceiverProductId",
                table: "Offer",
                column: "ReceiverProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Offer");
        }
    }
}
