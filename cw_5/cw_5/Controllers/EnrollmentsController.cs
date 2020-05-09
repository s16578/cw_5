
using System;
using System.Net;
using cw_5.DTOs.Requests;
using cw_5.Services;
using Microsoft.AspNetCore.Mvc;

namespace cw_5.Controllers
{

    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentService _service;

        public EnrollmentsController(IStudentService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult EnrollStudents(EnrollStudentRequests newStudent)
        {
            try
            {
                var enrollment = _service.EnrollStudent(newStudent);
                var result = new ObjectResult(enrollment);
                result.StatusCode = (int) HttpStatusCode.Created;
                return result;
            }
            catch (InvalidOperationException invalidOperation)
            {
                return BadRequest(invalidOperation.Message);
            }
            catch (Exception exception)
            {
                var result = new StatusCodeResult((int)HttpStatusCode.InternalServerError);
                return result;
            }
        }

        [Route("api/enrollments/promotions")]
        [HttpPost]

        public IActionResult EnrollStudentPromotions(EnrollStudentPromotions promotion)
        {
            try
            {
                var enrollment = _service.EnrollStudentPromotions(promotion);
                var result = new ObjectResult(enrollment);
                result.StatusCode = (int)HttpStatusCode.Created;
                return result;
            }
            catch (InvalidOperationException invalidOperation)
            {
                return BadRequest(invalidOperation.Message);
            }
            catch(Exception execption)
            {
                var result = new StatusCodeResult((int)HttpStatusCode.InternalServerError);
                return result;
            }
        }

    }


    
}