using Microsoft.AspNetCore.Mvc;
using OnlineAssessment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Textfile = System.IO;


namespace OnlineAssessment.Controllers
{
    public class HomeController : Controller
    {
        List<OnlineAssementEntity> QuesAns = new List<OnlineAssementEntity>();
        List<AssessmentTest> tests = new List<AssessmentTest>();

        public int MaxPageNo = 2;
        
       
        public HomeController()
        {
            LoadOnlineAssessment();
        }

        private void LoadOnlineAssessment()
        {
            string[] filePth = Textfile.File.ReadAllLines(@"C:\Users\Lavanya\source\repos\OnlineAssessment\OnlineAssessment\QuestionPaper.txt");
            foreach (var item in filePth)
            {
                OnlineAssementEntity online = new OnlineAssementEntity();
                string[] value = item.Split("?");
                online.Question = value[0];
                string[] opt = value[1].Split(",");
                online.Options = opt.ToList();
                online.Answer = value[2];
                QuesAns.Add(online);

            }
            foreach (OnlineAssementEntity it in QuesAns)
            {
                AssessmentTest selectedAnswer = new AssessmentTest();
                selectedAnswer.Question = it.Question;
                selectedAnswer.Options = it.Options;
                selectedAnswer.Answer = it.Answer;
                tests.Add(selectedAnswer);
            }

        }

        public IActionResult Index( )
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Set(Constants.SubmitedAnswer, tests);
            int page = 1;
            decimal totalpage = AllPagesDisply(QuesAns.Count);
            AssessmentResult result = MaxQuesDisplay(page,totalpage);
            result.Timer = QuesAns.Count * 30;
            return View("Exam",result);
        }
        public decimal AllPagesDisply(int allpages)
        {
            decimal total = allpages / Convert.ToDecimal(MaxPageNo);
            return Math.Ceiling(total);
        }
        private AssessmentResult MaxQuesDisplay(int page,decimal totalpage)
        {

            AssessmentResult result = new AssessmentResult();
            if (page==totalpage+1)
            {
                List<AssessmentTest> allResult = HttpContext.Session.Get<AssessmentTest>(Constants.SubmitedAnswer);
                result.Test = allResult;
                result.ShowResult = true;
                result.TotalMarks = QuesAns.Count;
                foreach (var i in allResult)
                {
                    if (i.Answer==i.SelectedAnswer)
                    {
                        result.TotalMarksSecured++;
                    }
                   
                }
                result.Percentage = result.TotalMarksSecured / Convert.ToDecimal(result.TotalMarks) / 100;
                ComputeStatus(result);
                return result;
            }



            List<AssessmentTest> assessment = new List<AssessmentTest>();

            QuesAns.Skip((page - 1) * MaxPageNo).Take(MaxPageNo).ToList()
                .ForEach(p =>
            {
                AssessmentTest assessmentTest = new AssessmentTest();
                assessmentTest.Question = p.Question;
                assessmentTest.Options = p.Options;
                assessment.Add(assessmentTest);
                result.Test = assessment;
                result.PageNo = page;
                result.TotalPages = totalpage;

            });
            return result;

        }
        [HttpPost]
        public ActionResult Exam(AssessmentResult re)
        {
            UpdatedSubmitedAns(re);
            return View("Exam", MaxQuesDisplay(re.PageNo,re.TotalPages));
        }
        private void UpdatedSubmitedAns(AssessmentResult result)
        {
            List<AssessmentTest> submitedAns = HttpContext.Session.Get<AssessmentTest>(Constants.SubmitedAnswer);
            result.Test.ForEach(a =>
            {
                AssessmentTest test = submitedAns.Where(c => c.Question == a.Question).FirstOrDefault();
                test.SelectedAnswer = a.SelectedAnswer;

            });
            HttpContext.Session.Clear();
            HttpContext.Session.Set(Constants.SubmitedAnswer, submitedAns);
        }
        private void ComputeStatus(AssessmentResult res)
        {
            if (res.Percentage<=35)
            {
                res.Status = Status.Poor;
            }
            else if (res.Percentage<=50)
            {
                res.Status = Status.Average;
            }
            else if (res.Percentage<=65)
            {
                res.Status = Status.Good;
            }
            else if (res.Percentage<=80)
            {
                res.Status = Status.Excellent;
            }
            else 
            {
                res.Status = Status.Outstanding;
            }
        }
        
        

            public IActionResult Home()
        {
            return View();
        }

    }
}
