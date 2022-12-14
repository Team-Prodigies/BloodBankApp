using BloodBankApp.Areas.SuperAdmin.ViewModels;
using System;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services.Interfaces {
    public interface IQuestionService {
        Task<QuestionModel> EditQuestion(Guid questionId);
        Task<bool> EditQuestion(QuestionModel questionModel, Guid questionId);
    }
}