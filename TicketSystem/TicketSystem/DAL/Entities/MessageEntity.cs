namespace TicketSystem.DAL.Entities;

public class MessageEntity : BaseEntity
{
    public string? Text { get; set; }
    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }
    public UserEntity User { get; set; } = null!;

    public int TicketId { get; set; }
    public TicketEntity Ticket { get; set; } = null!;
}