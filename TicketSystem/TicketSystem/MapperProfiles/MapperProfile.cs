using AutoMapper;
using TicketSystem.BLL.Models;
using TicketSystem.ViewModels.Messages;
using TicketSystem.ViewModels.Tickets;
using TicketSystem.ViewModels.Users;

namespace TicketSystem.MapperProfiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ShortMessage, MessageModel>();
            CreateMap<ShortUser, UserModel>();

            CreateMap<UserModel, UserVm>();
            CreateMap<TicketModel, TicketVm>();
            CreateMap<MessageModel, MessageVm>();
        }
    }
}