using InvoiceSharp.Data;
using InvoiceSharp.Exceptions;
using InvoiceSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSharp.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly AppDbContext _context;

        public InvoiceController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Invoice
        public async Task<IActionResult> Index()
        {
            var invoices = await _context.Invoices
                .Include(i => i.Client)
                .AsNoTracking()
                .ToListAsync();

            return View(invoices);
        }

        // GET: /Invoice/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var invoice = await _context.Invoices
                .Include(i => i.Client)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);

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

                if (invoice.Total != invoice.Subtotal + invoice.VATTotal)
                {
                    ModelState.AddModelError(nameof(invoice.Total),
                        "O Total deve ser igual a Subtotal + VATTotal.");
                }

                if (invoice.Date == default)
                {
                    invoice.Date = DateTime.Now;
                }

                if (!ModelState.IsValid)
                {
                    await LoadClientsAsync();
                    return View(invoice);
                }

                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();

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

        private async Task EnsureClientExistsAsync(int clientId)
        {
            bool exists = await _context.Clients.AnyAsync(c => c.Id == clientId);

            if (!exists)
            {
                throw new ClientNotFoundException(
                    "O cliente indicado não existe. Verifique o cliente selecionado.");
            }
        }

        private async Task LoadClientsAsync()
        {
            ViewBag.Clients = await _context.Clients
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
    }
}
