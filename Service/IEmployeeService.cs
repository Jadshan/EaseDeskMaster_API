using System.Collections.Generic;
using EaseDeskMaster_API.Repos.Models;

namespace EaseDeskMaster_API.Service
{
	public interface IEmployeeService
	{
		List<TblEmployee> GetAll();
	}
}
