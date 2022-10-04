using BloodBankApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.Donator.ViewModels
{
    public class QuestionnaireAnswers
    {
        public List<Question> Questions { get; set; }

        public QuestionnaireAnswers()
        {

        }
        public QuestionnaireAnswers(List<Question> questions)
        {
            Questions = questions;
        }
    }
}
