﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 12.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Twenty57.Linx.Components.File
{
    using Twenty57.Linx.Components.File.TextFileRead;
    using Twenty57.Linx.Plugin.Common.CodeGeneration;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Projects\Linx\linx5-components\linx5-components-file\Code\File\TextFileRead\Templates\TextFileReadStringsAndLoop_Gen.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
    public partial class TextFileReadStringsAndLoop_Gen : TextFileReadStringsAndLoop_GenBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("\r\n");
            this.Write("\r\n");
            this.Write("\r\n");
            this.Write("\r\n");
            this.Write("\r\nvar fileHandle = (Twenty57.Linx.Components.File.Common.TextFileHandle)");
            
            #line 21 "C:\Projects\Linx\linx5-components\linx5-components-file\Code\File\TextFileRead\Templates\TextFileReadStringsAndLoop_Gen.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(filePathPropertyName));
            
            #line default
            #line hidden
            this.Write(";\r\nfileHandle.LogEvent += message => ");
            
            #line 22 "C:\Projects\Linx\linx5-components\linx5-components-file\Code\File\TextFileRead\Templates\TextFileReadStringsAndLoop_Gen.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(functionContextPropertyName));
            
            #line default
            #line hidden
            this.Write(".Log(message);\r\nvar reader = new Twenty57.Linx.Components.File.TextFileRead.TextF" +
                    "ileReader(\r\n\tfileHandle, \r\n\t");
            
            #line 25 "C:\Projects\Linx\linx5-components\linx5-components-file\Code\File\TextFileRead\Templates\TextFileReadStringsAndLoop_Gen.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(CSharpUtilities.EnumAsString(readType)));
            
            #line default
            #line hidden
            this.Write(", \r\n\t");
            
            #line 26 "C:\Projects\Linx\linx5-components\linx5-components-file\Code\File\TextFileRead\Templates\TextFileReadStringsAndLoop_Gen.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(CSharpUtilities.EnumAsString(codePage)));
            
            #line default
            #line hidden
            this.Write(", \r\n\t");
            
            #line 27 "C:\Projects\Linx\linx5-components\linx5-components-file\Code\File\TextFileRead\Templates\TextFileReadStringsAndLoop_Gen.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(skipHeaderLines));
            
            #line default
            #line hidden
            this.Write(",\r\n\t");
            
            #line 28 "C:\Projects\Linx\linx5-components\linx5-components-file\Code\File\TextFileRead\Templates\TextFileReadStringsAndLoop_Gen.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(skipFooterLines));
            
            #line default
            #line hidden
            this.Write(");\r\nreader.LogEvent += message => ");
            
            #line 29 "C:\Projects\Linx\linx5-components\linx5-components-file\Code\File\TextFileRead\Templates\TextFileReadStringsAndLoop_Gen.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(functionContextPropertyName));
            
            #line default
            #line hidden
            this.Write(".Log(message);\r\n\r\n");
            
            #line 31 "C:\Projects\Linx\linx5-components\linx5-components-file\Code\File\TextFileRead\Templates\TextFileReadStringsAndLoop_Gen.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(executionPathOutputName));
            
            #line default
            #line hidden
            this.Write(" = reader.Lines().Select(v=>\r\n\t\t\tnew Twenty57.Linx.Plugin.Common.CodeGeneration.N" +
                    "extResult(\r\n\t\t\t\t\"");
            
            #line 33 "C:\Projects\Linx\linx5-components\linx5-components-file\Code\File\TextFileRead\Templates\TextFileReadStringsAndLoop_Gen.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(resultsExecutionPathName));
            
            #line default
            #line hidden
            this.Write("\",\r\n\t\t\t\tnew ");
            
            #line 34 "C:\Projects\Linx\linx5-components\linx5-components-file\Code\File\TextFileRead\Templates\TextFileReadStringsAndLoop_Gen.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(pathOutputTypeName));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t\t\t{ \r\n\t\t\t\t\t");
            
            #line 36 "C:\Projects\Linx\linx5-components\linx5-components-file\Code\File\TextFileRead\Templates\TextFileReadStringsAndLoop_Gen.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(outputLineNumberName));
            
            #line default
            #line hidden
            this.Write(" = v.LineNumber,\r\n\t\t\t\t\t");
            
            #line 37 "C:\Projects\Linx\linx5-components\linx5-components-file\Code\File\TextFileRead\Templates\TextFileReadStringsAndLoop_Gen.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(outputLineContentsName));
            
            #line default
            #line hidden
            this.Write(" = v.LineContents\r\n\t\t\t\t}\r\n\t\t\t)\r\n);\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "C:\Projects\Linx\linx5-components\linx5-components-file\Code\File\TextFileRead\Templates\TextFileReadStringsAndLoop_Gen.tt"

private string _functionContextPropertyNameField;

/// <summary>
/// Access the functionContextPropertyName parameter of the template.
/// </summary>
private string functionContextPropertyName
{
    get
    {
        return this._functionContextPropertyNameField;
    }
}

private string _filePathPropertyNameField;

/// <summary>
/// Access the filePathPropertyName parameter of the template.
/// </summary>
private string filePathPropertyName
{
    get
    {
        return this._filePathPropertyNameField;
    }
}

private global::Twenty57.Linx.Components.File.TextFileRead.FileReadOptions _readTypeField;

/// <summary>
/// Access the readType parameter of the template.
/// </summary>
private global::Twenty57.Linx.Components.File.TextFileRead.FileReadOptions readType
{
    get
    {
        return this._readTypeField;
    }
}

private global::Twenty57.Linx.Components.File.Common.TextCodepage _codePageField;

/// <summary>
/// Access the codePage parameter of the template.
/// </summary>
private global::Twenty57.Linx.Components.File.Common.TextCodepage codePage
{
    get
    {
        return this._codePageField;
    }
}

private string _skipHeaderLinesField;

/// <summary>
/// Access the skipHeaderLines parameter of the template.
/// </summary>
private string skipHeaderLines
{
    get
    {
        return this._skipHeaderLinesField;
    }
}

private string _skipFooterLinesField;

/// <summary>
/// Access the skipFooterLines parameter of the template.
/// </summary>
private string skipFooterLines
{
    get
    {
        return this._skipFooterLinesField;
    }
}

private string _outputTypeNameField;

/// <summary>
/// Access the outputTypeName parameter of the template.
/// </summary>
private string outputTypeName
{
    get
    {
        return this._outputTypeNameField;
    }
}

private string _pathOutputTypeNameField;

/// <summary>
/// Access the pathOutputTypeName parameter of the template.
/// </summary>
private string pathOutputTypeName
{
    get
    {
        return this._pathOutputTypeNameField;
    }
}

private string _resultsExecutionPathNameField;

/// <summary>
/// Access the resultsExecutionPathName parameter of the template.
/// </summary>
private string resultsExecutionPathName
{
    get
    {
        return this._resultsExecutionPathNameField;
    }
}

private string _outputLineNumberNameField;

/// <summary>
/// Access the outputLineNumberName parameter of the template.
/// </summary>
private string outputLineNumberName
{
    get
    {
        return this._outputLineNumberNameField;
    }
}

private string _outputLineContentsNameField;

/// <summary>
/// Access the outputLineContentsName parameter of the template.
/// </summary>
private string outputLineContentsName
{
    get
    {
        return this._outputLineContentsNameField;
    }
}

private string _executionPathOutputNameField;

/// <summary>
/// Access the executionPathOutputName parameter of the template.
/// </summary>
private string executionPathOutputName
{
    get
    {
        return this._executionPathOutputNameField;
    }
}


/// <summary>
/// Initialize the template
/// </summary>
public virtual void Initialize()
{
    if ((this.Errors.HasErrors == false))
    {
bool functionContextPropertyNameValueAcquired = false;
if (this.Session.ContainsKey("functionContextPropertyName"))
{
    this._functionContextPropertyNameField = ((string)(this.Session["functionContextPropertyName"]));
    functionContextPropertyNameValueAcquired = true;
}
if ((functionContextPropertyNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("functionContextPropertyName");
    if ((data != null))
    {
        this._functionContextPropertyNameField = ((string)(data));
    }
}
bool filePathPropertyNameValueAcquired = false;
if (this.Session.ContainsKey("filePathPropertyName"))
{
    this._filePathPropertyNameField = ((string)(this.Session["filePathPropertyName"]));
    filePathPropertyNameValueAcquired = true;
}
if ((filePathPropertyNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("filePathPropertyName");
    if ((data != null))
    {
        this._filePathPropertyNameField = ((string)(data));
    }
}
bool readTypeValueAcquired = false;
if (this.Session.ContainsKey("readType"))
{
    this._readTypeField = ((global::Twenty57.Linx.Components.File.TextFileRead.FileReadOptions)(this.Session["readType"]));
    readTypeValueAcquired = true;
}
if ((readTypeValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("readType");
    if ((data != null))
    {
        this._readTypeField = ((global::Twenty57.Linx.Components.File.TextFileRead.FileReadOptions)(data));
    }
}
bool codePageValueAcquired = false;
if (this.Session.ContainsKey("codePage"))
{
    this._codePageField = ((global::Twenty57.Linx.Components.File.Common.TextCodepage)(this.Session["codePage"]));
    codePageValueAcquired = true;
}
if ((codePageValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("codePage");
    if ((data != null))
    {
        this._codePageField = ((global::Twenty57.Linx.Components.File.Common.TextCodepage)(data));
    }
}
bool skipHeaderLinesValueAcquired = false;
if (this.Session.ContainsKey("skipHeaderLines"))
{
    this._skipHeaderLinesField = ((string)(this.Session["skipHeaderLines"]));
    skipHeaderLinesValueAcquired = true;
}
if ((skipHeaderLinesValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("skipHeaderLines");
    if ((data != null))
    {
        this._skipHeaderLinesField = ((string)(data));
    }
}
bool skipFooterLinesValueAcquired = false;
if (this.Session.ContainsKey("skipFooterLines"))
{
    this._skipFooterLinesField = ((string)(this.Session["skipFooterLines"]));
    skipFooterLinesValueAcquired = true;
}
if ((skipFooterLinesValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("skipFooterLines");
    if ((data != null))
    {
        this._skipFooterLinesField = ((string)(data));
    }
}
bool outputTypeNameValueAcquired = false;
if (this.Session.ContainsKey("outputTypeName"))
{
    this._outputTypeNameField = ((string)(this.Session["outputTypeName"]));
    outputTypeNameValueAcquired = true;
}
if ((outputTypeNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("outputTypeName");
    if ((data != null))
    {
        this._outputTypeNameField = ((string)(data));
    }
}
bool pathOutputTypeNameValueAcquired = false;
if (this.Session.ContainsKey("pathOutputTypeName"))
{
    this._pathOutputTypeNameField = ((string)(this.Session["pathOutputTypeName"]));
    pathOutputTypeNameValueAcquired = true;
}
if ((pathOutputTypeNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("pathOutputTypeName");
    if ((data != null))
    {
        this._pathOutputTypeNameField = ((string)(data));
    }
}
bool resultsExecutionPathNameValueAcquired = false;
if (this.Session.ContainsKey("resultsExecutionPathName"))
{
    this._resultsExecutionPathNameField = ((string)(this.Session["resultsExecutionPathName"]));
    resultsExecutionPathNameValueAcquired = true;
}
if ((resultsExecutionPathNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("resultsExecutionPathName");
    if ((data != null))
    {
        this._resultsExecutionPathNameField = ((string)(data));
    }
}
bool outputLineNumberNameValueAcquired = false;
if (this.Session.ContainsKey("outputLineNumberName"))
{
    this._outputLineNumberNameField = ((string)(this.Session["outputLineNumberName"]));
    outputLineNumberNameValueAcquired = true;
}
if ((outputLineNumberNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("outputLineNumberName");
    if ((data != null))
    {
        this._outputLineNumberNameField = ((string)(data));
    }
}
bool outputLineContentsNameValueAcquired = false;
if (this.Session.ContainsKey("outputLineContentsName"))
{
    this._outputLineContentsNameField = ((string)(this.Session["outputLineContentsName"]));
    outputLineContentsNameValueAcquired = true;
}
if ((outputLineContentsNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("outputLineContentsName");
    if ((data != null))
    {
        this._outputLineContentsNameField = ((string)(data));
    }
}
bool executionPathOutputNameValueAcquired = false;
if (this.Session.ContainsKey("executionPathOutputName"))
{
    this._executionPathOutputNameField = ((string)(this.Session["executionPathOutputName"]));
    executionPathOutputNameValueAcquired = true;
}
if ((executionPathOutputNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("executionPathOutputName");
    if ((data != null))
    {
        this._executionPathOutputNameField = ((string)(data));
    }
}


    }
}


        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
    public class TextFileReadStringsAndLoop_GenBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
