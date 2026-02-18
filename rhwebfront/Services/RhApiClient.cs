using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using rhdata;
using System.Text;
using rhapi.Endpoints;
using System.Net;

namespace RHWebFront.Services
{
    public class RhApiClient : IRhApiClient
    {
        public const string BASE_URL = @"https://localhost:7271/";

        private readonly ILogger<RhApiClient> _logger;
        private readonly HttpClient _httpClient;

        public RhApiClient(ILogger<RhApiClient> log, HttpClient client)
        {
            _logger = log;
            _httpClient = client;

            _logger.LogInformation("RhApiClient initialized");
        }

        #region infrastructure methods
        private async Task<T> MakeApiRequest<T>(string method, string path, string body = null) where T : class
        {
            try
            {
                var request = new HttpRequestMessage(new HttpMethod(method), path);
                if (method == HttpMethods.Post || method == HttpMethods.Patch) { request.Content = new StringContent(body, Encoding.UTF8, "application/json"); }

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error making api request");
                return null;
            }
        }

        private static string BuildQueryString(IDictionary<string, string[]> queryParams)
        {
            if (queryParams is null || queryParams.Count == 0) return string.Empty;

            var pairs = queryParams
                .Where(kv => kv.Key is not null && kv.Value is not null && kv.Value.Length > 0)
                .SelectMany(kv => kv.Value.Select(v => $"{WebUtility.UrlEncode(kv.Key)}={WebUtility.UrlEncode(v)}"));

            return pairs.Any() ? $"?{string.Join('&', pairs)}" : string.Empty;
        }
        #endregion

        #region Account - Account
        private const string GET_ACCOUNT_PATH = $"{RhAccountEndpoints.ACCT_GROUP}{RhAccountEndpoints.GET_ACCT}";
        public async Task<RHAccount> GetAcct() { return await MakeApiRequest<RHAccount>(HttpMethods.Get, GET_ACCOUNT_PATH); }
        #endregion

        #region Account - Holdings
        private const string GET_HOLDINGS_PATH = $"{RhAccountEndpoints.ACCT_GROUP}{RhAccountEndpoints.GET_HOLDINGS}";
        public async Task<RHHolding[]> GetHoldings(string[] symbols)
        {
            var query = BuildQueryString(new Dictionary<string, string[]> { ["symbols"] = symbols });
            var path = $"{GET_HOLDINGS_PATH}{query}";

            var resultToken = await MakeApiRequest<JToken>(HttpMethods.Get, path);
            return ExtractHoldingsResult(resultToken);
        }

        private static RHHolding[] ExtractHoldingsResult(JToken resultToken)
        {
            if (resultToken.IsNullOrEmpty()) return [];
            if (resultToken.Type == JTokenType.Array) return resultToken.ToObject<RHHolding[]>() ?? [];

            var resultsToken = resultToken["results"] ?? resultToken;
            if (resultsToken.IsNullOrEmpty()) return [];
            
            return resultsToken.ToObject<RHHolding[]>() ?? [];
        }
        #endregion

        #region Market - Trading Pairs
        private const string GET_TRADING_PAIRS_PATH = $"{RhMarketEndpoints.MARKET_GROUP}{RhMarketEndpoints.GET_TRADING_PAIRS}";
        public async Task<RHTradingPair[]> GetTradingPairs()
        {
            var resultToken = await MakeApiRequest<JToken>(HttpMethods.Get, GET_TRADING_PAIRS_PATH);

            if (resultToken.IsNullOrEmpty()) return [];
            if (resultToken.Type == JTokenType.Array) return resultToken.ToObject<RHTradingPair[]>() ?? [];

            var resultsToken = resultToken["results"] ?? resultToken;
            if (resultsToken.IsNullOrEmpty()) return [];
            
            return resultsToken.ToObject<RHTradingPair[]>() ?? [];
        }
        #endregion

        #region Market - Estimated Price
        private const string GET_EST_PRICE_PATH = $"{RhMarketEndpoints.MARKET_GROUP}{RhMarketEndpoints.GET_EST_PRICE}";
        public async Task<RHEstimatedPrice[]> GetEstimatedPrice(IDictionary<string, string[]> queryParams)
        {
            var query = BuildQueryString(queryParams);
            var path = $"{GET_EST_PRICE_PATH}{query}";

            var resultToken = await MakeApiRequest<JToken>(HttpMethods.Get, path);
            if (resultToken.IsNullOrEmpty()) return [];

            var resultsToken = resultToken["results"] ?? resultToken;
            if (resultsToken.IsNullOrEmpty()) return [];
            
            return resultsToken.ToObject<RHEstimatedPrice[]>() ?? [];
        }
        #endregion

        #region Market - Best Bid/Ask
        private const string GET_BID_ASK_PATH = $"{RhMarketEndpoints.MARKET_GROUP}{RhMarketEndpoints.GET_BID_ASK}";
        public async Task<RHBidAsk[]> GetBestBidAsk(IDictionary<string, string[]> queryParams)
        {
            var query = BuildQueryString(queryParams);
            var path = $"{GET_BID_ASK_PATH}{query}";

            var resultToken = await MakeApiRequest<JToken>(HttpMethods.Get, path);
            if (resultToken.IsNullOrEmpty()) return [];

            var resultsToken = resultToken["results"] ?? resultToken;
            if (resultsToken.IsNullOrEmpty()) return [];
            
            return resultsToken.ToObject<RHBidAsk[]>() ?? [];
        }
        #endregion

        #region Order - Orders
        private const string GET_ORDERS_PATH = $"{RhOrderEndpoints.ORDER_GROUP}{RhOrderEndpoints.GET_ORDERS}";
        
        public async Task<RHOrder[]> GetOrders(IDictionary<string, string[]> queryParams)
        {
            var query = BuildQueryString(queryParams);
            var path = $"{GET_ORDERS_PATH}{query}";

            var resultToken = await MakeApiRequest<JToken>(HttpMethods.Get, path);
            if (resultToken.IsNullOrEmpty()) return [];

            var resultsToken = resultToken["results"] ?? resultToken;
            if (resultsToken.IsNullOrEmpty()) return [];
            
            return resultsToken.ToObject<RHOrder[]>() ?? [];
        }

        public async Task<RHOrder> GetOrder(Guid orderId)
        {
            var orders = await GetOrders(new Dictionary<string, string[]> { ["id"] = [orderId.ToString()] });
            return orders.Length > 0 ? orders[0] : null;
        }

        public async Task<RHOrder[]> GetOpenOrders()
        { return await GetOrders(new Dictionary<string, string[]> { ["state"] = ["open", "partially_filled"] }); }
        #endregion
    }

    internal static class ApiClientExtensions
    {
        internal static bool IsNullOrEmpty(this JToken token)
        {
            return token is null 
                || token.Type == JTokenType.Null 
                || token.Type == JTokenType.Undefined 
                || !token.HasValues;
        }
    }
}
