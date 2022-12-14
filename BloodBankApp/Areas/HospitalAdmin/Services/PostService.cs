using AutoMapper;
using BloodBankApp.Areas.Donator.ViewModels;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Enums;

namespace BloodBankApp.Areas.HospitalAdmin.Services {
    public class PostService : IPostService {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHospitalService _hospitalService;

        public PostService(ApplicationDbContext context,
            IMapper mapper,
            IHospitalService hospitalService) {
            _context = context;
            _mapper = mapper;
            _hospitalService = hospitalService;
        }

        public async Task<bool> AddPost(DonationPost post, string id) {
            if (post == null) return false;
            if (post.DateRequired.Day < DateTime.Now.Day) {
                return false;
            }

            var hospital = await _hospitalService.GetHospitalForMedicalStaff(id);
            if (hospital == null) return false;
            
            post.HospitalId = hospital.HospitalId;
            post.PostStatus = PostStatus.ACTIVE;
            await _context.DonationPosts.AddAsync(post);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> DeletePost(Guid donationPostId) {
            var deletePost = await _context.DonationPosts.FindAsync(donationPostId);
            if (deletePost == null) return false;
            _context.DonationPosts.Remove(deletePost);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<PostModel> EditPost(Guid donationPostId) {
            var getPost = await _context.DonationPosts.FindAsync(donationPostId);

            var postModel = new PostModel {
                DonationPostId = getPost.DonationPostId,
                DateRequired = getPost.DateRequired,
                Description = getPost.Description,
                AmountRequested = getPost.AmountRequested,
                BloodTypeId = getPost.BloodTypeId,
                PostStatus = getPost.PostStatus,
            };
            return postModel;
        }

        public async Task<bool> EditPosts(PostModel post) {
            var getPost = await _context.DonationPosts.FindAsync(post.DonationPostId);
            if (getPost == null) return false;
            getPost.PostStatus = post.PostStatus;
            getPost.Description = post.Description;
            getPost.BloodTypeId = post.BloodTypeId;
            getPost.AmountRequested = post.AmountRequested;
            getPost.DateRequired = post.DateRequired;

            _context.DonationPosts.Update(getPost);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<DonationPost>> GetPost(Hospital getHospital, string filterBy = "Normal") {
            List<DonationPost> getPost;

            switch (filterBy) {
                case "Date":
                    getPost = await _context.DonationPosts
                        .Where(x => x.HospitalId == getHospital.HospitalId)
                        .OrderBy(post => post.DateRequired)
                        .ToListAsync();
                    break;
                case "Active":
                    getPost = await _context.DonationPosts
                        .Where(x => ((int)x.PostStatus) == 1)
                        .ToListAsync();
                    break;
                case "InActive":
                    getPost = await _context.DonationPosts
                        .Where(x => ((int)x.PostStatus) == 2)
                        .ToListAsync();
                    break;
                case "Normal":
                    getPost = await _context.DonationPosts
                        .Where(x => x.HospitalId == getHospital.HospitalId)
                        .ToListAsync();
                    break;

                default:
                    getPost = await _context.DonationPosts
                        .Where(x => x.HospitalId == getHospital.HospitalId)
                        .ToListAsync();
                    break;
            }
            var result = _mapper.Map<List<DonationPost>>(getPost);
            return result;
        }

        public async Task<List<PostModel>> GetPostsByBloodType(string filterBy = "Normal", int pageNumber = 1) {
            var skipRows = (pageNumber - 1) * 10;

            var getPosts = filterBy switch
            {
                "A+" => await _context.DonationPosts.Include(x => x.BloodType)
                    .Include(x => x.Hospital)
                    .Where(x => x.BloodType.BloodTypeName == "A+" && x.PostStatus == PostStatus.ACTIVE)
                    .Skip(skipRows)
                    .Take(10)
                    .ToListAsync(),
                "A-" => await _context.DonationPosts.Include(x => x.BloodType)
                    .Include(x => x.Hospital)
                    .Where(x => x.BloodType.BloodTypeName == "A-" && x.PostStatus == PostStatus.ACTIVE)
                    .Skip(skipRows)
                    .Take(10)
                    .ToListAsync(),
                "B+" => await _context.DonationPosts.Include(x => x.BloodType)
                    .Include(x => x.Hospital)
                    .Where(x => x.BloodType.BloodTypeName == "B+" && x.PostStatus == PostStatus.ACTIVE)
                    .Skip(skipRows)
                    .Take(10)
                    .ToListAsync(),
                "B-" => await _context.DonationPosts.Include(x => x.BloodType)
                    .Include(x => x.Hospital)
                    .Where(x => x.BloodType.BloodTypeName == "B-" && x.PostStatus == PostStatus.ACTIVE)
                    .Skip(skipRows)
                    .Take(10)
                    .ToListAsync(),
                "AB+" => await _context.DonationPosts.Include(x => x.BloodType)
                    .Include(x => x.Hospital)
                    .Where(x => x.BloodType.BloodTypeName == "AB+" && x.PostStatus == PostStatus.ACTIVE)
                    .Skip(skipRows)
                    .Take(10)
                    .ToListAsync(),
                "AB-" => await _context.DonationPosts.Include(x => x.BloodType)
                    .Include(x => x.Hospital)
                    .Where(x => x.BloodType.BloodTypeName == "AB-" && x.PostStatus == PostStatus.ACTIVE)
                    .Skip(skipRows)
                    .Take(10)
                    .ToListAsync(),
                "O+" => await _context.DonationPosts.Include(x => x.BloodType)
                    .Include(x => x.Hospital)
                    .Where(x => x.BloodType.BloodTypeName == "O+" && x.PostStatus == PostStatus.ACTIVE)
                    .Skip(skipRows)
                    .Take(10)
                    .ToListAsync(),
                "O-" => await _context.DonationPosts.Include(x => x.BloodType)
                    .Include(x => x.Hospital)
                    .Where(x => x.BloodType.BloodTypeName == "O-" && x.PostStatus == PostStatus.ACTIVE)
                    .Skip(skipRows)
                    .Take(10)
                    .ToListAsync(),
                "Normal" => await _context.DonationPosts.Include(x => x.BloodType)
                    .Include(x => x.Hospital)
                    .Where(x => x.PostStatus == PostStatus.ACTIVE)
                    .Skip(skipRows)
                    .Take(10)
                    .ToListAsync(),
                _ => await _context.DonationPosts.Include(x => x.BloodType)
                    .Include(x => x.Hospital)
                    .Where(x => x.PostStatus == PostStatus.ACTIVE)
                    .Skip(skipRows)
                    .Take(10)
                    .ToListAsync()
            };
            var result = _mapper.Map<List<PostModel>>(getPosts);
            return result;
        }

        public async Task<List<PostModel>> GetPostsByCity(Guid id, int pageNumber = 1) {
            var skipRows = (pageNumber - 1) * 10;
            var posts = await _context.DonationPosts
                .Include(x => x.BloodType)
                .Include(x => x.Hospital)
                .Where(x => x.PostStatus == PostStatus.ACTIVE && x.Hospital.CityId == id)
                .Skip(skipRows)
                .Take(10)
                .ToListAsync();
            var result = _mapper.Map<List<PostModel>>(posts);
            return result;

        }

        public async Task<List<PostModel>> GetPostsBySearch(string searchTerm, int pageNumber = 1) {
            var skipRows = (pageNumber - 1) * 10;
            var posts = await _context.DonationPosts
                .Include(x => x.Hospital)
                .Include(x => x.BloodType)
                .Where(x => x.Hospital.HospitalName.Replace(" ", "").ToUpper()
                .Contains(searchTerm.Replace(" ", "").ToUpper()))
                .Skip(skipRows).Take(10)
                .ToListAsync();
            var result = _mapper.Map<List<PostModel>>(posts);
            return result;
        }

        public async Task<QuestionnaireAnswers> GetQuestionnaireQuestions() {
            var questions = await _context.Questions
                .Select(q => new QuestionViewModel {
                    QuestionId = q.QuestionId,
                    Description = q.Description
                }).ToListAsync();
            var questionsList = new QuestionnaireAnswers(questions);

            return questionsList;
        }

        public async Task<List<Question>> GetAllQuestions() {
            var questions = await _context.Questions.ToListAsync();
            return questions;
        }

        public async Task<DonationPost> GetPost(Guid postId) {
            var getPost = await _context.DonationPosts.FindAsync(postId);

            return getPost;
        }

        public bool GetDonationRequest(Guid postId, Guid donorId) {
            var donationRequestExists = _context.DonationRequest
                .FirstOrDefault(x => x.DonationPostId == postId && x.DonorId == donorId);

            return donationRequestExists != null;
        }
    }
}