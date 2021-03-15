using CarShop.BLL.Interfaces;
using CarShop.BLL.Models;
using Nest;
using System.Collections.Generic;

namespace CarShop.BLL.Services
{
	public class CommentsService : ICommentsService
	{
		#region private members

		private ElasticClient _elasticClient;

		#endregion

		#region constuctor

		public CommentsService(ElasticClient elasticClient)
		{
			_elasticClient = elasticClient;
		}

		#endregion

		public IReadOnlyCollection<Comment> GetAll()
		{
			var cm = _elasticClient
				.Search<Comment>(c =>
				c.Query(q =>
					q.MatchAll()
				)
			);

			var comments = _elasticClient
				.Search<Comment>(c =>
					c.Query(q =>
						q.MatchAll()
					)
				)
				.Documents;

			return comments;
		}
	}
}
