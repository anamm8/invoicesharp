using InvoiceSharp.Interfaces;
using InvoiceSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSharp.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IClientsRepository _clientsRepository;

        public ClientsController(IClientsRepository clientsRepository)
        {
            _clientsRepository = clientsRepository;
        }

        // GET: /Clients
        public async Task<IActionResult> Index()
        {
            var clients = await _clientsRepository.GetAllAsync();
            return View(clients);
        }

        // GET: /Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var client = await _clientsRepository.GetByIdAsync(id.Value);

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

            await _clientsRepository.AddAsync(client);
            await _clientsRepository.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var client = await _clientsRepository.GetByIdAsync(id.Value);

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
                _clientsRepository.Update(client);
                await _clientsRepository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists = await _clientsRepository.ExistsAsync(client.Id);
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

            var client = await _clientsRepository.GetByIdAsync(id.Value);

            if (client == null)
                return NotFound();

            return View(client);
        }

        // POST: /Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _clientsRepository.DeleteAsync(id);
            await _clientsRepository.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
