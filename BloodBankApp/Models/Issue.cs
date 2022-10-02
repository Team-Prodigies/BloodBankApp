using BloodBankApp.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Models
{
    public class Issue
    {
        [Key]
        public Guid IssueId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Display(Name = "Date Reported")]
        [ReadOnly(true)]
        public DateTime DateReported { get; set; }  = DateTime.Now;
        public IssueStatus IssueStatus { get; set; } = IssueStatus.ONHOLD;
    }
}
