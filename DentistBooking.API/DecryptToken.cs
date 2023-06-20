using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DentistBooking.Helpers
{
    public static class DecryptToken
    {

        private static readonly IConfiguration Configuration;

        static DecryptToken()
        {
            // Initialize the configuration instance
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public static (string email, string role) DecryptAToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("QZAAIVO8Jm5v01EVn7VQkkNaxWhrrfPbysLOvCP2iJk=");

            // Set the validation parameters for the token
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            try
            {
                // Decrypt and validate the token
                var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var _);
                //var email = claimsPrincipal.Identity.Name;
                //var role = claimsPrincipal.FindFirst("role")?.Value;
                var emailClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                var roleClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

                var email = emailClaim?.Value ?? string.Empty; // Assign an empty string if emailClaim is null
                var role = roleClaim?.Value ?? string.Empty; // Assign an empty string if roleClaim is null

                return (email, role);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during token decryption/validation
                // Log or return an error response as needed
                throw new Exception(ex.ToString());
                //return (null, null);
            }
        }
    }
}
