using cw_5.DTOs.Requests;
using cw_5.DTOs.Responses;
using cw_5.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AuthenticationSampleWebApp.Handlers
{
    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        public BasicAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
            ) : base(options, logger, encoder, clock)
        {

        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing authorization header");

            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var credentialsBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialsBytes).Split(":");

            if (credentials.Length != 2)
                return AuthenticateResult.Fail("Incorrect authorization header value");

            
            var service = new SqlServerStudentService();
            var loginRequest = new LoginRequest();
            var student = new StudentResponse();

            loginRequest.Index = credentials[0];
            loginRequest.Password = credentials[1];



            student = service.Login(loginRequest);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, student.FirstName),
                new Claim(ClaimTypes.NameIdentifier, student.Index),
                new Claim(ClaimTypes.Role, "employee")
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name); //Basic, ...
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
