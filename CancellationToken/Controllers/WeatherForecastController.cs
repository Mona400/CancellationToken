using CancellationTokensimple.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CancellationTokensimple.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ApplicationDbContext _context;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet("no-cancellation")]
        public async Task<ActionResult> Get()
        {
            await DoSomething(Random.Shared.Next(1, 1000));
            return Ok("Done!!!");
        }
        [HttpGet("with-cancellation-source")]
        public async Task<ActionResult> GetWithCancellationSource()
        {

            using var cancellationTokenSource=new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await DoSomething(Random.Shared.Next(1, 1000), cancellationTokenSource.Token);
            return Ok("Done!!!");
        }
        [HttpGet("with-Request-cancelation")]
        public async Task<ActionResult> GetWithCancellationRequest()
        {
            await DoSomething(Random.Shared.Next(1,1000),HttpContext.RequestAborted);
            return Ok("Done!!!");
        }
        [HttpGet("with-ef-query-cancellation")]
        public async Task<ActionResult> GwtWithCancellationQuery()
        {
            await _context.Database.ExecuteSqlRawAsync("waitfor delay '00:00:10';select*from TestTables", HttpContext.RequestAborted);
            return Ok("Done!!!");
        }

        private async Task DoSomething(int number = 0, CancellationToken cancellationToken = default)
        {
            try
            {
                for (int i = 0; i < 30; i++)
                {
                    await Task.Delay(1000, cancellationToken);
                    Console.WriteLine($"*** Task is running {number} ***");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

        }
    }
}