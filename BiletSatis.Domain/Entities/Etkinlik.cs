namespace BiletSatis.Domain.Entities;

public class Etkinlik
{
    public int Id { get; set; }
    public string Isim { get; set; } = string.Empty;
    public DateTime Tarih { get; set; }
    public string Aciklama { get; set; } = string.Empty;

    public int SalonId { get; set; }
    public Salon Salon { get; set; } = null!;

    public ICollection<EtkinlikBolum> EtkinlikBolumleri { get; set; } = new List<EtkinlikBolum>();
}