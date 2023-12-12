namespace CustomerInvoice_WebApp.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IInvoiceRepository Invoices { get; }
        Task CompleteAsync();
    }
}
