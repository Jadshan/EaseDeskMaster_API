using AutoMapper;
using EaseDeskMaster_API.Model;
using EaseDeskMaster_API.Repos;
using EaseDeskMaster_API.Repos.Models;
using EaseDeskMaster_API.Service;


namespace EaseDeskMaster_API.Container
{
	public class EmployeeService: IEmployeeService
	{
		private readonly DevMasterDataContext _context;
		private readonly IMapper _mapper;
		public EmployeeService(DevMasterDataContext context, IMapper mapper) {
		this._context = context;
			this._mapper = mapper;
		}
		public  List<EmployeeModel> GetAll() 
		{ 
			List<EmployeeModel> _response = new List<EmployeeModel>();
			var _data =	 this._context.TblEmployees.ToList();
			if (_data != null ) { 
			
			_response= this._mapper.Map<List<TblEmployee>,List<EmployeeModel>>(_data);
				}
			return _response;  
		}
	}
}
