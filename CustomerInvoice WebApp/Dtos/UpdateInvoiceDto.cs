using CustomerInvoice_WebApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace CustomerInvoice_WebApp.Dtos
{
    public sealed record UpdateInvoiceDto(
        DateTime Date,
        [Required] string Status,
        decimal Amount);
}
