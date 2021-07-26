using Microsoft.EntityFrameworkCore;
using MyFabricTrackerWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFabricTrackerWebApp.Services
{
	public class FabricService : IFabricService
	{
		private readonly FabricTrackerDbContext _db;

		public FabricService(FabricTrackerDbContext db)
		{
			_db = db;
		}

		#region Get Methods
		public IOrderedQueryable<Fabric> GetFabrics()
		{
			return _db.Fabrics
				.Include(f => f.MainCategory)
				.Include(f => f.SubCategory)
				.Include(f => f.Source)
				.Include(f => f.FabricType)
				.OrderByDescending(f => f.DateAdded);
		}
		public IOrderedQueryable<Fabric> GetFabricsBySearchTerm(IOrderedQueryable<Fabric> fabrics, string searchString)
		{
			return fabrics.Where(f => f.FabricItemCode.ToUpper().Contains(searchString.ToUpper())).OrderByDescending(f => f.DateAdded);
		}
        public Task<Fabric> GetFabricById(long? id)
		{
            return _db.Fabrics
                .Include(f => f.MainCategory)
                .Include(f => f.SubCategory)
                .Include(f => f.Source)
                .Include(f => f.FabricType)
                .FirstOrDefaultAsync(m => m.FabricID == id);
        }
		#endregion
        public IOrderedQueryable<Fabric> SortFabrics(IOrderedQueryable<Fabric> fabrics, string sortOrder)
		{
            switch (sortOrder)
            {
                case "name_desc":
                    fabrics = fabrics.OrderByDescending(f => f.FabricName);
                    return fabrics;
                case "name_asce":
                    fabrics = fabrics.OrderBy(f => f.FabricName);
                    return fabrics;
                case "date_asce":
                    fabrics = fabrics.OrderBy(f => f.DateAdded);
                    return fabrics;
                case "date_desc":
                    fabrics = fabrics.OrderByDescending(f => f.DateAdded);
                    return fabrics;
                default:
                    fabrics = fabrics.OrderByDescending(f => f.DateAdded);
                    return fabrics;
            }
        }
		#region Item Code Methods
		public string CreateUniqueItemCode()
        {
            var lastFabricinDB = _db.Fabrics
                .OrderByDescending(f => f.FabricID)
                .FirstOrDefault();

            var newFabricId = lastFabricinDB != null ? lastFabricinDB.FabricID + 1 : 1;

            string leadingZeroes = "";

            if (newFabricId > 0 && newFabricId < 10)
            {
                leadingZeroes = "0000";
            }
            else if (newFabricId > 9 && newFabricId < 100)
            {
                leadingZeroes = "000";
            }
            else if (newFabricId > 99 && newFabricId < 1000)
            {
                leadingZeroes = "00";
            }
            else if (newFabricId > 999 && newFabricId < 10000)
            {
                leadingZeroes = "0";
            }
            else
            {
                leadingZeroes = "";
            }
            var uniqueFabricItemCode = "FAB"
                + "-"
                + leadingZeroes
                + newFabricId.ToString()
                + "-"
                + Guid.NewGuid().ToString().ToUpper().Substring(0, 4);

            return uniqueFabricItemCode;
        }
		#endregion


	}
}
