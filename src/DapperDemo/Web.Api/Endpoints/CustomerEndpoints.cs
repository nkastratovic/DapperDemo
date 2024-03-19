using Dapper;
using Web.Api.Models;
using Web.Api.Services;

namespace Web.Api.Endpoints
{
    public static class CustomerEndpoints
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("customers", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                const string sql = "SELECT * FROM Customer";
                var customers = await connection.QueryAsync<Customer>(sql);
                return Results.Ok(customers);
            });
        }
    }
}
