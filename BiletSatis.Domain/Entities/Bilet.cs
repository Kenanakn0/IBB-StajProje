namespace BiletSatis.Domain.Entities;

// Ödemesi tamamlanmış bilet
public class Bilet
{
    public int Id { get; set; }

    public int KullaniciId { get; set; }
    public Kullanici Kullanici { get; set; } = null!;

    public int EtkinlikBolumId { get; set; }
    public EtkinlikBolum EtkinlikBolum { get; set; } = null!;

    public int? KoltukId { get; set; }
    public Koltuk? Koltuk { get; set; }

    public int? IndirimId { get; set; }   // uygulanan indirim (opsiyonel)
    public Indirim? Indirim { get; set; }

    public decimal OdenenTutar { get; set; }
    public DateTime SatinAlmaZamani { get; set; }
}