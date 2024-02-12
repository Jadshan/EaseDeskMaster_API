using EaseDeskMaster_API.Helper;
using EaseDeskMaster_API.Model;

namespace EaseDeskMaster_API.Service
{
	public interface IEmployeeService
	{
		Task<List<EmployeeModel>> GetAll();
		Task<EmployeeModel> GetByCode(string code);

		Task<APIResponse> Remove(string code);

		Task<APIResponse> Create(EmployeeModel data);

		Task<APIResponse> Update(EmployeeModel data, string code);


	}
}
