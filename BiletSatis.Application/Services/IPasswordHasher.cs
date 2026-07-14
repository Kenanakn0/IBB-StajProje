namespace BiletSatis.Application.Services;

public interface IPasswordHasher
{
    string Hash(string sifre);
    bool Verify(string sifre, string hash);
}