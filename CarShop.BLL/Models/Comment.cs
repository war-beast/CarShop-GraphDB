using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;

namespace CarShop.BLL.Models
{
	public class Comment
	{
		public string Title { get; set; }

		public string Content { get; set; }

		public List<string> Tags { get; set; }
	}
}
