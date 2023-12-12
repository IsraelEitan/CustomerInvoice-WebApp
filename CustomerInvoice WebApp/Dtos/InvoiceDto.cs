using System.ComponentModel.DataAnnotations;

namespace CustomerInvoice_WebApp.Dtos
{
    public sealed record InvoiceDto(
        int Id,
        DateTime Date,
        string Status,
        decimal Amount,
        int CustomerId);
}
