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
            VentsCadLibrary.VentsCad svb = new VentsCadLibrary.VentsCad
            {
                ConnectionToSQL = "Data Source = srvkb; Initial Catalog = SWPlusDB; Persist Security Info = True; User ID = sa; Password = PDMadmin; MultipleActiveResultSets = True",
                DestVaultName = "Tets_debag",
                VaultName = "Vents-PDM"
            };                                                          
            var sdg = "";
            svb.Spigot("20", "1077", "803", out sdg);
            
            //var PdmBaseName = "wef";

            MessageBox.Show(sdg);
        }
    }
}
