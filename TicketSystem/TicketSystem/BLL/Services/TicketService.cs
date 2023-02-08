using AutoMapper;
using TicketSystem.BLL.Services.Abstractions;
using TicketSystem.BLL.Models;
using TicketSystem.DAL.Entities;
using TicketSystem.DAL.Repositories.Abstractions;
using TicketSystem.DAL.Entities.Enums;

namespace TicketSystem.BLL.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private const int MinutesToClose = 60;
        private readonly IMapper _mapper;

        public TicketService(ITicketRepository ticketRepository, IMapper mapper)
        {
            _mapper = mapper;
            _ticketRepository = ticketRepository;
        }

        public async Task<TicketModel> AddTicketAsync(TicketModel ticket, CancellationToken cancellationToken)
        {
            var ticketEntity = _mapper.Map<TicketEntity>(ticket);
            await _ticketRepository.CreateAsync(ticketEntity, cancellationToken);
            return _mapper.Map<TicketModel>(ticketEntity);
        }

        public async Task<TicketModel?> GetTicketByIdAsync(int id, CancellationToken cancellationToken)
        {
            var ticketsEntityByConditions =
                await _ticketRepository.GetTicketsByConditionsAsync(cancellationToken, t => t.Id == id);
            var ticketEntity = ticketsEntityByConditions.FirstOrDefault();

            return ticketEntity == null ? null : _mapper.Map<TicketModel>(ticketEntity);
        }

        public async Task<IEnumerable<TicketModel>> GetTicketsAsync(CancellationToken cancellationToken)
        {
            var ticketsEntity = await _ticketRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TicketModel>>(ticketsEntity);
        }

        public async Task<TicketModel?> UpdateTicketAsync(TicketModel ticket, CancellationToken cancellationToken)
        {
            var ticketEntity = _mapper.Map<TicketEntity>(ticket);
            ticketEntity = await _ticketRepository.UpdateAsync(ticketEntity, cancellationToken);
            return ticketEntity == null ? null : _mapper.Map<TicketModel>(ticketEntity);
        }

        public async Task<TicketModel?> DeleteTicketAsync(int id, CancellationToken cancellationToken)
        {
            var ticketEntity = await _ticketRepository.DeleteAsync(id, cancellationToken);
            return ticketEntity == null ? null : _mapper.Map<TicketModel>(ticketEntity);
        }

        public async Task CloseOpenTickets(CancellationToken cancellationToken = default)
        {
            var ticketsEntity = await _ticketRepository.GetTicketsByConditionsAsync(cancellationToken,
                t => t.TicketStatus == TicketStatusEnumEntity.Open
                     && t.Messages.Any()
                     && t.Messages.Last().User.UserRole.Name == "Operator");

            foreach (var ticketEntity in ticketsEntity)
            {
                if (ticketEntity.CreatedAt.AddMinutes(MinutesToClose) < DateTime.Now)
                    ticketEntity.TicketStatus = TicketStatusEnumEntity.Closed;
            }

            await _ticketRepository.SaveAsync();
        }
    }
}