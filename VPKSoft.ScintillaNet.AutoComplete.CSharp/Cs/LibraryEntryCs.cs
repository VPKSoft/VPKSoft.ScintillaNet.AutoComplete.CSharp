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
using VPKSoft.ScintillaNet.AutoComplete.CSharp.Enumerations;
using VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces;

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.Cs
{
    /// <summary>
    /// A class for the C# programming language assembly/library entry.
    /// </summary>
    public class LibraryEntryCs : ILibraryEntry<LibraryEntryCs, MethodDescriptionCs, EnumDescriptionCs>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryEntryCs"/> class.
        /// </summary>
        public LibraryEntryCs()
        {
            // an empty constructor to avoid excess overloading..
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryEntryCs"/> class.
        /// </summary>
        /// <param name="name">The name of the type in the assembly/library</param>
        /// <param name="nameSpaceName">Name of the name of the namespace of the assembly/library.</param>
        public LibraryEntryCs(string name, string nameSpaceName) : this(name,
            nameSpaceName, null, default, default)
        {
            Name = name;
            NameSpaceName = nameSpaceName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryEntryCs"/> class.
        /// </summary>
        /// <param name="name">The name of the type in the assembly/library</param>
        /// <param name="fileName">Name of the name of the file of the assembly/library.</param>
        /// <param name="nameSpaceName">Name of the name of the namespace of the assembly/library.</param>
        public LibraryEntryCs(string name, string nameSpaceName, string fileName) : this(
            name, nameSpaceName, fileName, default, default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryEntryCs"/> class.
        /// </summary>
        /// <param name="name">The name of the type in the assembly/library</param>
        /// <param name="nameSpaceName">Name of the name of the namespace of the assembly/library.</param>
        /// <param name="type">The type in question in the assembly/library.</param>
        public LibraryEntryCs(string name, string nameSpaceName, LanguageConstructType type) : this(name, nameSpaceName,
            null, type, default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryEntryCs"/> class.
        /// </summary>
        /// <param name="name">The name of the type in the assembly/library</param>
        /// <param name="fileName">Name of the name of the file of the assembly/library.</param>
        /// <param name="nameSpaceName">Name of the name of the namespace of the assembly/library.</param>
        /// <param name="type">The type in question in the assembly/library.</param>
        public LibraryEntryCs(string name, string nameSpaceName, string fileName, LanguageConstructType type) : this(
            name, nameSpaceName, fileName, type, default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryEntryCs"/> class.
        /// </summary>
        /// <param name="name">The name of the type in the assembly/library</param>
        /// <param name="nameSpaceName">Name of the name of the namespace of the assembly/library.</param>
        /// <param name="modifiers">The modifiers for the language construct.</param>
        public LibraryEntryCs(string name, string nameSpaceName, LanguageModifiers modifiers) : this(name,
            nameSpaceName, null, default, modifiers)
        {
            Name = name;
            NameSpaceName = nameSpaceName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryEntryCs"/> class.
        /// </summary>
        /// <param name="name">The name of the type in the assembly/library</param>
        /// <param name="fileName">Name of the name of the file of the assembly/library.</param>
        /// <param name="nameSpaceName">Name of the name of the namespace of the assembly/library.</param>
        /// <param name="modifiers">The modifiers for the language construct.</param>
        public LibraryEntryCs(string name, string nameSpaceName, string fileName, LanguageModifiers modifiers) : this(
            name, nameSpaceName, fileName, default, modifiers)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryEntryCs"/> class.
        /// </summary>
        /// <param name="name">The name of the type in the assembly/library</param>
        /// <param name="nameSpaceName">Name of the name of the namespace of the assembly/library.</param>
        /// <param name="type">The type in question in the assembly/library.</param>
        /// <param name="modifiers">The modifiers for the language construct.</param>
        public LibraryEntryCs(string name, string nameSpaceName, LanguageConstructType type,
            LanguageModifiers modifiers) : this(name, nameSpaceName, null, type, modifiers)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryEntryCs"/> class.
        /// </summary>
        /// <param name="name">The name of the type in the assembly/library</param>
        /// <param name="fileName">Name of the name of the file of the assembly/library.</param>
        /// <param name="nameSpaceName">Name of the name of the namespace of the assembly/library.</param>
        /// <param name="type">The type in question in the assembly/library.</param>
        /// <param name="modifiers">The modifiers for the language construct.</param>
        public LibraryEntryCs(string name, string nameSpaceName, string fileName, LanguageConstructType type, LanguageModifiers modifiers)
        {
            Name = name;
            FileName = fileName;
            NameSpaceName = nameSpaceName;
            Type = type;
            Modifiers = modifiers;
        }
        #endregion

        /// <summary>
        /// Gets or sets the name of the type in the assembly/library.
        /// </summary>
        /// <value>The name of the type in the assembly/library.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the file of the assembly/library.
        /// </summary>
        /// <value>The the name of the file of the assembly/library.</value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the name of the namespace of the assembly/library.
        /// </summary>
        /// <value>The name of the namespace.</value>
        public string NameSpaceName { get; set; }

        /// <summary>
        /// Adds a new field to <see cref="P:VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces.ILibraryEntry`3.Fields" /> collection of this instance.
        /// </summary>
        /// <param name="field">The field to add to the <see cref="P:VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces.ILibraryEntry`3.Fields" /> collection.</param>
        /// <returns>An instance to this class.</returns>
        public LibraryEntryCs AddField(LibraryEntryCs field)
        {
            if (!Fields.Any(f => f.Equals(field)))
            {
                Fields.Add(field);
            }

            return this;
        }

        /// <summary>
        /// Adds a new property to <see cref="P:VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces.ILibraryEntry`3.Properties" /> collection of this instance.
        /// </summary>
        /// <param name="property">The property to add to the <see cref="P:VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces.ILibraryEntry`3.Properties" /> collection.</param>
        /// <returns>An instance to this class.</returns>
        public LibraryEntryCs AddProperty(LibraryEntryCs property)
        {
            if (!Properties.Any(f => f.Equals(property)))
            {
                Properties.Add(property);
            }

            return this;
        }

        /// <summary>
        /// Adds a new method to <see cref="P:VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces.ILibraryEntry`3.Methods" /> collection of this instance.
        /// </summary>
        /// <param name="method">The method to add to the <see cref="P:VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces.ILibraryEntry`3.Methods" /> collection.</param>
        /// <returns>An instance to this class.</returns>
        public LibraryEntryCs AddMethod(MethodDescriptionCs method)
        {
            if (!Methods.Any(f => f.Equals(method)))
            {
                Methods.Add(method);
            }

            return this;
        }

        private static LanguageTypeNameGenericCs LanguageTypeName { get; } = new LanguageTypeNameGenericCs();

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public string ToString(out IList<HighLightPositionCs> highlightPositions, out string returnType)
        {
            highlightPositions = new List<HighLightPositionCs>();

            var name = Name.Split('.').Last() ?? Name;

            highlightPositions.Add(new HighLightPositionCs
                {HighlightType = HighLightStyleCs.BodyName, Start = 0, Length = name.Length});
            
/*            highlightPositions.Add(new HighLightPositionCs
                {HighlightType = HighLightStyleCs.ReturnValueType, Start = 0, Length = ReturnType.Name.Length});*/

            returnType = LanguageTypeName[ReturnType];
            return name;
        }

        /// <summary>
        /// Gets or sets the enumeration describing the type in question in the assembly/library.
        /// </summary>
        /// <value>The type in question in the assembly/library.</value>
        public LanguageConstructType Type { get; set; }

        /// <summary>
        /// Gets or sets the type of the construct.
        /// </summary>
        /// <value>The type of the construct.</value>
        public Type ConstructType { get; set; }

        /// <summary>
        /// Gets or sets the modifiers for the language construct.
        /// </summary>
        /// <value>The modifiers for the language construct.</value>
        public LanguageModifiers Modifiers { get; set; }

        /// <summary>
        /// Gets the field list containing the member fields in case this instance describes a class, a struct or another type with members.
        /// </summary>
        /// <value>The field list containing the member fields in case this instance describes a class, a struct or another type with members.</value>
        public List<LibraryEntryCs> Fields { get; } = new List<LibraryEntryCs>();

        /// <summary>
        /// Gets the property list containing the member properties in case this instance describes a class, a struct or another type with members.
        /// </summary>
        /// <value>The field list containing the member properties in case this instance describes a a class, a struct or another type with members.</value>
        public List<LibraryEntryCs> Properties { get; } = new List<LibraryEntryCs>();

        /// <summary>
        /// Gets or sets the methods in case this instance describes a class, a struct or another type with members.
        /// </summary>
        /// <value>The methods in case this instance describes a class, a struct or another type with members.</value>
        public List<MethodDescriptionCs> Methods { get; } = new List<MethodDescriptionCs>();

        /// <summary>
        /// Gets or sets the enum description instance.
        /// </summary>
        /// <value>The enum description instance.</value>
        public EnumDescriptionCs EnumDescription { get; set; }

        /// <summary>
        /// Gets or sets the return type of a property, method or a function.
        /// </summary>
        /// <value>The return type of a property, method or a function.</value>
        public Type ReturnType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the value described by this instance can be read.
        /// </summary>
        /// <value><c>true</c> if the value indicating whether the value described by this instance can be read; otherwise, <c>false</c>.</value>
        public bool CanRead { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the value described by this instance can be set.
        /// </summary>
        /// <value><c>true</c> if the value indicating whether the value described by this instance can be set; otherwise, <c>false</c>.</value>
        public bool CanWrite { get; set; } = true;

        /// <summary>
        /// Determines whether the specified <paramref name="other"/> instance is equal to the current instance.
        /// </summary>
        /// <param name="other">The <paramref name="other" /> instance to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified <paramref name="other" /> instance is equal to the current instance, <c>false</c> otherwise.</returns>
        public bool Equals(LibraryEntryCs other)
        {
            var result = Name == other.Name && FileName == other.FileName && NameSpaceName == other.NameSpaceName && Type == other.Type;

            if (result)
            {
                if (Fields?.Count == other.Fields?.Count && Fields?.Count != null)
                {
                    for (int i = 0; i < Fields.Count; i++)
                    {
                        result &= Fields[i].Equals(other.Fields[i]);
                        if (!result)
                        {
                            return false;
                        }
                    }
                }
                else if (Fields?.Count != null)
                {
                    result = false;
                }
            }

            if (result)
            {
                if (Properties?.Count == other.Properties?.Count && Properties?.Count != null)
                {
                    for (int i = 0; i < Properties.Count; i++)
                    {
                        result &= Properties[i].Equals(other.Properties[i]);
                        if (!result)
                        {
                            return false;
                        }
                    }
                }
                else if (Properties?.Count != null)
                {
                    result = false;
                }
            }

            if (result)
            {
                if (Methods?.Count == other.Methods?.Count && Methods?.Count != null)
                {
                    for (int i = 0; i < Methods.Count; i++)
                    {
                        result &= Methods[i].Equals(other.Methods[i]);
                        if (!result)
                        {
                            return false;
                        }
                    }
                }
                else if (Properties?.Count != null)
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
