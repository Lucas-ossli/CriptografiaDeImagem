using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CriptografiaDeImagem.Models;
using System.Net.NetworkInformation;

namespace CriptografiaDeImagem.Controllers;

public class StartController : Controller
{
    //private readonly ILogger<HomeController> _logger;

    public StartController(ILogger<HomeController> logger)
    {
        //_logger = logger;
    }

    public IActionResult FirstPage()
    {
        return View();
    }
}
