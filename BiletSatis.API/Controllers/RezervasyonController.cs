using System.Security.Claims;
using BiletSatis.Application.DTOs;
using BiletSatis.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiletSatis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]                       // tüm uçlar giriş ister
public class RezervasyonController : ControllerBase
{
    private readonly IRezervasyonService _service;

    public RezervasyonController(IRezervasyonService service) => _service = service;

    private int KullaniciId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
               ?? User.FindFirstValue("sub")!);

    // /api/rezervasyon/musait-koltuklar?etkinlikBolumId=1
    [HttpGet("musait-koltuklar")]
    [AllowAnonymous]              // koltuk durumu herkese açık
    public async Task<IActionResult> MusaitKoltuklar([FromQuery] int etkinlikBolumId) =>
        Ok(await _service.MusaitKoltuklar(etkinlikBolumId));

    [HttpPost]
    public async Task<IActionResult> Olustur(RezervasyonOlusturDto dto)
    {
        var (rezervasyon, hata) = await _service.Olustur(KullaniciId, dto);

        if (rezervasyon is null) return BadRequest(hata);

        return Ok(rezervasyon);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Iptal(int id) =>
        await _service.Iptal(KullaniciId, id) ? NoContent() : NotFound();
}