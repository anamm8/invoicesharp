using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceSharp.Models
{
    public class InvoiceModel
    {
        // Primary Key
        public int Id { get; set; }

        // Foreign Key for Client
        [Required(ErrorMessage = "Client selection is mandatory")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Invoice date is mandatory")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        // Precision for financial values
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Subtotal cannot be negative")]
        public decimal Subtotal { get; set; }

        [Range(0, (double)decimal.MaxValue, ErrorMessage = "VAT Total cannot be negative")]
        public decimal VATTotal { get; set; }

        // The total amount must be greater than zero to ensure that the invoice is valid and meaningful
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Total must be greater than zero")]
        public decimal Total { get; set; }

        /* * Navigation Property
         * This allows EF to link the Invoice back to its specific Client
         */
        [ForeignKey("ClientId")]

        public virtual ClientModel? Client { get; set; }

        public virtual ICollection<InvoiceItemModel> Items { get; set; } = new List<InvoiceItemModel>();
    }
}
