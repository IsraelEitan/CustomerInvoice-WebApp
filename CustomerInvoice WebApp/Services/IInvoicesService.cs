using CustomerInvoice_WebApp.Models;

namespace CustomerInvoice_WebApp.Services
{
    public interface IInvoicesService
    {
        Task<IEnumerable<Invoice>> GetAllInvoicesAsync();
        Task<Invoice> GetInvoiceByIdAsync(int id);
        Task<IEnumerable<Invoice>> GetInvoicesByCustomerIdAsync(int CustomerId);
        Task<IEnumerable<Invoice>> GetOverdueInvoicesAsync();
        Task<Invoice> CreateInvoiceAsync(Invoice invoice);
        Task UpdateInvoiceAsync(int id, Invoice invoice);
        Task DeleteInvoiceAsync(int id);
    }

}
