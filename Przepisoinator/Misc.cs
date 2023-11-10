using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Przepisoinator
{
    internal class Misc
    {
        static SHA256 hasher;
        public static JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true
        };
        static Misc()
        {
            hasher = SHA256.Create();
        }

        public static ulong HashString(string text)
        {
            return GetUInt64Hash(hasher, text);
        }

        public static ulong GetUInt64Hash(HashAlgorithm hasher, string text)
        {
            using (hasher)
            {
                var bytes = hasher.ComputeHash(Encoding.Default.GetBytes(text));
                Array.Resize(ref bytes, bytes.Length + bytes.Length % 8); //make multiple of 8 if hash is not, for exampel SHA1 creates 20 bytes. 
                return Enumerable.Range(0, bytes.Length / 8) // create a counter for de number of 8 bytes in the bytearray
                    .Select(i => BitConverter.ToUInt64(bytes, i * 8)) // combine 8 bytes at a time into a integer
                    .Aggregate((x, y) => x ^ y); //xor the bytes together so you end up with a ulong (64-bit int)
            }
        }
    }
}
