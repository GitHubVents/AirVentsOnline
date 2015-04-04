using System;
using System.Windows;
using System.Windows.Controls;
using AirVentsCadWpf.AdminkaWindows;
using AirVentsCadWpf.DataControls;
using AirVentsCadWpf.MenuControls;
using System.Deployment.Application;
using AirVentsCadWpf.Properties;
using AirVentsCadWpf.Логирование;

namespace AirVentsCadWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        public MainWindow()
        {
            Логгер.Информация("Программа запущена ", "", "InitializeComponent", "MainWindow");
            InitializeComponent();
            Switcher.PageSwitcher = this;
            Switcher.Switch(new AuthenticatedUc());

            CurrentState.Content = State;
        }

        public void NavigateMenu(UserControl nextPage)
        {
            MenuControlsGrid.Children.Clear();
            MenuControlsGrid.Children.Add(nextPage);
        }

        public void NavigateData(UserControl nextPage)
        {
            DataControlsGrid.Children.Clear();
            DataControlsGrid.Children.Add(nextPage);
        }

        public void NavigateButton(UserControl nextPage)
        {
            DataControlsGrid.Children.Clear();
            DataControlsGrid.Children.Add(nextPage);
        }

      // private readonly GroupUserControl _groupUser = new GroupUserControl();

        private readonly SettingsW _settings = new SettingsW();

        //   readonly LogViewer _nlogViewer = new LogViewer();

        private void Label_MouseLeftButtonDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
          //  _groupUser.Visibility = Visibility.Visible;
            _settings.Visibility = Visibility.Visible;
            _settings.Activate();
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //OwnedWindow ownedWindow = new OwnedWindow();
            //ownedWindow.Owner = this;
            //ownedWindow.Closing += new System.ComponentModel.CancelEventHandler(ownedWindow_Closing);
            //ownedWindow.Show();
            // this.OwnedWindows.Count;
            Application.Current.Shutdown();
        }


        private void Label_MouseLeftButtonDown_2(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //  _nlogViewer.Visibility = Visibility.Visible;
        }


        //private static Version AssemblyVersion
        //{
        //    get { return ApplicationDeployment.CurrentDeployment.CurrentVersion; }
        //}

        public string PublishVersion
        {
            get
            {
                if (!ApplicationDeployment.IsNetworkDeployed) return  "Not Published";
                var ver = ApplicationDeployment.CurrentDeployment.CurrentVersion;
                return string.Format("{0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision);
            }
        }

        private void CurrentVersion_Initialized(object sender, EventArgs e)
        {
            Title = " AirVentsCAD v." + PublishVersion;
            CurrentVersion.Content = Title;
            //CurrentVersion.Content = AssemblyVersion.Major + "." + AssemblyVersion.Minor + "." + AssemblyVersion.Build + "." + AssemblyVersion.Revision;
        }

        private void Label_MouseLeftButtonDown_3(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Settings.Default.UserName = "";
            Settings.Default.Password = "";
            Settings.Default.Reset();
           // Properties.Settings.Save();
            Switcher.Switch(new AuthenticatedUc());
            Switcher.SwitchData(new UnitElement());
        }
        
        private void Window_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            CurrentState.Content = State;
        }

        static string State
        {
            get
            {
                return String.Format("Хранилище: {0}     База данных: {1}",
                    Settings.Default.TestPdmBaseName,
                    Settings.Default.ConnectionToSQL.Split(';')[1].Split('=')[1]);
            }
        }

        
    }
}
