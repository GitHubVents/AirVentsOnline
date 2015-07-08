using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using ModelSw = AirVentsCadWpf.AirVentsClasses.UnitsBuilding.ModelSw;

namespace AirVentsCadWpf.DataControls
{
    /// <summary>
    /// Interaction logic for DamperUC.xaml
    /// </summary>
    public partial class DamperUc
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DamperUc"/> class.
        /// </summary>
        public DamperUc()
        {
            InitializeComponent();
        }

        private void BuildDamper_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var sw = new ModelSw();
                sw.Dumper(TypeOfDumper.Text, WidthDamper.Text, HeightDamper.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, ex.Message);
            }
        }

        private void Grid_Loaded_2(object sender, RoutedEventArgs e)
        {
            //var sw = new ModelSw();
            //sw.CreateDistDirectory(string.Format(@"{0}\{1}", @Properties.Settings.Default.DestinationFolder, sw.DamperDestinationFolder));
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var sw = new Bom();// Unit50Sw();//C:\Tets_debag\Vents-PDM\Проекты\Blauberg\02 - Панели\02-01-654-849-50-Az-Az-MW.SLDASM
            //  sw.RegistrationPdm(@"C:\Tets_debag\Vents-PDM\Проекты\Blauberg\02 - Панели\02-01-654-849-50-Az-Az-MW.SLDASM", true, Properties.Settings.Default.TestPdmBaseName);
        }

        private void GetLast_Click(object sender, RoutedEventArgs e)
        {
            var sw = new ModelSw();
           // sw.GetLastVersionPdm1(@"D:\Vents-PDM\Библиотека проектирования\DriveWorks\02 - Panels\02-01.SLDASM", 0);
        }

        private void Register1_Click(object sender, RoutedEventArgs e)
        {
            var sw = new ModelSw();
            //sw.GetLastVersionPdm1(@"D:\Vents-PDM\Библиотека проектирования\DriveWorks\02 - Panels\02-01.SLDASM", 1);
            
        }

        private void Register2_Click(object sender, RoutedEventArgs e)
        {
            var sw = new ModelSw();
            //sw.GetLastVersionPdmAsm(@"C:\Tets_debag\Vents-PDM\Проекты\Blauberg\11 - Регулятор расхода воздуха\11-20-954-654.SLDASM");
        }

        private void WidthDamper_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BuildDamper_Click(this, new RoutedEventArgs());
            }
        }

        private void HeightDamper_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BuildDamper_Click(this, new RoutedEventArgs());
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
