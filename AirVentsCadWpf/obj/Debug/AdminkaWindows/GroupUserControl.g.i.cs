﻿#pragma checksum "..\..\..\AdminkaWindows\GroupUserControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "253A6089CBCF69D9D2C5936A0B166D3A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18063
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


namespace AirVentsCadWpf.AdminkaWindows {
    
    
    /// <summary>
    /// GroupUserControl
    /// </summary>
    public partial class GroupUserControl : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\AdminkaWindows\GroupUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NewGroupName;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\AdminkaWindows\GroupUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid Groups;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\AdminkaWindows\GroupUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid GroupUsers;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\AdminkaWindows\GroupUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button DeleteGroupButton;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\AdminkaWindows\GroupUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddGroupButton;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\AdminkaWindows\GroupUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddUser;
        
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
            System.Uri resourceLocater = new System.Uri("/AirVentsCad;component/adminkawindows/groupusercontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\AdminkaWindows\GroupUserControl.xaml"
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
            
            #line 4 "..\..\..\AdminkaWindows\GroupUserControl.xaml"
            ((AirVentsCadWpf.AdminkaWindows.GroupUserControl)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing_2);
            
            #line default
            #line hidden
            
            #line 4 "..\..\..\AdminkaWindows\GroupUserControl.xaml"
            ((AirVentsCadWpf.AdminkaWindows.GroupUserControl)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded_1);
            
            #line default
            #line hidden
            
            #line 4 "..\..\..\AdminkaWindows\GroupUserControl.xaml"
            ((AirVentsCadWpf.AdminkaWindows.GroupUserControl)(target)).Activated += new System.EventHandler(this.Window_Activated_1);
            
            #line default
            #line hidden
            return;
            case 2:
            this.NewGroupName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.Groups = ((System.Windows.Controls.DataGrid)(target));
            
            #line 19 "..\..\..\AdminkaWindows\GroupUserControl.xaml"
            this.Groups.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Groups_SelectionChanged);
            
            #line default
            #line hidden
            
            #line 19 "..\..\..\AdminkaWindows\GroupUserControl.xaml"
            this.Groups.SelectedCellsChanged += new System.Windows.Controls.SelectedCellsChangedEventHandler(this.Groups_SelectedCellsChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.GroupUsers = ((System.Windows.Controls.DataGrid)(target));
            
            #line 21 "..\..\..\AdminkaWindows\GroupUserControl.xaml"
            this.GroupUsers.SelectedCellsChanged += new System.Windows.Controls.SelectedCellsChangedEventHandler(this.GroupUsers_SelectedCellsChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.DeleteGroupButton = ((System.Windows.Controls.Button)(target));
            
            #line 23 "..\..\..\AdminkaWindows\GroupUserControl.xaml"
            this.DeleteGroupButton.Click += new System.Windows.RoutedEventHandler(this.DeleteGroupButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.AddGroupButton = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\..\AdminkaWindows\GroupUserControl.xaml"
            this.AddGroupButton.Click += new System.Windows.RoutedEventHandler(this.AddGroupButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 25 "..\..\..\AdminkaWindows\GroupUserControl.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_1);
            
            #line default
            #line hidden
            return;
            case 8:
            this.AddUser = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\..\AdminkaWindows\GroupUserControl.xaml"
            this.AddUser.Click += new System.Windows.RoutedEventHandler(this.AddUser_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

