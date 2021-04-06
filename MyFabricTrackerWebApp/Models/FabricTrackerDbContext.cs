using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFabricTrackerWebApp.Models;

namespace MyFabricTrackerWebApp.Models
{
	public class FabricTrackerDbContext : DbContext
	{
		public FabricTrackerDbContext(DbContextOptions<FabricTrackerDbContext> options) 
			: base(options) { }

		public DbSet<Fabric> Fabrics { get; set; }
		public DbSet<FabricType> FabricTypes { get; set; }
		public DbSet<MainCategory> MainCategories { get; set; }
		public DbSet<Source> Sources { get; set; }
		public DbSet<SubCategory> SubCategories { get; set; }
		public DbSet<Transaction> Transactions { get; set; }
		public DbSet<MyFabricTrackerWebApp.Models.FabricType> FabricType { get; set; }
	}
}
