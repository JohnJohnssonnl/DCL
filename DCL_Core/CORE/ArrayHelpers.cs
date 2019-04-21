using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DCL_Core.CORE
{
    public class ArrayHelpers
    {
        //Implementation of https://gist.github.com/CodesInChaos/3175971
        public static T[] ConcatArrays<T>(params T[][] arrays)
        {
            Contract.Requires(arrays != null);
            Contract.Requires(Contract.ForAll(arrays, (arr) => arr != null));
            Contract.Ensures(Contract.Result<T[]>() != null);
            Contract.Ensures(Contract.Result<T[]>().Length == arrays.Sum(arr => arr.Length));

            var result = new T[arrays.Sum(arr => arr.Length)];
            int offset = 0;
            for (int i = 0; i < arrays.Length; i++)
            {
                var arr = arrays[i];
                Buffer.BlockCopy(arr, 0, result, offset, arr.Length);
                offset += arr.Length;
            }
            return result;
        }
        public static byte[] Keccak256Helper(byte[] _input)
        {
            KeccakDigest Kec256 = new KeccakDigest(256);
            Kec256.Reset();
            byte[] resultHashedKec256 = new byte[32];
            Kec256.BlockUpdate(_input, 0, _input.Length);
            Kec256.DoFinal(resultHashedKec256, 0);

            return resultHashedKec256;
        }

        public static T[] ConcatArrays<T>(T[] arr1, T[] arr2)
        {
            Contract.Requires(arr1 != null);
            Contract.Requires(arr2 != null);
            Contract.Ensures(Contract.Result<T[]>() != null);
            Contract.Ensures(Contract.Result<T[]>().Length == arr1.Length + arr2.Length);

            var result = new T[arr1.Length + arr2.Length];
            Buffer.BlockCopy(arr1, 0, result, 0, arr1.Length);
            Buffer.BlockCopy(arr2, 0, result, arr1.Length, arr2.Length);
            return result;
        }

        public static T[] SubArray<T>(T[] arr, int start, int length)
        {
            Contract.Requires(arr != null);
            Contract.Requires(start >= 0);
            Contract.Requires(length >= 0);
            Contract.Requires(start + length <= arr.Length);
            Contract.Ensures(Contract.Result<T[]>() != null);
            Contract.Ensures(Contract.Result<T[]>().Length == length);

            var result = new T[length];
            Buffer.BlockCopy(arr, start, result, 0, length);
            return result;
        }

        public static T[] SubArray<T>(T[] arr, int start)
        {
            Contract.Requires(arr != null);
            Contract.Requires(start >= 0);
            Contract.Requires(start <= arr.Length);
            Contract.Ensures(Contract.Result<T[]>() != null);
            Contract.Ensures(Contract.Result<T[]>().Length == arr.Length - start);

            return SubArray(arr, start, arr.Length - start);
        }
    }
}