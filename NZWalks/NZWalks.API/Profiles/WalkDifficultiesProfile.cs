﻿using AutoMapper;

namespace NZWalks.API.Profiles
{
    public class WalkDifficultiesProfile : Profile
    {
        public WalkDifficultiesProfile()
        {
            CreateMap<Models.Domain.WalkDifficulty, Models.DTO.WalkDifficulty>().ReverseMap();

        }
    }
}
