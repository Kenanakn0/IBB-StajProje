using BiletSatis.Application.DTOs;
using BiletSatis.Application.Services;
using BiletSatis.Domain.Entities;
using BiletSatis.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BiletSatis.Infrastructure.Services;

public class RezervasyonService : IRezervasyonService
{
    private readonly BiletSatisDbContext _db;
    private const int TutmaSuresiDakika = 5;

    public RezervasyonService(BiletSatisDbContext db) => _db = db;

    public async Task<List<MusaitKoltukDto>> MusaitKoltuklar(int etkinlikBolumId)
    {
        var eb = await _db.EtkinlikBolumleri
            .Include(x => x.Bolum)
            .FirstOrDefaultAsync(x => x.Id == etkinlikBolumId);

        if (eb is null || !eb.Bolum.Oturmali) return new List<MusaitKoltukDto>();

        var simdi = DateTime.UtcNow;

        // Aktif rezervasyonu veya bileti olan koltuklar
        var doluKoltuklar = await _db.Rezervasyonlar
            .Where(r => r.EtkinlikBolumId == etkinlikBolumId
                     && r.KoltukId != null
                     && r.SonGecerlilik > simdi)
            .Select(r => r.KoltukId!.Value)
            .Union(_db.Biletler
                .Where(b => b.EtkinlikBolumId == etkinlikBolumId && b.KoltukId != null)
                .Select(b => b.KoltukId!.Value))
            .ToListAsync();

        return await _db.Koltuklar
            .Where(k => k.BolumId == eb.BolumId && !doluKoltuklar.Contains(k.Id))
            .Select(k => new MusaitKoltukDto(k.Id, k.Blok, k.Sira, k.KoltukNo))
            .ToListAsync();
    }

    public async Task<(RezervasyonDto?, string?)> Olustur(int kullaniciId, RezervasyonOlusturDto dto)
    {
        var eb = await _db.EtkinlikBolumleri
            .Include(x => x.Bolum)
            .FirstOrDefaultAsync(x => x.Id == dto.EtkinlikBolumId);

        if (eb is null) return (null, "Etkinlik bölümü bulunamadı.");

        var simdi = DateTime.UtcNow;

        if (eb.Bolum.Oturmali)
        {
            if (dto.KoltukId is null)
                return (null, "Bu bölüm oturmalı, koltuk seçmelisiniz.");

            var koltuk = await _db.Koltuklar.FindAsync(dto.KoltukId.Value);
            if (koltuk is null || koltuk.BolumId != eb.BolumId)
                return (null, "Koltuk bu bölüme ait değil.");

            var doluMu = await _db.Rezervasyonlar.AnyAsync(r =>
                    r.EtkinlikBolumId == eb.Id && r.KoltukId == dto.KoltukId && r.SonGecerlilik > simdi)
                || await _db.Biletler.AnyAsync(b =>
                    b.EtkinlikBolumId == eb.Id && b.KoltukId == dto.KoltukId);

            if (doluMu) return (null, "Bu koltuk şu anda müsait değil.");
        }
        else
        {
            if (dto.KoltukId is not null)
                return (null, "Ayakta bölümde koltuk seçilemez.");

            var aktifRezervasyon = await _db.Rezervasyonlar
                .CountAsync(r => r.EtkinlikBolumId == eb.Id && r.SonGecerlilik > simdi);
            var satilan = await _db.Biletler
                .CountAsync(b => b.EtkinlikBolumId == eb.Id);

            if (aktifRezervasyon + satilan + dto.Adet > eb.Kontenjan)
                return (null, "Yeterli kontenjan yok.");
        }

        var rezervasyon = new Rezervasyon
        {
            KullaniciId = kullaniciId,
            EtkinlikBolumId = eb.Id,
            KoltukId = dto.KoltukId,
            OlusturmaZamani = simdi,
            SonGecerlilik = simdi.AddMinutes(TutmaSuresiDakika)
        };

        _db.Rezervasyonlar.Add(rezervasyon);

        // Kontenjanı ilgilendiren işlem: RowVersion kontrolünü tetikle
        _db.Entry(eb).Property(x => x.RowVersion).IsModified = true;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return (null, "Yoğunluk nedeniyle işlem tamamlanamadı, tekrar deneyin.");
        }
        catch (DbUpdateException)
        {
            // Unique index devreye girdi -> koltuk kapıldı
            return (null, "Bu koltuk az önce başkası tarafından alındı.");
        }

        return (new RezervasyonDto(rezervasyon.Id, rezervasyon.EtkinlikBolumId,
                                   rezervasyon.KoltukId, rezervasyon.SonGecerlilik), null);
    }

    public async Task<bool> Iptal(int kullaniciId, int rezervasyonId)
    {
        var r = await _db.Rezervasyonlar
            .FirstOrDefaultAsync(x => x.Id == rezervasyonId && x.KullaniciId == kullaniciId);

        if (r is null) return false;

        _db.Rezervasyonlar.Remove(r);
        await _db.SaveChangesAsync();
        return true;
    }
}