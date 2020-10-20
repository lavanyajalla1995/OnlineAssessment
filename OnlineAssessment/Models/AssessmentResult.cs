using System.Collections.Generic;

namespace OnlineAssessment.Models
{
    public class AssessmentResult
    {
        public List<AssessmentTest> Test { get; set; }
        public int PageNo { get; set; }
        public decimal TotalPages { get; set; }
        public bool ShowResult { get; set; } = false;
        public int TotalMarksSecured { get; set; }
        public int TotalMarks { get; set; }
        public decimal Percentage { get; set; }
        public Status  Status{ get; set; }
        public int PassMarks { get; set; } = 1;
        public int Timer { get; set; }
    }
    public enum Status
    {
        Outstanding,
        Excellent,
        Good,
        Average,
        Poor
    }
}
