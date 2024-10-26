using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecnoMundo.Application.DTOs;
using TecnoMundo.Domain.Entities;

namespace TecnoMundo.Application.Mappings
{
    public class DomainToDTOMappingIdentity
    {
        public static MapperConfiguration RegisterMaps()
        {
            var config = new MapperConfiguration(config =>
            {
                config.CreateMap<UserVO, User>();
                config.CreateMap<User, UserVO>()
                    .ForMember(x => x.Password, opt => opt.Ignore());
            });

            return config;
        }
    }
}
