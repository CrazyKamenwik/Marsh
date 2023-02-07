using TicketSystem.Services;
using TicketSystem.Services.Abstractions;

namespace TicketSystem.BackgroundService
{
    public class TicketTimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<TicketTimedHostedService> _logger;
        private readonly ITicketService _ticketService;
        private Timer? _timer = null;

        public TicketTimedHostedService(ILogger<TicketTimedHostedService> logger, ITicketService ticketService)
        {
            _ticketService = ticketService;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            _logger.LogInformation("Ticket Timed Hosted Service is working");

            await _ticketService.CloseOpenTickets();
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
