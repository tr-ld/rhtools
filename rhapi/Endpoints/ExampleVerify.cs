using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json.Linq;
using NSec.Cryptography;
using rhapi.Poco.DevVal;

namespace rhapi.Endpoints
{
    public class ExampleVerify
    {
        private const string PRKEY_64 = "xQnTJVeQLmw1/Mg2YimEViSpw/SdJcgNXZ5kQkAXNPU=";
        private const string PUKEY_64 = "jPItx4TLjcnSUnmnXQQyAKL4eJj3+oWNNMmmm2vATqk=";

        private const string API_KEY = "rh-api-6148effc-c0b1-486c-8940-a1d099456be6";
        private const string CURRENT_TIMESTAMP = "1698708981";
        private const string PATH = @"/api/v1/crypto/trading/orders/";
        private const string HTTP_METHOD = "POST";

        private static readonly JObject BODY = new()
        {
            { "client_order_id", "131de903-5a9c-4260-abc1-28d562a5dcf0" },
            { "side", "buy" },
            { "symbol", "BTC-USD" },
            { "type", "market" },
            { "market_order_config", new JObject { { "asset_quantity", "0.1" } } }
        };

        public Task<SignatureTestResult> DoSignatureVerify()
        {
            var pkSeed = Convert.FromBase64String(PRKEY_64);
            var puBytes = Convert.FromBase64String(PUKEY_64);

            //define the message
            var bodyJson = BODY.AsPythonJson();
            var message = $"{API_KEY}{CURRENT_TIMESTAMP}{PATH}{HTTP_METHOD}{bodyJson}";
            var messageBytes = Encoding.UTF8.GetBytes(message);
            Console.WriteLine($"message: {message}");
            Console.WriteLine();

            //sign the message
            var algo = SignatureAlgorithm.Ed25519;
            var privateKey = Key.Import(algo, pkSeed, KeyBlobFormat.RawPrivateKey);
            var signature = algo.Sign(privateKey, messageBytes);
            var sig64 = Convert.ToBase64String(signature);

            //verify the signature
            var publicKey = PublicKey.Import(algo, puBytes, KeyBlobFormat.RawPublicKey);
            var isValid = algo.Verify(publicKey, messageBytes, signature);

            //print results
            Console.WriteLine($"Signature: {sig64}");
            Console.WriteLine($"Signature valid: {isValid}");
            Console.WriteLine();

            return Task.FromResult(new SignatureTestResult { Message = message, Signature = sig64, IsValid = isValid });
        }
    }

    internal static class ExampleExtensions
    {
        internal static RouteGroupBuilder MapExampleEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/rhex"); 

            group.MapGet("/verifySignature", (ExampleVerify test) => test.DoSignatureVerify());

            return group;
        }
    }
}
