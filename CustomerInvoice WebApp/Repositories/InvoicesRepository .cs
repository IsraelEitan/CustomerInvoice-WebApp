using System.Linq.Expressions;
using CustomerInvoice_WebApp.Data;
using CustomerInvoice_WebApp.Exceptions;
using CustomerInvoice_WebApp.Models;
using CustomerInvoice_WebApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerInvoice_WebApp.Repositories
{
    internal sealed class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(CustomerInvoiceDbContext context, ILogger<InvoiceRepository> logger)
            : base(context, logger)
        {
        }

        public async Task<IEnumerable<Invoice>> GetOverdueInvoicesAsync()
        {
            return await GetInvoicesWithConditionAsync(
                invoice => invoice.Status == Enums.InvoiceStatus.Overdue,
                "Overdue status"
            );
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByCustomerIdAsync(int customerId)
        {
            return await GetInvoicesWithConditionAsync(
                invoice => invoice.CustomerId == customerId,
                $"Customer ID {customerId}"
            );
        }

        private async Task<IEnumerable<Invoice>> GetInvoicesWithConditionAsync(Expression<Func<Invoice, bool>> condition, string description)
        {
            var invoices = await _context.Invoices.Where(condition).ToListAsync();
            if (!invoices.Any())
            {
                throw new EntityNotFoundException($"No invoices found with {description}.");
            }
            return invoices;
        }
    }
}