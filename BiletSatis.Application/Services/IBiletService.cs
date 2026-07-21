using BiletSatis.Application.DTOs;

namespace BiletSatis.Application.Services;

public interface IBiletService
{
    Task<(BiletDto? Bilet, string? Hata)> Odeme(int kullaniciId, OdemeDto dto);
    Task<List<BiletDto>> Biletlerim(int kullaniciId);
}