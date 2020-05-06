using cw_5.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace cw_5.Services
{
    public interface IStudentDbService
    {
        IActionResult EnrollStudents(EnrollStudentRequests newStudent);
        IActionResult EnrollStudentPromotions(EnrollStudentPromotions promotion);
    }
}
