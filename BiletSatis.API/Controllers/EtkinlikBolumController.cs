using BiletSatis.Application.DTOs;
using BiletSatis.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiletSatis.API.Controllers;

[ApiController]
[Route("api/etkinlikbolum")]
public class EtkinlikBolumController : ControllerBase
{
    private readonly IEtkinlikBolumService _service;

    public EtkinlikBolumController(IEtkinlikBolumService service) => _service = service;

    // /api/etkinlikbolum?etkinlikId=1
    [HttpGet]
    public async Task<IActionResult> Listele([FromQuery] int etkinlikId) =>
        Ok(await _service.Listele(etkinlikId));

    [HttpGet("{id}")]
    public async Task<IActionResult> Getir(int id)
    {
        var eb = await _service.Getir(id);
        return eb is null ? NotFound() : Ok(eb);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Olustur(EtkinlikBolumOlusturDto dto)
    {
        var eb = await _service.Olustur(dto);
        if (eb is null)
            return BadRequest("Etkinlik/bölüm bulunamadı, bölüm etkinliğin salonuna ait değil veya bu bölüm bu etkinliğe zaten eklenmiş.");

        return CreatedAtAction(nameof(Getir), new { id = eb.Id }, eb);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Guncelle(int id, EtkinlikBolumOlusturDto dto) =>
        await _service.Guncelle(id, dto) ? NoContent() : NotFound();

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Sil(int id) =>
        await _service.Sil(id) ? NoContent() : NotFound();
}