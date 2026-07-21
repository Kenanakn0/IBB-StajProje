using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiletSatis.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CiftSatisOnleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rezervasyonlar_EtkinlikBolumId",
                table: "Rezervasyonlar");

            migrationBuilder.DropIndex(
                name: "IX_Biletler_EtkinlikBolumId",
                table: "Biletler");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "EtkinlikBolumleri",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Rezervasyonlar_EtkinlikBolumId_KoltukId",
                table: "Rezervasyonlar",
                columns: new[] { "EtkinlikBolumId", "KoltukId" },
                unique: true,
                filter: "[KoltukId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Biletler_EtkinlikBolumId_KoltukId",
                table: "Biletler",
                columns: new[] { "EtkinlikBolumId", "KoltukId" },
                unique: true,
                filter: "[KoltukId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rezervasyonlar_EtkinlikBolumId_KoltukId",
                table: "Rezervasyonlar");

            migrationBuilder.DropIndex(
                name: "IX_Biletler_EtkinlikBolumId_KoltukId",
                table: "Biletler");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "EtkinlikBolumleri");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervasyonlar_EtkinlikBolumId",
                table: "Rezervasyonlar",
                column: "EtkinlikBolumId");

            migrationBuilder.CreateIndex(
                name: "IX_Biletler_EtkinlikBolumId",
                table: "Biletler",
                column: "EtkinlikBolumId");
        }
    }
}
