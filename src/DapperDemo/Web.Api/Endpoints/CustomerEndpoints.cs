using Dapper;
using Web.Api.Models;
using Web.Api.Services;

namespace Web.Api.Endpoints
{
    public static class CustomerEndpoints
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("customers");
            group.MapGet("", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                const string sql = "SELECT * FROM Customer";
                var customers = await connection.QueryAsync<Customer>(sql);
                return Results.Ok(customers);
            });

            group.MapGet("id", async (int id, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                const string sql = "SELECT * FROM Customer WHERE Id = @CustomerId";
                var customer = await connection.QuerySingleOrDefaultAsync<Customer>(sql, new { CustomerId = id });
                return customer is not null ? Results.Ok(customer) : Results.NotFound();
            });

            group.MapPost("", async (Customer customer, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                const string sql = "INSERT INTO Customer (Id, FirstName, LastName, Email, DateOfBirth) VALUES (@Id, @FirstName, @LastName, @Email, @DateOfBirth)";
                await connection.ExecuteAsync(sql, customer);
                return Results.Ok();
            });

            group.MapPut("id", async (int id, Customer customer, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                const string sql = """
                UPDATE Customer 
                SET Id = @Id, FirstName = @FirstName, LastName = @LastName, Email = @Email, DateOfBirth = @DateOfBirth WHERE Id = @Id
                """;
                await connection.ExecuteAsync(sql, customer);
                return Results.NoContent();
            });


            group.MapDelete("id", async (int id, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                const string sql = "DELETE from Customer WHERE Id = @CustomerId";
                await connection.ExecuteAsync(sql, new { CustomerId = id });
                return Results.NoContent();
            });
        }
    }
}
