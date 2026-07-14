using BiletSatis.Domain.Entities;

namespace BiletSatis.Application.Services;

public interface ITokenService
{
    string Uret(Kullanici kullanici);
}