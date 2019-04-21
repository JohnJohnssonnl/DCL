using DCL_Core.Contracts;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using DCL_Core;
using DCL_Core.CORE;

namespace DCL_Core.Wallet
{
    public class WalletManager
    {
        //Make sure the Data can not be read directly as public member
        private WalletData Data { get; set; }

        public bool isLoggedIn()
        {
            return Data == null ? false : true;
        }
        public string PublicAddress()
        {
            return Data.PublicAddress;
        }
        public double Balance()
        {
            return Data.Balance;
        }
        public void LogOut()
        {
            //Only do something if we're actually logged in
            if (Data != null)
            {
                WriteWalletData(Data, Data.WalletName);
                Data = null;    //Remove data
            }
        }

        public bool Login(LoginRequest _loginRequest)
        {
            WalletData ret = null;

            if (_loginRequest.CreateNewWallet &&
                _loginRequest.PassCode != "" &&
                _loginRequest.WalletName != "" &&
                !FileBinIO.Exists(_loginRequest.WalletName))
            {
                ret = GenerateWallet(_loginRequest.PassCode, _loginRequest.WalletName);
            }
            else if (_loginRequest.CreateNewWallet)
            {
                Console.WriteLine("Missing wallet name or password, or password requirements do not match, or wallet already exists");
                return false;    //Todo: add exception error
            }
            else
            {
                ret = LoginWallet(_loginRequest);
            }

            Data = ret;

            return Data == null ? false : true;
        }

        private WalletData LoginWallet(LoginRequest _loginRequest)
        {
            WalletData ret;

            try
            {
                ret = ReadBlobToWalletData(_loginRequest.WalletName);
            }
            catch
            {
                return null;    //Ignore for now
            }

            if (ret == null)
            {
                return null;
            }

            if (!PasswordManager.CheckPass(_loginRequest.PassCode, Convert.ToBase64String(ret.PassCode)))
            {
                Console.WriteLine("Wrong passcode entered!");
                return null;
            }

            return ret;
        }

        private WalletData GenerateWallet(string _passCode, string _walletName)
        {
            WalletData ret = null;

            if (PasswordManager.CheckPassRequirements(_passCode))
            {
                ret = makeWallet(_passCode, _walletName);
                WriteWalletData(ret, _walletName);
            }

            return ret;
        }

        public static void WriteWalletData(WalletData _WalletDataToBlob, string _walletName)
        {
            //TODO: Move serialization to FileBinIO to avoid duplicating code on multiple objects
            MemoryStream memorystream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(memorystream, _WalletDataToBlob);
            byte[] BlobData = memorystream.ToArray();

            FileBinIO.WriteBin(BlobData, _walletName);
        }
        public static WalletData ReadBlobToWalletData(string _walletName)
        {
            //Read from bin
            //TODO: Move deserialization to FileBinIO to avoid duplicating code on multiple objects
            byte[] BlobData = FileBinIO.ReadBin(_walletName);

            MemoryStream memorystreamd = new MemoryStream(BlobData);
            BinaryFormatter bfd = new BinaryFormatter();
            WalletData deserializedBlock = bfd.Deserialize(memorystreamd) as WalletData;

            memorystreamd.Close();

            return deserializedBlock;
        }

        private WalletData makeWallet(string _passCode, string _walletName)
        {
            WalletData newWallet = new WalletData();
            Keys WalletKeys = new Keys
            {
                PrivateKey = GeneratePrivateKey(_passCode, _walletName) 	
            };
            WalletKeys.PrivateKeyByte   = Encoding.ASCII.GetBytes(WalletKeys.PrivateKey);
            WalletKeys.PublicKey        = GeneratePublicKey(WalletKeys.PrivateKeyByte); 
            WalletKeys.PublicKeyByte    = Encoding.ASCII.GetBytes(WalletKeys.PublicKey);
            newWallet.PublicAddress     = WalletKeys.PublicKey;
            newWallet.PassCode = PasswordManager.GenerateSaltedOutputBytes(_passCode);
            newWallet.WalletName = _walletName;
            newWallet.KeySet = WalletKeys;

            return newWallet;   //Return the public address, the rest we have to store somewhere safe...
        }

        
    public static string GeneratePrivateKey(string _passCode, string _userId)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(_passCode + _userId + Convert.ToString(DateTime.Now.Ticks));
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            return ByteArrayToString(hash);
        }
        public static string GeneratePublicKey(byte[] privateKeyByte)
        {
            int privateKeyInteger = BitConverter.ToInt32(privateKeyByte,0);
            var curve = SecNamedCurves.GetByName("secp256k1");
            var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
            var d = new Org.BouncyCastle.Math.BigInteger(privateKeyInteger.ToString());
            var q = domain.G.Multiply(d);
            var publicKey = new ECPublicKeyParameters(q, domain);

            return Base58Encoding.Encode(publicKey.Q.GetEncoded());

        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            Console.WriteLine(hex.ToString());
            return hex.ToString();
        }
    }
}