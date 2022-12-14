using AspNetCoreHero.ToastNotification.Abstractions;
using BloodBankApp.Models;
using BloodBankApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using BloodBankApp.Areas.SuperAdmin.Permission;

namespace BloodBankApp.Controllers
{
    [Authorize]
    public class IssueController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IIssueService _issueService;

        public IssueController(INotyfService notyfService,
            IIssueService issueService)
        {
            _notyfService = notyfService;
            _issueService = issueService;
        }

        [Authorize(Policy = Permissions.Issues.View)]
        public async Task<IActionResult> Index(string filterBy = "Date")
        {
            var issues = await _issueService.GetIssues(filterBy);
            ViewBag.FilterBy = filterBy;
            return View(issues.ToList());
        }

        public IActionResult ReportIssue()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ReportIssue([Bind("IssueId,Title,Description,DateReported,IssueStatus")] Issue issue)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _issueService.AddIssue(issue);
            if (!result)
            {
                _notyfService.Error("The issue could not be posted in the database.");
                return View();
            }
            _notyfService.Success("The issue reported successfully");
            return RedirectToAction("ReportIssue");
            
        }

        [Authorize(Policy = Permissions.Issues.Edit)]
        public async Task<IActionResult> Edit(Guid id)
        {
            var issue = await _issueService.Edit(id);
            return View(issue);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Policy = Permissions.Issues.Edit)]
        public async Task<IActionResult> Edit(Guid id,[Bind("IssueId,Title,Description,DateReported,IssueStatus")] Issue issue)
        {
            if (id != issue.IssueId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _issueService.EditIssue(id, issue);

            if (!result)
            {
                _notyfService.Error("Issue could not be edited!");
                return View();
            }
            _notyfService.Success("The issue has been edited successfully.");
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = Permissions.Issues.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var issue = await _issueService.Delete(id);

            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }

        [HttpPost, ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        [Authorize(Policy = Permissions.Issues.Delete)]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var issue = await _issueService.DeletePost(id);

            if(!issue)
            {
                return View();
            }
            _notyfService.Success("The issue has been deleted successfully.");
            return RedirectToAction("Index");
        }
    }
}