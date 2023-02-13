using TicketSystem.DAL.Abstractions;

namespace TicketSystem.DAL.Entities;

public class BaseEntity : IBaseEntity
{
    public int Id { get; set; }
}