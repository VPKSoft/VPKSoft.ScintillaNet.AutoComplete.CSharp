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
using VPKSoft.ScintillaNet.AutoComplete.CSharp.Enumerations;

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces
{

    /// <summary>
    /// An interface for a assembly/library entry skeleton.
    /// </summary>
    /// <typeparam name="TInheritClass">The type of the class implementing this instance.</typeparam>
    /// <typeparam name="TMethodDescriptor">The type of the method descriptor class.
    /// The type can implement the <see cref="IMethodDescription{TArgumentDescription,TInheritClass,TMethodReturnType}"/> interface.</typeparam> 
    /// <typeparam name="TEnumDescription">The type of the enum description class.
    /// The type can implement the <see cref="IEnumDescription{TInheritClass}"/> interface.</typeparam> 
    public interface ILibraryEntry<TInheritClass, TMethodDescriptor, TEnumDescription>
    {
        /// <summary>
        /// Gets or sets the name of the type in the assembly/library.
        /// </summary>
        /// <value>The name of the type in the assembly/library.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the file of the assembly/library.
        /// </summary>
        /// <value>The the name of the file of the assembly/library.</value>
        string FileName { get; set; }

        /// <summary>
        /// Gets or sets the name of the namespace of the assembly/library.
        /// </summary>
        /// <value>The name of the namespace.</value>
        string NameSpaceName { get; set; }

        #region AddMethods        
        /// <summary>
        /// Adds a new field to <see cref="Fields"/> collection of this instance.
        /// </summary>
        /// <param name="field">The field to add to the <see cref="Fields"/> collection.</param>
        /// <returns>An instance to this class.</returns>
        TInheritClass AddField(TInheritClass field);

        /// <summary>
        /// Adds a new property to <see cref="Properties"/> collection of this instance.
        /// </summary>
        /// <param name="property">The property to add to the <see cref="Properties"/> collection.</param>
        /// <returns>An instance to this class.</returns>
        TInheritClass AddProperty(TInheritClass property);

        /// <summary>
        /// Adds a new method to <see cref="Methods"/> collection of this instance.
        /// </summary>
        /// <param name="method">The method to add to the <see cref="Methods"/> collection.</param>
        /// <returns>An instance to this class.</returns>
        TInheritClass AddMethod(TMethodDescriptor method);
        #endregion

        /// <summary>
        /// Gets or sets the enumeration describing the type in question in the assembly/library.
        /// </summary>
        /// <value>The type in question in the assembly/library.</value>
        LanguageConstructType Type { get; set; }

        /// <summary>
        /// Gets or sets the modifiers for the language construct.
        /// </summary>
        /// <value>The modifiers for the language construct.</value>
        LanguageModifiers Modifiers { get; set; }

        /// <summary>
        /// Gets the field list containing the member fields in case this instance describes a class, a struct or another type with members.
        /// </summary>
        /// <value>The field list containing the member fields in case this instance describes a class, a struct or another type with members.</value>
        List<TInheritClass> Fields { get; }

        /// <summary>
        /// Gets the property list containing the member properties in case this instance describes a class, a struct or another type with members.
        /// </summary>
        /// <value>The field list containing the member properties in case this instance describes a a class, a struct or another type with members.</value>
        List<TInheritClass> Properties { get; }

        /// <summary>
        /// Gets or sets the methods in case this instance describes a class, a struct or another type with members.
        /// </summary>
        /// <value>The methods in case this instance describes a class, a struct or another type with members.</value>
        List<TMethodDescriptor> Methods { get; }

        /// <summary>
        /// Gets or sets the enum description instance.
        /// </summary>
        /// <value>The enum description instance.</value>
        TEnumDescription EnumDescription { get; set; }

        /// <summary>
        /// Gets or sets the return type of a property, method or a function.
        /// </summary>
        /// <value>The return type of a property, method or a function.</value>
        Type ReturnType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the value described by this instance can be read.
        /// </summary>
        /// <value><c>true</c> if the value indicating whether the value described by this instance can be read; otherwise, <c>false</c>.</value>
        bool CanRead { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the value described by this instance can be set.
        /// </summary>
        /// <value><c>true</c> if the value indicating whether the value described by this instance can be set; otherwise, <c>false</c>.</value>
        bool CanWrite { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        string ToString();

        /// <summary>
        /// Determines whether the specified <typeparamref name="TInheritClass"/> instance is equal to the current instance.
        /// </summary>
        /// <param name="other">The <typeparamref name="TInheritClass"/> instance to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified <typeparamref name="TInheritClass"/> instance is equal to the current instance, <c>false</c> otherwise.</returns>
        bool Equals(TInheritClass other);
    }
}
