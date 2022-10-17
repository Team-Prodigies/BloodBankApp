using BloodBankApp.Areas.Donator.ViewModels;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.HospitalAdmin.Services.Interfaces {
    public interface IPostService {
        Task<bool> AddPost(DonationPost post, string id);
        Task<List<DonationPost>> GetPost(Hospital getHospital, string filterBy = "Normal");
        Task<List<PostModel>> GetPostsByBloodType(string filterBy = "Normal", int pageNumber = 1);
        Task<List<PostModel>> GetPostsByCity(Guid id, int pageNumber = 1);
        Task<List<PostModel>> GetPostsBySearch(string searchTerm, int pageNumber = 1);
        Task<PostModel> EditPost(Guid DonationPostId);
        Task<bool> EditPosts(PostModel post);
        Task<bool> DeletePost(Guid DonationPostId);
        Task<QuestionnaireAnswers> GetQuestionnaireQuestions();
        Task<List<Question>> GetAllQuestions();
        Task<DonationPost> GetPost(Guid postId);
    }
}