using BiletSatis.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BiletSatis.API.BackgroundJobs;

public class RezervasyonTemizleyici : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RezervasyonTemizleyici> _logger;
    private static readonly TimeSpan CalismaAraligi = TimeSpan.FromMinutes(1);

    public RezervasyonTemizleyici(
        IServiceProvider serviceProvider,
        ILogger<RezervasyonTemizleyici> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<BiletSatisDbContext>();

                var simdi = DateTime.UtcNow;

                var silinen = await db.Rezervasyonlar
                    .Where(r => r.SonGecerlilik <= simdi)
                    .ExecuteDeleteAsync(stoppingToken);

                if (silinen > 0)
                    _logger.LogInformation("{Adet} adet süresi dolmuş rezervasyon temizlendi.", silinen);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rezervasyon temizleme sırasında hata oluştu.");
            }

            await Task.Delay(CalismaAraligi, stoppingToken);
        }
    }
}