﻿#pragma checksum "..\..\..\..\Mongo\Editors\TemplateTreeView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "68FF96455150CC89F78A4C17C7D96D45"
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
using Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor;
using Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.Converters;
using Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.ViewModel;


namespace Twenty57.Linx.Components.MongoDB.Editors {
    
    
    /// <summary>
    /// TemplateTreeView
    /// </summary>
    public partial class TemplateTreeView : System.Windows.Controls.TabItem, System.Windows.Markup.IComponentConnector {
        
        
        #line 20 "..\..\..\..\Mongo\Editors\TemplateTreeView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView TreeView;
        
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
            System.Uri resourceLocater = new System.Uri("/Twenty57.Linx.Components.Database;component/mongo/editors/templatetreeview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Mongo\Editors\TemplateTreeView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
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
            this.TreeView = ((System.Windows.Controls.TreeView)(target));
            
            #line 20 "..\..\..\..\Mongo\Editors\TemplateTreeView.xaml"
            this.TreeView.LostFocus += new System.Windows.RoutedEventHandler(this.OnLostFocus);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

