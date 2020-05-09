using System.ComponentModel.DataAnnotations;

namespace cw_5.DTOs.Requests
{
    public class EnrollStudentPromotions
    {
        [Required(ErrorMessage = "Study name cannot be empty")]
        public string Studies { get; set; }

        [Required]
        [RegularExpression("[1-8]")]
        public int Semester { get; set; }
    }
}
