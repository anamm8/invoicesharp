using InvoiceSharp.Models;

namespace InvoiceSharp.Interfaces
{
    public interface IClientsRepository
    {
        Task<IEnumerable<ClientModel>> GetAllAsync();
        Task<ClientModel?> GetByIdAsync(int id);
        Task AddAsync(ClientModel client);
        void Update(ClientModel client);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}