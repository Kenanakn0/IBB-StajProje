namespace BiletSatis.Application.DTOs;

public record EtkinlikDto(int Id, string Isim, DateTime Tarih, string Aciklama, int SalonId);

public record EtkinlikOlusturDto(string Isim, DateTime Tarih, string Aciklama, int SalonId);