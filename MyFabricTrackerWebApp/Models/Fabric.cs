using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyFabricTrackerWebApp.Models
{
	public class Fabric
	{
		public long FabricID { get; set; }
		public string FabricItemCode { get; set; }
		public string FabricName { get; set; }
		public string ImageFileName { get; set; }
		[NotMapped]
		public IFormFile ImageFile { get; set; }
		public long MainCategoryId { get; set; }
		public virtual MainCategory MainCategory { get; set; }
		public long SubCategoryId { get; set; }
		public virtual SubCategory SubCategory { get; set; }
		public long? TotalInches { get; set; }
		public string FabricType { get; set; }
		public string FabricWidth { get; set; }
		public string BackgroundColor { get; set; }
		public string? AccentColor1 { get; set; }
		public string? AccentColor2 { get; set; }
		public string? AccentColor3 { get; set; }
		public string FabricNotes { get; set; }
		public string FabricSourceName { get; set; }
		public string FabricSourceUrl { get; set; }
		public DateTime DateAdded { get; set; }
		public DateTime? DateModified { get; set; }
		public bool IsDiscontinued { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsPopular { get; set; }










	}
}
