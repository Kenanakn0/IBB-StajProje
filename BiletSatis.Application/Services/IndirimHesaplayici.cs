namespace BiletSatis.Application.Services;

public class IndirimHesaplayici : IIndirimHesaplayici
{
    public decimal Hesapla(decimal fiyat, decimal? indirimYuzde)
    {
        if (indirimYuzde is null || indirimYuzde <= 0) return fiyat;

        var yuzde = Math.Min(indirimYuzde.Value, 100);   // %100'ü aşamaz
        var tutar = fiyat - (fiyat * yuzde / 100);

        return Math.Round(tutar, 2);
    }
}