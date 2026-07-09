using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiletSatis.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IlkOlusturma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Indirimler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Yuzde = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indirimler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Eposta = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SifreHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Salonlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salonlar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bolumler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Oturmali = table.Column<bool>(type: "bit", nullable: false),
                    SalonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bolumler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bolumler_Salonlar_SalonId",
                        column: x => x.SalonId,
                        principalTable: "Salonlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Etkinlikler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Isim = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SalonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etkinlikler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Etkinlikler_Salonlar_SalonId",
                        column: x => x.SalonId,
                        principalTable: "Salonlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Koltuklar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Blok = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sira = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KoltukNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BolumId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Koltuklar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Koltuklar_Bolumler_BolumId",
                        column: x => x.BolumId,
                        principalTable: "Bolumler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EtkinlikBolumleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EtkinlikId = table.Column<int>(type: "int", nullable: false),
                    BolumId = table.Column<int>(type: "int", nullable: false),
                    Fiyat = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Kontenjan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EtkinlikBolumleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EtkinlikBolumleri_Bolumler_BolumId",
                        column: x => x.BolumId,
                        principalTable: "Bolumler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EtkinlikBolumleri_Etkinlikler_EtkinlikId",
                        column: x => x.EtkinlikId,
                        principalTable: "Etkinlikler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Biletler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciId = table.Column<int>(type: "int", nullable: false),
                    EtkinlikBolumId = table.Column<int>(type: "int", nullable: false),
                    KoltukId = table.Column<int>(type: "int", nullable: true),
                    IndirimId = table.Column<int>(type: "int", nullable: true),
                    OdenenTutar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SatinAlmaZamani = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biletler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Biletler_EtkinlikBolumleri_EtkinlikBolumId",
                        column: x => x.EtkinlikBolumId,
                        principalTable: "EtkinlikBolumleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Biletler_Indirimler_IndirimId",
                        column: x => x.IndirimId,
                        principalTable: "Indirimler",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Biletler_Koltuklar_KoltukId",
                        column: x => x.KoltukId,
                        principalTable: "Koltuklar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Biletler_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rezervasyonlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciId = table.Column<int>(type: "int", nullable: false),
                    EtkinlikBolumId = table.Column<int>(type: "int", nullable: false),
                    KoltukId = table.Column<int>(type: "int", nullable: true),
                    OlusturmaZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SonGecerlilik = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervasyonlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rezervasyonlar_EtkinlikBolumleri_EtkinlikBolumId",
                        column: x => x.EtkinlikBolumId,
                        principalTable: "EtkinlikBolumleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rezervasyonlar_Koltuklar_KoltukId",
                        column: x => x.KoltukId,
                        principalTable: "Koltuklar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rezervasyonlar_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Biletler_EtkinlikBolumId",
                table: "Biletler",
                column: "EtkinlikBolumId");

            migrationBuilder.CreateIndex(
                name: "IX_Biletler_IndirimId",
                table: "Biletler",
                column: "IndirimId");

            migrationBuilder.CreateIndex(
                name: "IX_Biletler_KoltukId",
                table: "Biletler",
                column: "KoltukId");

            migrationBuilder.CreateIndex(
                name: "IX_Biletler_KullaniciId",
                table: "Biletler",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Bolumler_SalonId",
                table: "Bolumler",
                column: "SalonId");

            migrationBuilder.CreateIndex(
                name: "IX_EtkinlikBolumleri_BolumId",
                table: "EtkinlikBolumleri",
                column: "BolumId");

            migrationBuilder.CreateIndex(
                name: "IX_EtkinlikBolumleri_EtkinlikId",
                table: "EtkinlikBolumleri",
                column: "EtkinlikId");

            migrationBuilder.CreateIndex(
                name: "IX_Etkinlikler_SalonId",
                table: "Etkinlikler",
                column: "SalonId");

            migrationBuilder.CreateIndex(
                name: "IX_Koltuklar_BolumId",
                table: "Koltuklar",
                column: "BolumId");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanicilar_Eposta",
                table: "Kullanicilar",
                column: "Eposta",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rezervasyonlar_EtkinlikBolumId",
                table: "Rezervasyonlar",
                column: "EtkinlikBolumId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervasyonlar_KoltukId",
                table: "Rezervasyonlar",
                column: "KoltukId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervasyonlar_KullaniciId",
                table: "Rezervasyonlar",
                column: "KullaniciId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Biletler");

            migrationBuilder.DropTable(
                name: "Rezervasyonlar");

            migrationBuilder.DropTable(
                name: "Indirimler");

            migrationBuilder.DropTable(
                name: "EtkinlikBolumleri");

            migrationBuilder.DropTable(
                name: "Koltuklar");

            migrationBuilder.DropTable(
                name: "Kullanicilar");

            migrationBuilder.DropTable(
                name: "Etkinlikler");

            migrationBuilder.DropTable(
                name: "Bolumler");

            migrationBuilder.DropTable(
                name: "Salonlar");
        }
    }
}
