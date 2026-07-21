namespace BiletSatis.Domain.Entities;

public class EtkinlikBolum
{
    public int Id { get; set; }

    public int EtkinlikId { get; set; }
    public Etkinlik Etkinlik { get; set; } = null!;

    public int BolumId { get; set; }
    public Bolum Bolum { get; set; } = null!;

    public decimal Fiyat { get; set; }
    public int Kontenjan { get; set; }

    public ICollection<Rezervasyon> Rezervasyonlar { get; set; } = new List<Rezervasyon>();
    public ICollection<Bilet> Biletler { get; set; } = new List<Bilet>();
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}