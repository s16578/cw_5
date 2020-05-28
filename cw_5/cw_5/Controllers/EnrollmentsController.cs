using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using cw_5.DTOs.Requests;
using cw_5.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        [Authorize(Roles = "employees")]
        public IActionResult EnrollStudents(EnrollStudentRequests newStudent)
        {
            try
            {
                var enrollment = _service.EnrollStudent(newStudent);
                var result = new ObjectResult(enrollment);
                result.StatusCode = (int)HttpStatusCode.Created;
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
        [Authorize(Roles = "employee")]

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
            catch (Exception execption)
            {
                var result = new StatusCodeResult((int)HttpStatusCode.InternalServerError);
                return result;
            }
        }

        [HttpPost]
        [Route("api/login")]

        public IActionResult Login(LoginRequest request)
        {

            var student = _service.Login(request);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, student.FirstName),
                new Claim(ClaimTypes.NameIdentifier, student.Index),
                new Claim(ClaimTypes.Role, student.Role)
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("asdhwuqidqkjdahszxcmnbdeiqpwoieqlaskdczlasjdi")); //Brak klasy Configuration (???)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "Gakko",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds
            );


            var refreshToken = Guid.NewGuid();
            _service.InsertToken(refreshToken, student.Index);

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken
            });
        }

        [HttpPost]
        [Route("api/token/{token}")]
        public IActionResult RefreshToken(Guid token)
        {
            var student = _service.RefreshToken(token);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, student.FirstName),
                new Claim(ClaimTypes.NameIdentifier, student.Index),
                new Claim(ClaimTypes.Role, student.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("asdhwuqidqkjdahszxcmnbdeiqpwoieqlaskdczlasjdi"));      //Brak klasy Configuration (???)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenJwt = new JwtSecurityToken
          (
              issuer: "Gakko",
              audience: "Students",
              claims: claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: creds
          );

            var newRefreshToken = Guid.NewGuid();
            _service.InsertToken(newRefreshToken, student.Index);

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(tokenJwt),
                newRefreshToken
            });
        }


    }
}