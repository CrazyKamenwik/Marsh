using AutoMapper;
using TicketSystem.API.ViewModels.Messages;
using TicketSystem.API.ViewModels.Tickets;
using TicketSystem.API.ViewModels.Users;
using TicketSystem.BLL.Models;

namespace TicketSystem.API.MapperProfiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<ShortMessageViewModel, Message>()
            .ForMember(mm => mm.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));
        CreateMap<ShortUserViewModel, User>()
            .ForMember(um => um.UserRole, opt => opt.MapFrom(src => new UserRole { Name = src.UserRole }));

        CreateMap<User, UserViewModel>()
            .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserRole.Name))
            .ReverseMap();

        CreateMap<Ticket, TicketViewModel>()
            .ForMember(dest => dest.TicketCreator, opt => opt.MapFrom(src => src.TicketCreator))
            .ForMember(dest => dest.Operator, opt => opt.MapFrom(src => src.Operator))
            .ReverseMap();

        CreateMap<Message, MessageViewModel>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ReverseMap();
    }
}