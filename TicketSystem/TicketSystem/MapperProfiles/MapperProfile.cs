using AutoMapper;
using TicketSystem.BLL.Models;
using TicketSystem.ViewModels.Messages;
using TicketSystem.ViewModels.Tickets;
using TicketSystem.ViewModels.Users;

namespace TicketSystem.MapperProfiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<ShortMessageViewModel, MessageModel>()
            .ForMember(mm => mm.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));
        CreateMap<ShortUserViewModel, UserModel>()
            .ForMember(um => um.UserRole, opt => opt.MapFrom(src => new UserRoleModel { Name = src.UserRole }));

        CreateMap<UserModel, UserViewModel>()
            .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserRole.Name))
            .ReverseMap();

        CreateMap<TicketModel, TicketViewModel>()
            .ForMember(dest => dest.TicketCreator, opt => opt.MapFrom(src => src.TicketCreator))
            .ForMember(dest => dest.Operator, opt => opt.MapFrom(src => src.Operator))
            .ReverseMap();

        CreateMap<MessageModel, MessageViewModel>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ReverseMap();
    }
}