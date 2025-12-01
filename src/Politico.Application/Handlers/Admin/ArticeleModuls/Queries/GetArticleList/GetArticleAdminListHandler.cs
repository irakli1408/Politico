using MediatR;
using Microsoft.EntityFrameworkCore;
using Politico.Application.DTO.ArticeleModuls;
using Politico.Application.Interfaces.Persistence;

namespace Politico.Application.Handlers.Admin.ArticeleModuls.Queries.GetArticleList
{
    public sealed class GetArticleAdminListHandler
        : IRequestHandler<GetArticleAdminListQuery, List<ArticleAdminListItemDto>>
    {
        private readonly IAppDbContext _db;

        public GetArticleAdminListHandler(IAppDbContext db) { _db = db; }

        public async Task<List<ArticleAdminListItemDto>> Handle(GetArticleAdminListQuery q, CancellationToken ct)
        {
            var query = _db.Articles
                .Include(a => a.Locales)
                .AsQueryable();

            if (q.Category.HasValue)
                query = query.Where(a => a.Category == q.Category.Value);

            if (q.Status.HasValue)
                query = query.Where(a => a.Status == q.Status.Value);

            if (q.IsActive.HasValue)
                query = query.Where(a => a.IsActive == q.IsActive.Value);

            return await query
                .OrderByDescending(a => a.PublishDate)
                .Select(a => new ArticleAdminListItemDto
                {
                    Id = a.Id,
                    Category = a.Category,
                    Status = a.Status,
                    IsActive = a.IsActive,
                    PublishDate = a.PublishDate,
                    Title = a.Locales
                        .OrderBy(l => l.Culture) 
                        .Select(l => l.Title)
                        .FirstOrDefault()
                })
                .ToListAsync(ct);
        }
    }
}
