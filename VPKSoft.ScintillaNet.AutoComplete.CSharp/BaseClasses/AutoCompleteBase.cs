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
using System.Drawing;
using System.Linq;
using System.Threading;
using ScintillaNET;
using VPKSoft.ScintillaNet.AutoComplete.CSharp.Enumerations;
using VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces;
using VPKSoft.ScintillaNet.AutoComplete.CSharp.Utility;

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.BaseClasses
{
    /// <summary>
    /// A class to implement some additional functionality to the <see cref="IScintillaAutoComplete{TLibraryEntry}"/> interface.
    /// </summary>
    public abstract class AutoCompleteBase<TLibraryEntry>
    {
        #region PublicMethods
        /// <summary>
        /// Gets or sets the <see cref="Scintilla"/> control associated with this instance.
        /// </summary>
        /// <value>The <see cref="Scintilla"/> control associated with this instance.</value>
        public virtual Scintilla Scintilla { get; set; }

        /// <summary>
        /// Gets the static library member list in case the <see cref="IScintillaAutoComplete{TLibraryEntry}.CacheLibraries"/> is with asStatic parameter set to true.
        /// </summary>
        /// <value>The static library member list.</value>
        public static List<TLibraryEntry> StaticLibraryMemberList { get; set; } = new List<TLibraryEntry>();

        /// <summary>
        /// Gets or sets the images used with the automatic code completion.
        /// </summary>
        /// <value>The images used with the automatic code completion.</value>
        public virtual Dictionary<LanguageConstructType, Bitmap> AutoCompleteImages { get; set; } = new Dictionary<LanguageConstructType, Bitmap>();

        /// <summary>
        /// Gets or sets the postpone timer for code re-evaluation.
        /// </summary>
        /// <value>The postpone timer for code re-evaluation.</value>
        internal virtual PostponeTimer PostponeTimer { get; set; }
        #endregion

        #region PublicMethods        
        /// <summary>
        /// Gets or sets the postpone timer interval.
        /// </summary>
        /// <value>The postpone timer interval.</value>
        public virtual int PostponeTimerInterval
        {
            get => PostponeTimer?.Interval ?? 1000;
            set
            {
                if (PostponeTimer != null)
                {
                    PostponeTimer.Interval = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the post pone timer resolution.
        /// </summary>
        /// <value>The post pone timer resolution.</value>
        public virtual int PostPoneTimerResolution
        {
            get => PostponeTimer?.Resolution ?? 10;
            set
            {
                if (PostponeTimer != null)
                {
                    PostponeTimer.Resolution = value;
                }
            }
        }

        /// <summary>
        /// Gets the bitmap matching a specified <see cref="T:VPKSoft.ScintillaNet.AutoComplete.CSharp.Enumerations.LanguageConstructType" /> value.
        /// </summary>
        /// <param name="languageConstructType">Type of the language construct to get the <see cref="T:System.Drawing.Bitmap" /> for.</param>
        /// <returns>A bitmap matching a specified <see cref="T:VPKSoft.ScintillaNet.AutoComplete.CSharp.Enumerations.LanguageConstructType" /> value.</returns>
        public virtual Bitmap GetBitmap(LanguageConstructType languageConstructType)
        {
            return AutoCompleteImages[languageConstructType];
        }

        /// <summary>
        /// Sets the bitmap matching a specified <see cref="T:VPKSoft.ScintillaNet.AutoComplete.CSharp.Enumerations.LanguageConstructType" /> value to a specified <see cref="T:System.Drawing.Bitmap" />.
        /// </summary>
        /// <param name="languageConstructType">Type of the language construct. to set the <see cref="T:System.Drawing.Bitmap" /> for.</param>
        /// <param name="bitmap">The bitmap to set to represent the specified <see cref="T:VPKSoft.ScintillaNet.AutoComplete.CSharp.Enumerations.LanguageConstructType" />.</param>
        public virtual void SetBitmap(LanguageConstructType languageConstructType, Bitmap bitmap)
        {
            if (bitmap == null)
            {
                AutoCompleteImages.Remove(languageConstructType);
                RegisterImages();
                return;
            }

            AutoCompleteImages[languageConstructType] = bitmap;
        }

        /// <summary>
        /// Registers the images to be used with the <see cref="Scintilla" /> control auto-complete.
        /// </summary>
        public virtual void RegisterImages()
        {
            Scintilla.ClearRegisteredImages();
            foreach (var languageConstructType in Enum.GetValues(typeof(LanguageConstructType))
                .Cast<LanguageConstructType>()) 
            {
                if (AutoCompleteImages.ContainsKey(languageConstructType))
                {
                    Scintilla.RegisterRgbaImage((int)languageConstructType, AutoCompleteImages[languageConstructType]);
                }
            }
        }

        /// <summary>
        /// Postpones the <see cref="PostponeTimer"/> with the specified amount in milliseconds.
        /// </summary>
        /// <param name="value">The amount in milliseconds to postpone the timer.</param>
        public virtual void Postpone(int value)
        {
            PostponeTimer?.Postpone(value);
        }

        /// <summary>
        /// Initializes the images used by the intelligent auto-completion.
        /// </summary>
        public virtual void InitializeImages()
        {
            KeywordImage = Properties.Resources.Keywords;
            BuiltInTypeImage = Properties.Resources.Types;
            ClassImage = Properties.Resources.Classes;
            StaticClassImage = Properties.Resources.StaticClasses;
            PropertyImage = Properties.Resources.PropertiesAndAttributes;
            VariableImage = Properties.Resources.Variables;
            FieldImage = Properties.Resources.Fields;
            TypeParameterImage = Properties.Resources.TypeParameters;
            ConstantImage = Properties.Resources.Constants;
            StructureImage = Properties.Resources.Structures;
            EventImage = Properties.Resources.Events;
            OperatorImage = Properties.Resources.Operators;
            ModuleImage = Properties.Resources.Modules;
            AttributeImage = Properties.Resources.PropertiesAndAttributes;
            ValueImage = Properties.Resources.ValuesAndEnumerations;
            EnumImage = Properties.Resources.ValuesAndEnumerations;
            ReferenceImage = Properties.Resources.References;
            UnitImage = Properties.Resources.Unit;
            SnippetImage = Properties.Resources.Snippets;
            StringImage = Properties.Resources.Words;
            CharImage = Properties.Resources.Words;
            InterfaceImage = Properties.Resources.Interfaces;
            LocalVariableImage = Properties.Resources.Variables;
        }

        /// <summary>
        /// Removes all images registered for auto-completion list of the <see cref="Scintilla"/> control instance.
        /// </summary>
        public virtual void ClearRegisteredImages()
        {
            Scintilla?.ClearRegisteredImages();
            AutoCompleteImages.Clear();
        }
        #endregion

        #region MiscellaneousImages        
        /// <summary>
        /// Gets or sets the folder image.
        /// </summary>
        /// <value>The folder image.</value>
        public virtual Bitmap FolderImage { get; set; } = Properties.Resources.Folders;

        /// <summary>
        /// Gets or sets the file image.
        /// </summary>
        /// <value>The file image.</value>
        public virtual Bitmap FileImage { get; set; } = Properties.Resources.Files;

        /// <summary>
        /// Gets or sets the color image.
        /// </summary>
        /// <value>The color image.</value>
        public virtual Bitmap ColorImage { get; set; } = Properties.Resources.Colors;

        /// <summary>
        /// Gets or sets the snippet prefix image.
        /// </summary>
        /// <value>The snippet prefix image.</value>
        public virtual Bitmap SnippetPrefixImage { get; set; } = Properties.Resources.Snippets;
        #endregion

        #region LanguageConstructImages
        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.Keyword"/> image.
        /// </summary>
        /// <value>The keyword image for the code completion.</value>
        public virtual Bitmap KeywordImage { get => GetBitmap(LanguageConstructType.Keyword); set => SetBitmap(LanguageConstructType.Keyword, value); }

        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.BuildInType"/> image.
        /// </summary>
        /// <value>The built in type image for the code completion.</value>
        public virtual Bitmap BuiltInTypeImage { get => GetBitmap(LanguageConstructType.BuildInType); set => SetBitmap(LanguageConstructType.BuildInType, value); }

        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.Class"/> image.
        /// </summary>
        /// <value>The class image for the code completion.</value>
        public virtual Bitmap ClassImage { get => GetBitmap(LanguageConstructType.Class); set => SetBitmap(LanguageConstructType.Class, value); }

        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.StaticClass"/> image.
        /// </summary>
        /// <value>The static class image for the code completion.</value>
        public virtual Bitmap StaticClassImage { get => GetBitmap(LanguageConstructType.StaticClass); set => SetBitmap(LanguageConstructType.StaticClass, value); }

        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.Property"/> image.
        /// </summary>
        /// <value>The property image for the code completion.</value>
        public virtual Bitmap PropertyImage { get => GetBitmap(LanguageConstructType.Property); set => SetBitmap(LanguageConstructType.Property, value); }

        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.Variable"/> image.
        /// </summary>
        /// <value>The variable image for the code completion.</value>
        public virtual Bitmap VariableImage { get => GetBitmap(LanguageConstructType.Variable); set => SetBitmap(LanguageConstructType.Variable, value); }


        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.LocalVariable"/> image.
        /// </summary>
        /// <value>The local variable image for the code completion.</value>
        public virtual Bitmap LocalVariableImage { get => GetBitmap(LanguageConstructType.LocalVariable); set => SetBitmap(LanguageConstructType.LocalVariable, value); }        
        
        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.Variable"/> image.
        /// </summary>
        /// <value>The variable image for the code completion.</value>
        public virtual Bitmap FieldImage { get => GetBitmap(LanguageConstructType.Field); set => SetBitmap(LanguageConstructType.Field, value); }

        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.TypeParameter"/> image.
        /// </summary>
        /// <value>The type parameter image for the code completion.</value>
        public virtual Bitmap TypeParameterImage { get => GetBitmap(LanguageConstructType.TypeParameter); set => SetBitmap(LanguageConstructType.TypeParameter, value); }

        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.Constant"/> image.
        /// </summary>
        /// <value>The constant image for the code completion.</value>
        public virtual Bitmap ConstantImage { get => GetBitmap(LanguageConstructType.Constant); set => SetBitmap(LanguageConstructType.Constant, value); }

        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.Struct"/> image.
        /// </summary>
        /// <value>The struct image for the code completion.</value>
        public virtual Bitmap StructureImage { get => GetBitmap(LanguageConstructType.Struct); set => SetBitmap(LanguageConstructType.Struct, value); }

        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.Event"/> image.
        /// </summary>
        /// <value>The event image for the code completion.</value>
        public virtual Bitmap EventImage { get => GetBitmap(LanguageConstructType.Event); set => SetBitmap(LanguageConstructType.Event, value); }

        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.Operator"/> image.
        /// </summary>
        /// <value>The operator image for the code completion.</value>
        public virtual Bitmap OperatorImage { get => GetBitmap(LanguageConstructType.Operator); set => SetBitmap(LanguageConstructType.Operator, value); }

        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.Module"/> image.
        /// </summary>
        /// <value>The module image for the code completion.</value>
        public virtual Bitmap ModuleImage { get => GetBitmap(LanguageConstructType.Module); set => SetBitmap(LanguageConstructType.Module, value); }

        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.Attribute"/> image.
        /// </summary>
        /// <value>The attribute image for the code completion.</value>
        public virtual Bitmap AttributeImage { get => GetBitmap(LanguageConstructType.Attribute); set => SetBitmap(LanguageConstructType.Attribute, value); }

        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.Value"/> image.
        /// </summary>
        /// <value>The value image for the code completion.</value>
        public virtual Bitmap ValueImage { get => GetBitmap(LanguageConstructType.Value); set => SetBitmap(LanguageConstructType.Value, value); }

        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.Enum"/> image.
        /// </summary>
        /// <value>The enumeration image for the code completion.</value>
        public virtual Bitmap EnumImage { get => GetBitmap(LanguageConstructType.Enum); set => SetBitmap(LanguageConstructType.Enum, value); }

        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.Reference"/> image.
        /// </summary>
        /// <value>The reference image for the code completion.</value>
        public virtual Bitmap ReferenceImage { get => GetBitmap(LanguageConstructType.Reference); set => SetBitmap(LanguageConstructType.Reference, value); }

        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.Unit"/> image.
        /// </summary>
        /// <value>The unit image for the code completion.</value>
        public virtual Bitmap UnitImage { get => GetBitmap(LanguageConstructType.Unit); set => SetBitmap(LanguageConstructType.Unit, value); }

        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.Snippet"/> image.
        /// </summary>
        /// <value>The snippet image for the code completion.</value>
        public virtual Bitmap SnippetImage { get => GetBitmap(LanguageConstructType.Snippet); set => SetBitmap(LanguageConstructType.Snippet, value); }

        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.String"/> image.
        /// </summary>
        /// <value>The string image for the code completion.</value>
        public virtual Bitmap StringImage { get => GetBitmap(LanguageConstructType.String); set => SetBitmap(LanguageConstructType.String, value); }

        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.Char"/> image.
        /// </summary>
        /// <value>The char image for the code completion.</value>
        public virtual Bitmap CharImage { get => GetBitmap(LanguageConstructType.Char); set => SetBitmap(LanguageConstructType.Char, value); }

        /// <summary>
        /// Gets or sets the image used by the intelligent code completion to describe a <see cref="LanguageConstructType.Interface"/> image.
        /// </summary>
        /// <value>The interface image for the code completion.</value>
        public virtual Bitmap InterfaceImage { get => GetBitmap(LanguageConstructType.Interface); set => SetBitmap(LanguageConstructType.Interface, value); }
        #endregion
    }
}
