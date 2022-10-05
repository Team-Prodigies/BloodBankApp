using BloodBankApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankApp.Services.Interfaces
{
    public interface IIssueService
    {
        Task<List<Issue>> GetIssues(string filterBy);
        Task<bool> AddIssue(Issue issue);
        Task<Issue> Edit(Guid id);
        Task<bool> EditIssue(Guid id, Issue issue);
        Task<Issue> Delete(Guid id);
        Task<bool> DeletePost(Guid id);
        Task<Issue> GetIssue(Guid id);
    }
}