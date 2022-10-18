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
using BloodBankApp.Areas.SuperAdmin.Permission;

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
        [Authorize(Policy = Permissions.SuperAdmin.CreateQuestionnaire)]
        public IActionResult CreateQuestionnaire()        
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = Permissions.SuperAdmin.CreateQuestionnaire)]
        public async Task<IActionResult> CreateQuestionnaire(HealthFormQuestionnaire questionnaire)
        {
            var getQuestionnaire = _context.HealthFormQuestionnaires.FirstOrDefault();
            if (getQuestionnaire != null) {
                _notyfService.Error("Questionnaire exists!");
                return View(nameof(CreateQuestionnaire));
            }

            questionnaire.LastUpdated = DateTime.Now;
            _notyfService.Success("Questionnaire is created");

            await _context.HealthFormQuestionnaires.AddAsync(questionnaire);
            await _context.SaveChangesAsync();

            return View(nameof(CreateQuestionnaire));
        }

        [HttpGet]
        [Authorize(Policy = Permissions.SuperAdmin.ManageQuestions)]
        public async Task<IActionResult> ManageQuestions()
        {
            var getQuestions = await _context.Questions.ToListAsync();
            return View(getQuestions);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.SuperAdmin.AddQuestions)]
        public async Task<IActionResult> CreateQuestion()
        {
            var getQuestionnaire =await _context.HealthFormQuestionnaires.ToListAsync();
            if (getQuestionnaire.Count == 0)
            {
                _notyfService.Error("Pls create a questionnaire first!");
                return RedirectToAction("CreateQuestionnaire");
            }
            return View();
        }

        [HttpPost]
        [Authorize(Policy = Permissions.SuperAdmin.AddQuestions)]
        public async Task<IActionResult> CreateQuestion(Question question)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Pls fill all the questions!");
                return View(nameof(CreateQuestion));
            }
            var getQuestionnaire = _context.HealthFormQuestionnaires.FirstOrDefault();
            if (getQuestionnaire != null) question.HealthFormQuestionnaireId = getQuestionnaire.HealthFormQuestionnaireId;

            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();

            _notyfService.Success("Question successfully created!");
            return RedirectToAction("ManageQuestions");
        }

        [HttpGet]
        [Authorize(Policy = Permissions.SuperAdmin.EditQuestions)]
        public async Task<IActionResult> Edit(Guid questionId)
        {
            if (questionId == Guid.Empty)
            {
                _notyfService.Error("The Question does not exist");
                return RedirectToAction("ManageQuestions");
            }

            var getQuestion = await _questionService.EditQuestion(questionId);
            ViewBag.Answer = Answer;
            if (getQuestion == null)
            {
                _notyfService.Error("The Question does not exist");
                return RedirectToAction("ManageQuestions");
            }

            return View(getQuestion);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.SuperAdmin.EditQuestions)]
        public async Task<IActionResult> Edit(QuestionModel questionModel, Guid questionId)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Answer = Answer;
                _notyfService.Error("Please fill form");
                return View(nameof(Edit));
            }
            if (questionId == Guid.Empty)
            {
                ViewBag.Answer = Answer;
                _notyfService.Error("The Question does not exist");
                return View(nameof(Edit));
            }
            var getQuestion = await _questionService.EditQuestion(questionModel, questionId);
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

        [Authorize(Policy = Permissions.SuperAdmin.DeleteQuestions)]
        public async Task<IActionResult> Delete(Guid questionId)
        {
            if (questionId == Guid.Empty)
            {
                _notyfService.Error("The Question does not exist");
                return View(nameof(ManageQuestions));
            }
            var getQuestion = await _context.Questions.FindAsync(questionId);

            _context.Questions.Remove(getQuestion);
            await _context.SaveChangesAsync();
            _notyfService.Success("Question Deleted!");
            return RedirectToAction("ManageQuestions");
        }
    }
}
