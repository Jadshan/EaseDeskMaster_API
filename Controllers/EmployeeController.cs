﻿using EaseDeskMaster_API.Model;
using EaseDeskMaster_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EaseDeskMaster_API.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeeController : ControllerBase
	{
		private readonly IEmployeeService _employeeService;
		public EmployeeController(IEmployeeService employeeService)
		{
			_employeeService = employeeService;
		}

		[AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var data = await _employeeService.GetAll();
			if (data == null)
			{
				return NotFound();
			}
			return Ok(data);
		}

		[HttpGet("GetByCode")]
		public async Task<IActionResult> GetByCode(string code)
		{
			var data = await _employeeService.GetByCode(code);
			if (data.Code == null)
			{
				return NotFound();
			}
			return Ok(data);
		}

		[HttpPost("Create")]
		public async Task<IActionResult> Create(EmployeeModel _employee)
		{
			var data = await _employeeService.Create(_employee);

			return Ok(data);
		}

		[HttpPut("Update")]
		public async Task<IActionResult> Update(EmployeeModel _employee, string code)
		{
			var data = await _employeeService.Update(_employee, code);

			return Ok(data);
		}

		[HttpDelete("Delete")]
		public async Task<IActionResult> Delete(string code)
		{
			var data = await _employeeService.Remove(code);

			return Ok(data);
		}
	}
}
