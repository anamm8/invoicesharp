using InvoiceSharp.Models;

namespace InvoiceSharp.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<IEnumerable<InvoiceModel>> GetAllWithClientsAsync();

        Task<InvoiceModel?> GetByIdWithDetailsAsync(int id);

        Task AddAsync(InvoiceModel invoice);

        Task<bool> SaveChangesAsync();

        Task<bool> ClientExistsInMemoryAsync(int clientId);
    }
}