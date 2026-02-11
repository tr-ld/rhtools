using System;

namespace rhapi.Poco.DevVal
{
    [Serializable]
    public record SignatureTestResult
    {
        public required string Message { get; set; }
        public required string Signature { get; set; }
        public required bool IsValid { get; set; }
    }
}
