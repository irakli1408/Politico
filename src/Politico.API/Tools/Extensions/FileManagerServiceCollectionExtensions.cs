using Politico.Application.Common.Helper.Model;
using Politico.FileManager.Common.Interfaces;
using Politico.FileManager.Media.Upload;
using Politico.FileManager.Storage;

namespace Politico.API.Tools.Extensions
{
    public static class FileManagerServiceCollectionExtensions
    {
        public static IServiceCollection AddFileManager(this IServiceCollection services, IConfiguration config)
        {
            // читаем настройки из appsettings.json
            services.Configure<MediaOptions>(config.GetSection(MediaOptions.SectionName));

            // регистрируем сторадж файлов
            services.AddScoped<IFileStorage, LocalFileStorage>();

            // регистрируем сервис Upload
            services.AddScoped<IUploadMediaService, UploadMediaService>();

            // позже сюда добавим TrashMediaService, RestoreMediaService и т.д.
            return services;
        }
    }
}
