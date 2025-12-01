using MediatR;
using Microsoft.EntityFrameworkCore;
using Politico.Application.Interfaces.Persistence;
using Politico.Common.ErrorHandler.Exceptions;
using Politico.Domain.Entities.Articels;

namespace Politico.Application.Handlers.Admin.ArticeleModuls.Commands.Update
{
    public sealed class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand, Unit>
    {
        private readonly IAppDbContext _db;

        public UpdateArticleCommandHandler(IAppDbContext db) { _db = db; }
       
        public async Task<Unit> Handle(UpdateArticleCommand cmd, CancellationToken ct)
        {
            var article = await _db.Articles
                .Include(a => a.Locales)
                .FirstOrDefaultAsync(a => a.Id == cmd.Id, ct);

            if (article == null)
                throw new NotFoundException("Article", cmd.Id);

            article.Category = cmd.Category;
            article.Status = cmd.Status;
            article.IsActive = cmd.IsActive;
            article.PublishDate = cmd.PublishDate;
            article.IsFeatured = cmd.IsFeatured;
            article.PriorityScore = cmd.PriorityScore;
            article.UpdateDate = DateTime.UtcNow;

            // Обновляем локали (простой вариант: удаляем старые и создаём заново)
            _db.ArticleLocales.RemoveRange(article.Locales);

            article.Locales.Clear();

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

            await _db.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
