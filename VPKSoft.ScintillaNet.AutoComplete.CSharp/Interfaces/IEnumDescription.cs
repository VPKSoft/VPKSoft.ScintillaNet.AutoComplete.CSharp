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

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces
{
    /// <summary>
    /// An interface describing an enumeration value.
    /// </summary>
    public interface IEnumDescription<in TInheritClass>
    {
        /// <summary>
        /// Gets or sets the underlying type of the enumeration.
        /// </summary>
        /// <value>The the underlying type of the enumeration.</value>
        Type BaseType { get; set; }

        /// <summary>
        /// Gets or sets the enum values and value names as strings.
        /// </summary>
        /// <value>The enum values and value names as strings.</value>
        Dictionary<string, object> EnumValuesAndNames { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Enum"/> described by this instance has a <see cref="Flags"/> flags attribute set.
        /// </summary>
        /// <value><c>true</c> if flags; otherwise, <c>false</c>.</value>
        bool Flags { get; set; }

        /// <summary>
        /// Gets or sets the name of the <c>enum</c> this instance describes.
        /// </summary>
        string EnumName { get; set; }

        /// <summary>
        /// Gets or sets the type of <c>enum</c> this instance describes.
        /// </summary>
        /// <value>The type of the <c>enum</c> this instance describes.</value>
        Type EnumType { get; set; }

        /// <summary>
        /// Determines whether the specified <typeparamref name="TInheritClass"/> instance is equal to the current instance.
        /// </summary>
        /// <param name="other">The <typeparamref name="TInheritClass"/> instance to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified <typeparamref name="TInheritClass"/> instance is equal to the current instance, <c>false</c> otherwise.</returns>
        bool Equals(TInheritClass other);
    }
}
