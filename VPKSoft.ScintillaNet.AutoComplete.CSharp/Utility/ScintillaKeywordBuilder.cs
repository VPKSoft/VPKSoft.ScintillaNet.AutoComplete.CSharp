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
using System.Linq;
using System.Text;
using ScintillaNET;
using VPKSoft.ScintillaNet.AutoComplete.CSharp.Enumerations;

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.Utility
{
    /// <summary>
    /// A class to build auto-complete keyword collections for the <see cref="Scintilla"/> control.
    /// </summary>
    public class ScintillaKeywordBuilder
    {
        /// <summary>
        /// Gets or sets the <see cref="StringComparison"/> comparison used to filter a word list.
        /// </summary>
        /// <value>The <see cref="StringComparison"/> comparison used to filter a word list.</value>
        public static StringComparison Comparison { get; set; } = StringComparison.OrdinalIgnoreCase;

        /// <summary>
        /// Adds a specified list of keywords to the class.
        /// </summary>
        /// <param name="keywords">The keywords to add.</param>
        /// <param name="constructType">Type of the language construct.</param>
        /// <param name="filter">The filter to use for the keywords.</param>
        /// <returns>An instance to this class.</returns>
        public ScintillaKeywordBuilder AddKeyWords(List<string> keywords, LanguageConstructType constructType, string filter)
        {
            if (keywords == null)
            {
                return this;
            }

            foreach (var keyword in keywords)
            {
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    if (keyword.IndexOf(filter, Comparison) == -1)
                    {
                        continue;
                    }
                }
                Keywords.Add(new KeyValuePair<LanguageConstructType, string>(constructType, keyword));
            }

            return this;
        }

        /// <summary>
        /// Removes the keywords with a specified type.
        /// </summary>
        /// <param name="constructType">Type of the language construct.</param>
        /// <returns>An instance to this class.</returns>
        public ScintillaKeywordBuilder RemoveKeywordsWithType(LanguageConstructType constructType)
        {
            Keywords.RemoveWhere(f => f.Key == constructType);
            return this;
        }

        /// <summary>
        /// Return a new empty instance of the <see cref="ScintillaKeywordBuilder"/> class.
        /// </summary>
        /// <returns>A new empty instance of the <see cref="ScintillaKeywordBuilder"/> class.</returns>
        public static ScintillaKeywordBuilder WithNew()
        {
            return new ScintillaKeywordBuilder();
        }
        
        /// <summary>
        /// Fills the <see cref="ScintillaKeywordBuilder"/> with existing formatted word list for the <see cref="Scintilla"/> control.
        /// </summary>
        /// <param name="currentWords">The current words list as a single string representation.</param>
        /// <returns>An instance to this class.</returns>
        public ScintillaKeywordBuilder WithWordList(string currentWords)
        {
            var words = currentWords.Split(' ');
            var scintillaKeywordBuilder = new ScintillaKeywordBuilder();

            List<KeyValuePair<string, LanguageConstructType>> collectedWords =
                new List<KeyValuePair<string, LanguageConstructType>>();
            
            foreach (var word in words)
            {
                var wordParts = word.Split('?');
                collectedWords.Add(new KeyValuePair<string, LanguageConstructType>(wordParts[0],
                    (LanguageConstructType) int.Parse(wordParts[1])));
            }

            var types = collectedWords.Select(f => f.Value).Distinct();

            foreach (var languageConstructType in types)
            {
                scintillaKeywordBuilder = AddKeyWords(
                    collectedWords
                        .Where(f => f.Value == languageConstructType)
                        .Select(f => f.Key).ToList(),
                    languageConstructType);
            }
            
            return scintillaKeywordBuilder;
        }
        
        /// <summary>
        /// Adds a specified list of keywords to the class.
        /// </summary>
        /// <param name="keywords">A space-delimited list of keywords to add.</param>
        /// <param name="constructType">Type of the language construct.</param>
        /// <param name="filter">The filter to use for the keywords.</param>
        /// <returns>An instance to this class.</returns>
        public ScintillaKeywordBuilder AddKeyWords(string keywords, LanguageConstructType constructType, string filter)
        {
            AddKeyWords(new List<string>(keywords.Split(' ')), constructType, filter);
            return this;
        }

        /// <summary>
        /// Adds a specified list of keywords to the class.
        /// </summary>
        /// <param name="keywords">The keywords to add.</param>
        /// <param name="constructType">Type of the language construct.</param>
        /// <returns>An instance to this class.</returns>
        public ScintillaKeywordBuilder AddKeyWords(List<string> keywords, LanguageConstructType constructType)
        {
            AddKeyWords(keywords, constructType, null);
            return this;
        }

        /// <summary>
        /// Adds the specified keyword types from the specified keyword collection to the class.
        /// </summary>
        /// <typeparam name="T">The type of objects in the <see cref="IList{T}"/>.</typeparam>
        /// <param name="libraryEntries">The library entries to filter with the language construct type.</param>
        /// <param name="constructType">Type of the construct.</param>
        /// <param name="filter">The filter to use for the keywords.</param>
        /// <returns>An instance to this class.</returns>
        /// <remarks>The items in the <see cref="IList{T}"/> must be of the same type and they must have public properties called <c>Name</c> and <c>Type</c>.</remarks>
        /// <exception cref="NullReferenceException">
        /// Type
        /// or
        /// Name
        /// </exception>
        public ScintillaKeywordBuilder AddKeywordsFrom<T>(IList<T> libraryEntries,
            LanguageConstructType constructType, string filter)
        {
            if (libraryEntries.Count == 0)
            {
                return this;
            }

            var propertyInfoName = libraryEntries[0].GetType().GetProperty("Name");
            var propertyInfoType = libraryEntries[0].GetType().GetProperty("Type");

            if (propertyInfoType == null)
            {
                throw new NullReferenceException("Type");
            }

            if (propertyInfoName == null)
            {
                throw new NullReferenceException("Name");
            }

            var keywords = 
                // ReSharper disable once PossibleNullReferenceException : The exception with this method is intentional..
                libraryEntries.Where(f => (LanguageConstructType)propertyInfoType.GetValue(f) == constructType)
                    .Select(f => (string)propertyInfoName.GetValue(f)).ToList();

            if (filter != null)
            {
                keywords = keywords.Where(f => f.IndexOf(filter, Comparison) != -1).ToList();
            }

            return AddKeyWords(keywords, constructType);
        }

        /// <summary>
        /// Adds the specified keyword types from the specified keyword collection to the class.
        /// </summary>
        /// <typeparam name="T">The type of objects in the <see cref="IList{T}"/>.</typeparam>
        /// <param name="libraryEntries">The library entries to filter with the language construct type.</param>
        /// <param name="constructType">Type of the construct.</param>
        /// <returns>An instance to this class.</returns>
        /// <remarks>The items in the <see cref="IList{T}"/> must be of the same type and they must have public properties called <c>Name</c> and <c>Type</c>.</remarks>
        /// <exception cref="NullReferenceException">
        /// Type
        /// or
        /// Name
        /// </exception>
        public ScintillaKeywordBuilder AddKeywordsFrom<T>(IList<T> libraryEntries,
            LanguageConstructType constructType)
        {
            return AddKeywordsFrom(libraryEntries, constructType, null);
        }

        /// <summary>
        /// Adds all the keyword types from the specified keyword collection to the class.
        /// </summary>
        /// <typeparam name="T">The type of objects in the <see cref="IList{T}"/>.</typeparam>
        /// <param name="libraryEntries">The library entries to filter with the language construct type.</param>
        /// <param name="filter">The filter to use for the keywords.</param>
        /// <returns>An instance to this class.</returns>
        /// <remarks>The items in the <see cref="IList{T}"/> must be of the same type and they must have public properties called <c>Name</c> and <c>Type</c>.</remarks>
        /// <exception cref="NullReferenceException">
        /// Type
        /// or
        /// Name
        /// </exception>
        public ScintillaKeywordBuilder AddKeywordsFrom<T>(IList<T> libraryEntries, string filter)
        {
            var values = typeof(LanguageConstructType).GetEnumValues();
            foreach (var value in values)
            {
                if (value == null)
                {
                    continue;
                }

                AddKeywordsFrom(libraryEntries, (LanguageConstructType) value, filter);
            }

            return this;
        }

        /// <summary>
        /// Adds all the keyword types from the specified keyword collection to the class.
        /// </summary>
        /// <typeparam name="T">The type of objects in the <see cref="IList{T}"/>.</typeparam>
        /// <param name="libraryEntries">The library entries to filter with the language construct type.</param>
        /// <returns>An instance to this class.</returns>
        /// <remarks>The items in the <see cref="IList{T}"/> must be of the same type and they must have public properties called <c>Name</c> and <c>Type</c>.</remarks>
        /// <exception cref="NullReferenceException">
        /// Type
        /// or
        /// Name
        /// </exception>
        public ScintillaKeywordBuilder AddKeywordsFrom<T>(IList<T> libraryEntries)
        {
            return AddKeywordsFrom(libraryEntries, null);
        }

        /// <summary>
        /// Adds a specified list of keywords to the class.
        /// </summary>
        /// <param name="keywords">A space-delimited list of keywords to add.</param>
        /// <param name="constructType">Type of the language construct.</param>
        /// <returns>An instance to this class.</returns>
        public ScintillaKeywordBuilder AddKeyWords(string keywords, LanguageConstructType constructType)
        {
            AddKeyWords(new List<string>(keywords.Split(' ')), constructType, null);
            return this;
        }

        /// <summary>
        /// Adds a specified keyword to the class.
        /// </summary>
        /// <param name="keyword">The keyword to add.</param>
        /// <param name="constructType">Type of the language construct.</param>
        /// <param name="filter">The filter to use for the keywords.</param>
        /// <returns>An instance to this class.</returns>
        public ScintillaKeywordBuilder AddKeyWord(string keyword, LanguageConstructType constructType, string filter)
        {
            AddKeyWords(new List<string>(new List<string>(new []{keyword})), constructType, filter);
            return this;
        }

        /// <summary>
        /// Adds a specified keyword to the class.
        /// </summary>
        /// <param name="keyword">The keyword to add.</param>
        /// <param name="constructType">Type of the language construct.</param>
        /// <returns>An instance to this class.</returns>
        public ScintillaKeywordBuilder AddKeyWord(string keyword, LanguageConstructType constructType)
        {
            AddKeyWords(new List<string>(new List<string>(new []{keyword})), constructType, null);
            return this;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            // create a StringBuilder for fast appending of the strings..
            var builder = new StringBuilder();
            
            // append the values to StringBuilder instance in Scintilla format..
            foreach (var pair in Keywords)
            {
                builder.Append(pair.Value + "?" + (int) pair.Key + " ");
            }

            // return the result..
            return builder.ToString().TrimEnd(' ');
        }

        /// <summary>
        /// Gets the keywords given to the class instance.
        /// </summary>
        /// <value>The keywords given to the class instance.</value>
        private HashSet<KeyValuePair<LanguageConstructType, string>> Keywords { get; } =
            new HashSet<KeyValuePair<LanguageConstructType, string>>();
    }
}
