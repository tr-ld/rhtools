using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using rhapi.Poco.Order;
using rhapi.Poco.Responses;
using rhapi.Services;
using System;
using System.Threading.Tasks;

namespace rhapi.Endpoints
{
    public class RhOrderEndpoints(IRhCryptoMarket market) : RhEndpoints(market)
    {
        public const string ORDER_GROUP = @"/order";
        public const string GET_ORDER = @"/getOrder/{orderId:guid}";
        public const string GET_ORDERS = @"/getOrders";
        public const string PLACE_ORDER = @"/placeOrder";

        public Task<RhOrder> GetOrderAsync(Guid orderId) { return _market.GetOrder(orderId); }
        public Task<RhOrdersResponse> GetOrdersAsync(RhOrderParams queryParams) { return _market.GetOrders(queryParams); }
        public Task<RhOrder> PlaceOrderAsync(RhPlaceOrderParams orderParams, IOptions<JsonOptions> options) { return _market.PlaceOrder(orderParams, options); }
    }

    internal static class RhOrderExtensions
    {
        internal static RouteGroupBuilder MapOrderEndpoints(this WebApplication app)
        {
            var group = app.MapGroup(RhOrderEndpoints.ORDER_GROUP);

            group.MapGet(RhOrderEndpoints.GET_ORDER, (RhOrderEndpoints ep, Guid orderId) => ep.GetOrderAsync(orderId));
            group.MapGet(RhOrderEndpoints.GET_ORDERS, (RhOrderEndpoints ep, [AsParameters] RhOrderParams queryParams) => ep.GetOrdersAsync(queryParams));
            group.MapPost(RhOrderEndpoints.PLACE_ORDER, (RhOrderEndpoints ep, RhPlaceOrderParams orderParams, IOptions<JsonOptions> options) => ep.PlaceOrderAsync(orderParams, options));
            //group.MapPost("/placeOrder", async (HttpContext ctx) =>
            //{
            //    var rawBody = await new StreamReader(ctx.Request.Body).ReadToEndAsync();
            //    return Results.Text($"Raw body: {rawBody}");
            //});

            return group;
        }
    }
}
