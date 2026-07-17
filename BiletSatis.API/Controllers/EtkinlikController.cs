using BiletSatis.Application.DTOs;
using BiletSatis.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiletSatis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EtkinlikController : ControllerBase
{
    private readonly IEtkinlikService _etkinlikService;

    public EtkinlikController(IEtkinlikService etkinlikService) =>
        _etkinlikService = etkinlikService;

    [HttpGet]
    public async Task<IActionResult> Listele() =>
        Ok(await _etkinlikService.Listele());

    [HttpGet("{id}")]
    public async Task<IActionResult> Getir(int id)
    {
        var etkinlik = await _etkinlikService.Getir(id);
        return etkinlik is null ? NotFound() : Ok(etkinlik);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Olustur(EtkinlikOlusturDto dto)
    {
        var etkinlik = await _etkinlikService.Olustur(dto);
        if (etkinlik is null) return BadRequest("Salon bulunamadı.");

        return CreatedAtAction(nameof(Getir), new { id = etkinlik.Id }, etkinlik);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Guncelle(int id, EtkinlikOlusturDto dto) =>
        await _etkinlikService.Guncelle(id, dto) ? NoContent() : NotFound();

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Sil(int id) =>
        await _etkinlikService.Sil(id) ? NoContent() : NotFound();
}