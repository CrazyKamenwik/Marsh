namespace TicketSystem.BLL.Models;

public class Message : BaseModel
{
    public Message(int ticketId, int userId)
    {
        TicketId = ticketId;
        UserId = userId;
    }

    public string? Text { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.Now;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int TicketId { get; set; }
    public Ticket Ticket { get; set; } = null!;
}