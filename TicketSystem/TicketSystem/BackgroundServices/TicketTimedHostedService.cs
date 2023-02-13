using TicketSystem.BLL.Abstractions.Services;

namespace TicketSystem.API.BackgroundServices;

public class TicketTimedHostedService : BackgroundService
{
    private readonly ILogger<TicketTimedHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private Timer? _timer;

    public TicketTimedHostedService(ILogger<TicketTimedHostedService> logger,
        IServiceProvider services)
    {
        _serviceProvider = services;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Scoped service running");

        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        _logger.LogInformation("Scoped service is working");

        using (var scope = _serviceProvider.CreateScope())
        {
            var ticketService = scope.ServiceProvider.GetRequiredService<ITicketService>();
            ticketService.CloseOpenTickets();
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Scoped service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        await base.StopAsync(cancellationToken);
    }
}