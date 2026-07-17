using BiletSatis.Application.DTOs;
using BiletSatis.Application.Services;
using BiletSatis.Domain.Entities;
using BiletSatis.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BiletSatis.Infrastructure.Services;

public class SalonService : ISalonService
{
    private readonly BiletSatisDbContext _db;

    public SalonService(BiletSatisDbContext db) => _db = db;

    public async Task<List<SalonDto>> Listele() =>
        await _db.Salonlar
            .Select(s => new SalonDto(s.Id, s.Ad, s.Adres))
            .ToListAsync();

    public async Task<SalonDto?> Getir(int id)
    {
        var s = await _db.Salonlar.FindAsync(id);
        return s is null ? null : new SalonDto(s.Id, s.Ad, s.Adres);
    }

    public async Task<SalonDto> Olustur(SalonOlusturDto dto)
    {
        var salon = new Salon { Ad = dto.Ad, Adres = dto.Adres };
        _db.Salonlar.Add(salon);
        await _db.SaveChangesAsync();
        return new SalonDto(salon.Id, salon.Ad, salon.Adres);
    }

    public async Task<bool> Guncelle(int id, SalonOlusturDto dto)
    {
        var salon = await _db.Salonlar.FindAsync(id);
        if (salon is null) return false;

        salon.Ad = dto.Ad;
        salon.Adres = dto.Adres;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Sil(int id)
    {
        var salon = await _db.Salonlar.FindAsync(id);
        if (salon is null) return false;

        _db.Salonlar.Remove(salon);
        await _db.SaveChangesAsync();
        return true;
    }
}