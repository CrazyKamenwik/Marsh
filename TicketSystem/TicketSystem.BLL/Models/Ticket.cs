using TicketSystem.BLL.Enums;

namespace TicketSystem.BLL.Models;

public class Ticket : BaseModel
{
    public Ticket(int ticketCreatorId)
    {
        TicketCreatorId = ticketCreatorId;
    }

    public DateTime CreatedAt { get; set; }
    public TicketStatusEnumModel TicketStatus { get; set; } = TicketStatusEnumModel.Open;

    public int TicketCreatorId { get; set; }
    public User TicketCreator { get; set; } = null!;

    public int? OperatorId { get; set; }
    public User? Operator { get; set; }

    public ICollection<Message> Messages { get; init; } = new List<Message>();
}