using BiletSatis.Application.DTOs;
using BiletSatis.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiletSatis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KoltukController : ControllerBase
{
    private readonly IKoltukService _koltukService;

    public KoltukController(IKoltukService koltukService) => _koltukService = koltukService;

    // /api/koltuk?bolumId=1
    [HttpGet]
    public async Task<IActionResult> Listele([FromQuery] int bolumId) =>
        Ok(await _koltukService.Listele(bolumId));

    [HttpPost("uret")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Uret(KoltukUretDto dto)
    {
        var adet = await _koltukService.Uret(dto);

        if (adet == 0)
            return BadRequest("Bölüm bulunamadı, bölüm ayakta tipinde, aralık geçersiz veya bu blok zaten üretilmiş.");

        return Ok(new { Mesaj = $"{adet} koltuk oluşturuldu.", Adet = adet });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Sil(int id) =>
        await _koltukService.Sil(id) ? NoContent() : NotFound();
}