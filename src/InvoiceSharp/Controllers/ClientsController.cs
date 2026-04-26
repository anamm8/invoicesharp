using InvoiceSharp.Data;
using InvoiceSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSharp.Controllers
{
    public class ClientsController : Controller
    {
        private readonly AppDbContext _context;

        public ClientsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Clients
        public async Task<IActionResult> Index()
        {
            var clients = await _context.Clients
                .AsNoTracking()
                .ToListAsync();

            return View(clients);
        }

        // GET: /Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var client = await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
                return NotFound();

            return View(client);
        }

        // GET: /Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientModel client)
        {
            if (!ModelState.IsValid)
                return View(client);

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var client = await _context.Clients.FindAsync(id);

            if (client == null)
                return NotFound();

            return View(client);
        }

        // POST: /Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClientModel client)
        {
            if (id != client.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(client);

            try
            {
                _context.Update(client);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists = await _context.Clients.AnyAsync(c => c.Id == client.Id);
                if (!exists)
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var client = await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
                return NotFound();

            return View(client);
        }

        // POST: /Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
