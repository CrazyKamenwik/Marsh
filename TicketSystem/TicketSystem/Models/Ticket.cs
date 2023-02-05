using TicketSystem.Data;

namespace TicketSystem.Models
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
        private readonly ApplicationContext _context;
        private readonly object _updateLock = new();

        public Ticket() { }

        public Ticket(ApplicationContext context, User ticketCreator, User? freeOperator = null)
        {
            CreatedAt = DateTime.Now;
            TicketStatus = TicketStatus.Open;
            TicketCreator = ticketCreator;
            _context = context;
            Operator = freeOperator;
            Messages = new List<Message>();
            _timer ??= new Timer(UpdateTicketStatus, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        }

        /// <summary>
        ///  Every minute, the timer launches this method which checks
        ///  all tickets with status open and the last message from
        ///  the operator sent an hour ago and where no user response was received. These tickets we close
        /// </summary>
        private void UpdateTicketStatus(object? state)
        {
            lock(_updateLock)
            {
                var tickets = _context.Tickets
                    .Where(t => t.TicketStatus == TicketStatus.Open &&
                    t.Messages.OrderByDescending(m => m.CreatedAt)
                    .First().User.UserRole == UserRole.Operator)
                    .ToList();

                foreach (var ticket in tickets)
                {
                    if (ticket.CreatedAt.AddHours(1) < DateTime.Now)
                        ticket.TicketStatus = TicketStatus.Closed;
                }

                _context.SaveChanges();
            }
        }
    }
}