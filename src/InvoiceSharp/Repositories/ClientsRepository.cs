using InvoiceSharp.Data;
using InvoiceSharp.Interfaces;
using InvoiceSharp.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSharp.Repositories
{
    // Using Primary Constructor for clean dependency injection
    public class ClientRepository(AppDbContext context) : IClientsRepository
    {
        public async Task<IEnumerable<ClientModel>> GetAllAsync()
        {
            return await context.Clients
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ClientModel?> GetByIdAsync(int id)
        {
            return await context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(ClientModel client)
        {
            await context.Clients.AddAsync(client);
        }

        public void Update(ClientModel client)
        {
            // Marking the entity as modified in the EF tracker
            context.Clients.Update(client);
        }

        public async Task DeleteAsync(int id)
        {
            var client = await context.Clients.FindAsync(id);

            if (client != null)
            {
                context.Clients.Remove(client);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Clients.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            // Persistence occurs here, returning true if any row was modified
            return (await context.SaveChangesAsync()) > 0;
        }
    }
}