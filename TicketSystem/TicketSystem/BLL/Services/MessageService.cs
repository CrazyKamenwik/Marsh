using AutoMapper;
using TicketSystem.BLL.Models;
using TicketSystem.BLL.Services.Abstractions;
using TicketSystem.DAL.Entities;
using TicketSystem.DAL.Repositories.Abstractions;

namespace TicketSystem.BLL.Services;

public class MessageService : IMessageService
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<MessageEntity> _messageRepository;
    private readonly ITicketService _ticketService;
    private readonly IUserService _userService;

    public MessageService(IGenericRepository<MessageEntity> messageRepository, IUserService userService,
        ITicketService ticketService, IMapper mapper)
    {
        _messageRepository = messageRepository;
        _userService = userService;
        _ticketService = ticketService;
        _mapper = mapper;
    }

    public async Task<MessageModel> AddMessageAsync(MessageModel messageModel, CancellationToken cancellationToken)
    {
        var userModel = await _userService.GetUserByIdAsync(messageModel.UserId, cancellationToken);

        TicketModel ticketModel;

        if (userModel.UserRole.Name == "Operator")
            ticketModel = await _ticketService.GetTicketByIdAsync(messageModel.TicketId, cancellationToken);
        else
            ticketModel = await _ticketService.GetOrCreateOpenTicket(userModel, cancellationToken);

        messageModel.TicketId = ticketModel.Id;
        messageModel.CreatedAt = DateTime.UtcNow;
        var messageEntity = _mapper.Map<MessageEntity>(messageModel);
        await _messageRepository.CreateAsync(messageEntity, cancellationToken);

        return _mapper.Map<MessageModel>(messageEntity);
    }
}