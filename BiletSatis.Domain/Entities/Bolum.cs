namespace BiletSatis.Domain.Entities;

public class Bolum
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;   // VIP / Standart / Ayakta
    public bool Oturmali { get; set; }               // true: koltuklu, false: ayakta

    public int SalonId { get; set; }
    public Salon Salon { get; set; } = null!;

    public ICollection<Koltuk> Koltuklar { get; set; } = new List<Koltuk>();
    public ICollection<EtkinlikBolum> EtkinlikBolumleri { get; set; } = new List<EtkinlikBolum>();
}