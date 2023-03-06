using TS.MailService.Infrastructure.Abstraction;

namespace TS.MailService.Infrastructure.Entities;

public class BaseEntity : IBaseEntity
{
    public int Id { get; set; }
}