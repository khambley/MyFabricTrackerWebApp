using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFabricTrackerWebApp.Models
{
	public class MainCategory
	{
		public int MainCategoryId { get; set; }
		public string MainCategoryName { get; set; }
		public List<SubCategory> SubCategories { get; set; }

	}
}
