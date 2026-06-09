using InvoiceSharp.Interfaces;
using InvoiceSharp.Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace InvoiceSharp.Services
{
    public class InvoicePdfGenerator : IInvoicePdfGenerator
    {
        public byte[] Generate(InvoiceModel invoice)
        {
            using var document = new PdfDocument();
            document.Info.Title = $"Fatura {invoice.Id}";

            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            var titleFont = new XFont("Arial", 18, XFontStyleEx.Bold);
            var headingFont = new XFont("Arial", 12, XFontStyleEx.Bold);
            var normalFont = new XFont("Arial", 10, XFontStyleEx.Regular);

            double y = 40;

            gfx.DrawString("InvoiceSharp", titleFont, XBrushes.Black, new XPoint(40, y));
            y += 30;

            gfx.DrawString($"Fatura n.º {invoice.Id}", headingFont, XBrushes.Black, new XPoint(40, y));
            y += 20;
            gfx.DrawString($"Data: {invoice.Date:dd/MM/yyyy}", normalFont, XBrushes.Black, new XPoint(40, y));
            y += 20;
            gfx.DrawString($"Cliente: {invoice.Client?.Name ?? "Cliente não identificado"}", normalFont, XBrushes.Black, new XPoint(40, y));
            y += 20;
            gfx.DrawString($"NIF: {invoice.Client?.NIF ?? "-"}", normalFont, XBrushes.Black, new XPoint(40, y));
            y += 30;

            gfx.DrawString("Itens da fatura", headingFont, XBrushes.Black, new XPoint(40, y));
            y += 20;

            gfx.DrawString("Descrição", headingFont, XBrushes.Black, new XPoint(40, y));
            gfx.DrawString("Qtd.", headingFont, XBrushes.Black, new XPoint(300, y));
            gfx.DrawString("Preço", headingFont, XBrushes.Black, new XPoint(350, y));
            gfx.DrawString("IVA", headingFont, XBrushes.Black, new XPoint(420, y));
            gfx.DrawString("Total", headingFont, XBrushes.Black, new XPoint(470, y));
            y += 15;

            gfx.DrawLine(XPens.Black, 40, y, 540, y);
            y += 15;

            if (invoice.Items != null && invoice.Items.Any())
            {
                foreach (var item in invoice.Items)
                {
                    var lineTotal = item.Quantity * item.UnitPrice * (1 + item.VATRate / 100);

                    gfx.DrawString(Shorten(item.Description, 35), normalFont, XBrushes.Black, new XPoint(40, y));
                    gfx.DrawString(item.Quantity.ToString(), normalFont, XBrushes.Black, new XPoint(300, y));
                    gfx.DrawString($"{item.UnitPrice:0.00} €", normalFont, XBrushes.Black, new XPoint(350, y));
                    gfx.DrawString($"{item.VATRate:0}%", normalFont, XBrushes.Black, new XPoint(420, y));
                    gfx.DrawString($"{lineTotal:0.00} €", normalFont, XBrushes.Black, new XPoint(470, y));

                    y += 18;
                }
            }
            else
            {
                gfx.DrawString("Nenhum item registado.", normalFont, XBrushes.Black, new XPoint(40, y));
                y += 18;
            }

            y += 20;
            gfx.DrawLine(XPens.Black, 330, y, 540, y);
            y += 20;

            gfx.DrawString($"Subtotal: {invoice.Subtotal:0.00} €", normalFont, XBrushes.Black, new XPoint(350, y));
            y += 18;
            gfx.DrawString($"IVA: {invoice.VATTotal:0.00} €", normalFont, XBrushes.Black, new XPoint(350, y));
            y += 18;
            gfx.DrawString($"Total: {invoice.Total:0.00} €", headingFont, XBrushes.Black, new XPoint(350, y));

            using var stream = new MemoryStream();
            document.Save(stream, false);
            return stream.ToArray();
        }

        private static string Shorten(string text, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(text) || text.Length <= maxLength)
                return text;

            return text[..(maxLength - 3)] + "...";
        }
    }
}
