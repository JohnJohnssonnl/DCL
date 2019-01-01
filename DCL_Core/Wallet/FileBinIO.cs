using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

namespace DCL_Core.Wallet
{
    class FileBinIO
    {
        public static void WriteBin(byte[] _storage, string _fileName)
        {
            //Write to wallet file
            String FileFolder = AppDomain.CurrentDomain.BaseDirectory + @"bin\\WalletData\\";
            String FilePath = FileFolder + _fileName + ".bin";

            if (!Directory.Exists(FileFolder))
            {
                //Create directory if it does not exist
                Directory.CreateDirectory(FileFolder);
            }

            BinaryWriter writer = new BinaryWriter(File.Open(FilePath, FileMode.Create));

            writer.Write(_storage);  //Write binaries
            writer.Close();
        }
        public static byte[] ReadBin(String _fileName)
        {
            String FileFolder = AppDomain.CurrentDomain.BaseDirectory + @"bin\\WalletData\\";
            String FilePath = FileFolder + _fileName + ".bin";

            StreamReader sr = new StreamReader(FilePath);
            var bytes = default(byte[]);
            using (var memstream = new MemoryStream())
            {
                sr.BaseStream.CopyTo(memstream);
                bytes = memstream.ToArray();
            }
            sr.Close(); //Close connection to avoid fileOpening errors

            return bytes;
        }
        public static Boolean Exists(String _fileName)
        {
            String FileFolder = AppDomain.CurrentDomain.BaseDirectory + @"bin\\WalletData\\";
            String FilePath = FileFolder + _fileName + ".bin";

            return File.Exists(FilePath);
        }
    }
}