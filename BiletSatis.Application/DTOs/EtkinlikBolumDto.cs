namespace BiletSatis.Application.DTOs;

public record EtkinlikBolumDto(
    int Id,
    int EtkinlikId,
    int BolumId,
    string BolumAd,
    bool Oturmali,
    decimal Fiyat,
    int Kontenjan);

public record EtkinlikBolumOlusturDto(
    int EtkinlikId,
    int BolumId,
    decimal Fiyat,
    int Kontenjan);