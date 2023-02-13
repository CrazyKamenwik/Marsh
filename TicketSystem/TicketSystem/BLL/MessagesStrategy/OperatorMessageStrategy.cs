using AutoMapper;
using TicketSystem.BLL.Abstractions.MessagesStrategy;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.Models;
using TicketSystem.DAL.Entities;
using TicketSystem.DAL.Entities.Abstractions;

namespace TicketSystem.BLL.MessagesStrategy;

public class OperatorMessageStrategy : MessageStrategy
{
    public OperatorMessageStrategy(IUserService userService, ITicketService ticketService,
        IGenericRepository<MessageEntity> messageRepository, IMapper mapper)
        : base(userService, ticketService, messageRepository, mapper)
    {
    }

    public override async Task<Message> AddMessageAsync(Message message, CancellationToken cancellationToken)
    {
        await UserService.GetUserByIdAsync(message.UserId, cancellationToken);
        await TicketService.GetTicketByIdAsync(message.TicketId, cancellationToken);
        var messageEntity = Mapper.Map<MessageEntity>(message);
        await MessageRepository.CreateAsync(messageEntity, cancellationToken);

        return Mapper.Map<Message>(message);
    }
}