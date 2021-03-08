using CarShop.BLL.Interfaces;
using CarShop.BLL.Models;
using Microsoft.Extensions.Options;
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
		private readonly GraphConnection _graphConnection;

		#endregion

		#region constructor

		public StorageService(IGraphClient client, IOptionsSnapshot<GraphConnection> options)
		{
			_client = client;
			_graphConnection = options?.Value ?? throw new ArgumentNullException(nameof(options.Value));
		}

		#endregion

		public async Task<IReadOnlyList<Block>> GetBlocks()
		{
			var result = (await _client
				.Cypher
				.Match("(p:SparePart)-[rel:PART_OF]->(b:Block)")
				.Return((p, b) => new {
					Blocks = b.CollectAs<Block>(),
					Parts = p.CollectAs<SparePart>()
				})
				.ResultsAsync)
				.ToList();

			//TODO: Можно получить данные через IDriver из библиотеки Neo4j.Driver, но маппить придётся в ручную
			//var driver = GraphDatabase.Driver($"{_graphConnection.Host}:{_graphConnection.Port}", AuthTokens.Basic(_graphConnection.User, _graphConnection.Password));
			//var session = driver.AsyncSession(o => o.WithDatabase(_client.DefaultDatabase));
			//var cursor = await session.RunAsync("MATCH (p:SparePart)-[rel:PART_OF]->(b:Block) RETURN p, b, rel");

			var blocks = result.First().Blocks;
			var spareParts = result.First().Parts;

			var resultList = spareParts.Select((x, idx) => 
				{
					x.Block = blocks.ElementAt(idx);
					return x;
				})
				.GroupBy(x => x.Block.Name)
				.Select(x => new Block()
				{
					Name = x.Key,
					Parts = x.ToList()
				})
				.ToList();

			return resultList;
		}

		public async Task<IReadOnlyList<SparePart>> GetParts()
		{
			throw new NotSupportedException();
		}
	}
}
