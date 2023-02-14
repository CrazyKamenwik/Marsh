using AutoMapper;
using Microsoft.Extensions.Logging;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.Exceptions;
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

    public async Task<Ticket> AddTicketAsync(Ticket ticketModel, CancellationToken cancellationToken)
    {
        var availableOperator = _userService.GetAvailableOperator(cancellationToken);

        ticketModel.OperatorId = availableOperator?.Id ?? null;

        var ticketEntity = _mapper.Map<TicketEntity>(ticketModel);
        await _ticketRepository.CreateAsync(ticketEntity, cancellationToken);

        return _mapper.Map<Ticket>(ticketEntity);
    }

    public async Task<Ticket> GetTicketByIdAsync(int id, CancellationToken cancellationToken)
    {
        var ticketEntity = await _ticketRepository.GetByIdWithIncludeAsync(id, cancellationToken,
            t => t.Messages,
            t => t.Operator,
            t => t.TicketCreator);

        if (ticketEntity == null)
            throw new NotFoundException($"Ticket with id {id} not found");

        return _mapper.Map<Ticket>(ticketEntity);
    }

    public async Task<IEnumerable<Ticket>> GetTicketsAsync(CancellationToken cancellationToken)
    {
        var ticketsEntity = await _ticketRepository.GetWithInclude(cancellationToken, false, null, null,
            t => t.Messages,
            t => t.Operator,
            t => t.TicketCreator
        );

        return _mapper.Map<IEnumerable<Ticket>>(ticketsEntity);
    }

    public async Task<Ticket> UpdateTicketAsync(Ticket ticketModel, CancellationToken cancellationToken)
    {
        var ticketEntity =
            await _ticketRepository.GetByIdWithIncludeAsync(ticketModel.Id, cancellationToken);

        if (ticketEntity == null)
            throw new NotFoundException($"Ticket with id {ticketModel.Id} not found");

        await _ticketRepository.UpdateAsync(ticketEntity, cancellationToken);

        return _mapper.Map<Ticket>(ticketEntity);
    }

    public async Task<Ticket> DeleteTicketAsync(int id, CancellationToken cancellationToken)
    {
        var ticketEntity = await _ticketRepository.GetByIdWithIncludeAsync(id, cancellationToken);
        if (ticketEntity == null)
            throw new NotFoundException($"Ticket with id {id} not found");

        await _ticketRepository.RemoveAsync(ticketEntity, cancellationToken);

        return _mapper.Map<Ticket>(ticketEntity);
    }

    public async Task CloseOpenTickets(CancellationToken cancellationToken = default)
    {
        var ticketsEntity = await _ticketRepository.GetWithInclude(cancellationToken,
            true,
            t => t is { TicketStatus: TicketStatusEnumEntity.Open, OperatorId: { } }
                 && t.Messages.Last().CreatedAt.AddMinutes(MinutesToClose) < DateTime.Now,
            null,
            t => t.Messages);

        foreach (var ticket in ticketsEntity)
        {
            _logger.LogInformation("Close ticket with id {ticket.Id}", ticket.Id);
            ticket.TicketStatus = TicketStatusEnumEntity.Closed;
        }

        await _ticketRepository.SaveAsync();
    }
}