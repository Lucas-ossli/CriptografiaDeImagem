namespace CriptografiaDeImagem.Models;

public class KeyPairModel
{
    public string PrivateKey { get; set; }
    public string PublicKey { get; set; }
    public bool AlreadySubmit { get; set; }

    public byte[]? EncryptedData { get; set; }
    public byte[] NormalImage { get; set; }

    public KeyPairModel()
    {
        PrivateKey = string.Empty;
        PublicKey = string.Empty;
    }
}
