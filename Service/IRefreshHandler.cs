namespace EaseDeskMaster_API.Service
{
	public interface IRefreshHandler
	{
		Task<string> GenerateToken(string userName);
	}
}
