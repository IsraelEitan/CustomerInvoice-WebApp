using CustomerInvoice_WebApp.Models;

namespace CustomerInvoice_WebApp.Repositories.Interfaces
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        Task<IEnumerable<Invoice>> GetOverdueInvoicesAsync();
        Task<IEnumerable<Invoice>> GetInvoicesByCustomerIdAsync(int customerId);
    }
}
