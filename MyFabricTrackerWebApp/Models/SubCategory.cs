using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFabricTrackerWebApp.Models
{
	public class SubCategory
	{
		public int SubCategoryId { get; set; }
		public string SubCategoryName { get; set; }
		public int MainCategoryId { get; set; }
		public virtual MainCategory MainCategory { get; set; }

	}
}
