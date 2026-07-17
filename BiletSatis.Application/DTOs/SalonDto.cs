namespace BiletSatis.Application.DTOs;

// Dışarı dönen
public record SalonDto(int Id, string Ad, string Adres);

// İçeri gelen (oluşturma/güncelleme)
public record SalonOlusturDto(string Ad, string Adres);