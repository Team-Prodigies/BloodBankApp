using AspNetCoreHero.ToastNotification.Abstractions;
using BloodBankApp.Data;
using BloodBankApp.Enums;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Linq;

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
        
        public IActionResult Index()
        {
            var issueList = _db.Issues.ToList();
            return View(issueList);
        }

        public IActionResult ReportIssue()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ReportIssue(Issue obj)
        {
            if (ModelState.IsValid)
            {
                _db.Issues.Add(obj);
                _db.SaveChanges();
                _notyfService.Success("Issue reported successfully! The support team will deal with it.");
                return RedirectToAction("ReportIssue");
            }
            return View();
        }

        public IActionResult Edit(Guid? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var issueFromDatabase = _db.Issues.FirstOrDefault(u => u.IssueId == id);

            if (issueFromDatabase == null)
            {
                return NotFound();
            }

            return View(issueFromDatabase);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Guid? id,[Bind("IssueId,Title,Description,DateReported,IssueStatus")] Issue issue)
        {
            if (id != issue.IssueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Issues.Update(issue);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(issue);
        }

        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issueFromDatabase = _db.Issues.FirstOrDefault(u => u.IssueId == id);

            if (issueFromDatabase == null)
            {
                return NotFound();
            }

            return View(issueFromDatabase);
        }

        [HttpPost, ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public IActionResult DeletePOST(Guid? id)
        {
            var issue = _db.Issues.FirstOrDefault(u => u.IssueId == id);

            if(issue == null)
            {
                return NotFound();
            }

            _db.Issues.Remove(issue);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
