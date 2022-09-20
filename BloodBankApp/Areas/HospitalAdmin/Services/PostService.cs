using AutoMapper;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.HospitalAdmin.Services {
    public class PostService : IPostService {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public PostService(ApplicationDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddPost(DonationPost post) {
            if(post != null) {
                if (post.DateRequired.Hour <= DateTime.Now.Hour || post.DateRequired.Day < DateTime.Now.Day) {
                   return false;
                }
                
                await _context.DonationPosts.AddAsync(post);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<PostModel> EditPost(Guid notificationId) {
            var getPost = await _context.DonationPosts.FindAsync(notificationId);

            var postModel = new PostModel {
                NotificationId = getPost.NotificationId,
                DateRequired = getPost.DateRequired,
                Description = getPost.Description,
                AmountRequested = getPost.AmountRequested,
                BloodTypeId = getPost.BloodTypeId,
                PostStatus = getPost.PostStatus,
            };
            return postModel;
        }

        public async Task<bool> EditPosts(PostModel post, Guid notificationId) {
            var getPost = await _context.DonationPosts.FindAsync(notificationId);
            getPost.PostStatus = post.PostStatus;
            getPost.Description = post.Description;
            getPost.BloodTypeId = post.BloodTypeId;
            getPost.AmountRequested = post.AmountRequested;
            getPost.DateRequired = post.DateRequired;
            if (getPost != null) {

                _context.DonationPosts.Update(getPost);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<DonationPost>> GetPost(Hospital getHospital, string filterBy = "Normal") {
            List<DonationPost> getPost;

            switch (filterBy) {
                case "Date":
                    getPost = await _context.DonationPosts.Where(x => x.HospitalId == getHospital.HospitalId).OrderBy(post => post.DateRequired).ToListAsync();
                    break;
                case "Active":
                    getPost = await _context.DonationPosts.Where(x => ((int)x.PostStatus) == 1).ToListAsync();
                    break;
                case "InActive":
                    getPost = await _context.DonationPosts.Where(x => ((int)x.PostStatus) == 2).ToListAsync();
                    break;
                case "Normal":
                    getPost = await _context.DonationPosts.Where(x => x.HospitalId == getHospital.HospitalId).ToListAsync();
                    break;

                default:
                    getPost = await _context.DonationPosts.Where(x => x.HospitalId == getHospital.HospitalId).ToListAsync();
                    break;
            }

            var result = _mapper.Map<List<DonationPost>>(getPost);
            return result;
        }
    }
}
