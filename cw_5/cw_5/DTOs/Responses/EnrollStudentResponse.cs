using System;

namespace cw_5.DTOs.Responses
{
    public class EnrollStudentResponse
    {
        public int IdEnrollment { get; set; }
        public int Semester { get; set; }
        public int IdStudy { get; set; }
        public DateTime StartDate { get; set; }
    }
}
