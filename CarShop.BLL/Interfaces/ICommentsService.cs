using CarShop.BLL.Models;
using System.Collections.Generic;

namespace CarShop.BLL.Interfaces
{
	public interface ICommentsService
	{
		IReadOnlyCollection<Comment> GetAll();
	}
}
