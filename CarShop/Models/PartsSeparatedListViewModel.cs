using System.Collections.Generic;
using CarShop.BLL.Models;

namespace CarShop.Models
{
	public record PartsSeparatedListViewModel
	{
		public string BlockName { get; init; }

		public IEnumerable<SparePart> SpareParts { get; init; }
	}
}
