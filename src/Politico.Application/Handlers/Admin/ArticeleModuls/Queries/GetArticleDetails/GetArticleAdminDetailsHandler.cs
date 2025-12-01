using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Politico.Application.Common.Helper.Model;
using Politico.Application.DTO.ArticeleModuls;
using Politico.Application.Interfaces.Persistence;
using Politico.Common.ErrorHandler.Exceptions;
using Politico.Domain.Common.Enums.Media;

namespace Politico.Application.Handlers.Admin.ArticeleModuls.Queries.GetArticleDetails
{
    public sealed class GetArticleAdminDetailsHandler
         : IRequestHandler<GetArticleAdminDetailsQuery, ArticleAdminDetailsDto>
    {
        private readonly IAppDbContext _db;
        private readonly MediaOptions _mediaOpt;

        public GetArticleAdminDetailsHandler(
            IAppDbContext db,
            IOptions<MediaOptions> mediaOpt)
        {
            _db = db;
            _mediaOpt = mediaOpt.Value;
        }

        public async Task<ArticleAdminDetailsDto> Handle(
            GetArticleAdminDetailsQuery q,
            CancellationToken ct)
        {
            var article = await _db.Articles
                .Include(a => a.Locales)
                .FirstOrDefaultAsync(a => a.Id == q.Id, ct);

            if (article == null)
                throw new NotFoundException("Article", q.Id);

            var attachments = await _db.MediaAttachments
                .Include(m => m.MediaAsset)
                .Where(m =>
                    m.OwnerType == MediaOwnerType.Article &&   
                    m.OwnerKey == article.Id.ToString())
                .OrderBy(m => m.Order)                         
                .ToListAsync(ct);

            var mediaDtos = attachments.Select(m => new CommonMediaItemDto
            {
                AssetId = m.MediaAssetId,
                Url = m.MediaAsset?.StoredPath is null
                              ? null
                              : MediaUrlHelper.ToUrl(_mediaOpt, m.MediaAsset.StoredPath),
                ThumbUrl = m.MediaAsset?.ThumbStoredPath is null
                              ? null
                              : MediaUrlHelper.ToUrl(_mediaOpt, m.MediaAsset.ThumbStoredPath),
                MediaType = m.MediaAsset?.Type ?? MediaType.Unknown,
                SortOrder = m.Order,                        
                IsCover = m.IsCover,
                CreatedAtUtc = m.MediaAsset?.CreatedAtUtc
                               ?? DateTime.MinValue            
            }).ToList();

            var cover = mediaDtos.FirstOrDefault(x => x.IsCover);

            return new ArticleAdminDetailsDto
            {
                Id = article.Id,
                Category = article.Category,
                Status = article.Status,
                IsActive = article.IsActive,
                PublishDate = article.PublishDate,
                IsFeatured = article.IsFeatured,
                PriorityScore = article.PriorityScore,

                Locales = article.Locales
                    .OrderBy(l => l.Culture)
                    .Select(l => new ArticleLocaleDto
                    {
                        Id = l.Id,
                        Culture = l.Culture,
                        Title = l.Title,
                        ShortSummary = l.ShortSummary,
                        Content = l.Content,
                        Slug = l.Slug
                    })
                    .ToList(),

                Cover = cover,
                Media = mediaDtos
            };
        }
    }
}
