using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceSharp.Models
{
    public class InvoiceModel
    {
        // Primary Key
        public int Id { get; set; }

        // Foreign Key for Client
        public int ClientId { get; set; }

        public DateTime Date { get; set; }

        // Precision for financial values
        public decimal Subtotal { get; set; }

        public decimal VATTotal { get; set; }

        public decimal Total { get; set; }

        /* * Navigation Property
         * This allows EF to link the Invoice back to its specific Client
         */
        [ForeignKey("ClientId")]

        public virtual ClientModel? Client { get; set; }

        public virtual ICollection<InvoiceItemModel> Items { get; set; } = new List<InvoiceItemModel>();
    }
}
