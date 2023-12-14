using AutoMapper;
using CustomerInvoice_WebApp.Dtos;
using CustomerInvoice_WebApp.Models;
using CustomerInvoice_WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerInvoice_WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class InvoicesController : ControllerBase
    {
        private readonly IInvoicesService _invoiceService;
        private readonly IMapper _mapper;

        public InvoicesController(IInvoicesService invoiceService, IMapper mapper)
        {
            _invoiceService = invoiceService;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a paginated list of all invoices.
        /// </summary>
        /// <param name="pageNumber">Page number for pagination, default is 1.</param>
        /// <param name="pageSize">Number of items per page, default is 10.</param>
        /// <returns>List of InvoiceDto</returns>
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetAllInvoices(int pageNumber = 1, int pageSize = 10)
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync(pageNumber, pageSize);
            var invoiceDtos = _mapper.Map<IEnumerable<InvoiceDto>>(invoices);

            return Ok(invoiceDtos);
        }

        /// <summary>
        /// Retrieves all overdue invoices.
        /// </summary>
        /// <returns>List of overdue InvoiceDto</returns>
        [HttpGet("Overdue")]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetAllOverdueInvoices()
        {
            var overdueInvoices = await _invoiceService.GetOverdueInvoicesAsync();
            var overdueInvoicesDtos = _mapper.Map<IEnumerable<InvoiceDto>>(overdueInvoices);

            return Ok(overdueInvoicesDtos);
        }

        /// <summary>
        /// Retrieves all invoices for a specific customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <returns>List of InvoiceDto for the specified customer.</returns>
        [HttpGet("ByCustomer/{customerId}")]
        public async Task<IActionResult> GetInvoicesByCustomer(int customerId)
        {
            var invoices = await _invoiceService.GetInvoicesByCustomerIdAsync(customerId);
            var invoiceDtos = _mapper.Map<IEnumerable<InvoiceDto>>(invoices);

            return Ok(invoiceDtos);
        }

        /// <summary>
        /// Retrieves a specific invoice by ID.
        /// </summary>
        /// <param name="id">The ID of the invoice.</param>
        /// <returns>An InvoiceDto</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceDto>> GetInvoice(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            var invoiceDto = _mapper.Map<InvoiceDto>(invoice);

            return Ok(invoiceDto);
        }

        /// <summary>
        /// Creates a new invoice.
        /// </summary>
        /// <param name="createInvoiceDto">The invoice creation data.</param>
        /// <returns>The created InvoiceDto</returns>
        [HttpPost]
        public async Task<ActionResult<InvoiceDto>> CreateInvoice(CreateInvoiceDto createInvoiceDto)
        {
            var invoice = _mapper.Map<Invoice>(createInvoiceDto);
            await _invoiceService.CreateInvoiceAsync(invoice);
            var invoiceDto = _mapper.Map<InvoiceDto>(invoice);

            return CreatedAtAction(nameof(GetInvoice), new { id = invoiceDto.Id }, invoiceDto);
        }

        /// <summary>
        /// Updates an existing invoice.
        /// </summary>
        /// <param name="id">The ID of the invoice to update.</param>
        /// <param name="updateInvoiceDto">The updated invoice data.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(int id, UpdateInvoiceDto updateInvoiceDto)
        {
            var invoiceToUpdate = await _invoiceService.GetInvoiceByIdAsync(id);
            _mapper.Map(updateInvoiceDto, invoiceToUpdate);
            await _invoiceService.UpdateInvoiceAsync(id, invoiceToUpdate);

            return NoContent();
        }

        /// <summary>
        /// Deletes an invoice.
        /// </summary>
        /// <param name="id">The ID of the invoice to delete.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            await _invoiceService.DeleteInvoiceAsync(id);

            return NoContent();
        }
    }
}
