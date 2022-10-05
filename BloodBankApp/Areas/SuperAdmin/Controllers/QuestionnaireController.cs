using AspNetCoreHero.ToastNotification.Abstractions;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Data;
using BloodBankApp.Enums;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize]
    public class QuestionnaireController : Controller
    {
        private SelectList Answer { get; set; }
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyfService;
        private readonly IQuestionService _questionService;
        public QuestionnaireController(
            ApplicationDbContext context,
            INotyfService notyfService,
            IQuestionService questionService)
        {
            _context = context;
            _notyfService = notyfService;
            _questionService = questionService;
            Answer = new SelectList(Enum.GetValues(typeof(Answer)).Cast<Answer>().ToList(), "Answer");
        }

        [HttpGet]
        public IActionResult CreateQuestionnaire()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestionnaire(HealthFormQuestionnaire questionnaire)
        {
            questionnaire.LastUpdated = DateTime.Now;
            if (questionnaire.LastUpdated == null)
            {
                _notyfService.Error("Questionnaire isnt created");
                return View(nameof(CreateQuestionnaire));
            }
            _notyfService.Success("Questionnaire is created");
            await _context.HealthFormQuestionnaires.AddAsync(questionnaire);
            _context.SaveChanges();
            return View(nameof(CreateQuestionnaire));
        }

        [HttpGet]
        public async Task<IActionResult> ManageQuestions()
        {
            var getQuestions = await _context.Questions.ToListAsync();
            return View(getQuestions);
        }

        [HttpGet]
        public async Task<IActionResult> CreateQuestion()
        {
            var getQuestinnaire = _context.HealthFormQuestionnaires.ToList();
            if (getQuestinnaire.Count == 0)
            {
                _notyfService.Error("Pls create a questionnaire first!");
                return RedirectToAction("CreateQuestionnaire");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion(Question question)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Pls fill all the questions!");
                return View(nameof(CreateQuestion));
            }
            var getQuestinnaire = _context.HealthFormQuestionnaires.FirstOrDefault();
            question.HealthFormQuestionnaireId = getQuestinnaire.HealthFormQuestionnaireId;

            await _context.Questions.AddAsync(question);
            _context.SaveChanges();

            _notyfService.Success("Question successfully created!");
            return RedirectToAction("ManageQuestions");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid QuestionId)
        {
            if (QuestionId == Guid.Empty)
            {
                _notyfService.Error("The Question does not exist");
                return RedirectToAction("ManageQuestions");
            }

            var getQuestion = await _questionService.EditQuestion(QuestionId);
            ViewBag.Answer = Answer;
            if (getQuestion == null)
            {
                _notyfService.Error("The Question does not exist");
                return RedirectToAction("ManageQuestions");
            }

            return View(getQuestion);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(QuestionModel questionModel, Guid QuestionId)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Answer = Answer;
                _notyfService.Error("Please fill form");
                return View(nameof(Edit));
            }
            if (QuestionId == Guid.Empty)
            {
                ViewBag.Answer = Answer;
                _notyfService.Error("The Question does not exist");
                return View(nameof(Edit));
            }
            var getQuestion = await _questionService.EditQuestion(questionModel, QuestionId);
            if (getQuestion == false)
            {
                ViewBag.Answer = Answer;
                _notyfService.Error("Something went wrong please try again");
                return RedirectToAction("ManageQuestions");
            }

            ViewBag.Answer = Answer;
            _notyfService.Success("The Question updated");
            return View(nameof(Edit));
        }

        public async Task<IActionResult> Delete(Guid QuestionId)
        {
            if (QuestionId == Guid.Empty)
            {
                _notyfService.Error("The Question does not exist");
                return View(nameof(ManageQuestions));
            }
            var getQuestion = await _context.Questions.FindAsync(QuestionId);

            _context.Questions.Remove(getQuestion);
            await _context.SaveChangesAsync();
            _notyfService.Success("Question Deleted!");
            return RedirectToAction("ManageQuestions");
        }
    }
}
