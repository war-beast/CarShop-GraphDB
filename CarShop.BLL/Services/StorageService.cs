using CarShop.BLL.Interfaces;
using CarShop.BLL.Models;
using Microsoft.Extensions.Options;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
			var blocksAndParts = (await _client
				.Cypher
				.Match("(p:SparePart)-[rel:PART_OF]->(b:Block)")
				.Return((p, b) => new {
					Blocks = b.CollectAs<Block>(),
					Parts = p.CollectAs<SparePart>()
				})
				.ResultsAsync)
				.First();

			//TODO: Можно получить данные через IDriver из библиотеки Neo4j.Driver, но маппить придётся вручную
			//var driver = GraphDatabase.Driver($"{_graphConnection.Host}:{_graphConnection.Port}", AuthTokens.Basic(_graphConnection.User, _graphConnection.Password));
			//var session = driver.AsyncSession(o => o.WithDatabase(_client.DefaultDatabase));
			//var cursor = await session.RunAsync("MATCH (p:SparePart)-[rel:PART_OF]->(b:Block) RETURN p.Name as partName, b.Name as blockName, p.Price as price");
			//var result = await cursor.ToListAsync(x => new SparePart
			//{
			//	Name = x["partName"].As<string>(),
			//	Price = x["price"].As<decimal>(),
			//  Block = new Block{
			//		Name = x["blockName"].As<string>()
			//  }
			//});

			//await session.CloseAsync();
			//await graphDriver.CloseAsync();

			var blocks = blocksAndParts.Blocks;
			var spareParts = blocksAndParts.Parts;

			var result = spareParts.Select((x, idx) => 
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

			return result;
		}

		public async Task<IReadOnlyList<SparePart>> GetParts(string blockName)
		{
			var searchCondition = string.IsNullOrWhiteSpace(blockName)
				? "-[:PART_OF]->(b:Block)"
				: $"-[:PART_OF]->(b:Block {{Name: '{blockName}'}})";

			var blocksAndParts = (await _client
					.Cypher
					.Match($"(p:SparePart){searchCondition}")
					.Return((p, b) => new {
						Blocks = b.CollectAs<Block>(),
						Parts = p.CollectAs<SparePart>()
					})
					.ResultsAsync)
				.First();

			var blocks = blocksAndParts.Blocks;
			var spareParts = blocksAndParts.Parts;

			var result = spareParts.Select((p, idx) =>
				{
					p.Block = new Block
					{
						Name = blocks.ElementAt(idx).Name
					};
					return p;
				})
				.ToList();

			return result;
		}
	}
}
