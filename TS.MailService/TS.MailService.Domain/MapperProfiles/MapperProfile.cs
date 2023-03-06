using System.Net.Mail;
using AutoMapper;
using TS.MailService.Domain.Models;
using TS.MailService.Infrastructure.Entities;

namespace TS.MailService.Domain.MapperProfiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<EmailMessage, EmailMessageEntity>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

        CreateMap<EmailMessage, MailMessage>()
            .ForMember(dest => dest.From, opt => opt.MapFrom(src => new MailAddress(src.Sender)))
            .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.Recipients));

        CreateMap<EmailMessageEntity, EmailMessage>();
    }
}