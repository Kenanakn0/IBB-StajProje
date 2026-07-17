using BiletSatis.Application.DTOs;
using BiletSatis.Application.Services;
using BiletSatis.Domain.Entities;
using BiletSatis.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BiletSatis.Infrastructure.Services;

public class KoltukService : IKoltukService
{
    private readonly BiletSatisDbContext _db;

    public KoltukService(BiletSatisDbContext db) => _db = db;

    public async Task<List<KoltukDto>> Listele(int bolumId) =>
        await _db.Koltuklar
            .Where(k => k.BolumId == bolumId)
            .Select(k => new KoltukDto(k.Id, k.Blok, k.Sira, k.KoltukNo, k.BolumId))
            .ToListAsync();

    public async Task<int> Uret(KoltukUretDto dto)
    {
        var bolum = await _db.Bolumler.FindAsync(dto.BolumId);
        if (bolum is null) return 0;

        // Ayakta bölümde koltuk olmaz
        if (!bolum.Oturmali) return 0;

        // Aralıklar geçerli mi?
        if (dto.SiraBaslangic > dto.SiraBitis) return 0;
        if (dto.KoltukBaslangic > dto.KoltukBitis) return 0;

        // Bu bölümde bu blokta zaten koltuk var mı? (tekrar üretimi engelle)
        var zatenVar = await _db.Koltuklar
            .AnyAsync(k => k.BolumId == dto.BolumId && k.Blok == dto.Blok);
        if (zatenVar) return 0;

        var koltuklar = new List<Koltuk>();

        for (int sira = dto.SiraBaslangic; sira <= dto.SiraBitis; sira++)
        {
            for (int no = dto.KoltukBaslangic; no <= dto.KoltukBitis; no++)
            {
                koltuklar.Add(new Koltuk
                {
                    BolumId = dto.BolumId,
                    Blok = dto.Blok,
                    Sira = sira.ToString(),
                    KoltukNo = no.ToString()
                });
            }
        }

        _db.Koltuklar.AddRange(koltuklar);
        await _db.SaveChangesAsync();

        return koltuklar.Count;
    }

    public async Task<bool> Sil(int id)
    {
        var koltuk = await _db.Koltuklar.FindAsync(id);
        if (koltuk is null) return false;

        _db.Koltuklar.Remove(koltuk);
        await _db.SaveChangesAsync();
        return true;
    }
}