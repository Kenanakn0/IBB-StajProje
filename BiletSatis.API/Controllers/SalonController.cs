using BiletSatis.Application.DTOs;
using BiletSatis.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiletSatis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalonController : ControllerBase
{
    private readonly ISalonService _salonService;

    public SalonController(ISalonService salonService) => _salonService = salonService;

    // Listeleme herkese açık (kullanıcılar da görecek)
    [HttpGet]
    public async Task<IActionResult> Listele() =>
        Ok(await _salonService.Listele());

    [HttpGet("{id}")]
    public async Task<IActionResult> Getir(int id)
    {
        var salon = await _salonService.Getir(id);
        return salon is null ? NotFound() : Ok(salon);
    }

    // Oluşturma / güncelleme / silme sadece Admin
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Olustur(SalonOlusturDto dto)
    {
        var salon = await _salonService.Olustur(dto);
        return CreatedAtAction(nameof(Getir), new { id = salon.Id }, salon);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Guncelle(int id, SalonOlusturDto dto) =>
        await _salonService.Guncelle(id, dto) ? NoContent() : NotFound();

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Sil(int id) =>
        await _salonService.Sil(id) ? NoContent() : NotFound();
}