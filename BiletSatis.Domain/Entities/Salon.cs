namespace BiletSatis.Domain.Entities;

public class Salon
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string Adres { get; set; } = string.Empty;

    public ICollection<Bolum> Bolumler { get; set; } = new List<Bolum>();
    public ICollection<Etkinlik> Etkinlikler { get; set; } = new List<Etkinlik>();
}