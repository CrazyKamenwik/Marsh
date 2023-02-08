using AutoMapper;
using TicketSystem.BLL.Models;
using TicketSystem.DAL.Entities;
using TicketSystem.ViewModels.Messages;
using TicketSystem.ViewModels.Tickets;
using TicketSystem.ViewModels.Users;

namespace TicketSystem.BLL.MapperProfiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserEntity, UserModel>().ReverseMap();
            CreateMap<TicketEntity, TicketModel>().ReverseMap();
            CreateMap<MessageEntity, MessageModel>().ReverseMap();
        }
    }
}