using CustomerInvoice_WebApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace CustomerInvoice_WebApp.Dtos
{
    public sealed record CreateInvoiceDto(
        [Required] DateTime Date,
        [Required] string Status,
        [Required, Range(0.01, double.MaxValue)] decimal Amount,
        [Required, Range(1, int.MaxValue)] int CustomerId);
}
