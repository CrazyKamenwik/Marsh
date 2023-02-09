using AutoMapper;
using TicketSystem.BLL.Models;
using TicketSystem.BLL.Models.Enums;
using TicketSystem.DAL.Entities;
using TicketSystem.DAL.Entities.Enums;

namespace TicketSystem.BLL.MapperProfiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<UserRoleEntity, UserRoleModel>().ReverseMap();

        CreateMap<TicketStatusEnumEntity, TicketStatusEnumModel>().ReverseMap();

        CreateMap<UserEntity, UserModel>()
            .ForMember(dest => dest.Tickets, opt => opt.MapFrom(src => src.Tickets))
            .ReverseMap();

        CreateMap<TicketEntity, TicketModel>()
            .ForMember(dest => dest.TicketCreator, opt => opt.MapFrom(src => src.TicketCreator))
            .ForMember(dest => dest.Operator, opt => opt.MapFrom(src => src.Operator))
            .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages))
            .ReverseMap();

        CreateMap<MessageEntity, MessageModel>().ReverseMap()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.Ticket, opt => opt.MapFrom(src => src.Ticket))
            .ReverseMap();
    }
}