using TicketSystem.BLL.Abstractions.Models;

namespace TicketSystem.BLL.Models;

public class BaseModel : IBaseModel
{
    public int Id { get; set; }
}