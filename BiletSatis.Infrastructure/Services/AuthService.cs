using BiletSatis.Application.DTOs;
using BiletSatis.Application.Services;
using BiletSatis.Domain.Entities;
using BiletSatis.Domain.Enums;
using BiletSatis.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BiletSatis.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly BiletSatisDbContext _db;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokenService;

    public AuthService(
        BiletSatisDbContext db,
        IPasswordHasher hasher,
        ITokenService tokenService)
    {
        _db = db;
        _hasher = hasher;
        _tokenService = tokenService;
    }

    public async Task<AuthCevapDto?> KayitOl(KayitDto dto)
    {
        // E-posta zaten kayıtlı mı?
        var varMi = await _db.Kullanicilar
            .AnyAsync(k => k.Eposta == dto.Eposta);

        if (varMi) return null;

        var kullanici = new Kullanici
        {
            Ad = dto.Ad,
            Eposta = dto.Eposta,
            SifreHash = _hasher.Hash(dto.Sifre),
            Rol = Rol.Kullanici          // kayıt olan herkes normal kullanıcı
        };

        _db.Kullanicilar.Add(kullanici);
        await _db.SaveChangesAsync();

        return new AuthCevapDto(
            _tokenService.Uret(kullanici),
            kullanici.Ad,
            kullanici.Eposta,
            kullanici.Rol.ToString());
    }

    public async Task<AuthCevapDto?> GirisYap(GirisDto dto)
    {
        var kullanici = await _db.Kullanicilar
            .FirstOrDefaultAsync(k => k.Eposta == dto.Eposta);

        if (kullanici is null) return null;

        if (!_hasher.Verify(dto.Sifre, kullanici.SifreHash)) return null;

        return new AuthCevapDto(
            _tokenService.Uret(kullanici),
            kullanici.Ad,
            kullanici.Eposta,
            kullanici.Rol.ToString());
    }
}