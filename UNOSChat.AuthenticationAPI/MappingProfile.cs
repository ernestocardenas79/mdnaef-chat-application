using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UNOSChat.AuthenticationAPI.Dtos;
using UNOSChat.AuthenticationAPI.Models;

namespace UNOSChat.AuthenticationAPI;

public class MappingProfile
{
    public MappingProfile()
    {    
        //CreateMap<RegistrationDto, User>()
        //    .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
    }
}
