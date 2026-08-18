using System.Security.Cryptography;
using System.Text;

namespace ERP.Api.Services;

/// <summary>
/// Provee un par de claves RSA (2048) en memoria para cifrar contraseñas
/// en el cliente (navegador) y que no viajen en claro por la red / Network tab.
/// El navegador obtiene la clave pública vía GET /api/auth/public-key y cifra
/// el password (RSA-OAEP SHA-256); el servidor lo descifra con la privada.
/// </summary>
public class CryptoService
{
    private readonly RSA _rsa;
    private readonly string _publicKeyPem;

    public CryptoService()
    {
        _rsa = RSA.Create(2048);
        _publicKeyPem = _rsa.ExportSubjectPublicKeyInfoPem();
    }

    public string PublicKeyPem => _publicKeyPem;

    /// <summary>Descifra el password cifrado en el cliente (base64 RSA-OAEP SHA-256).</summary>
    public string DecryptPassword(string encryptedBase64)
    {
        var cipher = Convert.FromBase64String(encryptedBase64);
        var plain = _rsa.Decrypt(cipher, RSAEncryptionPadding.OaepSHA256);
        return Encoding.UTF8.GetString(plain);
    }
}
