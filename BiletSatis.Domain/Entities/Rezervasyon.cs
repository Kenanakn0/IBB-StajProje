namespace BiletSatis.Domain.Entities;

// Satın almadan önceki 5 dakikalık geçici tutma kaydı
public class Rezervasyon
{
    public int Id { get; set; }

    public int KullaniciId { get; set; }
    public Kullanici Kullanici { get; set; } = null!;

    public int EtkinlikBolumId { get; set; }
    public EtkinlikBolum EtkinlikBolum { get; set; } = null!;

    public int? KoltukId { get; set; }   // ayakta bölümde koltuk yok -> nullable
    public Koltuk? Koltuk { get; set; }

    public DateTime OlusturmaZamani { get; set; }
    public DateTime SonGecerlilik { get; set; }   // OlusturmaZamani + 5 dk
}