using AutoMapper;
using CustomerInvoice_WebApp.Dtos;
using CustomerInvoice_WebApp.Models;
using CustomerInvoice_WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerInvoice_WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class InvoicesController : ControllerBase
    {
        private readonly IInvoicesService _invoiceService;
        private readonly IMapper _mapper;

        public InvoicesController(IInvoicesService invoiceService, IMapper mapper)
        {
            _invoiceService = invoiceService;
            _mapper = mapper;
        }

        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetAllInvoices()
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync();
            var invoiceDtos = _mapper.Map<IEnumerable<InvoiceDto>>(invoices);

            return Ok(invoiceDtos);
        }

        [HttpGet("Overdue")]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetAllOverdueInvoices()
        {
            var overdueInvoices = await _invoiceService.GetOverdueInvoicesAsync();
            var overdueInvoicesDtos = _mapper.Map<IEnumerable<InvoiceDto>>(overdueInvoices);

            return Ok(overdueInvoicesDtos);
        }

        [HttpGet("ByCustomer/{customerId}")]
        public async Task<IActionResult> GetInvoicesByCustomer(int customerId)
        {
            var invoices = await _invoiceService.GetInvoicesByCustomerIdAsync(customerId);
            var invoiceDtos = _mapper.Map<IEnumerable<InvoiceDto>>(invoices);

            return Ok(invoiceDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceDto>> GetInvoice(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            var invoiceDto = _mapper.Map<InvoiceDto>(invoice);

            return Ok(invoiceDto);
        }    

        [HttpPost]
        public async Task<ActionResult<InvoiceDto>> CreateInvoice(CreateInvoiceDto createInvoiceDto)
        {
            var invoice = _mapper.Map<Invoice>(createInvoiceDto);
            await _invoiceService.CreateInvoiceAsync(invoice);
            var invoiceDto = _mapper.Map<InvoiceDto>(invoice);

            return CreatedAtAction(nameof(GetInvoice), new { id = invoiceDto.Id }, invoiceDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(int id, UpdateInvoiceDto updateInvoiceDto)
        {
            var invoiceToUpdate = await _invoiceService.GetInvoiceByIdAsync(id);
            _mapper.Map(updateInvoiceDto, invoiceToUpdate);
            await _invoiceService.UpdateInvoiceAsync(id, invoiceToUpdate);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            await _invoiceService.DeleteInvoiceAsync(id);

            return NoContent();
        }
    }


}
