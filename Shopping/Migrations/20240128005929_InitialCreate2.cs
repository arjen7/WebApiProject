using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProducts_ProductLists_ProductListId",
                table: "UserProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProducts_Products_ProductId",
                table: "UserProducts");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "SaltValue" },
                values: new object[] { "4NjqZltEcPs802X15hIMRRuxIW8/I59K5kZp9sJP5IY=", new byte[] { 96, 237, 7, 187, 202, 38, 185, 248, 215, 102, 163, 110, 10, 111, 88, 100 } });

            migrationBuilder.AddForeignKey(
                name: "FK_UserProducts_ProductLists_ProductListId",
                table: "UserProducts",
                column: "ProductListId",
                principalTable: "ProductLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProducts_Products_ProductId",
                table: "UserProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProducts_ProductLists_ProductListId",
                table: "UserProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProducts_Products_ProductId",
                table: "UserProducts");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "SaltValue" },
                values: new object[] { "j8UhpUdlpnjBFNWc1mdgpj+6m6JzS6Jj3v466CujQCc=", new byte[] { 132, 177, 6, 146, 215, 88, 189, 93, 157, 249, 239, 225, 150, 29, 176, 134 } });

            migrationBuilder.AddForeignKey(
                name: "FK_UserProducts_ProductLists_ProductListId",
                table: "UserProducts",
                column: "ProductListId",
                principalTable: "ProductLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProducts_Products_ProductId",
                table: "UserProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
