namespace CriptografiaDeImagem.Models;

public class KeyPairModel
{
    public string PrivateKey { get; set; }
    public string PublicKey { get; set; }

    public KeyPairModel()
    {
        PrivateKey = string.Empty;
        PublicKey = string.Empty;
    }
}
