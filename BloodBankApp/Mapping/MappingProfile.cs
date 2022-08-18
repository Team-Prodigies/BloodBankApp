using AutoMapper;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BloodBankApp.Areas.Identity.Pages.Account.RegisterModel;

namespace BloodBankApp.Mapping
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<RegisterInputModel, User>()
                .ForMember(dest => dest.DateOfBirth,
                opts => opts.MapFrom(src => src.DateOfBirth));

            CreateMap<RegisterInputModel, Donor>()
                .ForMember(dest => dest.CityId,
                opts => opts.MapFrom(src => src.CityId))
                .ForMember(dest => dest.BloodTypeId,
                opts => opts.MapFrom(src => src.BloodTypeId));

            CreateMap<SuperAdminModel, User>();

            CreateMap<Donor, DonorModel>()
                 .ForPath(dest => dest.Locked,
                opts => opts.MapFrom(src => src.User.Locked))
                .ForPath(dest => dest.Name,
                opts => opts.MapFrom(src => src.User.Name))
                .ForPath(dest => dest.Surname,
                opts => opts.MapFrom(src => src.User.Surname))
                .ForPath(dest => dest.DateOfBirth,
                opts => opts.MapFrom(src => src.User.DateOfBirth))
                .ForPath(dest => dest.BloodTypeName,
                opts => opts.MapFrom(src => src.BloodType.BloodTypeName))
                .ForPath(dest => dest.CityName,
                opts => opts.MapFrom(src => src.City.CityName));

            CreateMap<HospitalModel, Hospital>();
            CreateMap<Hospital, HospitalModel>();

            CreateMap<HospitalModel, Location>()
                .ForMember(dest => dest.Longitude,
                opts => opts.MapFrom(src => src.Location.Longitude))
                .ForMember(dest => dest.Latitude,
                opts => opts.MapFrom(src => src.Location.Latitude));
        }
    }
}
