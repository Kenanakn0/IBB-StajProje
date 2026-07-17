using BiletSatis.Application.DTOs;
using BiletSatis.Application.Services;
using BiletSatis.Domain.Entities;
using BiletSatis.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BiletSatis.Infrastructure.Services;

public class EtkinlikService : IEtkinlikService
{
    private readonly BiletSatisDbContext _db;

    public EtkinlikService(BiletSatisDbContext db) => _db = db;

    public async Task<List<EtkinlikDto>> Listele() =>
        await _db.Etkinlikler
            .OrderBy(e => e.Tarih)
            .Select(e => new EtkinlikDto(e.Id, e.Isim, e.Tarih, e.Aciklama, e.SalonId))
            .ToListAsync();

    public async Task<EtkinlikDto?> Getir(int id)
    {
        var e = await _db.Etkinlikler.FindAsync(id);
        return e is null ? null : new EtkinlikDto(e.Id, e.Isim, e.Tarih, e.Aciklama, e.SalonId);
    }

    public async Task<EtkinlikDto?> Olustur(EtkinlikOlusturDto dto)
    {
        var salonVar = await _db.Salonlar.AnyAsync(s => s.Id == dto.SalonId);
        if (!salonVar) return null;

        var etkinlik = new Etkinlik
        {
            Isim = dto.Isim,
            Tarih = dto.Tarih,
            Aciklama = dto.Aciklama,
            SalonId = dto.SalonId
        };

        _db.Etkinlikler.Add(etkinlik);
        await _db.SaveChangesAsync();

        return new EtkinlikDto(etkinlik.Id, etkinlik.Isim, etkinlik.Tarih,
                               etkinlik.Aciklama, etkinlik.SalonId);
    }

    public async Task<bool> Guncelle(int id, EtkinlikOlusturDto dto)
    {
        var etkinlik = await _db.Etkinlikler.FindAsync(id);
        if (etkinlik is null) return false;

        var salonVar = await _db.Salonlar.AnyAsync(s => s.Id == dto.SalonId);
        if (!salonVar) return false;

        etkinlik.Isim = dto.Isim;
        etkinlik.Tarih = dto.Tarih;
        etkinlik.Aciklama = dto.Aciklama;
        etkinlik.SalonId = dto.SalonId;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Sil(int id)
    {
        var etkinlik = await _db.Etkinlikler.FindAsync(id);
        if (etkinlik is null) return false;

        _db.Etkinlikler.Remove(etkinlik);
        await _db.SaveChangesAsync();
        return true;
    }
}