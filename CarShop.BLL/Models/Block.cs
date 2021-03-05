using System.Collections.Generic;

namespace CarShop.BLL.Models
{
	public class Block : EntityBase
	{
		public List<SparePart> Parts { get; set; }
	}
}
