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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.Recommendations;
using Microsoft.CodeAnalysis.Text;
using ScintillaNET;
using VPKSoft.ScintillaNet.AutoComplete.CSharp.BaseClasses;
using VPKSoft.ScintillaNet.AutoComplete.CSharp.Enumerations;
using VPKSoft.ScintillaNet.AutoComplete.CSharp.GUI;
using VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces;
using VPKSoft.ScintillaNet.AutoComplete.CSharp.Utility;
using Document = Microsoft.CodeAnalysis.Document;

#if NETFRAMEWORK
using System.Text;
using  VPKSoft.ScintillaNet.AutoComplete.CSharp.PInvoke;
#endif

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.Cs
{
    /// <summary>
    /// A class for intelligent code completion for the C# programming language.
    /// </summary>
    public class AutoCompleteCs: AutoCompleteBase<LibraryEntryCs>, IScintillaAutoComplete<LibraryEntryCs>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoCompleteCs"/> class.
        /// </summary>
        /// <param name="scintilla">The scintilla.</param>
        /// <param name="staticAssemblyCache">A value indicating whether to search the auto-complete data from the </param>
        public AutoCompleteCs(Scintilla scintilla, bool staticAssemblyCache)
        {
            base.Scintilla = scintilla;
            base.Scintilla.CharAdded += scintilla_CharAdded;

            base.InitializeImages();
            base.RegisterImages();

            UseStaticLibraryMemberCache = staticAssemblyCache;

            LangKeywords =
                "abstract add alias as ascending async await base break case catch checked continue default delegate descending do dynamic else event explicit extern false finally fixed for foreach from get global goto group if implicit in interface internal into is join let lock namespace new null object operator orderby out override params partial private protected public readonly ref remove return sealed select set sizeof stackalloc switch this throw true try typeof unchecked unsafe using value virtual where while yield";

            LangTypeWords =
                "bool byte char class const decimal double enum float int long sbyte short static string struct uint ulong ushort var void";

            //base.Scintilla.AutoCOrder = Order.PerformSort;
            base.Scintilla.AutoCIgnoreCase = true;

            CallTipStyling = new CallTipStylingCs(scintilla);

            CallTipEntry<HighLightStyleCs>.AddTypeImage(LanguageConstructType.StaticClass, Properties.Resources.StaticClasses);
            CallTipEntry<HighLightStyleCs>.AddTypeImage(LanguageConstructType.Method, Properties.Resources.Methods);
            CallTipEntry<HighLightStyleCs>.AddTypeImage(LanguageConstructType.Property, Properties.Resources.PropertiesAndAttributes);
            CallTipEntry<HighLightStyleCs>.AddTypeImage(LanguageConstructType.Field, Properties.Resources.Fields);
            CallTipEntry<HighLightStyleCs>.AddStyle(HighLightStyleCs.OpeningBracket, Color.White, Color.Black, new Font(new FontFamily("Consolas"), 9));
            CallTipEntry<HighLightStyleCs>.AddStyle(HighLightStyleCs.ClosingBracket, Color.White, Color.Black, new Font(new FontFamily("Consolas"), 9));
            CallTipEntry<HighLightStyleCs>.AddStyle(HighLightStyleCs.ArgumentName, Color.DarkCyan, Color.Black, new Font(new FontFamily("Consolas"), 9));
            CallTipEntry<HighLightStyleCs>.AddStyle(HighLightStyleCs.BodyName, Color.Orchid, Color.Black, new Font(new FontFamily("Consolas"), 9));
            CallTipEntry<HighLightStyleCs>.AddStyle(HighLightStyleCs.ReturnValueType, Color.FromArgb(86, 156, 214), Color.Black, new Font(new FontFamily("Consolas"), 9));

            CacheLibraries(true);

            base.PostponeTimer = new PostponeTimer(base.PostponeTimerInterval, base.PostPoneTimerResolution)
                {Enabled = true};
            base.PostponeTimer.Timer += Timer;

            base.Scintilla.TextChanged += scintilla_TextChanged;
        }

        private void scintilla_TextChanged(object sender, EventArgs e)
        {
            PostponeTimer.Postpone(500);
        }

        readonly object dummyLock = new object();

        private void Timer(object sender, PostponeTimerEventArgs e)
        {
            lock (dummyLock)
            {
                var sourceText = "";
                if (Scintilla.IsDisposed)
                {
                    return;
                }

                Scintilla.Invoke(new MethodInvoker(() => sourceText = Scintilla.Text));
                CreateAddhocProject(sourceText);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoCompleteCs"/> class.
        /// </summary>
        /// <param name="scintilla">The scintilla.</param>
        public AutoCompleteCs(Scintilla scintilla): this(scintilla, true) { }

        #region PublicProperties

        /// <summary>
        /// Gets or sets the name of the project for the Roslyn.
        /// </summary>
        /// <value>The name of the project for the Roslyn.</value>
        public string ProjectName { get; set; } = @"NewProject";

        /// <summary>
        /// Gets or sets the call tip styling.
        /// </summary>
        /// <value>The call tip styling.</value>
        public CallTipStylingCs CallTipStyling { get; set; }

        /// <summary>
        /// Gets or sets the custom call tip <see cref="Form"/> form.
        /// </summary>
        /// <value>The custom call tip <see cref="Form"/> form.</value>
        public FormCustomCallTip<HighLightStyleCs> CustomCallTip { get; set; } = new FormCustomCallTip<HighLightStyleCs>();

        /// <summary>
        /// Gets or sets the word list for the <see cref="Scintilla"/> control.
        /// </summary>
        /// <value>The word list for the <see cref="Scintilla"/> control.</value>
        public string WordList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the GAC (P/Invoke) to load the assemblies.
        /// </summary>
        /// <value><c>true</c> whether to use the GAC (P/Invoke) to load the assemblies; otherwise, <c>false</c>.</value>
        // ReSharper disable once InconsistentNaming
        public bool UseGAC { get; set; } = true;

        // a field for the LangKeywords property..
        private List<string> langKeywords;

        /// <summary>
        /// Gets or sets the language keywords.
        /// </summary>
        /// <value>The language keywords.</value>
        public string LangKeywords
        {
            get => GetKeywords(langKeywords, LanguageConstructType.Keyword, Filter); 
            set => langKeywords = SetKeywords(value);
        }

        // a field for the LangTypeWords property..
        private List<string> langTypeWords;

        /// <summary>
        /// Gets or sets the built-in language type keywords.
        /// </summary>
        /// <value>The built-in language type keywords.</value>
        public string LangTypeWords
        {
            get => GetKeywords(langTypeWords, LanguageConstructType.BuildInType, Filter); 
            set => langTypeWords = SetKeywords(value);
        }

        /// <summary>
        /// Gets the automatic complete word list of all the word types combined.
        /// </summary>
        /// <value>The automatic complete word list of all the word types combined.</value>
        public string AutoCompleteWordList
        {
            get
            {
                var result = LangKeywords;
                result += string.IsNullOrWhiteSpace(LangTypeWords) ? "" : " " + LangTypeWords;
                return string.Join(" ", result.Split(' ').OrderBy(f => f).ThenBy(f => f.Length));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance should use the static library member.
        /// </summary>
        /// <value><c>true</c> if this instance should use the static library member cache; otherwise, <c>false</c>.</value>
        public bool UseStaticLibraryMemberCache { get; set; }

        /// <summary>
        /// Gets the instance library member list in case the <see cref="M:VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces.IScintillaAutoComplete`1.CacheLibraries(System.Boolean)" /> is with asStatic parameter set to false.
        /// </summary>
        /// <value>The instance library member list.</value>
        public List<LibraryEntryCs> InstanceLibraryMemberList { get; } = new List<LibraryEntryCs>();
        #endregion

        #region InternalEvents        
        /// <summary>
        /// Gets the type string at a specified document position..
        /// </summary>
        /// <param name="span">The location of the type.</param>
        /// <param name="document">The <see cref="Document"/> instance to search the type for using Roslyn.</param>
        /// <param name="loadedAssemblies">The currently loaded assemblies.</param>
        /// <returns>System.String.</returns>
        private static async Task<string> GetTypeStringAt(TextSpan span, Document document, Dictionary<string, Assembly> loadedAssemblies)
        {
            var result = await GetTypeAt(span, document, loadedAssemblies);
            return result?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Gets the type at a specified document position.
        /// </summary>
        /// <param name="span">The location of the type.</param>
        /// <param name="document">The <see cref="Document"/> instance to search the type for using Roslyn.</param>
        /// <param name="loadedAssemblies">The currently loaded assemblies.</param>
        /// <returns>Type.</returns>
        private static async Task<Type> GetTypeAt(TextSpan span, Document document, Dictionary<string, Assembly> loadedAssemblies)
        {
            var model = await document.GetSemanticModelAsync();
            var syntaxTree = await document.GetSyntaxTreeAsync();
            if (syntaxTree != null)
            {
                var root = await syntaxTree.GetRootAsync();

                var children = root.ChildNodes(); // TODO::Span from start to this point!!
                
                var nodes = root.DescendantNodes(span);

                var typeNode = (MemberAccessExpressionSyntax) nodes
                    .FirstOrDefault(f => f is MemberAccessExpressionSyntax);

                if (typeNode != null)
                {
                    var typeInfo = model.GetTypeInfo(typeNode.Expression);
                    var result = typeInfo.Type;

                    var resultName = result?.Name == null ? null : Type.GetType($"System.{result.Name}");

                    var type = resultName ?? Type.GetType($"{result?.ContainingNamespace}.{result?.Name}, {result?.ContainingAssembly.Name}");

                    if (type == null)
                    {
                        foreach (var assembly in loadedAssemblies)
                        {
                            var typeString = assembly.Key + "." + result?.Name;
                            if ((type = assembly.Value.GetType(typeString)) != null)
                            {
                                return type;
                            }
                        }
                    }

                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// Handles the CharAdded event of the scintilla control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CharAddedEventArgs"/> instance containing the event data.</param>
        private async void scintilla_CharAdded(object sender, CharAddedEventArgs e)
        {
            if (e.Char == '.')
            {
                CreateAddhocProject(Scintilla.Text);

                // find the word start..
                var currentPos = Scintilla.CurrentPosition;
                var wordStartPos = Scintilla.WordStartPosition(currentPos - 1, true);
                var word = Scintilla.Text.Substring(wordStartPos, currentPos - wordStartPos - 1);

                var construct = StaticLibraryMemberList
                    .FirstOrDefault(f => f.Type == LanguageConstructType.StaticClass && f.Name == word) ??
                                StaticLibraryMemberList
                    .FirstOrDefault(f => f.ConstructType != null && f.ConstructType.Name == word);

                Type type = null;
                //if (construct == null)
                {
                    type =
                        await GetTypeAt(new TextSpan(wordStartPos, currentPos - wordStartPos - 1), Document, LoadedAssemblies);
                }

                var instanceConstruct = StaticLibraryMemberList
                    .FirstOrDefault(f => f.ConstructType == type && type != null);

                construct ??= instanceConstruct;

                CustomCallTip.Clear();

                if (construct != null)
                {
                    foreach (var method in construct.Methods)
                    {
                        var body = method.ToString(false, out var highLights, out var returnType);

                        var callTipEntry = new CallTipEntry<HighLightStyleCs>
                            {CallTipBodyText = body, CallTipTypeText = returnType, LanguageConstructType = LanguageConstructType.Method, Style = HighLightStyleCs.BodyName};

                        foreach (var highLight in highLights)
                        {
                            callTipEntry.AddHighlightPosition(highLight);
                        }

                        CustomCallTip.AddEntry(callTipEntry);
                    }

                    foreach (var property in construct.Properties)
                    {
                        var body = property.ToString(out var highLights, out var returnType);

                        var callTipEntry = new CallTipEntry<HighLightStyleCs>
                            {CallTipBodyText = body, CallTipTypeText = returnType, LanguageConstructType = LanguageConstructType.Property, Style = HighLightStyleCs.BodyName};

                        foreach (var highLight in highLights)
                        {
                            callTipEntry.AddHighlightPosition(highLight);
                        }

                        CustomCallTip.AddEntry(callTipEntry);
                    }

                    foreach (var field in construct.Fields)
                    {
                        var body = field.ToString(out var highLights, out var returnType);

                        var callTipEntry = new CallTipEntry<HighLightStyleCs>
                            {CallTipBodyText = body, CallTipTypeText = returnType, LanguageConstructType = LanguageConstructType.Field, Style = HighLightStyleCs.BodyName};

                        foreach (var highLight in highLights)
                        {
                            callTipEntry.AddHighlightPosition(highLight);
                        }

                        CustomCallTip.AddEntry(callTipEntry);
                    }


                    //CustomCallTip.SetImage();



//                    CallTipStyling.ShowCallTip(returnType, body, highLights, 1, construct.Methods.Count);
                }
                CustomCallTip.CallTipShow(Scintilla);


//                Scintilla.CallTipShow(construct.Methods.FirstOrDefault().);

                // TODO::Make this work..
            }
            else
            {
                await AutoCompleteMemberUpdate(Scintilla.CurrentPosition);
                AutoCompleteSuggest();
            }
        }
        #endregion

        #region PrivateProperties        
        /// <summary>
        /// Gets or sets the filter to filter the language constructs with.
        /// </summary>
        /// <value>The filter to filter the language constructs with.</value>
        // ReSharper disable once UnusedAutoPropertyAccessor.Local, the usage is defined by a compiler directive..
        private string Filter { get; set; }

        /// <summary>
        /// Gets or sets the regular expression to match using clauses.
        /// </summary>
        /// <value>The regular expression to match using clauses.</value>
        private Regex UsingRegex { get; } = new Regex(@"using\s(\n|\r|.+)(\s?){1};", RegexOptions.Compiled);

        /// <summary>
        /// Gets or sets a value indicating whether GAC dictionary is loaded.
        /// </summary>
        /// <value><c>true</c> if the GAC dictionary is loaded; otherwise, <c>false</c>.</value>
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedMember.Local, the usage is defined by a compiler directive..
        private bool GACDictionaryLoaded { get; set; }

        /// <summary>
        /// The GAC dictionary.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once CollectionNeverUpdated.Local, the usage is defined by a compiler directive..
        private Dictionary<string, string> GACDictionary { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets the loaded assemblies.
        /// </summary>
        /// <value>The loaded assemblies.</value>
        private Dictionary<string, Assembly> LoadedAssemblies { get; } = new Dictionary<string, Assembly>();

        /// <summary>
        /// Gets the names of the assemblies failed to load.
        /// </summary>
        /// <value>The names of the assemblies failed to load.</value>
        private HashSet<string> FailedAssemblies { get; } = new HashSet<string>();
        #endregion

        #region PrivateFields
        /// <summary>
        /// A field indicating whether the instance has already been disposed of.
        /// </summary>
        private bool isDisposed;
        #endregion

        #region PublicMethods

        public async Task AutoCompleteMemberUpdate(int currentPosition)
        {
            var model = await Document.GetSemanticModelAsync();
            var syntaxTree = await Document.GetSyntaxTreeAsync();
            if (syntaxTree != null)
            {
                var symbols = await Recommender.GetRecommendedSymbolsAtPositionAsync(model, currentPosition, Workspace);
                symbols = symbols.Where(f => f.Kind == SymbolKind.Local);

                var names = symbols.Select(f => f.Name).ToList();

                WordList = ScintillaKeywordBuilder
                    .WithNew()
                    .WithWordList(WordList)
                    .RemoveKeywordsWithType(LanguageConstructType.LocalVariable)
                    .AddKeyWords(names, LanguageConstructType.LocalVariable).ToString();
            }
        }
        
        
        /// <summary>
        /// Displays the auto-complete complete suggestion menu for the <see cref="Scintilla"/> control.
        /// </summary>
        public virtual void AutoCompleteSuggest()
        {
            Scintilla.AutoCOrder = Order.PerformSort;
            
            // find the word start..
            var currentPos = Scintilla.CurrentPosition;
            var wordStartPos = Scintilla.WordStartPosition(currentPos, true);

            // display the auto-completion list..
            var lenEntered = currentPos - wordStartPos;
            if (lenEntered > 0 && !Scintilla.AutoCActive)
            {
                Scintilla.AutoCShow(lenEntered, WordList);
            }
        }

        /// <summary>
        /// Adds the specified instance to the collection used by the class.
        /// </summary>
        /// <param name="libraryEntry">The library entry to add.</param>
        /// <param name="asStatic">If set to <c>true</c> the entry should be added to a static collection.</param>
        /// <param name="isStatic">if set to <c>true</c> the entry is a static language construct.</param>
        /// <see cref="F:VPKSoft.ScintillaNETAutoComplete.Enumerations.LanguageConstructType.Static" /> flag should be applied to the <see cref="T:VPKSoft.ScintillaNETAutoComplete.Enumerations.LanguageConstructType" /> enumeration value.
        /// <exception cref="System.NotImplementedException"></exception>
        public bool AddLibraryEntry(LibraryEntryCs libraryEntry, bool asStatic, bool isStatic)
        {
            if (asStatic)
            {
                if (StaticLibraryMemberList.Contains(libraryEntry))
                {
                    return false;
                }
            }
            else
            {
                if (InstanceLibraryMemberList.Contains(libraryEntry))
                {
                    return false;
                }
            }

            if (isStatic)
            {
                libraryEntry.Modifiers |= LanguageModifiers.Static;
            }

            if (asStatic)
            {
                StaticLibraryMemberList.Add(libraryEntry);
            }
            else
            {
                InstanceLibraryMemberList.Add(libraryEntry);
            }

            return true;
        }

        /// <summary>
        /// Adds the specified instance to the collection used by the class.
        /// </summary>
        /// <param name="libraryEntry">The library entry to add.</param>
        /// <param name="asStatic">If set to <c>true</c> the entry should be added to a static collection.</param>
        public bool AddLibraryEntry(LibraryEntryCs libraryEntry, bool asStatic)
        {
            return AddLibraryEntry(libraryEntry, asStatic, false);
        }

        /// <summary>
        /// Returns an alphabetically and length ordered list of strings from a specified unordered list of strings.
        /// </summary>
        /// <param name="strings">The string list to sort.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public List<string> OrderStringList(List<string> strings)
        {
            return SetKeywords(string.Join(" ", strings));
        }

        /// <summary>
        /// Creates a list of strings from a space-delimited list of keyword ordering by alphabetically and the by their length.
        /// </summary>
        /// <param name="keywords">The keywords to create a list of string from.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public virtual List<string> SetKeywords(string keywords)
        {
            return new List<string>(keywords.Split(' ').OrderBy(f => f).ThenBy(f => f.Length));
        }

        /// <summary>
        /// Gets the keywords as a string combined with the specified keyword type identifier.
        /// </summary>
        /// <param name="keywords">The keywords as a list of strings.</param>
        /// <param name="wordType">Type of the word.</param>
        /// <returns>System.String.</returns>
        public string GetKeywords(List<string> keywords, LanguageConstructType wordType)
        {
            return GetKeywords(keywords, wordType, null);
        }

        /// <summary>
        /// Gets the keywords as a string combined with the specified keyword type identifier.
        /// </summary>
        /// <param name="keywords">The keywords as a list of strings.</param>
        /// <param name="wordType">Type of the word.</param>
        /// <param name="filter">A filter to filter the keywords with.</param>
        /// <returns>System.String.</returns>
        public string GetKeywords(List<string> keywords, LanguageConstructType wordType, string filter)
        {
            return keywords == null
                ? ""
                : string.Join("?" + (int) wordType + " ",
                      (filter == null ? keywords : keywords.Where(f => f.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0))) + 
                  "?" +
                  (int) wordType;
        }
        #endregion

        #region Assembly        
        /// <summary>
        /// Updates the assemblies in the <see cref="Scintilla"/> document to the internal list.
        /// </summary>
        public void GetDocumentAssemblies()
        {
            string CreateAssemblyName(string regexMatch)
            {
                try
                {
                    var assemblyName = regexMatch;
                    assemblyName = assemblyName.Replace("using", string.Empty);
                    assemblyName = assemblyName.Replace("static", string.Empty);
                    assemblyName = assemblyName.Trim().TrimEnd(';');
                    return assemblyName;
                }
                catch
                {
                    return string.Empty;
                }
            }

            #if NETFRAMEWORK
            if (UseGAC && !GACDictionaryLoaded)
            {
                GACDictionaryLoaded = true;
                var cacheEnum = AssemblyCache.CreateGACEnum();
                while (AssemblyCache.GetNextAssembly(cacheEnum, out var nameEnum) == 0)
                {
                    try
                    {
                        uint len = 1024;
                        var charBuffer = new StringBuilder((int) len);
                        nameEnum.GetDisplayName(charBuffer, ref len, 0);
                        var fullName = charBuffer.ToString();

                        len = 1024;
                        nameEnum.GetName(ref len, charBuffer);
                        var name = charBuffer.ToString();

                        if (GACDictionary.ContainsKey(name))
                        {
                            continue;
                        }

                        GACDictionary.Add(name, fullName);
                    }
                    catch
                    {
                        // just let it continue..
                    }
                }
            }
            #endif

            var usingClauses = UsingRegex.Matches(Scintilla.Text);
            for (int i = 0; i < usingClauses.Count; i++)
            {
                try
                {
                    var assemblyName = CreateAssemblyName(usingClauses[i].Value);

                    if (FailedAssemblies.Contains(assemblyName))
                    {
                        continue;
                    }

                    if (LoadedAssemblies.Any(f => f.Key == assemblyName))
                    {
                        continue;
                    }

                    var assembly = AppDomain.CurrentDomain
                        .GetAssemblies()
                        .FirstOrDefault(f => f.GetName().Name == assemblyName);

                    if (UseGAC && assembly == null)
                    {
                        try
                        {
                            var name = GACDictionary.FirstOrDefault(f => f.Key == assemblyName).Value;
                            if (name != null)
                            {
                                assembly = AppDomain.CurrentDomain.Load(name);
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    if (assembly == null)
                    {
                        continue;
                    }

                    LoadedAssemblies.Add(assemblyName, assembly);
                }
                catch
                {
                    var assemblyName = CreateAssemblyName(usingClauses[i].Value);
                    FailedAssemblies.Add(assemblyName);
                }
            }

            #if NETFRAMEWORK
            // This 'mscorlib' isn't same as the system..
            try
            {
                var coreLibrary = typeof(File).Assembly;
                LoadedAssemblies.Add("mscorlib", coreLibrary);
            }
            catch
            {
                // this shouldn't happen..
            }
            #endif
        }


        #region RoslynProperties        
        /// <summary>
        /// Gets or sets the project information.
        /// </summary>
        /// <value>The project information.</value>
        private ProjectInfo ProjectInfo { get; set; }

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>The project.</value>
        private Project Project { get; set; }

        /// <summary>
        /// Gets or sets the instance of the <see cref="MefHostServices"/> class.
        /// </summary>
        /// <value>The instance of the <see cref="MefHostServices"/> class.</value>
        private MefHostServices HostServices { get; set; }

        /// <summary>
        /// Gets or sets the instance of the <see cref="AdhocWorkspace"/> class.
        /// </summary>
        /// <value>The instance of the <see cref="AdhocWorkspace"/> class.</value>
        private AdhocWorkspace Workspace { get; set; }

        /// <summary>
        /// Gets or sets the instance of the <see cref="Document"/> class.
        /// </summary>
        /// <value>The instance of the <see cref="Document"/> class.</value>
        private Document Document { get; set; }

        /// <summary>
        /// Gets or sets the instance of the <see cref="CompilationOptions"/> class.
        /// </summary>
        /// <value>The instance of the <see cref="CompilationOptions"/> class.</value>
        private CSharpCompilationOptions CompilationOptions { get; set; }
        
        /// <summary>
        /// Gets or sets the previous text of the <see cref="Scintilla"/> control.
        /// </summary>
        private string PreviousText { get; set; }
        
        /// <summary>
        /// Gets or sets an instance of the <see cref="Microsoft.CodeAnalysis.Text.SourceText"/> class.
        /// </summary>
        private SourceText SourceText { get; set; }
        
        #endregion
        /// <summary>
        /// Creates a <see cref="AdhocWorkspace"/> and project with the updated source code.
        /// </summary>
        /// <param name="sourceText">The C# source code for the project.</param>
        public void CreateAddhocProject(string sourceText)
        {
            if (string.Compare(PreviousText, sourceText, StringComparison.InvariantCulture) != 0)
            {
                PreviousText = sourceText;
            }
            else
            {
                return;
            }
            
            Workspace?.Dispose();
            
            HostServices ??= LoadedAssemblies.Count > 0
                ? MefHostServices.Create(new List<Assembly>(LoadedAssemblies.Values.Select(assembly => assembly)))
                : MefHostServices.Create(MefHostServices.DefaultAssemblies);
            
            Workspace?.Dispose();
            
            HostServices = MefHostServices.Create(MefHostServices.DefaultAssemblies);
            Workspace = new AdhocWorkspace(HostServices);

            ProjectInfo = ProjectInfo
                .Create(ProjectId.CreateNewId(), VersionStamp.Create(), ProjectName, ProjectName, LanguageNames.CSharp)
                .WithMetadataReferences(new[] {MetadataReference.CreateFromFile(typeof(object).Assembly.Location)});

            Project = Workspace.AddProject(ProjectInfo);

            SourceText = SourceText.From(sourceText);
            
            Document = Workspace.AddDocument(Project.Id, ProjectName + @".cs", SourceText);

            CompletionService.GetService(Document);
        }
        
        /// <summary>
        /// Caches the assemblies used with the code auto-completion.
        /// </summary>
        /// <param name="asStatic">A value indicating whether the assembly cache should be a static cache.</param>
        public void CacheLibraries(bool asStatic)
        {
            GetDocumentAssemblies();
            
            CreateAddhocProject(Scintilla.Text);
            
            var assemblies = LoadedAssemblies.Select(f => f.Value).ToList();

            foreach (var assembly in assemblies)
            {
                string location = assembly.Location;
                if (!string.IsNullOrWhiteSpace(location) && // TODO::Document this condition...
                    (asStatic && StaticLibraryMemberList.All(f => f.FileName != location) ||
                     !asStatic &&
                     InstanceLibraryMemberList.All(f => f.FileName != location)) &&
                    Path.GetExtension(location).ToLower() != ".exe")
                {
                    // get the types within the assembly..
                    Type[] types;
                    try
                    {
                        types = assembly.DefinedTypes.Select(f => f.AsType()).ToArray();
                    }
                    catch
                    {
                        continue;
                    }

                    foreach (var typeInAssembly in types)
                    {
                        if (typeInAssembly == typeof(Color))
                        {
                            
                        }

                        // only enumerate public types..
                        if (!typeInAssembly.IsPublic)
                        {
                            continue;
                        }

                        // ..and only enumerate non-generic types..
                        if (typeInAssembly.IsGenericTypeDefinition)
                        {
                            continue;
                        }

                        // get the type name..
                        var name = typeInAssembly.FullName?.Substring(0,
                            typeInAssembly.FullName.Length - typeInAssembly.Name.Length - 1);

                        // no name, no deal..
                        if (name == null)
                        {
                            continue;
                        }

                        // determine if a class is static..
                        var isStaticClass = typeInAssembly.IsAbstract && typeInAssembly.IsSealed &&
                                            typeInAssembly.IsClass;

                        // determine if the type is a struct..
                        var isStruct = typeInAssembly.IsValueType && !typeInAssembly.IsEnum &&
                                       !typeInAssembly.IsEquivalentTo(typeof(decimal)) &&
                                       !typeInAssembly.IsPrimitive;

                        // the class, interface and the struct part..
                        if (typeInAssembly.IsClass || isStruct || typeInAssembly.IsInterface || typeInAssembly.IsPrimitive)
                        {
                            // ..set the type of the language construct..
                            var languageConstructType = LanguageConstructType.Class;

                            if (isStruct)
                            {
                                languageConstructType = LanguageConstructType.Struct;
                            }

                            if (typeInAssembly.IsInterface)
                            {
                                languageConstructType = LanguageConstructType.Interface;
                            }

                            if (isStaticClass && languageConstructType == LanguageConstructType.Class)
                            {
                                languageConstructType = LanguageConstructType.StaticClass;
                            }

                            if (typeInAssembly.IsPrimitive)
                            {
                                languageConstructType = LanguageConstructType.BuildInType;
                            }

                            // ..create an entry for the class, interface or the struct..
                            var entry = new LibraryEntryCs(typeInAssembly.Name, name,
                                location, languageConstructType) {ConstructType = typeInAssembly};

                            // ..add the entry to the collection..
                            AddLibraryEntry(entry, asStatic, isStaticClass);

                            // enumerate the public fields (both static and instance)..
                            var fields =
                                typeInAssembly.GetFields(BindingFlags.Instance | BindingFlags.Public |
                                                         BindingFlags.Static);

                            // ..loop through the fields and add them to the collection..
                            foreach (var field in fields)
                            {
                                entry.AddField(new LibraryEntryCs(typeInAssembly.Name + "." + field.Name, name,
                                    location,
                                    LanguageConstructType.Field)
                                {
                                    ReturnType = field.FieldType,
                                });
                            }

                            // enumerate the public instance (no PropertyInfo.IsStatic property, so two enumerations are required) properties..
                            var properties =
                                typeInAssembly.GetProperties(
                                    BindingFlags.Instance | BindingFlags.Public);

                            // ..loop through the public properties and add them to the collection..
                            foreach (var property in properties)
                            {
                                entry.AddProperty(new LibraryEntryCs(typeInAssembly.Name + "." + property.Name, name,
                                    location,
                                    LanguageConstructType.Property,
                                    LanguageModifiers.Public | LanguageModifiers.Instance)
                                {
                                    ReturnType = property.PropertyType, CanRead = property.CanRead,
                                    CanWrite = property.CanWrite,
                                });
                            }

                            // ..enumerate the public static (no PropertyInfo.IsStatic property, so two enumerations are required) properties..
                            properties = typeInAssembly.GetProperties(BindingFlags.Static | BindingFlags.Public);

                            // ..loop through the public static properties and add them to the collection..
                            foreach (var property in properties)
                            {
                                entry.AddProperty(new LibraryEntryCs(typeInAssembly.Name + "." + property.Name, name,
                                    location,
                                    LanguageConstructType.Property, LanguageModifiers.Static | LanguageModifiers.Public)
                                {
                                    ReturnType = property.PropertyType, CanRead = property.CanRead,
                                    CanWrite = property.CanWrite
                                });
                            }

                            // an interface doesn't have public properties..
                            if (typeInAssembly.IsInterface)
                            {
                                // ..enumerate the non-public properties..
                                properties = typeInAssembly.GetProperties(BindingFlags.NonPublic);

                                // ..loop through the "private" properties and add them to the collection..
                                foreach (var property in properties)
                                {
                                    entry.AddProperty(new LibraryEntryCs(typeInAssembly.Name + "." + property.Name,
                                        name,
                                        location,
                                        LanguageConstructType.Property,
                                        LanguageModifiers.Private | LanguageModifiers.Instance)
                                    {
                                        ReturnType = property.PropertyType, CanRead = property.CanRead,
                                        CanWrite = property.CanWrite
                                    });
                                }
                            }

                            // enumerate the public instance methods..
                            var methods =
                                typeInAssembly.GetMethods(BindingFlags.Public | BindingFlags.Instance |
                                                          BindingFlags.Static);

                            // ..loop through the methods add them to the collection..
                            foreach (var method in methods)
                            {
                                // determine if the method is declared inside the assemblies loaded in the current application domain..
                                var methodDeclared = assemblies.Any(f =>
                                    f.GetName().Name == method.DeclaringType?.Assembly.GetName().Name);

                                if (method.IsPublic && !method.IsSpecialName && methodDeclared)
                                {
                                    // ..for classes and structs..
                                    entry.AddMethod(new MethodDescriptionCs(method.Name,
                                        method.ReturnType,
                                        method.GetParameters()
                                            .Select(f => new KeyValuePair<string, ParameterInfo>(f.Name, f))
                                            .ToArray())
                                    {
                                        IsStatic = method.IsStatic, IsPrivate = method.IsPrivate,
                                        IsUsable = methodDeclared
                                    });
                                }
                            }

                            // interfaces have only "private methods"..
                            if (typeInAssembly.IsInterface)
                            {
                                // enumerate the private instance methods..
                                methods =
                                    typeInAssembly.GetMethods(BindingFlags.Public | BindingFlags.Instance |
                                                              BindingFlags.Static);

                                // ..loop through the methods add them to the collection..
                                foreach (var method in methods)
                                {
                                    // ..for interfaces only..
                                    if (method.IsPrivate && typeInAssembly.IsInterface)
                                    {
                                        entry.AddMethod(new MethodDescriptionCs(method.Name,
                                                method.ReturnType,
                                                method.GetParameters()
                                                    .Select(f => new KeyValuePair<string, ParameterInfo>(f.Name, f))
                                                    .ToArray())
                                            {IsStatic = method.IsStatic, IsPrivate = method.IsPrivate});
                                    }
                                }
                            }

                            // enumerate the public methods (both static and instance)..
                            var constructors =
                                typeInAssembly.GetConstructors(
                                    BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance);

                            // ..loop through the constructors add them to the collection..
                            foreach (var constructor in constructors)
                            {
                                entry.AddMethod(new MethodDescriptionCs(typeInAssembly.Name,
                                    typeInAssembly, // constructor returns the type of it self..
                                    constructor.GetParameters()
                                        .Select(f => new KeyValuePair<string, ParameterInfo>(f.Name, f)).ToArray())
                                {
                                    IsConstructor = true, IsStatic = constructor.IsStatic,
                                    IsPrivate = constructor.IsPrivate
                                });
                            }

                            if (methods.Length > 5)
                            {
                                //MessageBox.Show(entry.ToString());
                            }
                        }

                        // TODO::Class member enums, classes types!

                        // the enum..
                        if (typeInAssembly.IsEnum)
                        {
                            AddLibraryEntry(new LibraryEntryCs(typeInAssembly.Name, name,
                                    location,
                                    LanguageConstructType.Enum)
                                {EnumDescription = EnumDescriptionCs.FromEnum(typeInAssembly)}, asStatic);
                        }
                    }
                }
            }

            var test = StaticLibraryMemberList[StaticLibraryMemberList.Count - 666];

            foreach (var testMethod in test.Methods)
            {
                //MessageBox.Show(testMethod.ToString());
            }

            //MessageBox.Show(test.ToString());

            WordList = new ScintillaKeywordBuilder()
                .AddKeywordsFrom(StaticLibraryMemberList)
                .AddKeyWords(langKeywords, LanguageConstructType.Keyword)
                .AddKeyWords(langTypeWords, LanguageConstructType.BuildInType)
                .ToString();
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }
        
            if (disposing)
            {
                // free managed resources..
                Scintilla.CharAdded -= scintilla_CharAdded;
                base.PostponeTimer.Timer += Timer;
                base.Scintilla.TextChanged += scintilla_TextChanged;
                PostponeTimer?.Dispose();
                Workspace?.Dispose();
                ClearRegisteredImages();
            }
        
            isDisposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="AutoCompleteCs"/> class.
        /// </summary>
        ~AutoCompleteCs()
        {
            // finalizer calls Dispose(false)..
            Dispose(false);
        }
        #endregion
    }
}
