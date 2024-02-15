using System.Security.Cryptography;
using EaseDeskMaster_API.Repos;
using EaseDeskMaster_API.Repos.Models;
using EaseDeskMaster_API.Service;
using Microsoft.EntityFrameworkCore;

namespace EaseDeskMaster_API.Container
{
	public class RefreshService : IRefreshHandler
	{
		private readonly DevMasterDataContext _context;
		public RefreshService(DevMasterDataContext context)
		{
			_context = context;
		}
		public async Task<string> GenerateToken(string userName)
		{
			var randomnumber = new byte[32];
			using (var randomnumbergenerator = RandomNumberGenerator.Create())
			{
				randomnumbergenerator.GetBytes(randomnumber);
				string refreshToken = Convert.ToBase64String(randomnumber);
				var Existtoken = _context.TblRefreshtokens.FirstOrDefaultAsync(item => item.UserId == userName).Result;
				if (Existtoken != null)
				{
					Existtoken.RefreshToken = refreshToken;
				}
				else
				{
					await _context.TblRefreshtokens.AddAsync(new TblRefreshtoken
					{
						UserId = userName,
						TokenId = new Random().Next().ToString(),
						RefreshToken = refreshToken
					});
				}
				await _context.SaveChangesAsync();

				return refreshToken;

			}

		}
	}
}
