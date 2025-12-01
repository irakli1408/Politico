using MediatR;
using Politico.Application.Interfaces.Persistence;
using Politico.Domain.Entities.Articels;

namespace Politico.Application.Handlers.Admin.ArticeleModuls.Commands.Create
{
    public sealed class CreateArticleCommandHandler
       : IRequestHandler<CreateArticleCommand, long>
    {
        private readonly IAppDbContext _db;

        public CreateArticleCommandHandler(IAppDbContext db)
        {
            _db = db;
        }

        public async Task<long> Handle(CreateArticleCommand cmd, CancellationToken ct)
        {
            var now = DateTime.UtcNow;

            var article = new Article
            {
                Category = cmd.Category,
                Status = cmd.Status,
                IsActive = cmd.IsActive,
                PublishDate = cmd.PublishDate,
                IsFeatured = cmd.IsFeatured,
                PriorityScore = cmd.PriorityScore,
                CreateDate = now,
            };

            // локали
            foreach (var loc in cmd.Locales)
            {
                article.Locales.Add(new ArticleLocale
                {
                    Culture = loc.Culture,
                    Title = loc.Title,
                    ShortSummary = loc.ShortSummary,
                    Content = loc.Content,
                    Slug = loc.Slug,
                });
            }

            _db.Articles.Add(article);
            await _db.SaveChangesAsync(ct);

            return article.Id;
        }
    }
}
