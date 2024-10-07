using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectA.Data.Migrations
{
    /// <inheritdoc />
    public partial class addSPyeuthich : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SanPhamYeuThich",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SanPhamId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhamYeuThich", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SanPhamYeuThich_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SanPhamYeuThich_SanPham_SanPhamId",
                        column: x => x.SanPhamId,
                        principalTable: "SanPham",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamYeuThich_ApplicationUserId",
                table: "SanPhamYeuThich",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamYeuThich_SanPhamId",
                table: "SanPhamYeuThich",
                column: "SanPhamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SanPhamYeuThich");
        }
    }
}
