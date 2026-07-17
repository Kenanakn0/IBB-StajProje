namespace BiletSatis.Application.DTOs;

public record BolumDto(int Id, string Ad, bool Oturmali, int SalonId);

public record BolumOlusturDto(string Ad, bool Oturmali, int SalonId);