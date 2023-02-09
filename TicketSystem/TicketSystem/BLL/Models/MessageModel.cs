﻿namespace TicketSystem.BLL.Models;

public class MessageModel
{
    public int Id { get; set; }
    public string? Text { get; set; }
    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }
    public UserModel User { get; set; } = null!;

    public int TicketId { get; set; }
    public TicketModel Ticket { get; set; } = null!;
}