using CustomerInvoice_WebApp.Helpers.interfaces;
using CustomerInvoice_WebApp.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace CustomerInvoice_WebApp.Helpers
{
    public sealed class InvoicesCacheManager : IInvoiceCacheManager
    {
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30);
        private readonly PaginationSettings _paginationSettings;

        public InvoicesCacheManager(IMemoryCache cache, IOptions<PaginationSettings> paginationSettings)
        {
            _paginationSettings = paginationSettings.Value;
            _cache = cache;
        }

        public async Task<IEnumerable<Invoice>> GetOrSetAllInvoicesAsync(int pageNumber, int pageSize, Func<Task<IEnumerable<Invoice>>> fetchInvoices)
        {
            string cacheKey = GetPagedCacheKey(pageNumber, pageSize);
            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheDuration;
                return await fetchInvoices();
            });
        }

        public async Task<IEnumerable<Invoice>> GetOrSetAllInvoicesAsync(Func<Task<IEnumerable<Invoice>>> fetchInvoices)
        {
            return await GetOrSetCacheAsync("all_invoices", fetchInvoices);
        }

        public async Task<IEnumerable<Invoice>> GetOrSetOverdueInvoicesAsync(Func<Task<IEnumerable<Invoice>>> fetchOverdueInvoices)
        {
            return await GetOrSetCacheAsync("overdue_invoices", fetchOverdueInvoices);
        }

        public async Task<IEnumerable<Invoice>> GetOrSetInvoicesByCustomerIdAsync(int customerId, Func<Task<IEnumerable<Invoice>>> fetchInvoices)
        {
            return await GetOrSetCacheAsync($"invoices_customer_{customerId}", fetchInvoices);
        }

        public async Task<Invoice> GetOrSetInvoiceByIdAsync(int id, Func<Task<Invoice>> fetchInvoice)
        {
            return await GetOrSetCacheAsync($"invoice_{id}", fetchInvoice);
        }

        public void InvalidateInvoiceCache(int id)
        {
            _cache.Remove($"invoice_{id}");
        }

        public void InvalidateAllInvoicesCache()
        {
            _cache.Remove("all_invoices");
            _cache.Remove("overdue_invoices");
        }

        public void InvalidateAllPagedInvoicesCache()
        {
            
            for (int pageSize = _paginationSettings.MinPageSize; pageSize <= _paginationSettings.MaxPageSize; pageSize++)
            {
                for (int pageNumber = 1; pageNumber <= _paginationSettings.MaxPageNumber; pageNumber++)
                {
                    string cacheKey = GetPagedCacheKey(pageNumber, pageSize);
                    _cache.Remove(cacheKey);
                }
            }

            InvalidateAllInvoicesCache();
        }

        private async Task<T?> GetOrSetCacheAsync<T>(string cacheKey, Func<Task<T>> fetchData)
        {
            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheDuration;
                return await fetchData();
            });
        }
        private string GetPagedCacheKey(int pageNumber, int pageSize)
        {
            return $"all_invoices_paged_{pageNumber}_{pageSize}";
        }
    }
}
