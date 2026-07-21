using BiletSatis.Application.DTOs;

namespace BiletSatis.Application.Services;

public interface IRezervasyonService
{
    Task<List<MusaitKoltukDto>> MusaitKoltuklar(int etkinlikBolumId);
    Task<(RezervasyonDto? Rezervasyon, string? Hata)> Olustur(int kullaniciId, RezervasyonOlusturDto dto);
    Task<bool> Iptal(int kullaniciId, int rezervasyonId);
}