﻿#pragma checksum "..\..\..\DataControls\DamperUC.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "32D450B4BF4E58C2EF6CCEAC46BAA6F9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace AirVentsCadWpf.DataControls {
    
    
    /// <summary>
    /// DamperUc
    /// </summary>
    public partial class DamperUc : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 31 "..\..\..\DataControls\DamperUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox TypeOfDumper;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\DataControls\DamperUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox HeightDamper;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\DataControls\DamperUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label HeightLabel;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\DataControls\DamperUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox WidthDamper;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\DataControls\DamperUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label WidthLabel;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\DataControls\DamperUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BuildDamper;
        
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
            System.Uri resourceLocater = new System.Uri("/AirVentsCad;component/datacontrols/damperuc.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\DataControls\DamperUC.xaml"
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
            
            #line 22 "..\..\..\DataControls\DamperUC.xaml"
            ((System.Windows.Controls.Grid)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Grid_Loaded_2);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TypeOfDumper = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 3:
            this.HeightDamper = ((System.Windows.Controls.TextBox)(target));
            
            #line 42 "..\..\..\DataControls\DamperUC.xaml"
            this.HeightDamper.KeyDown += new System.Windows.Input.KeyEventHandler(this.HeightDamper_KeyDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.HeightLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.WidthDamper = ((System.Windows.Controls.TextBox)(target));
            
            #line 44 "..\..\..\DataControls\DamperUC.xaml"
            this.WidthDamper.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            
            #line 44 "..\..\..\DataControls\DamperUC.xaml"
            this.WidthDamper.KeyDown += new System.Windows.Input.KeyEventHandler(this.WidthDamper_KeyDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.WidthLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.BuildDamper = ((System.Windows.Controls.Button)(target));
            
            #line 52 "..\..\..\DataControls\DamperUC.xaml"
            this.BuildDamper.Click += new System.Windows.RoutedEventHandler(this.BuildDamper_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

