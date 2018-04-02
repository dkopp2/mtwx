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
        public static string Protect(
            this string clearText,
            string optionalEntropy = null,
            DataProtectionScope scope = DataProtectionScope.CurrentUser)
        {
            if (clearText == null)
                throw new ArgumentNullException(nameof(clearText));
            byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
            byte[] entropyBytes = string.IsNullOrEmpty(optionalEntropy)
                ? null
                : Encoding.UTF8.GetBytes(optionalEntropy);
            byte[] encryptedBytes = ProtectedData.Protect(clearBytes, entropyBytes, scope);
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Unprotect(
            this string encryptedText,
            string optionalEntropy = null,
            DataProtectionScope scope = DataProtectionScope.CurrentUser)
        {
            if (encryptedText == null)
                throw new ArgumentNullException(nameof(encryptedText));
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] entropyBytes = string.IsNullOrEmpty(optionalEntropy)
                ? null
                : Encoding.UTF8.GetBytes(optionalEntropy);
            byte[] clearBytes = ProtectedData.Unprotect(encryptedBytes, entropyBytes, scope);
            return Encoding.UTF8.GetString(clearBytes);
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