namespace BiletSatis.Application.DTOs;

public record OdemeDto(int RezervasyonId, int? IndirimId);

public record BiletDto(
    int Id,
    int EtkinlikBolumId,
    int? KoltukId,
    decimal OdenenTutar,
    DateTime SatinAlmaZamani);