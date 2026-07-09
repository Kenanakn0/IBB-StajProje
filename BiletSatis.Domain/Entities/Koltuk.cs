namespace BiletSatis.Domain.Entities;

public class Koltuk
{
    public int Id { get; set; }
    public string Blok { get; set; } = string.Empty;
    public string Sira { get; set; } = string.Empty;
    public string KoltukNo { get; set; } = string.Empty;

    public int BolumId { get; set; }
    public Bolum Bolum { get; set; } = null!;
}