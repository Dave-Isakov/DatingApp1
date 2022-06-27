using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDTO>()
                .ForMember(destination => destination.PhotoUrl, options => options
                    .MapFrom(source => source.Photos
                        .FirstOrDefault(photo => photo.IsMain).Url))
                .ForMember(destination => destination.Age, options => options
                    .MapFrom(source => source.DateOfBirth
                        .CalculateAge()));
            CreateMap<Photo, PhotoDTO>();
            CreateMap<MemberUpdateDTO, AppUser>();
            CreateMap<RegisterDTO, AppUser>();
            CreateMap<Message, MessageDTO>()
                .ForMember(destination => destination.SenderPhotoUrl, options => options
                    .MapFrom(source => source.Sender.Photos
                        .FirstOrDefault(photo => photo.IsMain).Url))
                .ForMember(destination => destination.RecipientPhotoUrl, options => options
                    .MapFrom(source => source.Recipient.Photos
                        .FirstOrDefault(photo => photo.IsMain).Url));
        }
    }
}