using System.Collections.Generic;
using System.Windows;

namespace TestVentsCadLibrary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            VentsCadLibrary.VentsCad cad = new VentsCadLibrary.VentsCad
            {
                ConnectionToSQL = "Data Source = srvkb; Initial Catalog = SWPlusDB; Persist Security Info = True; User ID = sa; Password = PDMadmin; MultipleActiveResultSets = True",
                DestVaultName = "Tets_debag",
                VaultName = "Vents-PDM"
            };


            //  cad.BatchUnLock();
            
            var Spigot = "";
            cad.Spigot("20", "1301", "1201", out Spigot);


            MessageBox.Show(Spigot);
        }
    }
}
