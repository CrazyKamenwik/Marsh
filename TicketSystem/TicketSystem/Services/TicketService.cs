using Microsoft.EntityFrameworkCore;
using TicketSystem.Data.Models;
using TicketSystem.Data.Repositories;

namespace TicketSystem.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
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

        public async Task<Ticket?> UpdateTicketAsync(int id, Ticket ticket)
        {
            if (id != ticket.Id)
                return null;

            return await _ticketRepository.UpdateAsync(id, ticket);
        }

        public async Task<Ticket?> DeleteTicketAsync(int id)
        {
            return await _ticketRepository.DeleteAsync(id);
        }

        public async Task<int> CloseOpenTickets(int minutesToClose)
        {
            return await _ticketRepository.CloseOpenTickets(minutesToClose);
        }
    }
}