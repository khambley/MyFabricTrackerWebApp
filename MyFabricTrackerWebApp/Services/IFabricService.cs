using MyFabricTrackerWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFabricTrackerWebApp.Services
{
	public interface IFabricService
	{
		IOrderedQueryable<Fabric> GetFabrics();
		IOrderedQueryable<Fabric> GetFabricsBySearchTerm(IOrderedQueryable<Fabric> fabrics, string searchString);
		string CreateUniqueItemCode();
		IOrderedQueryable<Fabric> SortFabrics(IOrderedQueryable<Fabric> fabrics, string sortOrder);
		Task<Fabric> GetFabricById(long? id);
	}
}
