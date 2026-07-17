namespace BiletSatis.Application.DTOs;

public record KoltukDto(int Id, string Blok, string Sira, string KoltukNo, int BolumId);

// Toplu üretim: A blok, 1-10 sıra, her sırada 1-20 koltuk
public record KoltukUretDto(
    int BolumId,
    string Blok,
    int SiraBaslangic,
    int SiraBitis,
    int KoltukBaslangic,
    int KoltukBitis);