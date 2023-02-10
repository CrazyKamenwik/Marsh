using TicketSystem.BLL.Models.Enums;

namespace TicketSystem.BLL.Models;

public class TicketModel : BaseModel
{
    public DateTime CreatedAt { get; set; }
    public TicketStatusEnumModel TicketStatus { get; set; }

    public int TicketCreatorId { get; set; }
    public UserModel TicketCreator { get; set; } = null!;

    public int? OperatorId { get; set; }
    public UserModel? Operator { get; set; }

    public ICollection<MessageModel> Messages { get; set; } = null!;
}