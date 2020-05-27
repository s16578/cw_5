using cw_5.DTOs.Requests;
using cw_5.DTOs.Responses;
using System;

namespace cw_5.Services
{
    public interface IStudentService
    {
        EnrollStudentResponse EnrollStudent(EnrollStudentRequests newStudent);
        PromotionStudentRepsonse EnrollStudentPromotions(EnrollStudentPromotions promotion);
        IndexStudentResponse GetStudent(string index);
        StudentResponse Login(LoginRequest login);
        void InsertToken(Guid token, string Index);
        StudentResponse RefreshToken(Guid token);

        
    }
}
