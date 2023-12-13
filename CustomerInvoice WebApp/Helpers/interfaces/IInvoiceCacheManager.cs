using CustomerInvoice_WebApp.Models;

namespace CustomerInvoice_WebApp.Helpers.interfaces
{
    public interface IInvoiceCacheManager
    {
        Task<IEnumerable<Invoice>> GetOrSetAllInvoicesAsync(int pageNumber, int pageSize, Func<Task<IEnumerable<Invoice>>> fetchInvoices);
        Task<IEnumerable<Invoice>> GetOrSetAllInvoicesAsync(Func<Task<IEnumerable<Invoice>>> fetchInvoices);
        Task<IEnumerable<Invoice>> GetOrSetOverdueInvoicesAsync(Func<Task<IEnumerable<Invoice>>> fetchOverdueInvoices);
        Task<IEnumerable<Invoice>> GetOrSetInvoicesByCustomerIdAsync(int customerId, Func<Task<IEnumerable<Invoice>>> fetchInvoices);
        Task<Invoice> GetOrSetInvoiceByIdAsync(int id, Func<Task<Invoice>> fetchInvoice);
        void InvalidateInvoiceCache(int id);
        void InvalidateAllInvoicesCache();
        void InvalidateAllPagedInvoicesCache();
    }
}
