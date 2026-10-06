using GameStore.Application;
using GameStore.Infrastructure;
using GameStore.Presentation.Endpoints;

const string DevelopmentCorsPolicy = "DevelopmentCors";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// The Angular client is served from its own dev server, so it is a different origin.
builder.Services.AddCors(options => options.AddPolicy(
  DevelopmentCorsPolicy,
  policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// Configure the HTTP request pipeline.
await app.Services.EnsureDatabaseCreatedAsync();

if (app.Environment.IsDevelopment())
{
  app.UseCors(DevelopmentCorsPolicy);
  await app.Services.SeedDevelopmentDataAsync();
}

app.MapApiEndpoints();


await app.RunAsync();
