using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSec.Cryptography;
using rhapi.Poco;
using rhapi.Poco.Market;
using rhapi.Poco.Order;
using rhapi.Poco.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace rhapi.Services
{
    public class RhCryptoMarket : IRhCryptoMarket
    {
        internal const string API_KEY_HEADER = "x-api-key";
        private const string SIGNATURE_HEADER = "x-signature";
        private const string TIMESTAMP_HEADER = "x-timestamp";
        
        private readonly ILogger<RhCryptoMarket> _logger;
        private readonly string _apiKey;
        private readonly string _rawPrKey;
        private readonly string _rawPuKey;

        internal const string BASE_URL = "https://trading.robinhood.com/";
        private readonly HttpClient _httpClient;
        private readonly AppSettings _settings;

        private readonly Key _pk;
        private readonly PublicKey _puk;

        private readonly SignatureAlgorithm _algo = SignatureAlgorithm.Ed25519;

        public RhCryptoMarket(ILogger<RhCryptoMarket> log, HttpClient client, AppSettings settings)
        {
            _logger = log;
            _httpClient = client;
            _settings = settings;

            _apiKey = Environment.GetEnvironmentVariable("rhak");
            _rawPrKey = Environment.GetEnvironmentVariable("rhpk");
            _rawPuKey = Environment.GetEnvironmentVariable("rhpuk");

            if (string.IsNullOrEmpty(_apiKey) || string.IsNullOrEmpty(_rawPrKey) || string.IsNullOrEmpty(_rawPuKey))
            {
                _logger.LogError("Service keys not found.");
                throw new InvalidOperationException("Service keys not found.");
            }

            try
            {
                var pkSeed = Convert.FromBase64String(_rawPrKey);
                _pk = Key.Import(_algo, pkSeed, KeyBlobFormat.RawPrivateKey);

                var puBytes = Convert.FromBase64String(_rawPuKey);
                _puk = PublicKey.Import(_algo, puBytes, KeyBlobFormat.RawPublicKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing keys.");
                throw;
            }

            _logger.LogInformation("RhCryptoMarket initialized with provided service keys.");
        }

        #region infrastructure methods
        private static long UtcNowTimestamp() => DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        private static string GetQueryParams(string key, params string[] args)
        {
            if (args.Length == 0) return "";

            var pairs = args.Select(arg => $"{key}={arg}");
            return $"?{string.Join('&', pairs)}";
        }

        private static string GetQueryParams<T>(T paramsArgs) where T : QueryParams
        { return paramsArgs?.GetParamString() ?? ""; }

        private Dictionary<string, string> GetAuthHeaders(string method, string path, string body, long timestamp)
        {
            var message = $"{_apiKey}{timestamp}{path}{method}{body}"; //assumes body is already correctly formatted
            var messageBytes = Encoding.UTF8.GetBytes(message);
            
            var signature = _algo.Sign(_pk, messageBytes);
            var sig64 = Convert.ToBase64String(signature);
            //var sig64 = Convert.ToBase64String(signature, Base64FormattingOptions.None);

            if (_settings.IsDevelopment)
            {
                Console.WriteLine();
                Console.WriteLine($"apiKey: {_apiKey}");
                Console.WriteLine();
                Console.WriteLine($"message: {message}");
                Console.WriteLine();
                Console.WriteLine($"sig: {sig64}");
                Console.WriteLine();
            }

            return new Dictionary<string, string> { { SIGNATURE_HEADER, sig64 }, { TIMESTAMP_HEADER, $"{timestamp}" } };
        }

        private async Task<T> MakeApiRequest<T>(string method, string path, string body = null) where T : class
        {
            try
            {
                var timestamp = UtcNowTimestamp();
                var headers = GetAuthHeaders(method, path, body, timestamp);

                var request = new HttpRequestMessage(new HttpMethod(method), path);
                request.Headers.Add(TIMESTAMP_HEADER, headers[TIMESTAMP_HEADER]);
                request.Headers.Add(SIGNATURE_HEADER, headers[SIGNATURE_HEADER]);

                if (method == HttpMethods.Post || method == HttpMethods.Patch) { request.Content = new StringContent(body, Encoding.UTF8, "application/json"); } //body here must already be utf8 json
                
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
        #endregion

        #region account info
        private const string GET_ACCOUNT_PATH = "/api/v1/crypto/trading/accounts/";
        public Task<RhAccountResponse> GetAccount()
        {
            return MakeApiRequest<RhAccountResponse>(HttpMethods.Get, GET_ACCOUNT_PATH);
        }

        private const string GET_HOLDINGS_PATH = "/api/v1/crypto/trading/holdings/";
        private const string GET_HOLDINGS_QUERY_PARAM_NAME = "asset_code";
        public Task<RhHoldingsResponse> GetHoldings(string[] symbols)
        {
            var queryParams = GetQueryParams(GET_HOLDINGS_QUERY_PARAM_NAME, symbols);
            var fullPath = $"{GET_HOLDINGS_PATH}{queryParams}";
            
            return MakeApiRequest<RhHoldingsResponse>(HttpMethods.Get, fullPath);
        }
        #endregion

        #region orders
        private const string ORDER_PATH = "/api/v1/crypto/trading/orders/";
        public Task<RhOrder> GetOrder(Guid orderId)
        {
            var path = $"{ORDER_PATH}{orderId}/";
            return MakeApiRequest<RhOrder>(HttpMethods.Get, path);
        }

        public Task<RhOrdersResponse> GetOrders(RhOrderParams orderParams)
        {
            var queryParams = GetQueryParams(orderParams);
            var fullPath = $"{ORDER_PATH}{queryParams}";

            return MakeApiRequest<RhOrdersResponse>(HttpMethods.Get, fullPath);
        }

        public Task<RhOrder> PlaceOrder(RhPlaceOrderParams orderParams, IOptions<JsonOptions> options)
        {
            var body = JsonSerializer.Serialize(orderParams, options.Value.SerializerOptions);

            return MakeApiRequest<RhOrder>(HttpMethods.Post, ORDER_PATH, body);
        }
        #endregion

        #region market
        private const string GET_TRADING_PAIRS_PATH = "/api/v1/crypto/trading/trading_pairs/";
        public Task<RhTradingPairsResponse> GetTradingPairs(RhTradingPairsParams pairParams)
        {
            var queryParams = GetQueryParams(pairParams);
            var fullPath = $"{GET_TRADING_PAIRS_PATH}{queryParams}";

            return MakeApiRequest<RhTradingPairsResponse>(HttpMethods.Get, fullPath);
        }

        private const string GET_ESTIMATED_PRICE_PATH = "/api/v1/crypto/marketdata/estimated_price/";
        public Task<RhEstimatedPriceResponse> GetEstimatedPrice(RhEstimatedPriceParams estParams)
        {
            var queryParams = GetQueryParams(estParams);
            var fullPath = $"{GET_ESTIMATED_PRICE_PATH}{queryParams}";

            return MakeApiRequest<RhEstimatedPriceResponse>(HttpMethods.Get, fullPath);
        }

        private const string GET_BEST_BID_ASK_PATH = "/api/v1/crypto/marketdata/best_bid_ask/";
        public Task<RhBidAskResponse> GetBestBidAsk(RhBidAskParams bidAskParams)
        {
            var queryParams = GetQueryParams(bidAskParams);
            var fullPath = $"{GET_BEST_BID_ASK_PATH}{queryParams}";

            return MakeApiRequest<RhBidAskResponse>(HttpMethods.Get, fullPath);
        }
        #endregion
    }
}
