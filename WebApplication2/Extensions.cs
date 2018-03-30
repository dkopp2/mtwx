using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace Mtwx.Web
{
    public static class StringExtensions
    {
        internal static string EncryptString(this string data)
        {
            // Data to protect. Convert a string to a byte[] using Encoding.UTF8.GetBytes().
            var plaintext = Encoding.UTF8.GetBytes(data);

            // Generate additional entropy (will be used as the Initialization vector)
            var entropy = new byte[20];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(entropy);
            }

            var ciphertext = ProtectedData.Protect(plaintext, entropy,
                DataProtectionScope.CurrentUser);

            return Encoding.UTF8.GetString(ciphertext);
        }
    }
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Adds the specified element at the end of the IEnummerable.
        /// </summary>
        /// <typeparam name="T">The type of elements the IEnumerable contans.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="item">The item to be concatenated.</param>
        /// <returns>An IEnumerable, enumerating first the items in the existing enumerable</returns>
        public static IEnumerable<T> ConcatItem<T>(this IEnumerable<T> target, T item)
        {
            if (null == target) throw new ArgumentException(nameof(target));
            foreach (T t in target) yield return t;
            yield return item;
        }

        /// <summary>
        /// Inserts the specified element at the start of the IEnumerable.
        /// </summary>
        /// <typeparam name="T">The type of elements the IEnumerable contans.</typeparam>
        /// <param name="target">The IEnummerable.</param>
        /// <param name="item">The item to be concatenated.</param>
        /// <returns>An IEnumerable, enumerating first the target elements, and then the new element.</returns>
        public static IEnumerable<T> ConcatTo<T>(this IEnumerable<T> target, T item)
        {
            if (null == target) throw new ArgumentException(nameof(target));
            yield return item;
            foreach (var t in target) yield return t;
        }
    }
}