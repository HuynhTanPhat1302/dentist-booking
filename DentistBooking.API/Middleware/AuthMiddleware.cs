using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using DentistBooking.Helpers;
using DentistBooking.Infrastructure.Repositories;
using DentistBooking.Application.Interfaces;
using DentistBooking.Application.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DentistBooking.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly List<(string Path, string HttpMethod)> _whitelistedRoutes;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
            _whitelistedRoutes = new List<(string, string)>
        {
            // Whitelisted routes and methods
            ("/api/login", "POST"),
            ("/api/propose-appointments", "POST")
        };
        }

        public async Task InvokeAsync(HttpContext context, IStaffService staffService, IPatientService patientService, IDentistService dentistService)
        {
            var requestPath = context.Request.Path.Value;
            var requestMethod = context.Request.Method;

            // Check if the request path and method are in the whitelist
            if (_whitelistedRoutes.Any(x => x.Path == requestPath && x.HttpMethod == requestMethod))
            {
                // Pass the request to the next middleware without performing authentication
                await _next(context);
                return;
            }

            // Continue with the authentication process for other routes
            var authResult = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
            if (!authResult.Succeeded || authResult.Failure != null && authResult.Failure.GetType() == typeof(SecurityTokenExpiredException))
            {
                string responseMessage = "Invalid/Null token or Expired token"; // Your custom message
                context.Response.StatusCode = 401; // Unauthorized
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { Message = responseMessage }));
                return;
            }

            // Check if the request contains a valid JWT token
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = 401; // Unauthorized
                return;
            }

            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var (email, role) = DecryptToken.DecryptAToken(token);

            if (email != null && role != null)
            {
                using (var scope = context.RequestServices.CreateScope())
                {
                    var scopedStaffService = scope.ServiceProvider.GetRequiredService<IStaffService>();
                    var scopedPatientService = scope.ServiceProvider.GetRequiredService<IPatientService>();
                    var scopedDentistService = scope.ServiceProvider.GetRequiredService<IDentistService>();


                    // Check in the database for staff
                    switch (role)
                    {
                        case "Staff":
                            // Check database
                            var staff = scopedStaffService.GetStaffByEmail(email);
                            if (staff != null)
                            {
                                // Grant staff permission to the user
                                var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Email, email),
                                    new Claim(ClaimTypes.Role, "Staff")
                                };

                                var identity = new ClaimsIdentity(claims, "Bearer");
                                context.User = new ClaimsPrincipal(identity);
                                context.Request.Headers["Authorization"] = "Bearer " + token; // Set the modified token in the request headers
                                await context.SignInAsync("Bearer", context.User);
                            }
                            break;
                        case "Patient":
                            var patient = scopedPatientService.GetPatientByEmail(email);
                            if (patient != null)
                            {
                                // Grant staff permission to the user
                                var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Email, email),
                                    new Claim(ClaimTypes.Role, "Patient")
                                };

                                var identity = new ClaimsIdentity(claims, "Bearer");
                                context.User = new ClaimsPrincipal(identity);
                                context.Request.Headers["Authorization"] = "Bearer " + token; // Set the modified token in the request headers
                                await context.SignInAsync("Bearer", context.User);
                            }

                            break;
                        case "Dentist":
                            var dentist = scopedDentistService.GetDentistByEmail(email);
                            if (dentist != null)
                            {
                                // Grant staff permission to the user
                                var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Email, email),
                                    new Claim(ClaimTypes.Role, "Dentist")
                                };

                                var identity = new ClaimsIdentity(claims, "Bearer");
                                context.User = new ClaimsPrincipal(identity);
                                context.Request.Headers["Authorization"] = "Bearer " + token; // Set the modified token in the request headers
                                await context.SignInAsync("Bearer", context.User);
                            }
                            break;
                        default:
                            // Handle unknown or unsupported roles
                            context.Response.StatusCode = 401; // Unauthorized
                            break;
                    }
                }
            }
            else
            {
                context.Response.StatusCode = 401; // Unauthorized
                return;
            }

            // Rest of the authentication logic...

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }

    public class User
    {
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}
