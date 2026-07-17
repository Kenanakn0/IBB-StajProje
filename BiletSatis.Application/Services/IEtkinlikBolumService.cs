using BiletSatis.Application.DTOs;

namespace BiletSatis.Application.Services;

public interface IEtkinlikBolumService
{
    Task<List<EtkinlikBolumDto>> Listele(int etkinlikId);
    Task<EtkinlikBolumDto?> Getir(int id);
    Task<EtkinlikBolumDto?> Olustur(EtkinlikBolumOlusturDto dto);
    Task<bool> Guncelle(int id, EtkinlikBolumOlusturDto dto);
    Task<bool> Sil(int id);
}