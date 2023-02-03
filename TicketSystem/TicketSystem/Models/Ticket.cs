﻿namespace TicketSystem.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Message>? Messages { get; set; }
        public TicketStatus TicketStatus { get; set; }
    }
}