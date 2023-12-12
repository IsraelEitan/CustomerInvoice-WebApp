namespace CustomerInvoice_WebApp.Dtos
{
    public sealed record CustomerDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        
    }
}
