using Dapper;
using Microsoft.Data.SqlClient;
using Web.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

app.MapGet("customers", async (IConfiguration configuration) =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    using var connection = new SqlConnection(connectionString);
    const string sql = "SELECT * FROM Customer";
    var customers = await connection.QueryAsync<Customer>(sql);
    return Results.Ok(customers);
});

app.Run();

