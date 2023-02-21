using AutoMapper;
using Microsoft.Extensions.Logging;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.Models;
using TicketSystem.DAL.Abstractions;
using TicketSystem.DAL.Entities;
using TicketSystem.DAL.Entities.Enums;

namespace TicketSystem.BLL.Services;

public class TicketService : ITicketService
{
    private const int MinutesToClose = 60;
    private readonly ILogger<TicketService> _logger;
    private readonly IMapper _mapper;
    private readonly IGenericRepository<TicketEntity> _ticketRepository;
    private readonly IUserService _userService;

    public TicketService(IGenericRepository<TicketEntity> ticketRepository, IMapper mapper, IUserService userService,
        ILogger<TicketService> logger)
    {
        _mapper = mapper;
        _ticketRepository = ticketRepository;
        _userService = userService;
        _logger = logger;
    }

    public async Task<Ticket> AddTicket(Ticket ticketModel, CancellationToken cancellationToken)
    {
        var availableOperator = await _userService.GetAvailableOperator(cancellationToken);

        ticketModel.OperatorId = availableOperator?.Id ?? null;

        var ticketEntity = _mapper.Map<TicketEntity>(ticketModel);
        await _ticketRepository.Create(ticketEntity, cancellationToken);

        return _mapper.Map<Ticket>(ticketEntity);
    }

    public async Task<Ticket> GetTicketById(int id, CancellationToken cancellationToken)
    {
        var ticketEntity = await _ticketRepository.GetByIdWithInclude(id, cancellationToken,
            t => t.Messages,
            t => t.Operator,
            t => t.TicketCreator);

        return _mapper.Map<Ticket>(ticketEntity);
    }

    public async Task<IEnumerable<Ticket>> GetTickets(CancellationToken cancellationToken)
    {
        var ticketsEntity = await _ticketRepository.GetWithInclude(cancellationToken, null, null,
            t => t.Messages,
            t => t.Operator,
            t => t.TicketCreator
        );

        return _mapper.Map<IEnumerable<Ticket>>(ticketsEntity);
    }

    public async Task<Ticket> UpdateTicket(Ticket ticket, CancellationToken cancellationToken)
    {
        var ticketEntity = _mapper.Map<TicketEntity>(ticket);

        await _ticketRepository.Update(cancellationToken, ticketEntity);

        return _mapper.Map<Ticket>(ticketEntity);
    }

    public async Task DeleteTicket(int id, CancellationToken cancellationToken)
    {
        await _ticketRepository.Remove(id, cancellationToken);
    }

    public async Task CloseOpenTickets(CancellationToken cancellationToken = default)
    {
        var ticketsEntity = await _ticketRepository.GetWithInclude(cancellationToken,
            t => t is { TicketStatus: TicketStatusEnumEntity.Open, OperatorId: { } }
                 && t.Messages.Last().CreatedAt.AddMinutes(MinutesToClose) < DateTime.Now, // TODO #1: Move this check
            null,
            t => t.Messages);

        foreach (var ticket in ticketsEntity)
        {
            _logger.LogInformation("Close ticket with id {ticket.Id}", ticket.Id);
            ticket.TicketStatus = TicketStatusEnumEntity.Closed;
        }

        await _ticketRepository.Update(cancellationToken, ticketsEntity);
    }
}