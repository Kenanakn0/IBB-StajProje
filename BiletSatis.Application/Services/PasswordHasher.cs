namespace BiletSatis.Application.Services;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string sifre) =>
        BCrypt.Net.BCrypt.HashPassword(sifre);

    public bool Verify(string sifre, string hash) =>
        BCrypt.Net.BCrypt.Verify(sifre, hash);
}