using cw_5.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw_5.Services
{
    public interface IStudentDbService
    {
        void EnrollStudents(EnrollStudentRequests request);
        void EnrollStudentPromotions(EnrollStudentPromotions request);
    }
}
