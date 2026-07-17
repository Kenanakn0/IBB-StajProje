using BiletSatis.Application.DTOs;

namespace BiletSatis.Application.Services;

public interface IEtkinlikService
{
    Task<List<EtkinlikDto>> Listele();
    Task<EtkinlikDto?> Getir(int id);
    Task<EtkinlikDto?> Olustur(EtkinlikOlusturDto dto);
    Task<bool> Guncelle(int id, EtkinlikOlusturDto dto);
    Task<bool> Sil(int id);
}