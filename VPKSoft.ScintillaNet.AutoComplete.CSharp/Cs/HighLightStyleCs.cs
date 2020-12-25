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

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.Cs
{
    /// <summary>
    /// An enumeration for the call tip highlight style for the C# programming language.
    /// </summary>
    public enum HighLightStyleCs
    {
        /// <summary>
        /// The highlight style is not defined.
        /// </summary>
        None,

        /// <summary>
        /// The highlight style is type.
        /// </summary>
        Type,

        /// <summary>
        /// The highlight style is return value type.
        /// </summary>
        ReturnValueType,

        /// <summary>
        /// The highlight style is argument name.
        /// </summary>
        ArgumentName,

        /// <summary>
        /// An opening bracket.
        /// </summary>
        OpeningBracket,

        /// <summary>
        /// A closing bracket.
        /// </summary>
        ClosingBracket,

        /// <summary>
        /// The body name of a method, property, field, etc. member.
        /// </summary>
        BodyName,
    }
}
