using BiletSatis.Application.DTOs;
using BiletSatis.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BiletSatis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("kayit")]
    public async Task<IActionResult> KayitOl(KayitDto dto)
    {
        var sonuc = await _authService.KayitOl(dto);

        if (sonuc is null)
            return BadRequest("Bu e-posta zaten kayıtlı.");

        return Ok(sonuc);
    }

    [HttpPost("giris")]
    public async Task<IActionResult> GirisYap(GirisDto dto)
    {
        var sonuc = await _authService.GirisYap(dto);

        if (sonuc is null)
            return Unauthorized("E-posta veya şifre hatalı.");

        return Ok(sonuc);
    }
}