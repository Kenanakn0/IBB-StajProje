namespace BiletSatis.Application.Services;

public interface IIndirimHesaplayici
{
    // İndirim uygulanmış tutarı döner
    decimal Hesapla(decimal fiyat, decimal? indirimYuzde);
}