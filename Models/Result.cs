using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace PRMS.Models
{
    public class Result
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string Name { set; get; }
        public int StudentId { set; get; }
        public int RegNo { set; get; }
        public string Faculty { get; set; }
        public string Session { set; get; }
        public int Semester { get; set; }
        public string Degree { get; set; }
        public string CourseResults { set; get; }
        public float GPA { get; set; }
        public float PrevCGPA { get; set; }
        public float PrevCCH { get; set; }
        public float CGPA { get; set; }
        public float CCH { get; set; }
        public string Remarks { get; set; }
        public Boolean IsMeritListed { get; set; }

        public Result()
        {

        }

        public static Boolean CalculateResult(List<StudentInfo> Students, int Semester, List<Course> Courses)
        {
            if (Students == null || Semester == 0 || Courses == null) return false;

            ProjectDB db = new ProjectDB();
            JavaScriptSerializer js = new JavaScriptSerializer();

            foreach (StudentInfo Student in Students)
            {
                Result Result = new Result();
                Result.Name = Student.Name;
                Result.StudentId = Student.StudentId;
                Result.RegNo = Student.Reg;
                Result.Faculty = Student.Faculty;
                Result.Session = Student.Session;
                Result.Semester = Semester;
                Result.Degree = "";

                float TotalCHGP = 0, TotalCH = 0;
                string Remarks = "";
                List<CourseResult> CourseResults = new List<CourseResult>();
                foreach (Course Course in Courses)
                {
                    Marks mark = new Marks().GetMark(Course.UnderFaculty, Course.Course_code, Student.StudentId);
                    if (mark != null)
                    {
                        float TotalMark = 0;

                        if (mark.Mid != -1) TotalMark += mark.Mid;
                        if (mark.Assignment != -1) TotalMark += mark.Assignment;
                        if (mark.Attendence != -1) TotalMark += mark.Attendence;
                        if (mark.Final != -1) TotalMark += mark.Final;

                        CourseResult CourseResult = new CourseResult();
                        CourseResult.CourseCode = Course.Course_code;
                        CourseResult.CourseTitle = Course.Course_title;
                        CourseResult.CreditHours = Course.Credit_hour;
                        CourseResult.TotalMark = TotalMark;
                        CourseResult.LetterGrade = Marks.CalculateLetterGrade(TotalMark);
                        float GP = Marks.CalculateGP(TotalMark);
                        CourseResult.GP = GP;
                        Boolean IsPassed = Marks.IsPassed(TotalMark);
                        CourseResult.IsPassed = IsPassed;

                        CourseResults.Add(CourseResult);

                        if (IsPassed)
                        {
                            TotalCHGP += (Course.Credit_hour * GP);
                            TotalCH += Course.Credit_hour;
                        }
                        else
                        {
                            Remarks += Course.Course_code + ",";
                        }
                    }
                }
                float GPA = 0;
                if(CourseResults != null) Result.CourseResults = js.Serialize(CourseResults);
                if (TotalCHGP != 0 && TotalCH != 0) GPA = TotalCHGP / TotalCH;
                Result.GPA = GPA;
                if (Remarks != "") Remarks = Remarks.Substring(0, Remarks.Length - 1);
                if (Semester == 1)
                {
                    Result.PrevCGPA = 0;
                    Result.PrevCCH = 0;
                    Result.CGPA = GPA;
                    Result.CCH = TotalCH;
                    Result.Remarks = Remarks;
                    if (Remarks == "") Result.IsMeritListed = true;
                    else Result.IsMeritListed = false;
                }
                else
                {
                    int PrevSem = Semester - 1;
                    Result PrevRes = db.FinalResults.Where(fr => fr.StudentId == Result.StudentId && fr.Semester == PrevSem).FirstOrDefault();
                    if (PrevRes != null)
                    {
                        Result.PrevCGPA = PrevRes.CGPA;
                        Result.PrevCCH = PrevRes.CCH;
                        Result.CGPA = Marks.CalculateCGPA(GPA, TotalCH, PrevRes.CGPA, PrevRes.CCH);
                        Result.CCH = TotalCH + PrevRes.CCH;
                        if (PrevRes.Remarks == "") Result.Remarks = Remarks;
                        else
                        {
                            string PrevRemarks = Course.CheckRemarks(Student.Faculty, Student.StudentId, PrevRes.Remarks);
                            if (Remarks != "") 
                            {
                                if(PrevRemarks != "" || PrevRemarks != null ) Result.Remarks = PrevRes.Remarks + "," + Remarks;
                                else Result.Remarks = Remarks;
                            }
                            else Result.Remarks = PrevRes.Remarks;
                        }

                        if (!PrevRes.IsMeritListed) Result.IsMeritListed = false;
                        else
                        {
                            if (Remarks == "") Result.IsMeritListed = true;
                            else Result.IsMeritListed = false;
                        }
                    }
                }

                Result OldRes = db.FinalResults.Where(fr => fr.StudentId == Result.StudentId && fr.Semester == Semester).FirstOrDefault();
                if (OldRes != null) db.FinalResults.Remove(OldRes); 
                db.FinalResults.Add(Result);
                db.SaveChanges();
            }

            return true;
        }

        public static Result GetResult(int StudentId, int Semester)
        {
            Result Result = new ProjectDB().FinalResults.Where(fr => fr.StudentId == StudentId && fr.Semester == Semester).FirstOrDefault();

            return Result;
        }

        public static List<Result> GetResults(string Session, int Semester)
        {
            int batchId = Convert.ToInt32(Session.Substring(2, 2)); //startIndex, Length
            int startRange = batchId * 100000;
            int endRange = ((batchId + 1) * 100000) - 1;

            ProjectDB db = new ProjectDB();
            List<Result> AllResults = new List<Result>();
            AllResults = db.FinalResults.Where(fr => fr.StudentId >= startRange && fr.StudentId <= endRange).ToList();

            return AllResults;
        }
    }
}