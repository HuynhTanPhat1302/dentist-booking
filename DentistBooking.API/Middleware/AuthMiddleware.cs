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

namespace DentistBooking.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly List<string> _whitelistedPaths;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
            _whitelistedPaths = new List<string>
            {
                "/api/login", // Whitelisted login endpoint
                "/api/guest", // Whitelisted guest endpoint
                // Add more whitelisted paths as needed
            };
        }

        public async Task InvokeAsync(HttpContext context, IStaffService staffService)
        {
            var requestPath = context.Request.Path.Value;

            // Check if the request path is in the whitelist
            if (_whitelistedPaths.Contains(requestPath))
            {
                // Pass the request to the next middleware without performing authentication
                await _next(context);
                return;
            }

            // Continue with the authentication process for other routes

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
                            
                            break;
                        case "Dentist":
                            // Logic for retrieving staff information based on the dentist's role
                            // ...
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
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
