using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EaseDeskMaster_API.Repos.Models;

[Table("tbl_employee")]
public partial class TblEmployee
{
	[Key]
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
}
