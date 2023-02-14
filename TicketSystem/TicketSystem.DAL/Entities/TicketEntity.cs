using TicketSystem.DAL.Entities.Enums;

namespace TicketSystem.DAL.Entities;

public class TicketEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public TicketStatusEnumEntity TicketStatus { get; set; }

    public int TicketCreatorId { get; set; }
    public UserEntity TicketCreator { get; set; } = null!;

    public int? OperatorId { get; set; }
    public UserEntity? Operator { get; set; }

    public ICollection<MessageEntity> Messages { get; set; } = null!;
}