using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CriptografiaDeImagem.Models;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Runtime.Intrinsics.Arm;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Reflection.PortableExecutable;

namespace CriptografiaDeImagem.Controllers;

public class EncryptController : Controller
{
    //private readonly ILogger<HomeController> _logger;

    public static byte[]? EncryptData;
    public static bool AlreadySubmit;
    public EncryptController(ILogger<HomeController> logger)
    {
        //_logger = logger;
    }

    [HttpGet]
    public IActionResult GenerateKeyPair()
    {
        KeyPairModel viewModel = new KeyPairModel();
        AlreadySubmit = false;
        viewModel.AlreadySubmit = AlreadySubmit;
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult GenerateKeyPair(int id)
    {
        if (AlreadySubmit)
        {
            AlreadySubmit = false;
            var viewModelx = new KeyPairModel() { AlreadySubmit = AlreadySubmit };
            return View(viewModelx);
        }

        RSA myRsa = RSA.Create();

        AlreadySubmit = true;
        KeyPairModel viewModel = new KeyPairModel()
        {
            PrivateKey = myRsa.ExportRSAPrivateKeyPem().Replace("\n", "<br>"),
            PublicKey = myRsa.ExportRSAPublicKeyPem().Replace("\n", "<br>"),
            AlreadySubmit = AlreadySubmit
        };

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult EncryptImage()
    {
        return View(new KeyPairModel());
    }


    [HttpPost]
    public IActionResult EncryptImage(KeyPairModel viewModel)
    {
        var imageInput = Request.Form.Files["imageInput"];

        if (imageInput != null && imageInput.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                imageInput.CopyTo(ms);
                viewModel.NormalImage = ms.ToArray();
            }
        }

        using (RSA rsa = RSA.Create())
        {
            int tamanhoBloco = (rsa.KeySize / 8) - 11;

            using (MemoryStream ms = new MemoryStream())
            {
                int offset = 0;

                while (offset < viewModel.NormalImage.Length)
                {
                    int tamanhoAtual = Math.Min(tamanhoBloco, viewModel.NormalImage.Length - offset);
                    byte[] bloco = new byte[tamanhoAtual];
                    Array.Copy(viewModel.NormalImage, offset, bloco, 0, tamanhoAtual);
                    byte[] blocoCriptografado = rsa.Encrypt(bloco, RSAEncryptionPadding.Pkcs1);
                    ms.Write(blocoCriptografado, 0, blocoCriptografado.Length);
                    offset += tamanhoAtual;
                }

                viewModel.EncryptedData = ms.ToArray();
                EncryptData = viewModel.EncryptedData;
            }
        }



        return View(viewModel);
    }


    public ActionResult DownloadEncryptedFile()
    {
        byte[] encryptedData = EncryptData;
        string fileName = "encrypted_data.dat";

        return File(encryptedData, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
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

    }



}
