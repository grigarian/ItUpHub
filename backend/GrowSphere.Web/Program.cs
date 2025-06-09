using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.S3;
using GrowSphere.Application.Categories;
using GrowSphere.Application.Interfaces;
using GrowSphere.Application.Interfaces.Auth;
using GrowSphere.Application.Issues;
using GrowSphere.Application.Projects;
using GrowSphere.Application.ProjectVacancies;
using GrowSphere.Application.Skills;
using GrowSphere.Application.Users;
using GrowSphere.Domain.Interfaces;
using GrowSphere.Extentions;
using GrowSphere.Hubs;
using GrowSphere.Infrastructure;
using GrowSphere.Infrastructure.Middlewares;
using GrowSphere.Infrastructure.Repositories;
using GrowSphere.Infrastructure.Services;
using GrowSphere.Web.RealTime;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://+:8090");

// Add services to the container.
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

var jwtOptions = builder.Configuration.
    GetSection(nameof(JwtOptions)).
    Get<JwtOptions>();

builder.Services.AddSingleton<IAmazonS3>(sp => 
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new AmazonS3Client(
        config["MinIO:AccessKey"],
        config["MinIO:SecretKey"],
        new AmazonS3Config
        {
            ServiceURL = config["MinIO:Endpoint"],
            ForcePathStyle = true,
            UseHttp = !bool.Parse(config["MinIO:WithSSL"] ?? "false")
        }
    );
});

builder.Services.AddApiAuthentication(Options.Create(jwtOptions));

// Program.cs или Startup.cs
builder.Services.AddControllers()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddScoped<ApplicationDbContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR()
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });



builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<CurrentUserService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<IFileStorage, S3FileStorage>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<SkillService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationHub, NotificationHubSender>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<IJoinRequestRepository, JoinRequestRepository>();
builder.Services.AddScoped<JoinRequestService>();
builder.Services.AddScoped<IIssueRepository, IssueRepository>();
builder.Services.AddScoped<IssueService>();
builder.Services.AddScoped<IProjectVacancyRepository, ProjectVacancyRepository>();
builder.Services.AddScoped<IVacancyApplicationRepository, VacancyApplicationRepository>();
builder.Services.AddScoped<ProjectVacancyService>();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:3000",
                 "http://localhost:8080",
                  "http://localhost:8081",
                  "http://localhost:8090",
                  "https://ituphub.ru",
                  "http://ituphub.ru",
                  "https://www.ituphub.ru",
                  "https://www.ituphub.ru/api",
                  "https://ituphub.ru/api",
                  "http://localhost",
                  "http://localhost:80"
                  )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

// Применяем миграции
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,  // Разрешаем кросс-сайтовые куки
    HttpOnly = HttpOnlyPolicy.None,  // Разрешаем доступ из JavaScript
    Secure = CookieSecurePolicy.None,  // Разрешаем HTTP
    CheckConsentNeeded = context => false // Отключаем проверку согласия
});




app.UseSerilogRequestLogging();

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();


app.MapControllers();
app.MapHub<ProjectMessageHub>("/hubs/projectMessage");
app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();
