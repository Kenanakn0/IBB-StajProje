using BiletSatis.Application.DTOs;
using BiletSatis.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiletSatis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BolumController : ControllerBase
{
    private readonly IBolumService _bolumService;

    public BolumController(IBolumService bolumService) => _bolumService = bolumService;

    // /api/bolum          -> tüm bölümler
    // /api/bolum?salonId=1 -> sadece 1 numaralı salonun bölümleri
    [HttpGet]
    public async Task<IActionResult> Listele([FromQuery] int? salonId) =>
        Ok(await _bolumService.Listele(salonId));

    [HttpGet("{id}")]
    public async Task<IActionResult> Getir(int id)
    {
        var bolum = await _bolumService.Getir(id);
        return bolum is null ? NotFound() : Ok(bolum);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Olustur(BolumOlusturDto dto)
    {
        var bolum = await _bolumService.Olustur(dto);
        if (bolum is null) return BadRequest("Salon bulunamadı.");

        return CreatedAtAction(nameof(Getir), new { id = bolum.Id }, bolum);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Guncelle(int id, BolumOlusturDto dto) =>
        await _bolumService.Guncelle(id, dto) ? NoContent() : NotFound();

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Sil(int id) =>
        await _bolumService.Sil(id) ? NoContent() : NotFound();
}