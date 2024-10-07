using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaiKiemTra03_02.Data.Migrations
{
    /// <inheritdoc />
    public partial class bkt2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Sach",
                newName: "AuthorId1");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Sach",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sach_AuthorId1",
                table: "Sach",
                column: "AuthorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Sach_TacGia_AuthorId1",
                table: "Sach",
                column: "AuthorId1",
                principalTable: "TacGia",
                principalColumn: "AuthorId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sach_TacGia_AuthorId1",
                table: "Sach");

            migrationBuilder.DropIndex(
                name: "IX_Sach_AuthorId1",
                table: "Sach");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Sach");

            migrationBuilder.RenameColumn(
                name: "AuthorId1",
                table: "Sach",
                newName: "AuthorId");
        }
    }
}
