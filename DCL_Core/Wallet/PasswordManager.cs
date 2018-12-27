using System;
using System.Security.Cryptography;
using System.Linq;

namespace DCL_Core.Wallet
{
    class PasswordManager
    {
        //this class will hold all required code for creating a new salted password and checking the input when logging into a wallet against the stored one
        public static string GenerateSaltedOutput(string _unsaltedInput)
        {
            string saltedPass = "";
            Byte[] saltStore;
            Byte[] passHashStore;
            Byte[] passHashSaltedStore;

            new RNGCryptoServiceProvider().GetBytes(saltStore = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(_unsaltedInput, saltStore, 10000);

            passHashStore = pbkdf2.GetBytes(20);
            passHashSaltedStore = new Byte[36];
            Array.Copy(saltStore, 0, passHashSaltedStore, 0, 16);
            Array.Copy(passHashStore, 0, passHashSaltedStore, 16, 20);
            saltedPass = Convert.ToBase64String(passHashSaltedStore);

            return saltedPass;
        }

        public static byte[] GenerateSaltedOutputBytes(string _unsaltedInput)   //Generates bytes output
        {
            Byte[] saltStore;
            Byte[] passHashStore;
            Byte[] passHashSaltedStore;

            new RNGCryptoServiceProvider().GetBytes(saltStore = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(_unsaltedInput, saltStore, 10000);

            passHashStore = pbkdf2.GetBytes(20);
            passHashSaltedStore = new Byte[36];
            Array.Copy(saltStore, 0, passHashSaltedStore, 0, 16);
            Array.Copy(passHashStore, 0, passHashSaltedStore, 16, 20);

            return passHashSaltedStore;
        }

        public static Boolean CheckPassRequirements(String _pass)
        {
            Boolean Ret = true;

            if (_pass.Length < 8 || _pass.Length > 32)
            {
                Console.WriteLine("Pass needs to be minimal 8 chars and max 32 chars");
                Ret = false;
            }
            else if (_pass.Where(char.IsUpper).Count() < 2 || _pass.Where(char.IsLower).Count() < 2)
            {
                Console.WriteLine("Pass requires at least 2 uppercases and 2 lowercases");
                Ret = false;
            }
            return Ret;
        }

        public static Boolean CheckPass(string _unsaltedInput, string _saltedPass)
        {
            Boolean ret = true;
            Byte[] saltInput;
            Byte[] passHashInput;
            Byte[] passHashSaltedStore;

            passHashSaltedStore = Convert.FromBase64String(_saltedPass);
            saltInput = new byte[16];
            Array.Copy(passHashSaltedStore, 0, saltInput, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(_unsaltedInput, saltInput, 10000);
            passHashInput = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
            {
                if (passHashSaltedStore[i + 16] != passHashInput[i])
                {
                    ret = false;
                }
            }

            return ret;
        }
    }
}