using TicketSystem.BLL.Services.Abstractions;
using TicketSystem.DAL.Entities;
using TicketSystem.DAL.Entities.Enums;
using TicketSystem.DAL.Repositories.Abstractions;

namespace TicketSystem.BLL.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private const int MinutesToClose = 60;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<TicketEntity> AddTicketAsync(TicketEntity ticket, CancellationToken cancellationToken)
        {
            return await _ticketRepository.CreateAsync(ticket, cancellationToken);
        }

        public async Task<TicketEntity?> GetTicketByIdAsync(int id, CancellationToken cancellationToken)
        {
            var ticketsByConditions =
                await _ticketRepository.GetTicketsByConditionsAsync(cancellationToken, t => t.Id == id);

            return ticketsByConditions.FirstOrDefault();
        }

        public async Task<IEnumerable<TicketEntity>> GetTicketsAsync(CancellationToken cancellationToken)
        {
            return await _ticketRepository.GetAllAsync(cancellationToken);
        }

        public async Task<TicketEntity?> UpdateTicketAsync(TicketEntity ticket, CancellationToken cancellationToken)
        {
            return await _ticketRepository.UpdateAsync(ticket, cancellationToken);
        }

        public async Task<TicketEntity?> DeleteTicketAsync(int id, CancellationToken cancellationToken)
        {
            return await _ticketRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task CloseOpenTickets(CancellationToken cancellationToken = default)
        {
            var tickets = await _ticketRepository.GetTicketsByConditionsAsync(cancellationToken,
                t => t.TicketStatus == TicketStatusEnumEntity.Open
                     && t.Messages.Any()
                     && t.Messages.Last().User.UserRole.Name == "Operator");

            foreach (var ticket in tickets)
            {
                if (ticket.CreatedAt.AddMinutes(MinutesToClose) < DateTime.Now)
                    ticket.TicketStatus = TicketStatusEnumEntity.Closed;
            }

            await _ticketRepository.SaveAsync();
        }
    }
}