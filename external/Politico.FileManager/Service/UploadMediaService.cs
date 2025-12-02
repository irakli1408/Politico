using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Politico.Application.Common.Helper.Model;
using Politico.Application.Interfaces.Persistence;
using Politico.Domain.Common.Enums.Media;
using Politico.Domain.Entities.Media;
using Politico.FileManager.Common.DTO;
using Politico.FileManager.Common.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using Img = SixLabors.ImageSharp.Image;

namespace Politico.FileManager.Media.Upload;

public class UploadMediaService : IUploadMediaService
{
    private readonly IAppDbContext _db;
    private readonly IFileStorage _storage;
    private readonly MediaOptions _opt;

    public UploadMediaService(IAppDbContext db, IFileStorage storage, IOptions<MediaOptions> opt)
    {
        _db = db;
        _storage = storage;
        _opt = opt.Value;
    }

    public async Task<UploadMediaResult> UploadAsync(MediaType type, IFormFile file, CancellationToken ct = default)
    {
        if (file == null || file.Length <= 0)
            throw new ArgumentException("empty_file");

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

        // 👇 Выбор списков разрешённых расширений и лимитов по типу
        string[] allow;
        int maxMb;

        switch (type)
        {
            case MediaType.Photo:
                allow = _opt.AllowedImageExtensions;
                maxMb = _opt.MaxImageSizeMb;
                break;

            case MediaType.Video:
                allow = _opt.AllowedVideoExtensions;
                maxMb = _opt.MaxVideoSizeMb;
                break;

            case MediaType.Audio:                 // NEW
                allow = _opt.AllowedAudioExtensions;
                maxMb = _opt.MaxAudioSizeMb;
                break;

            default:
                throw new ArgumentException("unsupported_media_type");
        }

        if (!allow.Contains(ext, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException("unsupported_extension");

        if (file.Length > maxMb * 1024L * 1024L)
            throw new ArgumentException("file_too_large");

        var now = DateTime.UtcNow;
        var guid = $"{Guid.NewGuid():N}";

        // ===== ВИДЕО =====
        if (type == MediaType.Video)
        {
            var subfolder = $"{now:yyyy}/{now:MM}";
            var newName = $"{guid}{ext}";

            await using var input = file.OpenReadStream();
            var relPath = await _storage.SaveAsync(input, subfolder, newName, ct);

            var asset = new MediaAsset
            {
                Type = MediaType.Video,
                OriginalFileName = file.FileName,
                StoredPath = relPath,
                ContentType = file.ContentType,
                SizeBytes = file.Length,
                Extension = ext,
                CreatedAtUtc = now
            };

            _db.MediaAssets.Add(asset);
            await _db.SaveChangesAsync(ct);

            var url = BuildUrl(relPath);
            return new UploadMediaResult(asset.Id, asset.OriginalFileName, (int)asset.Type, url, null);
        }

        // ===== АУДИО =====  (никаких превью, просто сохранить файл как есть)
        if (type == MediaType.Audio)
        {
            // отдельная подпапка для аудио, чтобы не мешалось с картинками/видео
            var subfolder = $"audio/{now:yyyy}/{now:MM}";   // NEW
            var newName = $"{guid}{ext}";

            await using var input = file.OpenReadStream();
            var relPath = await _storage.SaveAsync(input, subfolder, newName, ct);

            var asset = new MediaAsset
            {
                Type = MediaType.Audio,
                OriginalFileName = file.FileName,
                StoredPath = relPath,
                ContentType = file.ContentType,
                SizeBytes = file.Length,
                Extension = ext,
                CreatedAtUtc = now,
                // для аудио превью нет:
                ThumbStoredPath = null,
                PosterPath = null,
                Width = null,
                Height = null,
                ThumbWidth = null,
                ThumbHeight = null
            };

            _db.MediaAssets.Add(asset);
            await _db.SaveChangesAsync(ct);

            var url = BuildUrl(relPath);
            return new UploadMediaResult(asset.Id, asset.OriginalFileName, (int)asset.Type, url, null);
        }

        // ===== ФОТО =====
        // сюда попадаем только если type == Photo
        var subfolderPhoto = $"{now:yyyy}/{now:MM}";

        await using var imgStream = file.OpenReadStream();
        using var img = await Img.LoadAsync(imgStream, ct);

        var origW = img.Width;
        var origH = img.Height;

        // основной JPEG
        using var mainMs = new MemoryStream();
        var mainEncoder = new JpegEncoder { Quality = _opt.JpegQualityMain };
        await img.SaveAsJpegAsync(mainMs, mainEncoder, ct);
        mainMs.Position = 0;

        var finalExt = ".jpg";
        var mainName = $"{guid}{finalExt}";
        var mainRelPath = await _storage.SaveAsync(mainMs, subfolderPhoto, mainName, ct);

        // превью
        var maxW = _opt.ThumbMaxWidth;
        var scale = (double)maxW / origW;
        var thW = Math.Min(maxW, origW);
        var thH = (int)Math.Max(1, Math.Round(origH * scale));

        using var thumbImg = img.Clone(ctx =>
        {
            ctx.Resize(new ResizeOptions
            {
                Size = new Size(thW, thH),
                Mode = ResizeMode.Max,
                Sampler = KnownResamplers.Lanczos3
            });
        });

        using var thumbMs = new MemoryStream();
        var thumbEncoder = new JpegEncoder { Quality = _opt.JpegQualityThumb };
        await thumbImg.SaveAsJpegAsync(thumbMs, thumbEncoder, ct);
        thumbMs.Position = 0;

        var thumbName = $"000_{guid}_thumb{finalExt}";
        var thumbRelPath = await _storage.SaveAsync(thumbMs, subfolderPhoto, thumbName, ct);

        var assetPhoto = new MediaAsset
        {
            Type = MediaType.Photo,
            OriginalFileName = file.FileName,
            StoredPath = mainRelPath,
            ThumbStoredPath = thumbRelPath,
            ContentType = "image/jpeg",
            SizeBytes = mainMs.Length,
            Width = origW,
            Height = origH,
            ThumbWidth = thW,
            ThumbHeight = thH,
            Extension = finalExt,
            CreatedAtUtc = now
        };

        _db.MediaAssets.Add(assetPhoto);
        await _db.SaveChangesAsync(ct);

        var mainUrl = BuildUrl(assetPhoto.StoredPath);
        var thumbUrl = assetPhoto.ThumbStoredPath is null ? null : BuildUrl(assetPhoto.ThumbStoredPath);

        return new UploadMediaResult(assetPhoto.Id, assetPhoto.OriginalFileName, (int)assetPhoto.Type, mainUrl, thumbUrl);
    }

    private string BuildUrl(string relPath)
        => $"{_opt.RequestPath.TrimEnd('/')}/{relPath}"
            .Replace("//", "/")
            .Replace("\\", "/");
}
