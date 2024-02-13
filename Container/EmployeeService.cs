using AutoMapper;
using EaseDeskMaster_API.Helper;
using EaseDeskMaster_API.Model;
using EaseDeskMaster_API.Repos;
using EaseDeskMaster_API.Repos.Models;
using EaseDeskMaster_API.Service;
using Microsoft.EntityFrameworkCore;

namespace EaseDeskMaster_API.Container
{
	public class EmployeeService : IEmployeeService
	{
		private readonly DevMasterDataContext _context;
		private readonly IMapper _mapper;
		public EmployeeService(DevMasterDataContext context, IMapper mapper)
		{
			this._context = context;
			this._mapper = mapper;
		}

		public async Task<APIResponse> Create(EmployeeModel data)
		{
			APIResponse response = new();
			try
			{
				TblEmployee _employee = _mapper.Map<EmployeeModel, TblEmployee>(data);
				await _context.TblEmployees.AddAsync(_employee);
				await _context.SaveChangesAsync();
				response.ResponseCode = 201;
				response.Result = data.Code;

			}
			catch (Exception ex)
			{
				response.ResponseCode = 400;
				response.ErrorMessage = ex.Message;
			}
			return response;

		}

		public async Task<List<EmployeeModel>> GetAll()
		{
			List<EmployeeModel> _response = new();
			var _data = await _context.TblEmployees.ToListAsync();
			if (_data != null)
			{

				_response = this._mapper.Map<List<TblEmployee>, List<EmployeeModel>>(_data);
			}
			return _response;
		}



		public async Task<EmployeeModel> GetByCode(string code)
		{
			EmployeeModel _response = new();
			var _data = await this._context.TblEmployees.FindAsync(code);
			if (_data != null)
			{
				_response = this._mapper.Map<TblEmployee, EmployeeModel>(_data);
			}
			return _response;
		}

		public async Task<APIResponse> Remove(string code)
		{
			APIResponse response = new();
			try
			{
				var _employee = await _context.TblEmployees.FindAsync(code);
				if (_employee != null)
				{
					_context.TblEmployees.Remove(_employee);
					await _context.SaveChangesAsync();
				}
				else
				{
					response.ResponseCode = 404;
					response.ErrorMessage = "Data Not Found!";
				}
				response.ResponseCode = 201;
				response.Result = code;

			}
			catch (Exception ex)
			{
				response.ResponseCode = 400;
				response.ErrorMessage = ex.Message;
			}
			return response;
		}

		public async Task<APIResponse> Update(EmployeeModel data, string code)
		{
			APIResponse response = new();
			try
			{
				var _employee = await _context.TblEmployees.FindAsync(code);
				if (_employee != null)
				{
					_employee.Name = data.Name;
					_employee.Email = data.Email;
					_employee.Phone = data.Phone;
					_employee.Type = data.Type;
					_employee.Adress = data.Adress;
					_employee.EmployeeGroup = data.EmployeeGroup;
					_employee.IsActive = data.IsActive;
					await _context.SaveChangesAsync();
					response.ResponseCode = 200;
					response.Result = data.Code;
				}
				else
				{
					response.ResponseCode = 404;
					response.ErrorMessage = "Data Not Found!";
				}
				response.ResponseCode = 201;
				response.Result = code;

			}
			catch (Exception ex)
			{
				response.ResponseCode = 400;
				response.ErrorMessage = ex.Message;
			}
			return response;
		}
	}
}
