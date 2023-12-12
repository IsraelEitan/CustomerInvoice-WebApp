using Newtonsoft.Json;

namespace CustomerInvoice_WebApp.Dtos
{
    public sealed record ErrorDetails
    {
        public int StatusCode { get; init; }
        public string? Message { get; init; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
