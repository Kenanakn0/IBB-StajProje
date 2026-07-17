using BiletSatis.Application.DTOs;

namespace BiletSatis.Application.Services;

public interface IKoltukService
{
    Task<List<KoltukDto>> Listele(int bolumId);
    Task<int> Uret(KoltukUretDto dto);   // kaç koltuk üretildi
    Task<bool> Sil(int id);
}