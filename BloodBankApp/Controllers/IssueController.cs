using AspNetCoreHero.ToastNotification.Abstractions;
using BloodBankApp.Data;
using BloodBankApp.Enums;
using BloodBankApp.Models;
using BloodBankApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Controllers
{
    public class IssueController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly INotyfService _notyfService;
        private readonly IIssueService _issueService;
        private SelectList IssueStatus { get; set; }

        public IssueController(ApplicationDbContext db, INotyfService notyfService, IIssueService issueService)
        {
            _db = db;
            _notyfService = notyfService;
            _issueService = issueService;

            IssueStatus = new SelectList(Enum.GetValues(typeof(IssueStatus))
               .Cast<IssueStatus>()
               .ToList(), "IssueStatus");
        }
        
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
            }
            _notyfService.Success("The issue reported succesfuly");
            return RedirectToAction("ReportIssue");
            
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var issue = await _issueService.Edit(id);

            return View(issue);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
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
            _notyfService.Success("The issue has been edited successfuly.");
            return RedirectToAction(nameof(Index));
        }

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
        public async Task<IActionResult> DeletePOST(Guid id)
        {
            var issue = await _issueService.DeletePost(id);

            if(!issue)
            {
                return View();
            }
            _notyfService.Success("The issue has been deleted successfuly.");
            return RedirectToAction("Index");
        }

    }
}
