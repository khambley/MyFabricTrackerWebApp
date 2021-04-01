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

		[Display(Name = "Item Code")]
		public string FabricItemCode { get; set; }
		[Display(Name = "Name")]
		public string FabricName { get; set; }
		
		[NotMapped]
		public IFormFile ImageFile { get; set; }
		public string ImageFileName { get; set; }

		public long MainCategoryId { get; set; }
		public virtual MainCategory MainCategory { get; set; }
		public long SubCategoryId { get; set; }
		public virtual SubCategory SubCategory { get; set; }
		[Display(Name = "Total Inches")]
		public long? TotalInches { get; set; }

		[Display(Name = "Total Fat Qtrs")]
		public long? FatQtrQty { get; set; }

		[Display(Name = "Type")]
		public string FabricType { get; set; }

		[Display(Name = "Width")]
		public string FabricWidth { get; set; }

		[Display(Name = "Background Color")]
		public string BackgroundColor { get; set; }

		[Display(Name = "Accent Color 1")]
		public string? AccentColor1 { get; set; }

		[Display(Name = "Accent Color 2")]
		public string? AccentColor2 { get; set; }

		[Display(Name = "Accent Color 3")]
		public string? AccentColor3 { get; set; }

		[Display(Name = "Notes")]
		public string FabricNotes { get; set; }

		//[Display(Name = "Source Name")]
		//public string FabricSourceName { get; set; }

		//[Display(Name = "Source Url")]
		//public string FabricSourceUrl { get; set; }
		public int? SourceId { get; set; }
		public Source Source { get; set; }

		[Display(Name = "Date Added")]
		public DateTime DateAdded { get; set; }

		[Display(Name = "Date Modified")]
		public DateTime? DateModified { get; set; }

		[Display(Name = "Discontinued?")]
		public bool IsDiscontinued { get; set; }

		[Display(Name = "Discontinued?")]
		public bool IsDeleted { get; set; }
		public bool IsPopular { get; set; }










	}
}
