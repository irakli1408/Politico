using MediatR;
using Microsoft.EntityFrameworkCore;
using Politico.Application.Handlers.Admin.ArticeleModuls.Commands.Delete;
using Politico.Application.Interfaces.Persistence;
using Politico.Common.ErrorHandler.Exceptions;

namespace Politico.Application.Handlers.Admin.ArticleModuls.Commands.Delete
{
    public sealed class DeleteArticleCommandHandler
        : IRequestHandler<DeleteArticleCommand, Unit>
    {
        private readonly IAppDbContext _db;

        public DeleteArticleCommandHandler(IAppDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(DeleteArticleCommand cmd, CancellationToken ct)
        {
            var article = await _db.Articles
                .Include(a => a.Locales)          // 👈 подтягиваем локали
                .FirstOrDefaultAsync(a => a.Id == cmd.Id, ct);

            if (article is null)
                throw new NotFoundException("Article", cmd.Id);

            var now = DateTime.UtcNow;

            // помечаем саму статью
            article.DeleteDate = now;

            // помечаем все локали
            foreach (var locale in article.Locales)
            {
                locale.DeleteDate = now;
            }

            await _db.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
