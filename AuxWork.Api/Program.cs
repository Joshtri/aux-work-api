using AuxWork.Api.Data;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using AuxWork.Api.Repositories;
using AuxWork.Api.Repositories.Abstractions;
using AuxWork.Api.Repositories.Projects;
using AuxWork.Api.Repositories.WorkItems;
using AuxWork.Api.Services.Projects;
using AuxWork.Api.Services;
using AuxWork.Api.Infrastructure.Serialization;
using AuxWork.Api.Infrastructure.Routing;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

var builder = WebApplication.CreateBuilder(args);

// 1) Load .env (jika ada)
Env.Load();

// 2) Bangun connection string dari ENV (beri default biar aman)
string host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
string port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
string name = Environment.GetEnvironmentVariable("DB_NAME") ?? "Db_Sisurat_kelLiliba";
string user = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
string pass = Environment.GetEnvironmentVariable("DB_PASS") ?? "postgres";

var conn = $"Host={host};Port={port};Database={name};Username={user};Password={pass};Search Path=public";

// 3) Registrasi services (SEBELUM Build)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(conn));


builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IWorkItemRepository, WorkItemRepository>();

builder.Services.AddScoped<IProjectsService, ProjectsService>();
builder.Services.AddControllers(options =>
{
    // ubah token [controller]/[action] -> snake_case
    options.Conventions.Add(
        new RouteTokenTransformerConvention(new SnakeCaseParameterTransformer()));
});

// (opsional) sekalian paksa lower-case URL & querystring
builder.Services.AddRouting(o =>
{
    o.LowercaseUrls = true;
    o.LowercaseQueryStrings = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4) Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 5) Cek koneksi DB sekali saat startup (pakai logger)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var log = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    if (db.Database.CanConnect())
        log.LogInformation("✅ Database connected");
    else
        log.LogError("❌ Database connection failed");
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
