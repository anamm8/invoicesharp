using Microsoft.AspNetCore.Mvc;
using InvoiceSharp.Models;

namespace InvoiceSharp.Controllers
{
    public class InvoicesController : Controller
    {
        // ============================
        // GET: /Invoices/Create
        // Mostra o formulário
        // ============================
        public IActionResult Create()
        {
            // Simulação temporária depois vem da Base Dados
            var clients = new List<ClientModel>
            {
                new ClientModel { Id = 1, Name = "Cliente A" },
                new ClientModel { Id = 2, Name = "Cliente B" }

            };

            ViewBag.Clients = clients;
            
            return View();
        }

        // ============================
        // POST: /Invoices/Create
        // Recebe os dados da view
        // ============================
        [HttpPost]
        public IActionResult Create(InvoiceModel model)
        {
            // 🔹 Só para teste (não guardar nada ainda)

            // Simular sucesso
            ViewBag.Success = true;

            return View(model);
        }
    }
}