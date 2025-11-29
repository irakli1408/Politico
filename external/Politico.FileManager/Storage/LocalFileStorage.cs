using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Politico.FileManager.Common.Interfaces;
using Politico.FileManager.Common.Options;

namespace Politico.FileManager.Storage;

public sealed class LocalFileStorage : IFileStorage
{
    private readonly string _root;

    public LocalFileStorage(IOptions<MediaOptions> opt, IHostEnvironment env)
    {
        // Например: wwwroot/uploads/media
        var wwwroot = Path.Combine(env.ContentRootPath, "wwwroot");
        _root = Path.Combine(wwwroot, opt.Value.RootPath);

        Directory.CreateDirectory(_root);
    }

    public async Task<string> SaveAsync(Stream file, string subfolder, string fileName, CancellationToken ct = default)
    {
        var folder = Path.Combine(_root, subfolder);
        Directory.CreateDirectory(folder);

        var path = Path.Combine(folder, fileName);

        // Если файл существует — переименовать
        if (File.Exists(path))
        {
            var name = Path.GetFileNameWithoutExtension(fileName);
            var ext = Path.GetExtension(fileName);

            fileName = $"{name}_{Guid.NewGuid():N}{ext}";
            path = Path.Combine(folder, fileName);
        }

        using var outStream = File.Create(path);
        await file.CopyToAsync(outStream, ct);

        // вернуть относительный путь для сохранения в БД
        return Path.Combine(subfolder, fileName).Replace("\\", "/");
    }

    public Task DeleteAsync(string relativePath, CancellationToken ct = default)
    {
        var full = Path.Combine(_root, relativePath);
        if (File.Exists(full))
            File.Delete(full);

        return Task.CompletedTask;
    }
}
