using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyFabricTrackerWebApp.Models
{
	public class Fabric
	{
		public int FabricID { get; set; }
		public string FabricItemCode { get; set; }
		public string FabricName { get; set; }
		public IFormFile ImageFile { get; set; }
		public int MainCategoryId { get; set; }
		public virtual MainCategory MainCategory { get; set; }
		public int SubCategoryId { get; set; }
		public virtual SubCategory SubCategory { get; set; }
		public string FabricType { get; set; }
		public string FabricNotes { get; set; }
		public string FabricSourceName { get; set; }
		public string FabricSourceUrl { get; set; }
		public DateTime DateAdded { get; set; }
		public DateTime? DateModified { get; set; }








	}
}
