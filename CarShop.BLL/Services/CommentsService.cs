using CarShop.BLL.Interfaces;
using CarShop.BLL.Models;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
			var response = _elasticClient
				.Search<Comment>(c =>
				c.Query(q =>
					q.MatchAll()
				)
			);

			var comments = response.Documents;

			return comments;
		}

		public async Task<bool> SaveComment(PostComment newComment)
		{
			Comment comment = new()
			{
				Title = newComment.Title,
				Content = newComment.Content,
				Tags = newComment.Tags
					.Split(",")
					.Select(x => x.Trim())
					.ToList()
			};
			
			var response = await _elasticClient.IndexAsync(comment, index => index.Index("carshop"));
			return response.Result == Result.Created;
		}
	}
}
