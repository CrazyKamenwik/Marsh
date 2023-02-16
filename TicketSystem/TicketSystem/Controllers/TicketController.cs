using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.API.ViewModels.Tickets;
using TicketSystem.BLL.Abstractions.Services;

namespace TicketSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    private readonly ILogger<TicketController> _logger;
    private readonly IMapper _mapper;
    private readonly ITicketService _ticketService;

    public TicketController(ITicketService ticketService, ILogger<TicketController> logger, IMapper mapper)
    {
        _mapper = mapper;
        _ticketService = ticketService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<TicketViewModel>> Get(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get all Tickets");
        var ticketsModel = await _ticketService.GetTickets(cancellationToken);

        return _mapper.Map<IEnumerable<TicketViewModel>>(ticketsModel);
    }

    [HttpGet("{id}")]
    public async Task<TicketViewModel> Get(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get Ticket by id {id}", id);
        var ticketModel = await _ticketService.GetTicketById(id, cancellationToken);

        return _mapper.Map<TicketViewModel>(ticketModel);
    }
}