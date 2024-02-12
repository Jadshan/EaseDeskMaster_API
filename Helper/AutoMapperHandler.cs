
using AutoMapper;
using EaseDeskMaster_API.Model;
using EaseDeskMaster_API.Repos.Models;

namespace EaseDeskMaster_API.Helper

{
	public class AutoMapperHandler: Profile
	{
		public AutoMapperHandler() { 
			CreateMap<TblEmployee, EmployeeModel>().ForMember(item => item.StatusName, opt => opt.MapFrom(
				item => item.IsActive.HasValue && item.IsActive.Value ? "Active" : "In active")
			
			);
		}
	}
}
 