using AspNetCoreHero.ToastNotification.Abstractions;
using BloodBankApp.Data;
using BloodBankApp.Enums;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Controllers
{
    public class IssueController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly INotyfService _notyfService;

        public IssueController(ApplicationDbContext db, INotyfService notyfService)
        {
            _db = db;
            _notyfService = notyfService;
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _db.Issues.ToListAsync());
        }

        public IActionResult ReportIssue()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ReportIssue([Bind("IssueId,Title,Description,DateReported,IssueStatus")] Issue issue)
        {
            if (ModelState.IsValid)
            {
                issue.IssueId = Guid.NewGuid();
                _db.Add(issue);
                await _db.SaveChangesAsync();
                return RedirectToAction("ReportIssue");
            }
            return View();
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var issueFromDatabase = await _db.Issues.FirstOrDefaultAsync(u => u.IssueId == id);

            if (issueFromDatabase == null)
            {
                return NotFound();
            }

            return View(issueFromDatabase);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(Guid? id,[Bind("IssueId,Title,Description,DateReported,IssueStatus")] Issue issue)
        {
            if (id != issue.IssueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Issues.Update(issue);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(issue);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issueFromDatabase = await _db.Issues.FirstOrDefaultAsync(u => u.IssueId == id);

            if (issueFromDatabase == null)
            {
                return NotFound();
            }

            return View(issueFromDatabase);
        }

        [HttpPost, ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeletePOST(Guid? id)
        {
            var issue = await _db.Issues.FirstOrDefaultAsync(u => u.IssueId == id);

            if(issue == null)
            {
                return NotFound();
            }

            _db.Issues.Remove(issue);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
