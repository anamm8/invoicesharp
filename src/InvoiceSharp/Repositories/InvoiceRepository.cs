using InvoiceSharp.Data;
using InvoiceSharp.Interfaces;
using InvoiceSharp.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSharp.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AppDbContext _context;

        public InvoiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InvoiceModel>> GetAllWithClientsAsync()
        {
            return await _context.Invoices
                .AsNoTracking()
                .Include(i => i.Client)
                .ToListAsync();
        }
        public async Task<InvoiceModel?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Invoices
                .AsNoTracking()
                .Include(i => i.Client)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddAsync(InvoiceModel invoice)
        {
            await _context.Invoices.AddAsync(invoice);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> ClientExistsInMemoryAsync(int clientId)
        {
            // Instead of executing an "AnyAsync" query that hits the database single-purposefully,
            // we fetch only the client IDs into memory (acting as a fast local cache)
            // and perform the validation check in-memory using LINQ to Objects.
            var activeClientIds = await _context.Clients
                .AsNoTracking()
                .Select(c => c.Id)
                .ToListAsync();

            return activeClientIds.Contains(clientId);
        }
    }
}