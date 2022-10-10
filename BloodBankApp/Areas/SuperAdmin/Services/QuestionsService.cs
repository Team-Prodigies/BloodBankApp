using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Data;
using System;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services {
    public class QuestionsService : IQuestionService 
    {
        private readonly ApplicationDbContext _context;

        public QuestionsService(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<QuestionModel> EditQuestion(Guid questionId) {
            var getQuestion = await _context.Questions.FindAsync(questionId);

            var question = new QuestionModel {
                Description = getQuestion.Description,
                Answer = getQuestion.Answer,
                QuestionId = getQuestion.HealthFormQuestionnaireId
            };
            return question;
        }

        public async Task<bool> EditQuestion(QuestionModel questionModel, Guid questionId) {
            var getQuestion = await _context.Questions.FindAsync(questionId);
            getQuestion.Description = questionModel.Description;
            getQuestion.Answer = questionModel.Answer;

            if (getQuestion == null) {
                return false;
            }
            _context.Questions.Update(getQuestion);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}