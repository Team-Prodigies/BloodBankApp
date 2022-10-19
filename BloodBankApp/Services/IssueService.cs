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
            var issueDb = await GetIssue(id);
            if (issue == null) return false;
            issueDb.IssueStatus = issue.IssueStatus;
            _context.Update(issueDb);
            await _context.SaveChangesAsync();
            return true;

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
            var getIssues = filterBy switch
            {
                "Date" => await _context.Issues.OrderBy(issue => issue.DateReported).ToListAsync(),
                "OnHold" => await _context.Issues.Where(issue => issue.IssueStatus == Enums.IssueStatus.ONHOLD)
                    .ToListAsync(),
                "InProgres" => await _context.Issues.Where(issue => issue.IssueStatus == Enums.IssueStatus.INPROGRES)
                    .ToListAsync(),
                "Fixed" => await _context.Issues.Where(issue => issue.IssueStatus == Enums.IssueStatus.FIXED)
                    .ToListAsync(),
                "Normal" => await _context.Issues.ToListAsync(),
                _ => await _context.Issues.ToListAsync()
            };

            var result = _mapper.Map<List<Issue>>(getIssues);

            return result;
        }
    }
}
