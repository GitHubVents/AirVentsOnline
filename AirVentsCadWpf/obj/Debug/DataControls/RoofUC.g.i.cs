﻿#pragma checksum "..\..\..\DataControls\RoofUC.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "72DF4C30A8F8B306E5D2582E3359A5E7"
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
    /// RoofUc
    /// </summary>
    public partial class RoofUc : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 31 "..\..\..\DataControls\RoofUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox TypeOfRoof;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\DataControls\RoofUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox LenghtRoof;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\DataControls\RoofUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label HeightLabel;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\DataControls\RoofUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox WidthRoof;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\DataControls\RoofUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label WidthLabel;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\DataControls\RoofUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BuildRoof;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\DataControls\RoofUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image PictureRoof;
        
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
            System.Uri resourceLocater = new System.Uri("/AirVentsCad;component/datacontrols/roofuc.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\DataControls\RoofUC.xaml"
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
            
            #line 7 "..\..\..\DataControls\RoofUC.xaml"
            ((System.Windows.Controls.Grid)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Grid_Loaded_1);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TypeOfRoof = ((System.Windows.Controls.ComboBox)(target));
            
            #line 31 "..\..\..\DataControls\RoofUC.xaml"
            this.TypeOfRoof.LayoutUpdated += new System.EventHandler(this.TypeOfRoof_LayoutUpdated);
            
            #line default
            #line hidden
            return;
            case 3:
            this.LenghtRoof = ((System.Windows.Controls.TextBox)(target));
            
            #line 46 "..\..\..\DataControls\RoofUC.xaml"
            this.LenghtRoof.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            
            #line 46 "..\..\..\DataControls\RoofUC.xaml"
            this.LenghtRoof.KeyDown += new System.Windows.Input.KeyEventHandler(this.LenghtRoof_KeyDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.HeightLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.WidthRoof = ((System.Windows.Controls.TextBox)(target));
            
            #line 48 "..\..\..\DataControls\RoofUC.xaml"
            this.WidthRoof.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            
            #line 48 "..\..\..\DataControls\RoofUC.xaml"
            this.WidthRoof.KeyDown += new System.Windows.Input.KeyEventHandler(this.WidthRoof_KeyDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.WidthLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.BuildRoof = ((System.Windows.Controls.Button)(target));
            
            #line 57 "..\..\..\DataControls\RoofUC.xaml"
            this.BuildRoof.Click += new System.Windows.RoutedEventHandler(this.BuildRoof_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.PictureRoof = ((System.Windows.Controls.Image)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

