using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyFabricTrackerWebApp.Models
{
	public class Source
	{
		public int SourceId { get; set; }
		[Display(Name = "Source Name")]
		public string SourceName { get; set; }
		public string? WebsiteUrl { get; set; }
		[Display(Name = "Street Address")]
		public string? StreetAddress { get; set; }
		public string? City { get; set; }
		public string? State { get; set; }

		[Display(Name = "Phone Number")]
		[Phone]
		public string? PhoneNumber { get; set; }

		[Display(Name = "Date Added")]
		public DateTime DateAdded { get; set; }

		[Display(Name = "Date Modified")]
		public DateTime? DateModified { get; set; }
	}
}
