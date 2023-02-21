using AutoMapper;
using TicketSystem.BLL.Abstractions.MessagesStrategy;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.Constants;
using TicketSystem.BLL.Models;

namespace TicketSystem.BLL.Services;

public class MessageService : IMessageService
{
    private readonly IEnumerable<IMessageStrategy> _messageStrategies;
    private readonly IUserService _userService;

    public MessageService(IUserService userService, IEnumerable<IMessageStrategy> messageStrategy)
    {
        _userService = userService;
        _messageStrategies = messageStrategy;
    }

    public async Task<Message> AddMessage(Message message, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserById(message.UserId, cancellationToken);

        ThrowExceptionIfOperatorHasIncorrectTicketId(user, message);

        message = await _messageStrategies.First(x => x.IsApplicable(user.UserRole.Name))
            .AddMessage(message, user, cancellationToken);

        return message;
    }

    private static void ThrowExceptionIfOperatorHasIncorrectTicketId(User user, Message message)
    {
        if (user.UserRole.Name == RolesConstants.Operator && message.TicketId == 0)
            throw new ArgumentException(
                "Operator needs to enter ticketId, in which it is necessary to add the message");
    }
}