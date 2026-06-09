using InvoiceSharp.Models;

namespace InvoiceSharp.Interfaces
{
    public interface IInvoicePdfGenerator
    {
        byte[] Generate(InvoiceModel invoice);
    }
}
