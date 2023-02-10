using TicketSystem.BLL.Models.Abstractions;

namespace TicketSystem.BLL.Models;

public class BaseModel : IBaseModel
{
    public int Id { get; set; }
}