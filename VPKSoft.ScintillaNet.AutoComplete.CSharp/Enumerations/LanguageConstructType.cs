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

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.Enumerations
{
    /// <summary>
    /// An enumeration to describe a language construct (i.e. a class or static field).
    /// </summary>
    public enum LanguageConstructType
    {
        /// <summary>
        /// The undefined type.
        /// </summary>
        Undefined,

        /// <summary>
        /// The keyword type.
        /// </summary>
        Keyword,

        /// <summary>
        /// The built-in programming language type.
        /// /// </summary>
        BuildInType,

        /// <summary>
        /// The class type.
        /// </summary>
        Class,

        /// <summary>
        /// The class type.
        /// </summary>
        StaticClass,

        /// <summary>
        /// The property type.
        /// </summary>
        Property,

        /// <summary>
        /// The method type.
        /// </summary>
        Method,

        /// <summary>
        /// The field type.
        /// </summary>
        Field,

        /// <summary>
        /// The variable type.
        /// </summary>
        Variable,

        /// <summary>
        /// The local variable type.
        /// </summary>
        LocalVariable,

        /// <summary>
        /// The struct type.
        /// </summary>
        Struct,

        /// <summary>
        /// The tuple type.
        /// </summary>
        Tuple,

        /// <summary>
        /// The enum type.
        /// </summary>
        Enum,

        /// <summary>
        /// The interface type.
        /// </summary>
        Interface,

        /// <summary>
        /// The constructor type.
        /// </summary>
        Constructor,

        /// <summary>
        /// The type parameter.
        /// </summary>
        TypeParameter,

        /// <summary>
        /// A constant value.
        /// </summary>
        Constant,

        /// <summary>
        /// The event language construct type.
        /// </summary>
        Event,

        /// <summary>
        /// The operator.
        /// </summary>
        Operator,

        /// <summary>
        /// The module.
        /// </summary>
        Module,

        /// <summary>
        /// The attribute.
        /// </summary>
        Attribute,

        /// <summary>
        /// The value.
        /// </summary>
        Value,

        /// <summary>
        /// The reference.
        /// </summary>
        Reference,

        /// <summary>
        /// The unit.
        /// </summary>
        Unit,

        /// <summary>
        /// The snippet.
        /// </summary>
        Snippet,

        /// <summary>
        /// The string.
        /// </summary>
        String,

        /// <summary>
        /// The character.
        /// </summary>
        Char,
    }
}
