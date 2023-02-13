using AutoMapper;
using TicketSystem.BLL.Abstractions.MessagesStrategy;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.Models;

namespace TicketSystem.BLL.Services;

public class MessageService : IMessageService
{
    private readonly IMapper _mapper;
    private readonly IEnumerable<IMessageStrategy> _messageStrategies;
    private readonly IUserService _userService;

    public MessageService(IUserService userService, IMapper mapper, IEnumerable<IMessageStrategy> messageStrategy)
    {
        _userService = userService;
        _mapper = mapper;
        _messageStrategies = messageStrategy;
    }

    public async Task<Message> AddMessageAsync(Message message, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByIdAsync(message.UserId, cancellationToken);

        message = await _messageStrategies.First(x => x.IsApplicable(user.UserRole.Name))
            .AddMessageAsync(message, user, cancellationToken);

        return _mapper.Map<Message>(message);
    }
}