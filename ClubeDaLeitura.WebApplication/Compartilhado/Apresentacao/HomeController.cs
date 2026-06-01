using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeitura.WebApplication.ModuloHome.Apresentacao;

public class HomeController : Controller
{
    [HttpGet]
    public ActionResult Index()
    {
        return View();
    }
}

