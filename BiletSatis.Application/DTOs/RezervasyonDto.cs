namespace BiletSatis.Application.DTOs;

public record MusaitKoltukDto(int KoltukId, string Blok, string Sira, string KoltukNo);

public record RezervasyonOlusturDto(int EtkinlikBolumId, int? KoltukId, int Adet = 1);

public record RezervasyonDto(
    int Id,
    int EtkinlikBolumId,
    int? KoltukId,
    DateTime SonGecerlilik);