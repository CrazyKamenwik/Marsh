using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TicketSystem.Data.Models;
using TicketSystem.Data.Models.Enums;
using TicketSystem.Data.Repositories.Abstractions;
using TicketSystem.Services.Abstractions;

namespace TicketSystem.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private const int MinutesToClose = 60;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<Ticket> AddTicketAsync(Ticket ticket, CancellationToken cancellationToken)
        {
            return await _ticketRepository.CreateAsync(ticket, cancellationToken);
        }

        public async Task<Ticket?> GetTicketByIdAsync(int id, CancellationToken cancellationToken)
        {
            var ticketsByConditions =
                await _ticketRepository.GetTicketsByConditionsAsync(cancellationToken, t => t.Id == id);

            return  ticketsByConditions.FirstOrDefault();
        }

        public async Task<IEnumerable<Ticket>> GetTicketsAsync(CancellationToken cancellationToken)
        {
            return await _ticketRepository.GetAllAsync(cancellationToken);
        }

        public async Task<Ticket?> UpdateTicketAsync(Ticket ticket, CancellationToken cancellationToken)
        {
            return await _ticketRepository.UpdateAsync(ticket, cancellationToken);
        }

        public async Task<Ticket?> DeleteTicketAsync(int id, CancellationToken cancellationToken)
        {
            return await _ticketRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task CloseOpenTickets(CancellationToken cancellationToken = default)
        {
            var tickets = await _ticketRepository.GetTicketsByConditionsAsync(cancellationToken,
                t => t.TicketStatus == TicketStatus.Open
                     && t.Messages.Any()
                     && t.Messages.Last().User.UserRole == UserRole.Operator);

            foreach (var ticket in tickets)
            {
                if (ticket.CreatedAt.AddMinutes(MinutesToClose) < DateTime.Now)
                    ticket.TicketStatus = TicketStatus.Closed;
            }

            await _ticketRepository.SaveAsync();
        }
    }
}