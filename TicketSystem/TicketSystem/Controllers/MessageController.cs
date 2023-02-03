using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Models;

namespace TicketSystem.Controllers
{
    public class MessageController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<MessageController> _logger;

        public MessageController(ILogger<MessageController> logger)
        {
            _context = new ApplicationContext();
            _logger = logger;
        }

        [HttpGet(Name = "PostMessage")]
        public Message Post(Message postMessage)
        {
            _context.Messages.Add(postMessage);
            _context.SaveChanges();
            return postMessage;
        }
    }
}
