using BiletSatis.Application.DTOs;

namespace BiletSatis.Application.Services;

public interface ISalonService
{
    Task<List<SalonDto>> Listele();
    Task<SalonDto?> Getir(int id);
    Task<SalonDto> Olustur(SalonOlusturDto dto);
    Task<bool> Guncelle(int id, SalonOlusturDto dto);
    Task<bool> Sil(int id);
}