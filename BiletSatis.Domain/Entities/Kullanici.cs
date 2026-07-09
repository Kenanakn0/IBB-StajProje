using BiletSatis.Domain.Enums;

namespace BiletSatis.Domain.Entities;

public class Kullanici
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string Eposta { get; set; } = string.Empty;
    public string SifreHash { get; set; } = string.Empty;   // düz şifre DEĞİL, hash'i tutulur
    public Rol Rol { get; set; } = Rol.Kullanici;

    public ICollection<Rezervasyon> Rezervasyonlar { get; set; } = new List<Rezervasyon>();
    public ICollection<Bilet> Biletler { get; set; } = new List<Bilet>();
}