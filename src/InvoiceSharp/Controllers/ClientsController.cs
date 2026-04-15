using Microsoft.AspNetCore.Mvc;

namespace InvoiceSharp.Controllers
{
    public class ClientsController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}