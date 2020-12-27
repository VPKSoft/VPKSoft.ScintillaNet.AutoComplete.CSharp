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
using VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces;

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.Cs
{
    /// <summary>
    /// Class LanguageTypeNameGenericCs.
    /// Implements the <see cref="ILanguageTypeName{T1,T2}" />
    /// </summary>
    /// <seealso cref="ILanguageTypeName{T1,T2}" />
    public class LanguageTypeNameGenericCs : ILanguageTypeName<Type, string>
    {
        /// <summary>
        /// Gets the language type name for a specified type <paramref name="type" />. I.e. <c>System.String</c> --&gt; <c>string</c>.
        /// </summary>
        /// <param name="type">The value describing the common type.</param>
        /// <returns>A value describing the language type name for the <paramref name="type" />.</returns>
        public string GetLanguageTypeName(Type type)
        {
            if (type == null)
            {
                return string.Empty;
            }

            var result = TypeMapping.FirstOrDefault(f => f.Key == type);
            if (result.Equals(default(KeyValuePair<Type, string>)))
            {
                if (type.IsGenericType)
                {
                    var genericArguments = type.GenericTypeArguments.AsEnumerable().Select(GetLanguageTypeName);
                    return type.BaseType?.Name + "<" + string.Join(", ", genericArguments) + ">";
                }

                if (type.IsArray && IsTypeMapped(type.GetElementType()))
                {
                    var arrayType = type;
                    var arrayTypeString = this[type.GetElementType()];

                    while (arrayType != null && arrayType.HasElementType)
                    {
                        var rank = arrayType.GetArrayRank();
                        arrayType = arrayType.GetElementType();

                        arrayTypeString += "[";
                        for (int i = 0; i < rank - 1; i++)
                        {
                            arrayTypeString += ",";
                        }
                        arrayTypeString += "]";
                    }

                    return arrayTypeString;

                    //var elementType = type.GetElementType(); // the array consists of type of..
                    //var rank = type.GetArrayRank(); // the depth of the array is..
                    
                }

                return type.Name;
            }

            return result.Value;
        }

        /// <summary>
        /// Determines whether the language type name has a corresponding common type name mapping.
        /// </summary>
        /// <param name="type">The language type to check for.</param>
        /// <returns><c>true</c> if the language type name has a corresponding common type name mapping; otherwise, <c>false</c>.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsTypeMapped(Type type)
        {
            if (type == null)
            {
                return false;
            }

            return TypeMapping.Exists(f => f.Key == type);
        }

        /// <summary>
        /// Gets the language type name for a specified type. I.e. <c>System.String</c> --> <c>string</c>.
        /// </summary>
        /// <param name="type">The value describing the common type.</param>
        /// <returns>The language type name for requested type.</returns>
        public string this[Type type] => GetLanguageTypeName(type);

        /// <summary>
        /// Gets the common type name for a specified type <paramref name="type" />. I.e. <c>string</c> --&gt; <c>System.String</c>.
        /// </summary>
        /// <param name="type">The value describing the language type.</param>
        /// <returns>A value describing the common type name for the <paramref name="type" />.</returns>
        public Type GetCommonTypeName(string type)
        {
            var result = TypeMapping.FirstOrDefault(f => f.Value == type);
            if (result.Equals(default(KeyValuePair<Type, string>)))
            {
                return Type.GetType(type);
            }

            return result.Key;
        }

        /// <summary>
        /// Gets the common type name for a specified type. I.e. <c>string</c> --> <c>System.String</c>.
        /// </summary>
        /// <param name="type">The type to get the common type name for.</param>
        /// <returns>The common type name for requested type.</returns>
        public Type this[string type] => GetCommonTypeName(type);

        /// <summary>
        /// Gets or sets the type mapping of type with multiple names.
        /// </summary>
        /// <value>The type mapping of type with multiple names.</value>
        public List<KeyValuePair<Type, string>> TypeMapping { get; set; } = new List<KeyValuePair<Type, string>>(new[]
        {
            new KeyValuePair<Type, string>(typeof(bool), "bool"),
            new KeyValuePair<Type, string>(typeof(byte), "byte"),
            new KeyValuePair<Type, string>(typeof(sbyte), "sbyte"),
            new KeyValuePair<Type, string>(typeof(char), "char"),
            new KeyValuePair<Type, string>(typeof(decimal), "decimal"),
            new KeyValuePair<Type, string>(typeof(double), "double"),
            new KeyValuePair<Type, string>(typeof(float), "float"),
            new KeyValuePair<Type, string>(typeof(int), "int"),
            new KeyValuePair<Type, string>(typeof(uint), "uint"),
            new KeyValuePair<Type, string>(typeof(long), "long"),
            new KeyValuePair<Type, string>(typeof(ulong), "ulong"),
            new KeyValuePair<Type, string>(typeof(short), "short"),
            new KeyValuePair<Type, string>(typeof(ushort), "ushort"),
            new KeyValuePair<Type, string>(typeof(object), "object"),
            new KeyValuePair<Type, string>(typeof(string), "string"),
            new KeyValuePair<Type, string>(typeof(void), "void"),
        });

        /// <summary>
        /// Adds the pair of language type name and common type name to the <see cref="P:VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces.ILanguageTypeName`2.TypeMapping" /> collection.
        /// </summary>
        /// <param name="languageTypeName">The language type name for a type. I.e. <c>string</c>.</param>
        /// <param name="commonTypeName">The common type name for a given. I.e. <c>System.String</c>.</param>
        /// <returns><c>true</c> if the type name pair was successfully added to the collection, <c>false</c> otherwise.</returns>
        public bool AddPair(Type languageTypeName, string commonTypeName)
        {
            if (TypeMapping.All(f => f.Key != languageTypeName))
            {
                TypeMapping.Add(new KeyValuePair<Type, string>(languageTypeName, commonTypeName));
                return true;
            }

            return false;
        }
    }
}
