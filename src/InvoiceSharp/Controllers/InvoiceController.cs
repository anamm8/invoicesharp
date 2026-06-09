using InvoiceSharp.Exceptions;
using InvoiceSharp.Interfaces;
using InvoiceSharp.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceSharp.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IClientsRepository _clientsRepository;
        private readonly IInvoicePdfGenerator _invoicePdfGenerator;

        public InvoiceController(
            IInvoiceRepository invoiceRepository,
            IClientsRepository clientsRepository,
            IInvoicePdfGenerator invoicePdfGenerator)
        {
            _invoiceRepository = invoiceRepository;
            _clientsRepository = clientsRepository;
            _invoicePdfGenerator = invoicePdfGenerator;
        }

        // GET: /Invoice
        public async Task<IActionResult> Index()
        {
            var invoices = await _invoiceRepository.GetAllWithClientsAsync();
            return View(invoices);
        }

        // GET: /Invoice/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var invoice = await _invoiceRepository.GetByIdWithDetailsAsync(id.Value);

            if (invoice == null)
                return NotFound();

            return View(invoice);
        }

        // GET: /Invoice/Create
        public async Task<IActionResult> Create()
        {
            await LoadClientsAsync();

            return View(new InvoiceModel
            {
                Date = DateTime.Now
            });
        }

        // POST: /Invoice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InvoiceModel invoice)
        {
            try
            {
                await EnsureClientExistsAsync(invoice.ClientId);

                if (invoice.Date == default)
                {
                    invoice.Date = DateTime.Now;
                }

                // If line items are provided, calculate invoice totals from them.
                // This prevents valid invoices from being rejected merely because totals were left at zero in the form.
                if (invoice.Items != null && invoice.Items.Any())
                {
                    CalculateTotalsFromItems(invoice);
                    ClearCalculatedTotalsFromModelState();
                }

                if (invoice.Total != invoice.Subtotal + invoice.VATTotal)
                {
                    ModelState.AddModelError(nameof(invoice.Total),
                        "O Total deve ser igual a Subtotal + IVA.");
                }

                if (!ModelState.IsValid)
                {
                    await LoadClientsAsync();
                    return View(invoice);
                }

                await _invoiceRepository.AddAsync(invoice);
                await _invoiceRepository.SaveChangesAsync();

                TempData["SuccessMessage"] = "Fatura criada com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            catch (ClientNotFoundException ex)
            {
                ModelState.AddModelError(nameof(invoice.ClientId), ex.Message);

                await LoadClientsAsync();
                return View(invoice);
            }
        }

        // GET: /Invoice/GeneratePdf/5
        public async Task<IActionResult> GeneratePdf(int? id)
        {
            if (id == null)
                return NotFound();

            var invoice = await _invoiceRepository.GetByIdWithDetailsAsync(id.Value);

            if (invoice == null)
                return NotFound();

            var pdfBytes = _invoicePdfGenerator.Generate(invoice);
            var fileName = $"fatura-{invoice.Id}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }

        private async Task EnsureClientExistsAsync(int clientId)
        {
            bool exists = await _clientsRepository.ExistsAsync(clientId);

            if (!exists)
            {
                throw new ClientNotFoundException(
                    "O cliente indicado não existe. Verifique o cliente selecionado.");
            }
        }

        private async Task LoadClientsAsync()
        {
            var clients = await _clientsRepository.GetAllAsync();
            ViewBag.Clients = clients.OrderBy(c => c.Name).ToList();
        }

        private static void CalculateTotalsFromItems(InvoiceModel invoice)
        {
            invoice.Subtotal = invoice.Items.Sum(item => item.Quantity * item.UnitPrice);
            invoice.VATTotal = invoice.Items.Sum(item => item.Quantity * item.UnitPrice * item.VATRate / 100);
            invoice.Total = invoice.Subtotal + invoice.VATTotal;
        }

        private void ClearCalculatedTotalsFromModelState()
        {
            ModelState.Remove(nameof(InvoiceModel.Subtotal));
            ModelState.Remove(nameof(InvoiceModel.VATTotal));
            ModelState.Remove(nameof(InvoiceModel.Total));
        }
    }
}
