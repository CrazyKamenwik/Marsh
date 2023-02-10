using AutoMapper;
using TicketSystem.BLL.Exceptions;
using TicketSystem.BLL.Models;
using TicketSystem.BLL.Services.Abstractions;
using TicketSystem.DAL.Entities;
using TicketSystem.DAL.Entities.Enums;
using TicketSystem.DAL.Repositories.Abstractions;

namespace TicketSystem.BLL.Services;

public class TicketService : ITicketService
{
    private const int MinutesToClose = 60;
    private readonly IMapper _mapper;
    private readonly IGenericRepository<TicketEntity> _ticketRepository;

    public TicketService(IGenericRepository<TicketEntity> ticketRepository, IMapper mapper)
    {
        _mapper = mapper;
        _ticketRepository = ticketRepository;
    }

    public async Task<TicketModel> AddTicketAsync(TicketModel ticketModel, CancellationToken cancellationToken)
    {
        ticketModel.Messages = new List<MessageModel>();
        var ticketEntity = _mapper.Map<TicketEntity>(ticketModel);
        await _ticketRepository.CreateAsync(ticketEntity, cancellationToken);

        return _mapper.Map<TicketModel>(ticketEntity);
    }

    public async Task<TicketModel> GetTicketByIdAsync(int id, CancellationToken cancellationToken)
    {
        var ticketEntity = await _ticketRepository.GetByIdWithIncludeAsync(id, cancellationToken,
            t => t.Messages,
            t => t.Operator,
            t => t.TicketCreator);

        if (ticketEntity == null)
            throw new NotFoundException($"Ticket with id {id} not found");

        return _mapper.Map<TicketModel>(ticketEntity);
    }

    public async Task<IEnumerable<TicketModel>> GetTicketsAsync(CancellationToken cancellationToken)
    {
        var ticketsEntity = await _ticketRepository.GetAsync();

        return _mapper.Map<IEnumerable<TicketModel>>(ticketsEntity);
    }

    public async Task<TicketModel> UpdateTicketAsync(TicketModel ticketModel, CancellationToken cancellationToken)
    {
        var ticketEntity =
            await _ticketRepository.GetByIdAsync(ticketModel.Id, cancellationToken);

        if (ticketEntity == null)
            throw new NotFoundException($"Ticket with id {ticketModel.Id} not found");
        
        await _ticketRepository.UpdateAsync(ticketEntity, cancellationToken);

        return _mapper.Map<TicketModel>(ticketEntity);
    }

    public async Task<TicketModel> DeleteTicketAsync(int id, CancellationToken cancellationToken)
    {
        var ticketEntity = await _ticketRepository.GetByIdAsync(id, cancellationToken);
        if (ticketEntity == null)
            throw new NotFoundException($"Ticket with id {id} not found");

        await _ticketRepository.RemoveAsync(ticketEntity, cancellationToken);

        return _mapper.Map<TicketModel>(ticketEntity);
    }

    public async Task CloseOpenTickets(CancellationToken cancellationToken = default)
    {
        //var ticketsEntity = await _ticketRepository.GetTicketsByConditionsAsync(cancellationToken,
        //    t => t.TicketStatus == TicketStatusEnumEntity.Open
        //         && t.Messages.Any()
        //         && t.Messages.Last().User.UserRole.Name == "Operator");

        var ticketsEntity = _ticketRepository.GetWithInclude(t => t.TicketStatus == TicketStatusEnumEntity.Open
                                                                  && t.Messages.Any()
                                                                  && t.Messages.Last().User.UserRole.Name ==
                                                                  "Operator");

        foreach (var ticketEntity in ticketsEntity)
            if (ticketEntity.CreatedAt.AddMinutes(MinutesToClose) < DateTime.Now)
                ticketEntity.TicketStatus = TicketStatusEnumEntity.Closed;

        await _ticketRepository.SaveAsync();
    }
}