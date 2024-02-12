using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EaseDeskMaster_API.Model
{
	public class EmployeeModel
	{
		[StringLength(50)]
		[Unicode(false)]
		public string Code { get; set; } = null!;

		[StringLength(50)]
		[Unicode(false)]
		public string Name { get; set; } = null!;

		[StringLength(50)]
		[Unicode(false)]
		public string? Email { get; set; }

		[StringLength(50)]
		[Unicode(false)]
		public string? Phone { get; set; }

		[StringLength(50)]
		[Unicode(false)]
		public string? Type { get; set; }

		[StringLength(250)]
		[Unicode(false)]
		public string? Adress { get; set; }

		[StringLength(50)]
		[Unicode(false)]
		public string? EmployeeGroup { get; set; }

		public bool? IsActive { get; set; }

		public string? StatusName { get; set; }



	}
}
