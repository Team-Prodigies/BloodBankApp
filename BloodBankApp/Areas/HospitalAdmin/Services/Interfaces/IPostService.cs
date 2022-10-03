using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.HospitalAdmin.Services.Interfaces {
    public interface IPostService {
        Task<bool> AddPost(DonationPost post);
        Task<List<DonationPost>> GetPost(Hospital getHospital, string filterBy = "Normal");
        Task<List<DonationPost>> GetAllPosts();
        Task<List<DonationPost>> GetPostsByBloodType(string filterBy = "Normal", int pageNumber = 1);
        Task<List<DonationPost>> GetPostsByCity(Guid id, int pageNumber = 1);
        Task<List<DonationPost>> GetPostsBySearch(string searchTerm, int pageNumber = 1);
        Task<PostModel> EditPost(Guid notificationId);
        Task<bool> EditPosts(PostModel post, Guid notificationId);
    }
}
