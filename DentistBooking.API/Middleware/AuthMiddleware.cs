using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using DentistBooking.Helpers;


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

        public async Task InvokeAsync(HttpContext context)
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

                //check in database dentist
                if (true)
                {

                    // Return the decrypted email and role

                }
                else
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    return;
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
