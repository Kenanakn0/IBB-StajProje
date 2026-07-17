using BiletSatis.Application.DTOs;
using BiletSatis.Application.Services;
using BiletSatis.Domain.Entities;
using BiletSatis.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BiletSatis.Infrastructure.Services;

public class BolumService : IBolumService
{
    private readonly BiletSatisDbContext _db;

    public BolumService(BiletSatisDbContext db) => _db = db;

    public async Task<List<BolumDto>> Listele(int? salonId = null)
    {
        var sorgu = _db.Bolumler.AsQueryable();

        if (salonId is not null)
            sorgu = sorgu.Where(b => b.SalonId == salonId);

        return await sorgu
            .Select(b => new BolumDto(b.Id, b.Ad, b.Oturmali, b.SalonId))
            .ToListAsync();
    }

    public async Task<BolumDto?> Getir(int id)
    {
        var b = await _db.Bolumler.FindAsync(id);
        return b is null ? null : new BolumDto(b.Id, b.Ad, b.Oturmali, b.SalonId);
    }

    public async Task<BolumDto?> Olustur(BolumOlusturDto dto)
    {
        // Salon var mı kontrol et
        var salonVar = await _db.Salonlar.AnyAsync(s => s.Id == dto.SalonId);
        if (!salonVar) return null;

        var bolum = new Bolum
        {
            Ad = dto.Ad,
            Oturmali = dto.Oturmali,
            SalonId = dto.SalonId
        };

        _db.Bolumler.Add(bolum);
        await _db.SaveChangesAsync();

        return new BolumDto(bolum.Id, bolum.Ad, bolum.Oturmali, bolum.SalonId);
    }

    public async Task<bool> Guncelle(int id, BolumOlusturDto dto)
    {
        var bolum = await _db.Bolumler.FindAsync(id);
        if (bolum is null) return false;

        var salonVar = await _db.Salonlar.AnyAsync(s => s.Id == dto.SalonId);
        if (!salonVar) return false;

        bolum.Ad = dto.Ad;
        bolum.Oturmali = dto.Oturmali;
        bolum.SalonId = dto.SalonId;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Sil(int id)
    {
        var bolum = await _db.Bolumler.FindAsync(id);
        if (bolum is null) return false;

        _db.Bolumler.Remove(bolum);
        await _db.SaveChangesAsync();
        return true;
    }
}