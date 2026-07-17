using BiletSatis.Application.DTOs;
using BiletSatis.Application.Services;
using BiletSatis.Domain.Entities;
using BiletSatis.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BiletSatis.Infrastructure.Services;

public class EtkinlikBolumService : IEtkinlikBolumService
{
    private readonly BiletSatisDbContext _db;

    public EtkinlikBolumService(BiletSatisDbContext db) => _db = db;

    public async Task<List<EtkinlikBolumDto>> Listele(int etkinlikId) =>
        await _db.EtkinlikBolumleri
            .Where(eb => eb.EtkinlikId == etkinlikId)
            .Include(eb => eb.Bolum)
            .Select(eb => new EtkinlikBolumDto(
                eb.Id, eb.EtkinlikId, eb.BolumId,
                eb.Bolum.Ad, eb.Bolum.Oturmali,
                eb.Fiyat, eb.Kontenjan))
            .ToListAsync();

    public async Task<EtkinlikBolumDto?> Getir(int id) =>
        await _db.EtkinlikBolumleri
            .Where(eb => eb.Id == id)
            .Include(eb => eb.Bolum)
            .Select(eb => new EtkinlikBolumDto(
                eb.Id, eb.EtkinlikId, eb.BolumId,
                eb.Bolum.Ad, eb.Bolum.Oturmali,
                eb.Fiyat, eb.Kontenjan))
            .FirstOrDefaultAsync();

    public async Task<EtkinlikBolumDto?> Olustur(EtkinlikBolumOlusturDto dto)
    {
        var etkinlik = await _db.Etkinlikler.FindAsync(dto.EtkinlikId);
        if (etkinlik is null) return null;

        var bolum = await _db.Bolumler.FindAsync(dto.BolumId);
        if (bolum is null) return null;

        // Bölüm, etkinliğin salonuna ait olmalı
        if (bolum.SalonId != etkinlik.SalonId) return null;

        // Aynı etkinlik-bölüm ikilisi zaten var mı?
        var zatenVar = await _db.EtkinlikBolumleri
            .AnyAsync(eb => eb.EtkinlikId == dto.EtkinlikId && eb.BolumId == dto.BolumId);
        if (zatenVar) return null;

        var eb = new EtkinlikBolum
        {
            EtkinlikId = dto.EtkinlikId,
            BolumId = dto.BolumId,
            Fiyat = dto.Fiyat,
            Kontenjan = dto.Kontenjan
        };

        _db.EtkinlikBolumleri.Add(eb);
        await _db.SaveChangesAsync();

        return new EtkinlikBolumDto(eb.Id, eb.EtkinlikId, eb.BolumId,
                                    bolum.Ad, bolum.Oturmali,
                                    eb.Fiyat, eb.Kontenjan);
    }

    public async Task<bool> Guncelle(int id, EtkinlikBolumOlusturDto dto)
    {
        var eb = await _db.EtkinlikBolumleri.FindAsync(id);
        if (eb is null) return false;

        eb.Fiyat = dto.Fiyat;
        eb.Kontenjan = dto.Kontenjan;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Sil(int id)
    {
        var eb = await _db.EtkinlikBolumleri.FindAsync(id);
        if (eb is null) return false;

        _db.EtkinlikBolumleri.Remove(eb);
        await _db.SaveChangesAsync();
        return true;
    }
}