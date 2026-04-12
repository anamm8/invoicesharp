using System.ComponentModel.DataAnnotations;

namespace InvoiceSharp.Models
{
    public class ClientModel
    {
        // Primary Key - The Entity Framework convention automatically sets this as Autoincrement
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        // Tax Identification Number (NIF) kept as string to preserve leading zeros
        [Required(ErrorMessage = "NIF is required")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF must be exactly 9 digits")]
        public string NIF { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number format")]
        public string Phone { get; set; } = string.Empty;
    }
}
