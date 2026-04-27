using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceSharp.Models
{
    public class InvoiceItemModel
    {
        // Primary Key
        public int Id { get; set; }

        // Foreign Key to the Parent Invoice
        public int InvoiceId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string Description { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive integer greater than 0")]
        public int Quantity { get; set; }

        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Unit price must be greater than 0")]
        public decimal UnitPrice { get; set; }

        // Tax percentage (e.g., 23 for 23%)
        [Range(0, 100, ErrorMessage = "VAT Rate must be between 0 and 100")]
        public decimal VATRate { get; set; }

        /* * Navigation Property
         * Links each item back to its specific Invoice
         */
        [ForeignKey("InvoiceId")]

        public virtual InvoiceModel? Invoice { get; set; }
    }
}
