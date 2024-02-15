using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EaseDeskMaster_API.Model;
using EaseDeskMaster_API.Repos;
using EaseDeskMaster_API.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EaseDeskMaster_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthorizeController : ControllerBase
	{
		private readonly DevMasterDataContext _context;
		private readonly JwtSettings _jwtSettings;
		private readonly IRefreshHandler _refresh;
		public AuthorizeController(DevMasterDataContext context, IOptions<JwtSettings> options, IRefreshHandler refresh)
		{
			_context = context;
			_jwtSettings = options.Value;
			_refresh = refresh;

		}
		[HttpPost("GenerateToken")]
		public async Task<ActionResult> GenerateToken([FromBody] UserCredential userCred)
		{
			var user = await _context.TblUsers.FirstOrDefaultAsync(item => item.UserName == userCred.UserName && item.Password == userCred.Password);

			if (user != null)
			{
				//generate token
				var tokenhandler = new JwtSecurityTokenHandler();
				var tokenkey = Encoding.UTF8.GetBytes(_jwtSettings.securityKey);
				var tokendesc = new SecurityTokenDescriptor
				{
					Subject = new ClaimsIdentity(new Claim[]
					{
						new Claim(ClaimTypes.Name,user.UserName),
						new Claim(ClaimTypes.Role,user.Role)
					}),
					Expires = DateTime.UtcNow.AddSeconds(300),
					SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
				};
				var token = tokenhandler.CreateToken(tokendesc);
				var finaltoken = tokenhandler.WriteToken(token);
				return Ok(new TokenResponse() { Token = finaltoken, RefreshToken = await _refresh.GenerateToken(userCred.UserName) });
			}
			else
			{
				return Unauthorized();
			}
		}

		[HttpPost("GenerateRefreshToken")]
		public async Task<ActionResult> GenerateToken([FromBody] TokenResponse token)
		{
			var _refreshToken = await _context.TblRefreshtokens.FirstOrDefaultAsync(item => item.RefreshToken == token.RefreshToken);

			if (_refreshToken != null)
			{
				//generate token
				var tokenhandler = new JwtSecurityTokenHandler();
				var tokenkey = Encoding.UTF8.GetBytes(_jwtSettings.securityKey);
				SecurityToken securityToken;
				var principal = tokenhandler.ValidateToken(token.Token, new TokenValidationParameters()
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(tokenkey),
					ValidateIssuer = false,
					ValidateAudience = false,

				}, out securityToken);

				var _token = securityToken as JwtSecurityToken;
				if (_token != null && _token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
				{
					string userName = principal.Identity?.Name;
					var _existdata = await _context.TblRefreshtokens.FirstOrDefaultAsync(item => item.UserId == userName
					&& item.RefreshToken == token.RefreshToken);
					if (_existdata != null)
					{
						var _newToken = new JwtSecurityToken(
							claims: principal.Claims.ToArray(),
							expires: DateTime.Now.AddSeconds(30),
							signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.securityKey)),
							SecurityAlgorithms.HmacSha256)
							);

						var _finaltoken = tokenhandler.WriteToken(_newToken);
						return Ok(new TokenResponse() { Token = _finaltoken, RefreshToken = await _refresh.GenerateToken(userName) });
					}
					else
					{
						return Unauthorized();
					}
				}
				else
				{
					return Unauthorized();
				}
			}
			else
			{
				return Unauthorized();
			}
		}
	}
}
