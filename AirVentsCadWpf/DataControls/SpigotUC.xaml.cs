using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using ModelSw = AirVentsCadWpf.AirVentsClasses.UnitsBuilding.ModelSw;
namespace AirVentsCadWpf.DataControls
{
    /// <summary>
    /// Interaction logic for SpigotUC.xaml
    /// </summary>
    public partial class SpigotUc
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpigotUc"/> class.
        /// </summary>
        public SpigotUc()
        {
            InitializeComponent();
        }

        private void BuildSpigot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //VentsCadLibrary.VentsCad vcad = new VentsCadLibrary.VentsCad();
                //var unit = "";
                //vcad.SpigotStr(TypeOfSpigot.Text, WidthSpigot.Text, HeightSpigot.Text, out unit);
                //MessageBox.Show(unit);
                var sw = new ModelSw();
                sw.Spigot(TypeOfSpigot.Text, WidthSpigot.Text, HeightSpigot.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            //var sw = new ModelSw();
            //sw.CreateDistDirectory(string.Format(@"{0}\{1}", @Properties.Settings.Default.DestinationFolder, sw.SpigotDestinationFolder));
        }

        private void WidthSpigot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BuildSpigot_Click(this, new RoutedEventArgs());
            }
        }

        private void HeightSpigot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BuildSpigot_Click(this, new RoutedEventArgs());
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var sw = new ModelSw();
            //sw.Mamba();
        }
    }
}
