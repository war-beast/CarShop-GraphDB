using System.Collections.Generic;
using System.Threading.Tasks;
using CarShop.BLL.Models;

namespace CarShop.BLL.Interfaces
{
	public interface IStorageService : IService
	{
		Task<IReadOnlyList<Block>> GetBlocks();

		Task<IReadOnlyList<SparePart>> GetParts(string blockName);
	}
}
