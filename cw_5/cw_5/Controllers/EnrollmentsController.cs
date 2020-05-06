
using cw_5.DTOs.Requests;
using cw_5.Services;
using Microsoft.AspNetCore.Mvc;

namespace cw_5.Controllers
{

    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentDbService _service;

        public EnrollmentsController(IStudentDbService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult EnrollStudents(EnrollStudentRequests newStudent)
        {
            var service = new SqlServerStudentDbService();
            return service.EnrollStudents(newStudent);
            // zwrocenie obiektu enrollment
        }

        [Route("api/enrollments/promotions")]
        [HttpPost]

        public IActionResult EnrollStudentPromotions(EnrollStudentPromotions promotion)
        {
            var service = new SqlServerStudentDbService();
            return service.EnrollStudentPromotions(promotion);
            
        }

    }


    
}