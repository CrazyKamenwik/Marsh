using Microsoft.EntityFrameworkCore;
using TicketSystem.Data.Models;
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

        public async Task<Ticket> AddTicketAsync(Ticket ticket)
        {
            return await _ticketRepository.CreateAsync(ticket);
        }

        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            return await _ticketRepository.GetUsersByConditionsAsync(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Ticket>> GetTicketsAsync()
        {
            return await _ticketRepository.GetAllAsync();
        }

        public async Task<Ticket?> UpdateTicketAsync(Ticket ticket)
        {
            return await _ticketRepository.UpdateAsync(ticket);
        }

        public async Task<Ticket?> DeleteTicketAsync(int id)
        {
            return await _ticketRepository.DeleteAsync(id);
        }
    }
}