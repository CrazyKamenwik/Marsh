using TicketSystem.Services;

namespace TicketSystem.Data.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public TicketStatus TicketStatus { get; set; }

        public int TicketCreatorId { get; set; }
        public User TicketCreator { get; set; } = null!;

        public int? OperatorId { get; set; }
        public User? Operator { get; set; }

        public ICollection<Message> Messages { get; set; } = null!;

        private static Timer? _timer;
        private readonly ITicketService _ticketService;
        private readonly object _updateLock = new();

        public Ticket()
        {
        }

        public Ticket(ITicketService ticketService, User ticketCreator, User? freeOperator = null)
        {
            CreatedAt = DateTime.Now;
            TicketStatus = TicketStatus.Open;
            TicketCreator = ticketCreator;
            _ticketService = ticketService;
            Operator = freeOperator;
            Messages = new List<Message>();
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
                _ticketService.CloseOpenTickets(60);
            }
        }
    }
}