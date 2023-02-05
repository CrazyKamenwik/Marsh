using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Models;

namespace TicketSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly ApplicationContext _context;

        public MessageController(ILogger<MessageController> logger)
        {
            _context = new ApplicationContext();
            _logger = logger;
        }

        [HttpPost(Name = "PostMessage")]
        public Message Post(Message postMessage)
        {
            if (postMessage.Ticket == null)
                throw new BadHttpRequestException("Ticket can't be null", 400);
            else
            {
                _context.Messages.Add(postMessage);
                _context.SaveChanges();
                return postMessage;
            }
        }
    }
}
