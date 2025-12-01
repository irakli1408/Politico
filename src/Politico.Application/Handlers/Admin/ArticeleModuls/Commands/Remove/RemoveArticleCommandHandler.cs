using MediatR;
using Microsoft.EntityFrameworkCore;
using Politico.Application.Interfaces.Persistence;
using Politico.Common.ErrorHandler.Exceptions;

namespace Politico.Application.Handlers.Admin.ArticeleModuls.Commands.Remove
{
    public sealed class RemoveArticleCommandHandler
     : IRequestHandler<RemoveArticleCommand, Unit>
    {
        private readonly IAppDbContext _db;

        public RemoveArticleCommandHandler(IAppDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(RemoveArticleCommand cmd, CancellationToken ct)
        {
            var article = await _db.Articles
                .IgnoreQueryFilters()               // важно!
                .Include(a => a.Locales)
                .FirstOrDefaultAsync(a => a.Id == cmd.Id, ct);

            if (article is null)
                throw new NotFoundException("Article", cmd.Id);

            // Удаляем локали
            _db.ArticleLocales.RemoveRange(article.Locales);

            // Удаляем саму статью
            _db.Articles.Remove(article);

            await _db.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}