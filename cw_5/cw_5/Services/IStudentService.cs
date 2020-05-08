using cw_5.DTOs.Requests;
using cw_5.DTOs.Responses;

namespace cw_5.Services
{
    public interface IStudentService
    {
        EnrollStudentResponse EnrollStudent(EnrollStudentRequests newStudent);
        void EnrollStudentPromotions(EnrollStudentPromotions promotion);
    }
}
