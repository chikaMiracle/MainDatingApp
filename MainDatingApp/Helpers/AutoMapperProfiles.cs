using AutoMapper;
using MainDatingApp.Dtos;
using MainDatingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainDatingApp.Helpers
{
    public class AutoMapperProfiles: Profile
    {

        public AutoMapperProfiles()
        {

            CreateMap<User, UserForListDto>()
                .ForMember(des=>des.PhotoUrl, opt=>{
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(des=>des.Age, opt=>
                {
                    opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
                });

            CreateMap<User, UserForDetailedDto>()

                .ForMember(des => des.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                    })
                    .ForMember(des => des.Age, opt =>
                    {
                        opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
                    });

            CreateMap<Photo, PhotosForDetailedDto>();
            CreateMap<UserForUpdateDto, User>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap< Photo, PhotoForReturnDto>();
            CreateMap<UserForRegisterDto, User>();
            CreateMap<MessageForCreationDto, Message>();
            CreateMap<Message, MessageToReturnDto>().ForMember(m => m.SenderPhotoUrl, opt => opt.MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
              .ForMember(m => m.RecipientPhotoUrl, opt => opt.MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));


        }
    }
}
