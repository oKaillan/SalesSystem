using Microsoft.AspNetCore.Mvc;
using SalesSystem.Database;
using SalesSystem.Entities;

namespace SalesSystem.API.Extensions
{
    public static class SalesLogExtensions
    {
        public static void AddEndPointsSalesLog(this WebApplication app)
        {
            app.MapGet("/SalesLog/", ([FromServices] DAL<SalesLog> slogDAL) =>
            {
                var slogCheck = slogDAL.GetAll();
                if (slogCheck is null)
                {
                    return Results.NoContent();
                }
                return Results.Ok(slogCheck);
            });

            app.MapGet("/SalesLog/{iD}", ([FromServices] DAL<SalesLog> slogDAL, Guid iD) =>
            {
                var slogCheck = slogDAL.GetBy(s => s.SaleId.Equals(iD));
                if (slogCheck is null)
                {
                    return Results.NoContent();
                }
                return Results.Ok(slogCheck);
            });
        }
    }
}
