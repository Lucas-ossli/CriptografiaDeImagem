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
    public static byte[]? DecryptData;
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

    #region Encrypt

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
        else
        {
            return View(new KeyPairModel());
        }

        try
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportFromPem(viewModel.PublicKey.TrimEnd().TrimStart());
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
        }
        catch (System.Exception ex)
        {
            return View(new KeyPairModel());
        }

        return View(viewModel);
    }

    public ActionResult DownloadEncryptedFile()
    {
        byte[] encryptedData = EncryptData;
        string fileName = "encrypted_data.dat";

        return File(encryptedData, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
    }

    #endregion

    #region Decrypt 

    [HttpGet]
    public IActionResult DecryptImage()
    {
        return View(new KeyPairModel());
    }

    [HttpPost]
    public IActionResult DecryptImage(KeyPairModel viewModel)
    {
        var imageInput = Request.Form.Files["imageInput"];

        if (imageInput != null && imageInput.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                imageInput.CopyTo(ms);
                viewModel.EncryptedData = ms.ToArray();
            }
        }
        else
        {
            return View(new KeyPairModel());
        }


        try
        {
            RSA rsa = RSA.Create();
            rsa.ImportFromPem(viewModel.PrivateKey.TrimEnd().TrimStart());

            int blockSize = rsa.KeySize / 8;

            using (MemoryStream decryptedStream = new MemoryStream())
            {
                int offset = 0;
                while (offset < viewModel.EncryptedData?.Length)
                {
                    int blockSizeRemaining = viewModel.EncryptedData.Length - offset;
                    int blockSizeToProcess = Math.Min(blockSize, blockSizeRemaining);
                    byte[] block = new byte[blockSizeToProcess];
                    Array.Copy(viewModel.EncryptedData, offset, block, 0, blockSizeToProcess);

                    byte[] decryptedBlock = rsa.Decrypt(block, RSAEncryptionPadding.Pkcs1);
                    decryptedStream.Write(decryptedBlock, 0, decryptedBlock.Length);

                    offset += blockSize;
                }

                viewModel.NormalImage = decryptedStream.ToArray();
                DecryptData = viewModel.NormalImage;
            }
        }
        catch (System.Exception ex)
        {
            return View(new KeyPairModel());
        }

        return View(viewModel);
    }

    public ActionResult DownloadDecryptedFile()
    {
        byte[] decryptData = DecryptData;
        string fileName = "Decrypt_data.png";

        return File(decryptData, "image/png", fileName);
    }

    #endregion  
}
