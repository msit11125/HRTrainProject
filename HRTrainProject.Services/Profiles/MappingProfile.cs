using AutoMapper;
using HRTrainProject.Core.Models;
using HRTrainProject.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRTrainProject.Service.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Hrmt01, UserProfile>()
                .ForMember(x => x.Roles, opt => opt.Ignore());


            CreateMap<UserEditViewModel, Hrmt01>();
            CreateMap<Hrmt01, UserEditViewModel>();

            
        }
    }
}
