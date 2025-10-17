using Npgsql;
using BookStore.Api.Data;
using BookStore.Api.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'Default' not found.");

builder.Services.AddSingleton(sp =>
{
    var dsb = new NpgsqlDataSourceBuilder(connString);
    return dsb.Build();
});

builder.Services.AddScoped<IBooksRepository, PostgresBooksRepository>();
builder.Services.AddScoped<IBooksService, BooksService>();
builder.Services.AddControllers();


var app = builder.Build();

app.MapControllers();

app.Run();
