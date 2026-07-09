namespace BiletSatis.Domain.Entities;

public class Indirim
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;   // örn. "Öğrenci"
    public decimal Yuzde { get; set; }               // örn. 20 -> %20

    public ICollection<Bilet> Biletler { get; set; } = new List<Bilet>();
}