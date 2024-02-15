using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using EaseDeskMaster_API.Repos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


namespace EaseDeskMaster_API.Helper
{
	public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
	{
		private readonly DevMasterDataContext _context;
		public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, DevMasterDataContext context) : base(options, logger, encoder, clock)
		{
			_context = context;
		}

		protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			if (!Request.Headers.ContainsKey("Authorization"))
			{
				return AuthenticateResult.Fail("No Authorization header found");
			}

			var headerValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
			if (headerValue == null || !headerValue.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
			{
				return AuthenticateResult.Fail("Invalid or missing Basic Authentication header");
			}

			try
			{
				if (string.IsNullOrEmpty(headerValue.Parameter))
				{
					return AuthenticateResult.Fail("Empty credentials");
				}

				var bytes = Convert.FromBase64String(headerValue.Parameter);
				string credentials = Encoding.UTF8.GetString(bytes);
				string[] array = credentials.Split(':');

				if (array.Length != 2)
				{
					return AuthenticateResult.Fail("Invalid credentials format");
				}

				string userName = array[0];
				string password = array[1];

				var user = await _context.TblUsers.FirstOrDefaultAsync(item => item.UserName == userName && item.Password == password);

				if (user != null)
				{
					var claims = new[] { new Claim(ClaimTypes.Name, user.Code) };
					var identity = new ClaimsIdentity(claims, Scheme.Name);
					var principal = new ClaimsPrincipal(identity);
					var ticket = new AuthenticationTicket(principal, Scheme.Name);
					return AuthenticateResult.Success(ticket);
				}
				else
				{
					return AuthenticateResult.Fail("Unauthorized");
				}
			}
			catch (Exception)
			{
				return AuthenticateResult.Fail("An error occurred while processing credentials");
			}
		}
	}
}
