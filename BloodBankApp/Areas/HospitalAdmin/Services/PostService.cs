﻿using AutoMapper;
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

        public async Task<List<DonationPost>> GetAllPosts()
        {
            var posts = await _context.DonationPosts.Include(x => x.BloodType).Include(x => x.Hospital).Where(x => x.PostStatus == Enums.PostStatus.ACTIVE).ToListAsync();
            return posts;
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

        public async Task<List<DonationPost>> GetPostsByBloodType(string filterBy = "Normal")
        {
            List<DonationPost> getPosts;

            switch (filterBy)
            {
                case "A+":
                    getPosts = await _context.DonationPosts.Include(x => x.BloodType).Include(x => x.Hospital).Where(x => x.BloodType.BloodTypeName == "A+" && x.PostStatus == Enums.PostStatus.ACTIVE).ToListAsync();
                    break;
                case "A-":
                    getPosts = await _context.DonationPosts.Include(x => x.BloodType).Include(x => x.Hospital).Where(x => x.BloodType.BloodTypeName == "A-" && x.PostStatus == Enums.PostStatus.ACTIVE).ToListAsync();
                    break;
                case "B+":
                    getPosts = await _context.DonationPosts.Include(x => x.BloodType).Include(x => x.Hospital).Where(x => x.BloodType.BloodTypeName == "B+" && x.PostStatus == Enums.PostStatus.ACTIVE).ToListAsync();
                    break;
                case "B-":
                    getPosts = await _context.DonationPosts.Include(x => x.BloodType).Include(x => x.Hospital).Where(x => x.BloodType.BloodTypeName == "B-" && x.PostStatus == Enums.PostStatus.ACTIVE).ToListAsync();
                    break;
                case "AB+":
                    getPosts = await _context.DonationPosts.Include(x => x.BloodType).Include(x => x.Hospital).Where(x => x.BloodType.BloodTypeName == "AB+" && x.PostStatus == Enums.PostStatus.ACTIVE).ToListAsync();
                    break;
                case "AB-":
                    getPosts = await _context.DonationPosts.Include(x => x.BloodType).Include(x => x.Hospital).Where(x => x.BloodType.BloodTypeName == "AB-" && x.PostStatus == Enums.PostStatus.ACTIVE).ToListAsync();
                    break;
                case "O+":
                    getPosts = await _context.DonationPosts.Include(x => x.BloodType).Include(x => x.Hospital).Where(x => x.BloodType.BloodTypeName == "O+" && x.PostStatus == Enums.PostStatus.ACTIVE).ToListAsync();
                    break;
                case "O-":
                    getPosts = await _context.DonationPosts.Include(x => x.BloodType).Include(x => x.Hospital).Where(x => x.BloodType.BloodTypeName == "O-").ToListAsync();
                    break;
                case "Normal":
                    getPosts = await _context.DonationPosts.Include(x => x.BloodType).Include(x => x.Hospital).Where(x => x.PostStatus == Enums.PostStatus.ACTIVE).ToListAsync();
                    break;

                default:
                    getPosts = await _context.DonationPosts.Include(x => x.BloodType).Include(x => x.Hospital).Where(x => x.PostStatus == Enums.PostStatus.ACTIVE).ToListAsync();
                    break;
            }
            var result = _mapper.Map<List<DonationPost>>(getPosts);
            return result;
        }

        public async Task<List<DonationPost>> GetPostsBySearch(string searchTerm)
        {
            var posts = await _context.DonationPosts
                .Include(x => x.Hospital)
                .Include(x => x.BloodType)
                .Where(x => x.Hospital.HospitalName.Replace(" ", "").ToUpper()
                .Contains(searchTerm.Replace(" ", "").ToUpper()))
                .ToListAsync();

            return posts;
        }
    }
}
