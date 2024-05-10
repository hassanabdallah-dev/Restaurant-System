using Microsoft.EntityFrameworkCore.Migrations;

namespace Phenicienn.Migrations
{
    public partial class FixOrderItemRels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersItems_Item_FK_ORDERITEMS_ITEM_CUSTOM",
                table: "OrdersItems");

            migrationBuilder.DropIndex(
                name: "IX_OrdersItems_FK_ORDERITEMS_ITEM_CUSTOM",
                table: "OrdersItems");

            migrationBuilder.DropColumn(
                name: "FK_ORDERITEMS_ITEM_CUSTOM",
                table: "OrdersItems");

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "OrdersItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrdersItems_ItemId",
                table: "OrdersItems",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersItems_Item_ItemId",
                table: "OrdersItems",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersItems_Item_ItemId",
                table: "OrdersItems");

            migrationBuilder.DropIndex(
                name: "IX_OrdersItems_ItemId",
                table: "OrdersItems");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "OrdersItems");

            migrationBuilder.AddColumn<int>(
                name: "FK_ORDERITEMS_ITEM_CUSTOM",
                table: "OrdersItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrdersItems_FK_ORDERITEMS_ITEM_CUSTOM",
                table: "OrdersItems",
                column: "FK_ORDERITEMS_ITEM_CUSTOM",
                unique: true,
                filter: "[FK_ORDERITEMS_ITEM_CUSTOM] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersItems_Item_FK_ORDERITEMS_ITEM_CUSTOM",
                table: "OrdersItems",
                column: "FK_ORDERITEMS_ITEM_CUSTOM",
                principalTable: "Item",
                principalColumn: "ItemId");
        }
    }
}
