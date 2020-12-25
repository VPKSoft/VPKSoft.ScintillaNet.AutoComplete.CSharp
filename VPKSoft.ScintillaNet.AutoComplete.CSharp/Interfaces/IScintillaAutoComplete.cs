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

using System;
using System.Collections.Generic;
using ScintillaNET;
using VPKSoft.ScintillaNet.AutoComplete.CSharp.Enumerations;

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces
{
    /// <summary>
    /// Interface IScintillaAutoComplete
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <typeparam name="TLibraryEntry">The type of the library entry used with the automatic code completion.
    /// The type can implement the <see cref="ILibraryEntry{TInheritClass,TMethodDescriptor,TEnumDescription}"/> interface.</typeparam>
    /// <seealso cref="System.IDisposable" />
    public interface IScintillaAutoComplete<TLibraryEntry>: IDisposable
    {
        #region KeyWords
        /// <summary>
        /// Gets or sets the language keywords.
        /// </summary>
        /// <value>The language keywords.</value>
        string LangKeywords { get; set; }

        /// <summary>
        /// Gets or sets the built-in language type keywords.
        /// </summary>
        /// <value>The built-in language type keywords.</value>
        string LangTypeWords { get; set; }

        /// <summary>
        /// Gets the automatic complete word list of all the word types combined.
        /// </summary>
        /// <value>The automatic complete word list of all the word types combined.</value>
        string AutoCompleteWordList { get; }
        #endregion

        #region AutoComplete
        /// <summary>
        /// Displays the auto-complete complete suggestion menu for the <see cref="Scintilla"/> control.
        /// </summary>
        void AutoCompleteSuggest();
        #endregion

        #region AssemblyAndLibrary                        
        /// <summary>
        /// Gets or sets a value indicating whether this instance should use the static library member.
        /// </summary>
        /// <value><c>true</c> if this instance should use the static library member cache; otherwise, <c>false</c>.</value>
        bool UseStaticLibraryMemberCache { get; set; }

        /// <summary>
        /// Gets the instance library member list in case the <see cref="CacheLibraries"/> is with asStatic parameter set to false.
        /// </summary>
        /// <value>The instance library member list.</value>
        List<TLibraryEntry> InstanceLibraryMemberList { get; }

        /// <summary>
        /// Adds the specified instance to the collection used by the class.
        /// </summary>
        /// <param name="libraryEntry">The library entry to add.</param>
        /// <param name="asStatic">If set to <c>true</c> the entry should be added to a static collection.</param>
        /// <param name="isStatic">if set to <c>true</c> the enumeration value of
        /// <see cref="LanguageModifiers.Static"/> flag should be applied to the <see cref="LanguageModifiers"/> enumeration value.</param>
        /// <returns><c>true</c> if <paramref name="libraryEntry"/> was successfully added to the collection, <c>false</c> otherwise.</returns>
        bool AddLibraryEntry(TLibraryEntry libraryEntry, bool asStatic, bool isStatic);

        /// <summary>
        /// Adds the specified instance to the collection used by the class.
        /// </summary>
        /// <param name="libraryEntry">The library entry to add.</param>
        /// <param name="asStatic">If set to <c>true</c> the entry should be added to a static collection.</param>
        /// <returns><c>true</c> if <paramref name="libraryEntry"/> was successfully added to the collection, <c>false</c> otherwise.</returns>
        bool AddLibraryEntry(TLibraryEntry libraryEntry, bool asStatic);

        /// <summary>
        /// Caches the libraries and their member types used with the code auto-completion.
        /// </summary>
        /// <param name="asStatic">A value indicating whether the cache should be a static cache.</param>
        void CacheLibraries(bool asStatic);
        #endregion

        #region WordHelpers
        /// <summary>
        /// Returns an alphabetically and length ordered list of strings from a specified unordered list of strings.
        /// </summary>
        /// <param name="strings">The string list to sort.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        List<string> OrderStringList(List<string> strings);

        /// <summary>
        /// Creates a list of strings from a space-delimited list of keyword ordering by alphabetically and the by their length.
        /// </summary>
        /// <param name="keywords">The keywords to create a list of string from.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        List<string> SetKeywords(string keywords);

        /// <summary>
        /// Gets the keywords as a string combined with the specified keyword type identifier.
        /// </summary>
        /// <param name="keywords">The keywords as a list of strings.</param>
        /// <param name="wordType">Type of the word.</param>
        /// <param name="filter">A filter to filter the keywords with.</param>
        /// <returns>System.String.</returns>
        string GetKeywords(List<string> keywords, LanguageConstructType wordType, string filter);

        /// <summary>
        /// Gets the keywords as a string combined with the specified keyword type identifier.
        /// </summary>
        /// <param name="keywords">The keywords as a list of strings.</param>
        /// <param name="wordType">Type of the word.</param>
        /// <returns>System.String.</returns>
        string GetKeywords(List<string> keywords, LanguageConstructType wordType);
        #endregion
    }
}
