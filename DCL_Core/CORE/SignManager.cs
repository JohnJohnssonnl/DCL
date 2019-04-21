using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Text;

namespace DCL_Core.CORE
{
    class SignManager
    {
        //Implementation of https://blog.todotnet.com/2018/02/public-private-keys-and-signing/
        public static string GetSignature(string privateKey, string message)
        {
            Org.BouncyCastle.Asn1.X9.X9ECParameters curve = SecNamedCurves.GetByName("secp256k1");
            ECDomainParameters domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
            ECPrivateKeyParameters keyParameters = new ECPrivateKeyParameters(new Org.BouncyCastle.Math.BigInteger(privateKey), domain);
            ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");
            signer.Init(true, keyParameters);
            signer.BlockUpdate(Encoding.ASCII.GetBytes(message), 0, message.Length);
            byte[] signature = signer.GenerateSignature();

            return Base58Encoding.Encode(signature);

        }
        public static bool VerifySignature(string message, string publicKey, string signature)
        {
            var curve = SecNamedCurves.GetByName("secp256k1");
            var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
            var publicKeyBytes = Base58Encoding.Decode(publicKey);
            var q = curve.Curve.DecodePoint(publicKeyBytes);
            var keyParameters = new ECPublicKeyParameters(q, domain);
            ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");
            signer.Init(false, keyParameters);
            signer.BlockUpdate(Encoding.ASCII.GetBytes(message), 0, message.Length);
            var signatureBytes = Base58Encoding.Decode(signature);

            return signer.VerifySignature(signatureBytes);
        }
    }
}
