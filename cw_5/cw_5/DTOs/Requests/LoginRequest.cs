using System.ComponentModel.DataAnnotations;

namespace cw_5.DTOs.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage ="Missing index")]
        public string Index { get; set; }
        [Required(ErrorMessage ="Missing password")]
        public string Password { get; set; }

    }
}
