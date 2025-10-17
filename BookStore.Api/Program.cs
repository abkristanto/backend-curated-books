using Npgsql;
using BookStore.Api.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connString = builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("Connection string 'Default' not found.");

builder.Services.AddSingleton(sp =>
{
    var dsb = new NpgsqlDataSourceBuilder(connString);
    return dsb.Build();
});

builder.Services.AddScoped<IBooksRepository, PostgresBooksRepository>();

var app = builder.Build();

app.MapControllers();

app.Run();
