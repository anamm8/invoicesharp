using InvoiceSharp.Data;
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
            ViewBag.Clients = await _context.Clients
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();

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
            if (!await _context.Clients.AnyAsync(c => c.Id == invoice.ClientId))
            {
                ModelState.AddModelError(nameof(invoice.ClientId), "Selecione um cliente válido.");
            }

            if (invoice.Total != invoice.Subtotal + invoice.VATTotal)
            {
                ModelState.AddModelError(nameof(invoice.Total), "O Total deve ser igual a Subtotal + VATTotal.");
            }

            if (invoice.Date == default)
            {
                invoice.Date = DateTime.Now;
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Clients = await _context.Clients
                    .AsNoTracking()
                    .OrderBy(c => c.Name)
                    .ToListAsync();

                return View(invoice);
            }

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}