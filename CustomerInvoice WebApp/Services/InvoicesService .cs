using CustomerInvoice_WebApp.Exceptions;
using CustomerInvoice_WebApp.Helpers;
using CustomerInvoice_WebApp.Helpers.interfaces;
using CustomerInvoice_WebApp.Models;
using CustomerInvoice_WebApp.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace CustomerInvoice_WebApp.Services
{
    public class InvoiceService : IInvoicesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInvoiceCacheManager _cacheManager; 
        private readonly ILogger<InvoiceService> _logger;
        private readonly PaginationSettings _paginationSettings;

        public InvoiceService(
            IUnitOfWork unitOfWork,
            IInvoiceCacheManager cacheManager,  
            ILogger<InvoiceService> logger,
            IOptions<PaginationSettings> paginationSettings)
        {
            _unitOfWork = unitOfWork;
            _cacheManager = cacheManager; 
            _logger = logger;
            _paginationSettings = paginationSettings.Value;
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync(int pageNumber, int pageSize)
        {
            pageSize = Math.Clamp(pageSize, _paginationSettings.MinPageSize, _paginationSettings.MaxPageSize);

            return await ExecuteWithLoggingAndReturnValueAsync(
              () => _cacheManager.GetOrSetAllInvoicesAsync(pageNumber, pageSize,
                  () => _unitOfWork.Invoices.GetAllAsync(pageNumber, pageSize)),
              $"retrieving all invoices for page {pageNumber} with page size {pageSize}");
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync()
        {
            return await ExecuteWithLoggingAndReturnValueAsync(
                () => _cacheManager.GetOrSetAllInvoicesAsync(() => _unitOfWork.Invoices.GetAllAsync()),
                "retrieving all invoices");
        }

        public async Task<Invoice> GetInvoiceByIdAsync(int id)
        {
            return await ExecuteWithLoggingAndReturnValueAsync(
                () => _cacheManager.GetOrSetInvoiceByIdAsync(id, () => _unitOfWork.Invoices.GetByIdAsync(id)),
                $"retrieving invoice with ID {id}");
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByCustomerIdAsync(int CustomerId)
        {
            return await ExecuteWithLoggingAndReturnValueAsync(
                 () => _cacheManager.GetOrSetInvoicesByCustomerIdAsync(CustomerId,() => _unitOfWork.Invoices.GetInvoicesByCustomerIdAsync(CustomerId)),
                 $"retrieving invoices with customer ID {CustomerId}");
        }

        public async Task<IEnumerable<Invoice>> GetOverdueInvoicesAsync()
        {
            return await ExecuteWithLoggingAndReturnValueAsync(
                 () => _cacheManager.GetOrSetOverdueInvoicesAsync(() => _unitOfWork.Invoices.GetOverdueInvoicesAsync()),
                 $"retrieving all overdue status invoices");
        }

        public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
        {
            if (invoice == null) throw new ArgumentNullException(nameof(invoice));

            await ExecuteWithLoggingAsync(async () =>
            {
                await _unitOfWork.Invoices.AddAsync(invoice);
                await _unitOfWork.CompleteAsync();
                _cacheManager.InvalidateAllPagedInvoicesCache();
            }, "creating a new invoice");

            return invoice;
        }

        public async Task UpdateInvoiceAsync(int id, Invoice invoice)
        {
            ValidateInvoiceUpdate(id, invoice);

            await ExecuteWithLoggingAsync(async () =>
            {
                await UpdateExistingInvoiceAsync(id, invoice);
                _cacheManager.InvalidateInvoiceCache(id);
                _cacheManager.InvalidateAllPagedInvoicesCache();
            }, $"updating invoice with ID {id}");
        }

        public async Task DeleteInvoiceAsync(int id)
        {
            await ExecuteWithLoggingAsync(async () =>
            {
                var invoice = await _unitOfWork.Invoices.GetByIdAsync(id);
                if (invoice == null) throw new EntityNotFoundException("Invoice not found");

                await _unitOfWork.Invoices.DeleteAsync(invoice);
                await _unitOfWork.CompleteAsync();
                _cacheManager.InvalidateInvoiceCache(id);
                _cacheManager.InvalidateAllPagedInvoicesCache();
            }, $"deleting invoice with ID {id}");
        }

        private async Task ExecuteWithLoggingAsync(Func<Task> operation, string operationDescription)
        {
            try
            {
                await operation();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error {operationDescription}.");
                throw;
            }
        }

        private async Task<T> ExecuteWithLoggingAndReturnValueAsync<T>(Func<Task<T>> operation, string operationDescription)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error {operationDescription}.");
                throw;
            }
        }

        private void ValidateInvoiceUpdate(int id, Invoice invoice)
        {
            if (invoice == null) throw new ArgumentNullException(nameof(invoice));
            if (id != invoice.Id) throw new ArgumentException("ID mismatch");
        }

        private async Task UpdateExistingInvoiceAsync(int id, Invoice invoice)
        {
            var existingInvoice = await _unitOfWork.Invoices.GetByIdAsync(id);
            if (existingInvoice == null) throw new EntityNotFoundException("Invoice not found");

            existingInvoice.Date = invoice.Date;
            existingInvoice.Status = invoice.Status;
            existingInvoice.Amount = invoice.Amount;
            existingInvoice.CustomerId = invoice.CustomerId;

            await _unitOfWork.Invoices.UpdateAsync(existingInvoice);
            await _unitOfWork.CompleteAsync();
        }
    }
}
