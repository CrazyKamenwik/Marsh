using Microsoft.EntityFrameworkCore;
using TicketSystem.Data.Models;
using TicketSystem.Data.Models.Enums;
using TicketSystem.Data.Repositories.Abstractions;
using TicketSystem.Services.Abstractions;

namespace TicketSystem.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private static Timer? _timer;
        private readonly object _updateLock;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
            _updateLock = new object();
            _timer ??= new Timer(UpdateTicketStatus, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
        }

        /// <summary>
        ///  Every 5 minutes, the timer launches this method which checks
        ///  all tickets with status open and the last message from
        ///  the operator sent an 1 hour ago and where no user response was received. These tickets we close
        /// </summary>
        private void UpdateTicketStatus(object? state)
        {
            lock (_updateLock)
            {
                _ticketRepository.CloseOpenTickets(60);
            }
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
    }
}