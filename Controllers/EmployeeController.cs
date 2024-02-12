using EaseDeskMaster_API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EaseDeskMaster_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeeController : ControllerBase
	{
		private readonly IEmployeeService _employeeService;
		public EmployeeController(IEmployeeService employeeService) {
			this._employeeService = employeeService;
		}
		[HttpGet]
		public IActionResult Get()
		{
			var data = this._employeeService.GetAll();
			if (data == null)
			{
				return NotFound();
			}
			return Ok(data);
		}
	}
}
