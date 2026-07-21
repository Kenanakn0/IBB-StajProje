using BiletSatis.Application.DTOs;
using BiletSatis.Application.Services;
using BiletSatis.Domain.Entities;
using BiletSatis.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BiletSatis.Infrastructure.Services;

public class BiletService : IBiletService
{
    private readonly BiletSatisDbContext _db;
    private readonly IIndirimHesaplayici _indirimHesaplayici;

    public BiletService(BiletSatisDbContext db, IIndirimHesaplayici indirimHesaplayici)
    {
        _db = db;
        _indirimHesaplayici = indirimHesaplayici;
    }

    public async Task<(BiletDto?, string?)> Odeme(int kullaniciId, OdemeDto dto)
    {
        var rezervasyon = await _db.Rezervasyonlar
            .Include(r => r.EtkinlikBolum)
            .FirstOrDefaultAsync(r => r.Id == dto.RezervasyonId
                                   && r.KullaniciId == kullaniciId);

        if (rezervasyon is null)
            return (null, "Rezervasyon bulunamadı.");

        if (rezervasyon.SonGecerlilik <= DateTime.UtcNow)
            return (null, "Rezervasyon süresi doldu, lütfen tekrar deneyin.");

        // İndirim
        decimal? yuzde = null;
        if (dto.IndirimId is not null)
        {
            var indirim = await _db.Indirimler.FindAsync(dto.IndirimId.Value);
            if (indirim is null) return (null, "İndirim bulunamadı.");
            yuzde = indirim.Yuzde;
        }

        var tutar = _indirimHesaplayici.Hesapla(rezervasyon.EtkinlikBolum.Fiyat, yuzde);

        // --- Ödeme simülasyonu ---
        var odemeBasarili = true;
        if (!odemeBasarili) return (null, "Ödeme başarısız.");

        var bilet = new Bilet
        {
            KullaniciId = kullaniciId,
            EtkinlikBolumId = rezervasyon.EtkinlikBolumId,
            KoltukId = rezervasyon.KoltukId,
            IndirimId = dto.IndirimId,
            OdenenTutar = tutar,
            SatinAlmaZamani = DateTime.UtcNow
        };

        using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            _db.Biletler.Add(bilet);
            _db.Rezervasyonlar.Remove(rezervasyon);   // rezervasyon bilete dönüştü
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
        }
        catch (DbUpdateException)
        {
            await tx.RollbackAsync();
            return (null, "Bu koltuk için zaten bilet oluşturulmuş.");
        }

        return (new BiletDto(bilet.Id, bilet.EtkinlikBolumId, bilet.KoltukId,
                             bilet.OdenenTutar, bilet.SatinAlmaZamani), null);
    }

    public async Task<List<BiletDto>> Biletlerim(int kullaniciId) =>
        await _db.Biletler
            .Where(b => b.KullaniciId == kullaniciId)
            .OrderByDescending(b => b.SatinAlmaZamani)
            .Select(b => new BiletDto(b.Id, b.EtkinlikBolumId, b.KoltukId,
                                      b.OdenenTutar, b.SatinAlmaZamani))
            .ToListAsync();
}