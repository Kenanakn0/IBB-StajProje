using BiletSatis.Application.DTOs;

namespace BiletSatis.Application.Services;

public interface IBolumService
{
    Task<List<BolumDto>> Listele(int? salonId = null);
    Task<BolumDto?> Getir(int id);
    Task<BolumDto?> Olustur(BolumOlusturDto dto);
    Task<bool> Guncelle(int id, BolumOlusturDto dto);
    Task<bool> Sil(int id);
}