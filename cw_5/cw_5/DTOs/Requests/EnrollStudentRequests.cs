using System;
using System.ComponentModel.DataAnnotations;

namespace cw_5.DTOs.Requests
{
    public class EnrollStudentRequests
    {
        [Required(ErrorMessage = "Incorrect index / cannot be empty")]
        [RegularExpression("^s[0-9]+$")]
        public string Index { get; set; }
        
        [Required(ErrorMessage = "Name cannot be empty")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Last name cannot be empty")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage ="Date cannot be empty")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage ="Studies cannot be empty")]
        public string Studies { get; set; }
    }
}
