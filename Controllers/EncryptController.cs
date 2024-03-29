using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CriptografiaDeImagem.Models;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Runtime.Intrinsics.Arm;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace CriptografiaDeImagem.Controllers;

public class EncryptController : Controller
{
    //private readonly ILogger<HomeController> _logger;

    public EncryptController(ILogger<HomeController> logger)
    {
        //_logger = logger;
    }

    [HttpGet]
    public IActionResult GenerateKeyPair()
    {
        KeyPairModel viewModel = new KeyPairModel();
        TempData["AlreadySubmit"] = false;
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult GenerateKeyPair(int id)
    {

        if (Convert.ToBoolean(TempData["AlreadySubmit"]))
        {
            TempData["AlreadySubmit"] = false;
            return View(new KeyPairModel());
        }

        RSA myRsa = RSA.Create();

        KeyPairModel viewModel = new KeyPairModel()
        {
            PrivateKey = myRsa.ExportRSAPrivateKeyPem().Replace("\n", "<br>"),
            PublicKey = myRsa.ExportRSAPublicKeyPem().Replace("\n", "<br>")
        };

        TempData["AlreadySubmit"] = true;
        return View(viewModel);
    }

    public void HashCode()
    {
        var imageInput = Request.Form.Files["imageInput"];

        if (imageInput != null && imageInput.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                imageInput.CopyTo(ms);
                var imagem = ms.ToArray();
            }
        }

        //byte[] hashvalue =
    }

}
