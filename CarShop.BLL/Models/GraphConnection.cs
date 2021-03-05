using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.BLL.Models
{
	public class GraphConnection
	{
		public string Host { get; set; }

		public int Port { get; set; }

		public string User { get; set; }

		public string Password { get; set; }
	}
}
