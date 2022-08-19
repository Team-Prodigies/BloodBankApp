using AutoMapper;
using BloodBankApp.Areas.Identity.Pages.Account.Manage;
using BloodBankApp.Areas.Identity.Pages.Account.ViewModels;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            CreateMap<SuperAdminModel, User>();
        }
    }
}
