using rhapi.Services;

namespace rhapi.Endpoints
{
    public abstract class RhEndpoints(IRhCryptoMarket market)
    {
        protected readonly IRhCryptoMarket _market = market;
    }

    //todo: maybe implement this (return all routes)
    //internal static class RouteExtensions
    //{
    //    internal static RouteGroupBuilder MapAccountEndpoints(this WebApplication app)
    //    {
    //        var group = app.MapGroup(RhEndpoints.META_GROUP);

    //        group.MapGet(RhEndpoints.ROUTES, (EndpointDataSource ds) =>
    //        {
    //            var routes = ds.Endpoints
    //                           .OfType<RouteEndpoint>()
    //                           .Select(e => new
    //                           {
    //                               Route = e.RoutePattern.RawText,
    //                               Methods = e.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault()?.HttpMethods,
    //                               e.DisplayName
    //                           });

    //            return Results.Ok(routes);
    //        });

    //        return group;
    //    }
    //}
}
