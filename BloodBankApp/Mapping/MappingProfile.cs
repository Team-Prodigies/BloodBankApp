using AutoMapper;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Identity;
using System;
using static BloodBankApp.Areas.Identity.Pages.Account.RegisterMedicalStaffModel;
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

            CreateMap<RegisterMedicalStaffInputModel, User>()
                .ForMember(dest => dest.DateOfBirth,
                opts => opts.MapFrom(src => src.DateOfBirth));

            CreateMap<RegisterMedicalStaffInputModel, MedicalStaff>()
                .ForMember(dest => dest.HospitalId,
                opts => opts.MapFrom(src => src.HospitalId));

            CreateMap<HospitalModel, Hospital>().ReverseMap();
            CreateMap<MedicalStaffModel, MedicalStaff>().ReverseMap();

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

            CreateMap<HospitalModel, Location>()
              .ForMember(dest => dest.Longitude,
              opts => opts.MapFrom(src => src.Location.Longitude))
              .ForMember(dest => dest.Latitude,
              opts => opts.MapFrom(src => src.Location.Latitude)).ReverseMap();

            CreateMap<SuperAdminModel, User>();

            CreateMap<RoleModel, IdentityRole<Guid>>()
                .ForMember(dest => dest.Name,
                 opts => opts.MapFrom(src => src.RoleName)).ReverseMap();

            CreateMap<BloodType, BloodTypeModel>();

            CreateMap<Donor, DonorDto>()
                .ForPath(dest => dest.BloodType.BloodTypeName,
                    opts => opts.MapFrom(src => src.BloodType.BloodTypeName)).ReverseMap();

            CreateMap<City, CityModel>().ReverseMap();
        }
    }
}
