using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using ContractInProcessAPI.Models;
using System.Net.Http.Headers;
using System.Configuration;

namespace ContractInProcessAPI.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly AuthenticationSettingsME _authenticationSettings;
        private readonly IConfiguration _configuration;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IOptions<AuthenticationSettingsME> authenticationSettings,
            IConfiguration configuration)
            : base(options, logger, encoder, clock)
        {
            _authenticationSettings = authenticationSettings.Value;
            _configuration = configuration;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                var credentials = GetCredentialsFromHeader(authHeader);
                var username = credentials[0];
                var password = credentials[1];

                var validUsers = _configuration.GetSection("AuthenticationSettingsME:Users").Get<List<AuthenticationSettingsME>>();
                //var user = validUsers.FirstOrDefault(u => u.UserName == username && u.Password == password);

                if (username == null || password == null || username == null && password == null)
                {
                    return AuthenticateResult.Fail("Invalid credentials");
                }

                var claims = new[] { new Claim(ClaimTypes.Name, username) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);

                return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Error: {ex.Message}");
            }
        }

        private string[] GetCredentialsFromHeader(string header)
        {
            var encodedCredentials = header.Substring("Basic ".Length).Trim();
            var credentialBytes = Convert.FromBase64String(encodedCredentials);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');

            if (credentials.Length != 2)
            {
                throw new FormatException("Invalid credentials format");
            }

            return credentials;
        }
    }
}
