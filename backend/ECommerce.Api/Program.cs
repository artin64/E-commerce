using ECommerce.Api.Data;
using ECommerce.Api.Services.Auth;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ECommerce.Api.Services.Auth.JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddSingleton<ECommerce.Api.Services.Auth.JwtTokenService>();

builder.Services.AddDbContext<ECommerce.Api.Data.AppDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("Postgres");
    options.UseNpgsql(cs);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
