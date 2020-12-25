#region License
/*
MIT License

Copyright(c) 2020 Petteri Kautonen

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion

using System.Collections.Generic;

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces
{
    /// <summary>
    /// An interface for mapping type names which represent the same language construct with different names.
    /// </summary>
    /// <typeparam name="T1">The first pairing type for the type name in the <see cref="TypeMapping"/> list.</typeparam>
    /// <typeparam name="T2">The second pairing type for the type name in the <see cref="TypeMapping"/> list.</typeparam>
    public interface ILanguageTypeName<T1, T2>
    {
        /// <summary>
        /// Gets the language type name for a specified type <paramref name="type"/>. I.e. <c>System.String</c> --> <c>string</c>.
        /// </summary>
        /// <param name="type">The value describing the common type.</param>
        /// <returns>A value describing the language type name for the <paramref name="type"/>.</returns>
        T2 GetLanguageTypeName(T1 type);

        /// <summary>
        /// Determines whether the language type name has a corresponding common type name mapping.
        /// </summary>
        /// <param name="type">The language type to check for.</param>
        /// <returns><c>true</c> if the language type name has a corresponding common type name mapping; otherwise, <c>false</c>.</returns>
        bool IsTypeMapped(T1 type);

        /// <summary>
        /// Gets the language type name for a specified type. I.e. <c>System.String</c> --> <c>string</c>.
        /// </summary>
        /// <param name="type">The value describing the common type.</param>
        /// <returns>The language type name for requested type.</returns>
        T2 this[T1 type] { get; }

        /// <summary>
        /// Gets the common type name for a specified type <paramref name="type"/>. I.e. <c>string</c> --> <c>System.String</c>.
        /// </summary>
        /// <param name="type">The value describing the language type.</param>
        /// <returns>A value describing the common type name for the <paramref name="type" />.</returns>
        T1 GetCommonTypeName(T2 type);

        /// <summary>
        /// Gets the common type name for a specified type. I.e. <c>string</c> --> <c>System.String</c>.
        /// </summary>
        /// <param name="type">The type to get the common type name for.</param>
        /// <returns>The common type name for requested type.</returns>
        T1 this[T2 type] { get; }

        /// <summary>
        /// Gets or sets the type mapping of type with multiple names.
        /// </summary>
        /// <value>The type mapping of type with multiple names.</value>
        List<KeyValuePair<T1, T2>> TypeMapping { get; set; }

        /// <summary>
        /// Adds the pair of language type name and common type name to the <see cref="TypeMapping"/> collection.
        /// </summary>
        /// <param name="languageTypeName">The language type name for a type. I.e. <c>string</c>.</param>
        /// <param name="commonTypeName">The common type name for a given. I.e. <c>System.String</c>.</param>
        /// <returns><c>true</c> if the type name pair was successfully added to the collection, <c>false</c> otherwise.</returns>
        bool AddPair(T1 languageTypeName, T2 commonTypeName);
    }
}
