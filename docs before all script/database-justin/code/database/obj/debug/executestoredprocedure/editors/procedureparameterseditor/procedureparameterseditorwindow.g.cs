﻿#pragma checksum "..\..\..\..\..\ExecuteStoredProcedure\Editors\ProcedureParametersEditor\ProcedureParametersEditorWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "48F3C9A41D26FF37B076371D7D2E6A59"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using GongSolutions.Wpf.DragDrop;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.ProcedureParametersEditor.Validators;
using Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.ProcedureParametersEditor.ViewModel;
using Twenty57.Linx.Components.Database.UI.Converters;
using Twenty57.Linx.Plugin.UI.Helpers;
using Twenty57.Linx.Plugin.UI.Windows;


namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.ProcedureParametersEditor {
    
    
    /// <summary>
    /// ProcedureParametersEditorWindow
    /// </summary>
    public partial class ProcedureParametersEditorWindow : Twenty57.Linx.Plugin.UI.Windows.CustomWindow, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 41 "..\..\..\..\..\ExecuteStoredProcedure\Editors\ProcedureParametersEditor\ProcedureParametersEditorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock NameHeader;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\..\..\ExecuteStoredProcedure\Editors\ProcedureParametersEditor\ProcedureParametersEditorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock DirectionHeader;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\..\ExecuteStoredProcedure\Editors\ProcedureParametersEditor\ProcedureParametersEditorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TypeHeader;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Twenty57.Linx.Components.Database;component/executestoredprocedure/editors/proce" +
                    "dureparameterseditor/procedureparameterseditorwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\ExecuteStoredProcedure\Editors\ProcedureParametersEditor\ProcedureParametersEditorWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.NameHeader = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.DirectionHeader = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.TypeHeader = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            
            #line 46 "..\..\..\..\..\ExecuteStoredProcedure\Editors\ProcedureParametersEditor\ProcedureParametersEditorWindow.xaml"
            ((System.Windows.Controls.ListView)(target)).KeyUp += new System.Windows.Input.KeyEventHandler(this.ListView_KeyUp);
            
            #line default
            #line hidden
            
            #line 46 "..\..\..\..\..\ExecuteStoredProcedure\Editors\ProcedureParametersEditor\ProcedureParametersEditorWindow.xaml"
            ((System.Windows.Controls.ListView)(target)).Loaded += new System.Windows.RoutedEventHandler(this.ListView_Loaded);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            System.Windows.EventSetter eventSetter;
            switch (connectionId)
            {
            case 5:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.GotKeyboardFocusEvent;
            
            #line 68 "..\..\..\..\..\ExecuteStoredProcedure\Editors\ProcedureParametersEditor\ProcedureParametersEditorWindow.xaml"
            eventSetter.Handler = new System.Windows.Input.KeyboardFocusChangedEventHandler(this.TextBox_GotKeyboardFocus);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            case 6:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.GotKeyboardFocusEvent;
            
            #line 113 "..\..\..\..\..\ExecuteStoredProcedure\Editors\ProcedureParametersEditor\ProcedureParametersEditorWindow.xaml"
            eventSetter.Handler = new System.Windows.Input.KeyboardFocusChangedEventHandler(this.TextBox_GotKeyboardFocus);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            }
        }
    }
}

