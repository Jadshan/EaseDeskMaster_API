using EaseDeskMaster_API.Repos.Models;
using Microsoft.EntityFrameworkCore;

namespace EaseDeskMaster_API.Repos;

public partial class DevMasterDataContext : DbContext
{
	public DevMasterDataContext()
	{
	}

	public DevMasterDataContext(DbContextOptions<DevMasterDataContext> options)
		: base(options)
	{
	}

	public virtual DbSet<TblEmployee> TblEmployees { get; set; }

	public virtual DbSet<TblRefreshtoken> TblRefreshtokens { get; set; }

	public virtual DbSet<TblUser> TblUsers { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<TblEmployee>(entity =>
		{
			entity.HasKey(e => e.Code).HasName("PK_tbl_customer");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
