using CarShop.BLL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarShop.BLL.Interfaces
{
	public interface ICommentsService : IService
	{
		IReadOnlyCollection<Comment> GetAll();

		Task<bool> SaveComment(PostComment newComment);
	}
}
