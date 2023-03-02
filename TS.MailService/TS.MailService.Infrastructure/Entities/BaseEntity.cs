using TS.MailService.Infrastructure.Abstraction;

namespace TS.MailService.Infrastructure.Entities;

public class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; }
}