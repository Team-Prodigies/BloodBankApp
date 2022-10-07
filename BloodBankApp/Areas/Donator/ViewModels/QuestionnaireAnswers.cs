using System.Collections.Generic;

namespace BloodBankApp.Areas.Donator.ViewModels
{
    public class QuestionnaireAnswers
    {
        public List<QuestionViewModel> Questions { get; set; }

        public QuestionnaireAnswers()
        {

        }
        public QuestionnaireAnswers(List<QuestionViewModel> questions)
        {
            Questions = questions;
        }
    }
}
