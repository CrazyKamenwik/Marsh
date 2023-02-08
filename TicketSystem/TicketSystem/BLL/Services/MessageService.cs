﻿using AutoMapper;
using TicketSystem.BLL.Models;
using TicketSystem.BLL.Models.Enums;
using TicketSystem.BLL.Services.Abstractions;

namespace TicketSystem.BLL.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUserService _userService;
        private readonly ITicketService _ticketService;
        private readonly IMapper _mapper;

        public MessageService(IUserService userService, ITicketService ticketService, IMapper mapper)
        {
            _mapper = mapper;
            _userService = userService;
            _ticketService = ticketService;
        }

        public async Task<MessageModel?> AddMessageAsync(MessageModel message, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByIdAsync(message.UserId, cancellationToken);
            if (user == null)
                return null;

            TicketModel ticket;
            var freeOperator = await _userService.GetNotBusyOperator(cancellationToken);

            // If the user has an open ticket, the message will be written into it
            if (user.Tickets == null || user.Tickets.Count == 0 && user.Tickets.OrderByDescending(t => t.CreatedAt).First().TicketStatus == TicketStatusEnumModel.Open)
            {
                ticket = user.Tickets!.OrderByDescending(t => t.CreatedAt).First();
            }
            else
            {
                ticket = new TicketModel()
                {
                    TicketCreator = user,
                    Operator = freeOperator
                };
                await _ticketService.AddTicketAsync(ticket, cancellationToken);
            }

            message.Ticket = ticket;
            message.CreatedAt = DateTime.UtcNow;
            ticket.Messages!.Add(message);
            await _ticketService.UpdateTicketAsync(ticket, cancellationToken);

            return message;
        }
    }
}