using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFabricTrackerWebApp.Models
{
	public class Transaction
	{
		public long TransactionId { get; set; }

		public long FabricId { get; set; }
		public virtual Fabric Fabric { get; set; }

		public int? InchesQty { get; set; }
		public int? FatQtrQty { get; set; }
		public string Reason { get; set; }
		public DateTime TransactionDate { get; set; }
		public bool IsDeleted { get; set; }

	}
}
