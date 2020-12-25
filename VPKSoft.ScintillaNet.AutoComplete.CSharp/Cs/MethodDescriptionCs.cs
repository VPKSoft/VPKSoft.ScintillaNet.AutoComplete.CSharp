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
using ScintillaNET;
using VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces;

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.Cs
{
    /// <summary>
    /// A class to describe a method in the C# programming language.
    /// </summary>
    public class MethodDescriptionCs: IMethodDescription<ParameterInfo, MethodDescriptionCs, Type>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodDescriptionCs"/> class.
        /// </summary>
        public MethodDescriptionCs()
        {
            // the empty constructor..
        }

        private static LanguageTypeNameGenericCs LanguageTypeName { get; } = new LanguageTypeNameGenericCs();

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodDescriptionCs"/> class.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="returnType">The return type of a method or a function.</param>
        /// <param name="arguments">The arguments.</param>
        public MethodDescriptionCs(string methodName, Type returnType, params KeyValuePair<string, ParameterInfo>[] arguments)
        {
            MethodName = methodName;
            ReturnType = returnType;
            foreach (var pair in arguments)
            {
                Arguments.Add(pair);
            }
        }

        /// <summary>
        /// Gets or sets the name of the method or a function.
        /// </summary>
        /// <value>The name of the method or a function.</value>
        public string MethodName { get; set; }

        /// <summary>
        /// Gets the arguments of the method. This is a <see cref="Dictionary{TKey,TValue}"/> containing the argument name and the argument type as an object.
        /// </summary>
        /// <value>The arguments.</value>
        public IDictionary<string, ParameterInfo> Arguments { get; } = new Dictionary<string, ParameterInfo>();

        /// <summary>
        /// Adds the argument to the <see cref="Arguments" /> property.
        /// </summary>
        /// <param name="argumentName">Name of the argument.</param>
        /// <param name="argumentDescription">The argument description instance of type of <see cref="MethodDescriptionCs"/>.</param>
        /// <returns>An instance to this class.</returns>
        public MethodDescriptionCs AddArgument(string argumentName, ParameterInfo argumentDescription)
        {
            Arguments.Add(argumentName, argumentDescription);
            return this;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the method is a constructor.
        /// </summary>
        /// <value><c>true</c> if the method is a constructor; otherwise, <c>false</c>.</value>
        public bool IsConstructor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the method is a static method.
        /// </summary>
        /// <value><c>true</c> if the method is static; otherwise, <c>false</c>.</value>
        public bool IsStatic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the method is a private method.
        /// </summary>
        /// <value><c>true</c> if the method is private; otherwise, <c>false</c>.</value>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a method or a function is usable. I.e. in .NET the assembly is loaded for the method.
        /// </summary>
        /// <value><c>true</c> if the method or the function is usable; otherwise, <c>false</c>.</value>
        public bool IsUsable { get; set; }

        /// <summary>
        /// Gets or sets the return type of a method or a function.
        /// </summary>
        /// <value>The return type of a method or a function.</value>
        public Type ReturnType { get; set; }

        /// <summary>
        /// Determines whether the specified <paramref name="other" /> instance is equal to the current instance.
        /// </summary>
        /// <param name="other">The <paramref name="other" /> instance to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified <paramref name="other" /> instance is equal to the current instance, <c>false</c> otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Equals(MethodDescriptionCs other)
        {
            var result = MethodName == other.MethodName && IsConstructor == other.IsConstructor &&
                         IsPrivate == other.IsPrivate && IsStatic == other.IsStatic;

            if (result)
            {
                if (Arguments.Count == other.Arguments.Count)
                {
                    if (other.Arguments.Count(f => Arguments.ContainsKey(f.Key) && !f.Value.Equals(Arguments[f.Key])) != 0)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="returnTypeToBody">A value indicating whether the return value should be included within the resulting body text.</param>
        /// <param name="highlightPositions">A list of positions to highlight in the <see cref="Scintilla"/> call tips.</param>
        /// <param name="returnType">The return type or a type of a member.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public string ToString(bool returnTypeToBody, out IList<HighLightPositionCs> highlightPositions, out string returnType)
        {
            highlightPositions = new List<HighLightPositionCs>();

            returnType = LanguageTypeName[ReturnType];

            var result = returnTypeToBody ? string.Join(" ", returnType, MethodName) : MethodName;

            highlightPositions.Add(new HighLightPositionCs
                {HighlightType = HighLightStyleCs.BodyName, Start = result.Length - MethodName.Length, Length = MethodName.Length});

            result += "(";
            highlightPositions.Add(new HighLightPositionCs
                {HighlightType = HighLightStyleCs.OpeningBracket, Start = result.Length - 1, Length = 1});
            foreach (var argument in Arguments)
            {
                var parameterTypeDescription = LanguageTypeName[argument.Value.ParameterType];
                highlightPositions.Add(new HighLightPositionCs
                {
                    HighlightType = HighLightStyleCs.Type, Start = result.Length,
                    Length = parameterTypeDescription.Length
                });

                highlightPositions.Add(new HighLightPositionCs
                {
                    HighlightType = HighLightStyleCs.ArgumentName, Start = result.Length + parameterTypeDescription.Length + 1,
                    Length = argument.Key.Length
                });

                result += parameterTypeDescription + " " + argument.Key + ", ";
            }

            result = result.TrimEnd(' ', ',');

            result += ")";
            highlightPositions.Add(new HighLightPositionCs
                {HighlightType = HighLightStyleCs.ClosingBracket, Start = result.Length - 1, Length = 1});

            if (returnTypeToBody)
            {
                highlightPositions.Add(new HighLightPositionCs
                    {HighlightType = HighLightStyleCs.ReturnValueType, Start = 0, Length = returnType.Length});
            }

            return result;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return ToString(true, out _, out _);
        }
    }
}
