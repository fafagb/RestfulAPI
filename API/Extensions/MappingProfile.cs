using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using API.Core.Entities;
using API.Repository.Resources;

namespace API.Extensions
{
    /// <summary>
    /// 映射
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TxClues, ClueResource>().ForMember(dest=> dest.UpdateTime,opt=>opt.MapFrom(src=>src.UpdateTime));
            CreateMap<ClueResource, TxClues>();


            CreateMap<ClueAddResource, TxClues>();
            CreateMap<TxClues, ClueAddResource>();
            CreateMap<ClueUpdateResource, TxClues>();

        }

    }
}
