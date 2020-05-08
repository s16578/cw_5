using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw_5.DTOs.Responses
{
    public class PromotionStudentRepsonse
    {
        public int IdEnrollment { get; set; }
        public int Semester { get; set; }
        public int IdStudy { get; set; }
        public DateTime StartDate { get; set; }
    }
}
