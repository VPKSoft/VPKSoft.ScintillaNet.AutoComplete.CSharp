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
using System.Reflection;
using VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces;

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.Cs
{
    /// <summary>
    /// A class for describing .NET enumerations.
    /// Implements the <see cref="IEnumDescription{TInheritClass}" />
    /// </summary>
    /// <seealso cref="IEnumDescription{TInheritClass}" />
    public class EnumDescriptionCs: IEnumDescription<EnumDescriptionCs>
    {
        /// <summary>
        /// Generates a new instance of the <see cref="EnumDescriptionCs"/> class from a specified enum type.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns>A new instance of the <see cref="EnumDescriptionCs"/> class from a specified enum type.</returns>
        public static EnumDescriptionCs FromEnum(Type enumType)
        {
            var result = new EnumDescriptionCs
            {
                BaseType = enumType.GetEnumUnderlyingType(),
                EnumType = enumType,
                Flags = enumType.GetCustomAttributes<FlagsAttribute>().Any(),
                EnumName = enumType.Name
            };
            var fields = enumType.GetFields();


            foreach (var field in fields) 
            {
                if (field.Name.Equals("value__"))
                {
                    continue;
                }

                result.EnumValuesAndNames.Add(field.Name, field.GetRawConstantValue());
            }

            return result;
        }

        /// <summary>
        /// Gets or sets the underlying type of the enumeration.
        /// </summary>
        /// <value>The the underlying type of the enumeration.</value>
        public Type BaseType { get; set; }

        /// <summary>
        /// Gets or sets the enum values and value names as strings.
        /// </summary>
        /// <value>The enum values and value names as strings.</value>
        public Dictionary<string, object> EnumValuesAndNames { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="P:VPKSoft.ScintillaNETAutoComplete.Interfaces.IEnumDescription`1.Enum" /> described by this instance has a <see cref="P:VPKSoft.ScintillaNETAutoComplete.Interfaces.IEnumDescription`1.Flags" /> flags attribute set.
        /// </summary>
        /// <value><c>true</c> if flags; otherwise, <c>false</c>.</value>
        public bool Flags { get; set; }

        /// <summary>
        /// Gets or sets the name of the <c>enum</c> this instance describes.
        /// </summary>
        /// <value>The name of the enum.</value>
        public string EnumName { get; set; }

        /// <summary>
        /// Gets or sets the type of <c>enum</c> this instance describes.
        /// </summary>
        /// <value>The type of the <c>enum</c> this instance describes.</value>
        public Type EnumType { get; set; }

        /// <summary>
        /// Determines whether the specified <paramref name="other" /> instance is equal to the current instance.
        /// </summary>
        /// <param name="other">The <paramref name="other" /> instance to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified <paramref name="other" /> instance is equal to the current instance, <c>false</c> otherwise.</returns>
        public bool Equals(EnumDescriptionCs other)
        {
            var result = 
                BaseType == other.BaseType && Flags == other.Flags && EnumName == other.EnumName &&
                         EnumType == other.EnumType;

            if (result)
            {
                if (other.EnumValuesAndNames.Count(f => !f.Value.Equals(EnumValuesAndNames[f.Key])) != 0)
                {
                    return false;
                }
            }

            return result;
        }
    }
}
