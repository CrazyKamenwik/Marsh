using AutoMapper;
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
    private readonly ITicketRepository _ticketRepository;

    public TicketService(ITicketRepository ticketRepository, IMapper mapper)
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

    public async Task<TicketModel?> GetTicketByIdAsync(int id, CancellationToken cancellationToken)
    {
        var ticketsEntityByConditions =
            await _ticketRepository.GetTicketsByConditionsAsync(cancellationToken,
                t => t.Id == id,
                includeProperties: "TicketCreator,Operator,Messages");
        var ticketEntity = ticketsEntityByConditions.FirstOrDefault();

        return ticketEntity == null ? null : _mapper.Map<TicketModel>(ticketEntity);
    }

    public async Task<IEnumerable<TicketModel>> GetTicketsAsync(CancellationToken cancellationToken)
    {
        var ticketsEntity = await _ticketRepository.GetTicketsByConditionsAsync(cancellationToken,
            includeProperties: "TicketCreator,Operator,Messages");

        return _mapper.Map<IEnumerable<TicketModel>>(ticketsEntity);
    }

    public async Task<TicketModel?> UpdateTicketAsync(TicketModel ticketModel, CancellationToken cancellationToken)
    {
        var ticketsEntity =
            await _ticketRepository.GetTicketsByConditionsAsync(cancellationToken, t => t.Id == ticketModel.Id);
        if (ticketsEntity.FirstOrDefault() == null)
            return null;

        var ticketEntity = _mapper.Map<TicketEntity>(ticketModel);
        ticketEntity = await _ticketRepository.UpdateAsync(ticketEntity, cancellationToken);

        return _mapper.Map<TicketModel>(ticketEntity);
    }

    public async Task<TicketModel?> DeleteTicketAsync(int id, CancellationToken cancellationToken)
    {
        var ticketsEntity = await _ticketRepository.GetTicketsByConditionsAsync(cancellationToken,
            t => t.Id == id,
            includeProperties: "TicketCreator,Operator,Messages");
        var ticketEntity = ticketsEntity.FirstOrDefault();
        if (ticketEntity == null)
            return null;

        await _ticketRepository.DeleteAsync(ticketEntity, cancellationToken);

        return _mapper.Map<TicketModel>(ticketEntity);
    }

    public async Task CloseOpenTickets(CancellationToken cancellationToken = default)
    {
        var ticketsEntity = await _ticketRepository.GetTicketsByConditionsAsync(cancellationToken,
            t => t.TicketStatus == TicketStatusEnumEntity.Open
                 && t.Messages.Any()
                 && t.Messages.Last().User.UserRole.Name == "Operator");

        foreach (var ticketEntity in ticketsEntity)
            if (ticketEntity.CreatedAt.AddMinutes(MinutesToClose) < DateTime.Now)
                ticketEntity.TicketStatus = TicketStatusEnumEntity.Closed;

        await _ticketRepository.SaveAsync();
    }
}