﻿using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace DentistBooking.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class AuthController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Send a request to the Authentication server to get the JWT
            var authenticationServerUrl = "https://localhost:7214/api/auth/login";
            var response = await _httpClient.PostAsJsonAsync(authenticationServerUrl, model);

            if (response.IsSuccessStatusCode)
            {
                // Read the JWT from the response
                var jwt = await response.Content.ReadAsStringAsync();

                // Return the JWT to the client
                return Ok(jwt);
            }
            else
            {
                // Return an error response if the login request failed
                var errorMessage = await response.Content.ReadAsStringAsync();
                return Unauthorized(errorMessage);
            }
        }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
