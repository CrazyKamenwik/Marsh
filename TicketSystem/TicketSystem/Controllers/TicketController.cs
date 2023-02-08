using Microsoft.AspNetCore.Mvc;
using TicketSystem.BLL.Services.Abstractions;
using TicketSystem.DAL.Entities;

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
        public async Task<IEnumerable<TicketEntity>> Get(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get all Tickets");
            return await _ticketService.GetTicketsAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<TicketEntity?> Get(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Ticket by id {id}", id);
            return await _ticketService.GetTicketByIdAsync(id, cancellationToken);
        }
    }
}