using AutoMapper;
using BloodBankApp.Data;
using BloodBankApp.Models;
using BloodBankApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Services
{
    public class IssueService: IIssueService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public IssueService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddIssue(Issue issue)
        {
            await _context.Issues.AddAsync(issue);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Issue> Edit(Guid id)
        {
            var getIssue = await GetIssue(id);
            var postIssue = new Issue
            {
                IssueId = getIssue.IssueId,
                Title = getIssue.Title,
                Description = getIssue.Description,
                DateReported = getIssue.DateReported,
                IssueStatus = getIssue.IssueStatus
            };

            return postIssue;
        }

        public async Task<bool> EditIssue(Guid id, Issue issue)
        {
            var issueDB = await GetIssue(id);
            issueDB.IssueStatus = issue.IssueStatus;

            if(issue != null)
            {
                _context.Update(issueDB);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<Issue> Delete(Guid id)
        {
            var getIssue = await GetIssue(id);
            var postIssue = new Issue
            {
                IssueId = getIssue.IssueId,
                Title = getIssue.Title,
                Description = getIssue.Description,
                DateReported = getIssue.DateReported,
                IssueStatus = getIssue.IssueStatus
            };

            return postIssue;
        }

        public async Task<bool> DeletePost(Guid id)
        {
            var issue = await GetIssue(id);
            if (issue != null)
            {
                _context.Remove(issue);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<Issue> GetIssue(Guid id)
        {
            var issue = await _context.Issues.FindAsync(id);

            return issue;
        }
        public async Task<List<Issue>> GetIssues(string filterBy = "Date")
        {
            List<Issue> getIssues;

            switch (filterBy)
            {
                case"Date":
                    getIssues = await _context.Issues.OrderBy(issue => issue.DateReported).ToListAsync();
                    break;
                case "OnHold":
                    getIssues = await _context.Issues.Where(issue => issue.IssueStatus == Enums.IssueStatus.ONHOLD).ToListAsync();
                    break;
                case "InProgres":
                    getIssues = await _context.Issues.Where(issue => issue.IssueStatus == Enums.IssueStatus.INPROGRES).ToListAsync();
                    break;
                case "Fixed":
                    getIssues = await _context.Issues.Where(issue => issue.IssueStatus == Enums.IssueStatus.FIXED).ToListAsync();
                    break;
                case "Normal":
                    getIssues = await _context.Issues.ToListAsync();
                    break;

                default:
                    getIssues = await _context.Issues.ToListAsync();
                    break;
            }

            var result = _mapper.Map<List<Issue>>(getIssues);

            return result;
        }
    }
}
