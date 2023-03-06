using TS.MailService.Domain.Abstraction;

namespace TS.MailService.Domain.Models;

public class BaseModel : IBaseModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
}