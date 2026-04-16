using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceSharp.Models
{
    public class InvoiceItemModel
    {
        // Primary Key
        public int Id { get; set; }

        // Foreign Key to the Parent Invoice
        public int InvoiceId { get; set; }

        public string Description { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        // Tax percentage (e.g., 23 for 23%)
        public decimal VATRate { get; set; }

        /* * Navigation Property
         * Links each item back to its specific Invoice
         */
        [ForeignKey("InvoiceId")]

        public virtual InvoiceModel? Invoice { get; set; }
    }
}
