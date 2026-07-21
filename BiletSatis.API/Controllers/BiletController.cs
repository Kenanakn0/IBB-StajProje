using System.Security.Claims;
using BiletSatis.Application.DTOs;
using BiletSatis.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiletSatis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BiletController : ControllerBase
{
    private readonly IBiletService _service;

    public BiletController(IBiletService service) => _service = service;

    private int KullaniciId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
               ?? User.FindFirstValue("sub")!);

    [HttpPost("odeme")]
    public async Task<IActionResult> Odeme(OdemeDto dto)
    {
        var (bilet, hata) = await _service.Odeme(KullaniciId, dto);
        return bilet is null ? BadRequest(hata) : Ok(bilet);
    }

    [HttpGet("biletlerim")]
    public async Task<IActionResult> Biletlerim() =>
        Ok(await _service.Biletlerim(KullaniciId));
}