using System;
using CustomerInvoice_WebApp.Data;
using CustomerInvoice_WebApp.Repositories.Interfaces;

namespace CustomerInvoice_WebApp.Repositories
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    internal class InvoicesUnitOfWork : IUnitOfWork
    {
        private readonly CustomerInvoiceDbContext _context;
        private readonly ILogger<InvoicesUnitOfWork> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private IInvoiceRepository _invoices;

        public InvoicesUnitOfWork(CustomerInvoiceDbContext context, ILogger<InvoicesUnitOfWork> logger, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = logger;
            _loggerFactory = loggerFactory;
        }

        public IInvoiceRepository Invoices => _invoices ??= new InvoiceRepository(_context, _loggerFactory.CreateLogger<InvoiceRepository>());

        public async Task CompleteAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Unit of work completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while completing the unit of work.");
                throw;
            }
        }
    }
}
