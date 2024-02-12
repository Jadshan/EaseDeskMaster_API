using EaseDeskMaster_API.Repos;
using EaseDeskMaster_API.Repos.Models;
using EaseDeskMaster_API.Service;

namespace EaseDeskMaster_API.Container
{
	public class EmployeeService: IEmployeeService
	{
		private readonly DevMasterDataContext _context;
		public EmployeeService(DevMasterDataContext context) {
		this._context = context;
		}
		public  List<TblEmployee> GetAll() 
		{ 
			return this._context.TblEmployees.ToList();
		}
	}
}
