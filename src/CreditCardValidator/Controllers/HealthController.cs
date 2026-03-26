using CreditCardValidator.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CreditCardValidator.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<HealthController> _logger;

    public HealthController(AppDbContext dbContext, ILogger<HealthController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var health = new HealthCheckResponse
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow
        };

        try
        {
            await _dbContext.Database.CanConnectAsync();
            health.Database = "Connected";
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Database health check failed.");
            health.Status = "Degraded";
            health.Database = "Unavailable";
        }

        var statusCode = health.Status == "Healthy"
            ? StatusCodes.Status200OK
            : StatusCodes.Status503ServiceUnavailable;

        return StatusCode(statusCode, health);
    }

    private class HealthCheckResponse
    {
        public string Status { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}