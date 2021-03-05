using CarShop.BLL.Interfaces;
using CarShop.BLL.Models;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.BLL.Services
{
	public class StorageService : IStorageService
	{
		#region private memebers

		private readonly IGraphClient _client;

		#endregion

		#region constructor

		public StorageService(IGraphClient client)
		{
			_client = client;
		}

		#endregion

		public async Task<IReadOnlyList<Block>> GetBlocks()
		{
			var result = await _client
				.Cypher
				.Match("(b:Block)")
				.Return(b => b.CollectAs<Block>())
				.ResultsAsync;

			return result
				.First()
				.Select(x => new Block
				{
					Name = x.Name,
					Parts = new List<SparePart>()
				})
				.ToList();
		}

		public async Task<IReadOnlyList<SparePart>> GetParts()
		{
			throw new NotSupportedException();
		}
	}
}
