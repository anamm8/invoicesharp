namespace InvoiceSharp.Models
{
    public class ClientModel
    {
        // Primary Key - The Entity Framework convention automatically sets this as Autoincrement
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        // Tax Identification Number (NIF) kept as string to preserve leading zeros
        public string NIF { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
    }
}
