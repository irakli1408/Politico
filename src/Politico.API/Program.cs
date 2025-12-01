using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Politico.API.Services.Auth;
using Politico.API.Tools.Extensions;
using Politico.Application.Common.Helper.Model;
using Politico.Application.Handlers.Admin.Publish.Commands;
using Politico.Application.Interfaces.Auth;
using Politico.Application.Interfaces.Cashing;
using Politico.Application.Interfaces.Logger;
using Politico.Application.Interfaces.Notificaion;
using Politico.Application.Interfaces.Persistence;
using Politico.Common.ErrorHandler.Middleware;
using Politico.Common.Model;
using Politico.Domain.Entities.Auth;
using Politico.FileManager.Common.Interfaces;
using Politico.FileManager.Media.Upload;
using Politico.FileManager.Service;
using Politico.Infrastructure.Logging;
using Politico.NotificationManager.Tools.Extensions;
using Politico.Persistence;
using Politico.Persistence.Cashing;
using Politico.Persistence.Identity;
using Politico.Persistence.Repositories.Auth;
using Politico.Persistence.Services.Auth;
using Politico.Persistence.Services.Logging;
using SixLabors.ImageSharp;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PoliticoDbConnection")));

builder.Services.AddScoped<IAppDbContext>(provider =>
    provider.GetRequiredService<AppDbContext>());

// ------------ API Versioning + Explorer ------------

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddControllers();

builder.Services.Configure<OutputCacheSettings>(
    builder.Configuration.GetSection("OutputCacheSettings"));

var cacheSettings = builder.Configuration
    .GetSection("OutputCacheSettings")
    .Get<OutputCacheSettings>()
    ?? throw new InvalidOperationException("OutputCacheSettings missing in appsettings.json");


builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<ApplicationAssemblyMarker>();
});

builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("CacheShort", b =>
        b.Expire(TimeSpan.FromSeconds(cacheSettings.ShortSeconds))
         .SetVaryByQuery("lang"));

    options.AddPolicy("CacheMedium", b =>
        b.Expire(TimeSpan.FromMinutes(cacheSettings.MediumMinutes))
         .SetVaryByQuery("lang"));

    options.AddPolicy("CacheLong", b =>
        b.Expire(TimeSpan.FromMinutes(cacheSettings.LongMinutes))
         .SetVaryByQuery("lang"));
});

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

// 2) Auth services
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasherAdapter>();

builder.Services.AddScoped<IErrorLogService, ErrorLogService>();
builder.Services.AddSingleton<ILoggerProvider, DbLoggerProvider>();

var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtOptions = jwtSection.Get<JwtOptions>()!;

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole(RoleNames.Admin, RoleNames.SuperAdmin));

    options.AddPolicy("SuperAdminOnly", policy =>
        policy.RequireRole(RoleNames.SuperAdmin));
});

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IAppCache, MemoryAppCache>();

// Swagger

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(SetActiveCommonClassHandler).Assembly));


builder.Services.AddEndpointsApiExplorer();
//builder.Services.ConfigureOptions<SwaggerConfiguration>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddCommon();

builder.Services.AddFileManager(builder.Configuration);

builder.Services.AddNotificationManager(builder.Configuration, builder.Environment);


builder.Services.AddScoped<IUploadMediaService, UploadMediaService>();
builder.Services.AddScoped<ITrashMediaService, TrashMediaService>();
builder.Services.AddScoped<IRestoreMediaService, RestoreMediaService>();
builder.Services.AddScoped<IDeleteMediaService, DeleteMediaService>();
builder.Services.AddScoped<IGetMediaListService, GetMediaListService>();
builder.Services.AddScoped<IGetTrashMediaListService, GetTrashMediaListService>();
builder.Services.AddScoped<IAttachMediaService, AttachMediaService>();
builder.Services.AddScoped<IDetachMediaService, DetachMediaService>();
builder.Services.AddScoped<IReorderMediaService, ReorderMediaService>();
builder.Services.AddScoped<ISetCoverMediaService, SetCoverMediaService>();
builder.Services.AddScoped<IGetOwnerMediaService, GetOwnerMediaService>();
builder.Services.AddScoped<IGetMediaLibraryService, GetMediaLibraryService>();

builder.Services.AddLocalization(o => o.ResourcesPath = "Resources");


// CORS (dev-only) — разрешаем любые loopback-хосты (localhost, 127.0.0.1) на любых портах
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("dev-cors", p =>
    {
        p.SetIsOriginAllowed(origin =>
        {
            // Разрешить все локальные origin-ы (http/https, любой порт)
            if (Uri.TryCreate(origin, UriKind.Absolute, out var uri))
                return uri.IsLoopback; // localhost / 127.0.0.1

            return false;
        })
        .AllowAnyHeader()
        .AllowAnyMethod()
        // если фронт шлёт куки/Authorization заголовок из браузера — оставь AllowCredentials
        .AllowCredentials()
        // полезно, чтобы фронт видел эти заголовки
        .WithExposedHeaders("X-RateLimit-Policy", "Retry-After", "Content-Disposition");
    });

    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssemblies(
            typeof(Program).Assembly,
            typeof(SetActiveHandler<,>).Assembly
        );
    });

    // (опционально) фиксированный список origin-ов, если не нужны "все локальные порты"
    opt.AddPolicy("dev-cors-fixed", p =>
    {
        p.WithOrigins(
            "http://localhost:3000",   // React
            "http://localhost:5173",   // Vite
            "http://localhost:4200",   // Angular
            "http://localhost:19006",  // Expo
            "http://127.0.0.1:5173",
            "https://localhost:3000",
            "https://localhost:5173"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithExposedHeaders("X-RateLimit-Policy", "Retry-After", "Content-Disposition");
    });
});

var rateSection = builder.Configuration.GetSection("RateLimiting");
var globalCfg = rateSection.GetSection("Global").Get<RateLimitPolicyOptions>()
               ?? new RateLimitPolicyOptions { PermitLimit = 80, WindowSeconds = 60 };

var policiesSection = rateSection.GetSection("Policies");
var policies = policiesSection.GetChildren()
    .Select(s => new
    {
        Name = s.Key,
        Options = s.Get<RateLimitPolicyOptions>()
    })
    .Where(x => x.Options is not null)
    .ToList();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (ctx, token) =>
    {
        TimeSpan? retryAfter = null;
        if (ctx.Lease.TryGetMetadata(MetadataName.RetryAfter, out var ra))
            retryAfter = ra;

        if (retryAfter is { } t)
            ctx.HttpContext.Response.Headers["Retry-After"] =
                ((int)Math.Ceiling(t.TotalSeconds)).ToString();

        ctx.HttpContext.Response.Headers["X-RateLimit-Policy"] =
            $"global-fixed-window; {globalCfg.PermitLimit} req / {globalCfg.WindowSeconds}s";
        ctx.HttpContext.Response.ContentType =
            "application/problem+json; charset=utf-8";

        var problem = new
        {
            type = "https://httpstatuses.com/429",
            title = "Too Many Requests",
            status = 429,
            detail = retryAfter is null
                ? "You have sent too many requests. Please try again later."
                : $"Rate limit exceeded. Retry after ~{Math.Ceiling(retryAfter.Value.TotalSeconds)} seconds.",
            traceId = ctx.HttpContext.TraceIdentifier
        };

        await ctx.HttpContext.Response.WriteAsJsonAsync(problem, cancellationToken: token);
    };

    // Глобальный лимитер (кроме /admin)
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
    {
        var path = ctx.Request.Path.Value ?? string.Empty;
        if (path.Contains("/admin/", StringComparison.OrdinalIgnoreCase))
            return RateLimitPartition.GetNoLimiter("admin");

        var key = ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: key,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = globalCfg.PermitLimit,
                Window = TimeSpan.FromSeconds(globalCfg.WindowSeconds),
                QueueLimit = 0,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            });
    });

    // 🔹 Регистрируем ВСЕ политики из appsettings
    foreach (var p in policies)
    {
        options.AddPolicy(p.Name, httpContext =>
        {
            var key = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            return RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: key,
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    AutoReplenishment = true,
                    PermitLimit = p.Options!.PermitLimit,
                    Window = TimeSpan.FromSeconds(p.Options.WindowSeconds),
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                });
        });
    }
});


// API Versioning
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Politico API",
        Version = "v1"
    });

    // описание схемы авторизации
    var jwtScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Введите JWT токен в формате: Bearer {token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    };

    // регистрируем схему
    options.AddSecurityDefinition("Bearer", jwtScheme);

    // говорим Swagger'у, что все операции могут использовать эту схему
    var requirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    };

    options.AddSecurityRequirement(requirement);
});



builder.Services.ConfigureHttpJsonOptions(options =>
{
    // Всегда сериализовать DateTime в UTC с суффиксом Z
    options.SerializerOptions.Converters.Add(new JsonConverterDateTimeUtc());
});

var app = builder.Build();

// ------------ Pipeline ------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    // Swagger UI c поддержкой версий
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                $"Politico API {description.GroupName.ToUpperInvariant()}"
            );
        }
    });
}

app.UseRateLimiter();


app.UseHttpsRedirection();

// глобальный middleware обработки ошибок
app.UseMiddleware<ExceptionHandlingMiddleware>();


app.UseOutputCache();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/test-email", async (IEmailSender sender) =>
{
    await sender.SendAsync("giorgisupatashvili0@gmail.com", "ashotrustavelyan1@gmail.com", "<b>Hello from Politico</b>");
    return "ok";
});


app.Run();
