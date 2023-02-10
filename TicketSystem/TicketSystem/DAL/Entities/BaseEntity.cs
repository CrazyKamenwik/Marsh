using TicketSystem.DAL.Entities.Abstractions;

namespace TicketSystem.DAL.Entities;

public class BaseEntity : IBaseEntity
{
    public int Id { get; set; }
}