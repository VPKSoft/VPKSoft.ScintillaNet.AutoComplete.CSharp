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
    /// An interface to describe a single method or a function.
    /// </summary>
    /// <typeparam name="TArgumentDescription">The type of the class describing an argument for a method or a function of a programming language.</typeparam>
    /// <typeparam name="TInheritClass">The type of the class implementing this instance.</typeparam>
    /// <typeparam name="TMethodReturnType">The type used to describe the return value of a single method or a function.</typeparam>
    public interface IMethodDescription<TArgumentDescription, TInheritClass, TMethodReturnType>
    {
        /// <summary>
        /// Gets or sets the name of the method or a function.
        /// </summary>
        /// <value>The name of the method or a function.</value>
        string MethodName { get; set; }

        /// <summary>
        /// Gets the arguments of the method. This is a <see cref="Dictionary{TKey,TArgumentDescription}"/> containing the argument name and the argument type as an instance of the <typeparamref name="TArgumentDescription"/>.
        /// </summary>
        /// <value>The arguments of a method.</value>
        IDictionary<string, TArgumentDescription> Arguments { get; }

        /// <summary>
        /// Adds the argument to the <see cref="Arguments"/> property.
        /// </summary>
        /// <param name="argumentName">Name of the argument.</param>
        /// <param name="argumentDescription">The argument description instance of type of <typeparamref name="TArgumentDescription"/>.</param>
        /// <returns>An instance to this class.</returns>
        TInheritClass AddArgument(string argumentName,
            TArgumentDescription argumentDescription);

        /// <summary>
        /// Gets or sets a value indicating the method is a constructor.
        /// </summary>
        /// <value><c>true</c> if the method is a constructor; otherwise, <c>false</c>.</value>
        bool IsConstructor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the method is a static method.
        /// </summary>
        /// <value><c>true</c> if the method is static; otherwise, <c>false</c>.</value>
        bool IsStatic { get; set; } 

        /// <summary>
        /// Gets or sets a value indicating whether the method is a private method.
        /// </summary>
        /// <value><c>true</c> if the method is private; otherwise, <c>false</c>.</value>
        bool IsPrivate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a method or a function is usable. I.e. in .NET the assembly is loaded for the method.
        /// </summary>
        /// <value><c>true</c> if the method or the function is usable; otherwise, <c>false</c>.</value>
        bool IsUsable { get; set; }

        /// <summary>
        /// Gets or sets the return type of a method or a function.
        /// </summary>
        /// <value>The return type of a method or a function.</value>
        TMethodReturnType ReturnType { get; set; }

        /// <summary>
        /// Determines whether the specified <typeparamref name="TInheritClass"/> instance is equal to the current instance.
        /// </summary>
        /// <param name="other">The <typeparamref name="TInheritClass"/> instance to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified <typeparamref name="TInheritClass"/> instance is equal to the current instance, <c>false</c> otherwise.</returns>
        bool Equals(TInheritClass other);

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        string ToString();
    }
}
