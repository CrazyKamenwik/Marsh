using Microsoft.AspNetCore.Mvc;
using TicketSystem.Data.Models;
using TicketSystem.Services.Abstractions;

namespace TicketSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly ILogger<TicketController> _logger;

        public TicketController(ITicketService ticketService, ILogger<TicketController> logger)
        {
            _ticketService = ticketService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Ticket>> Get(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get all Tickets");
            return await _ticketService.GetTicketsAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<Ticket?> Get(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Ticket by id {id}", id);
            return await _ticketService.GetTicketByIdAsync(id, cancellationToken);
        }
    }
}