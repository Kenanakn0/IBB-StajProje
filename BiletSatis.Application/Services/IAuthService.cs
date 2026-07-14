using BiletSatis.Application.DTOs;

namespace BiletSatis.Application.Services;

public interface IAuthService
{
    Task<AuthCevapDto?> KayitOl(KayitDto dto);
    Task<AuthCevapDto?> GirisYap(GirisDto dto);
}