using System.Windows;

namespace AirVentsCadWpf.DataControls.Specification
{

    /// <summary>
    /// 
    /// </summary>
    public partial class SpecificationBomUc
    {
        readonly BomFormat _bomFormat = new BomFormat();
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecificationUc"/> class.
        /// </summary>
        public SpecificationBomUc()
        {
            InitializeComponent();

            BomTablePrt2.Visibility = Visibility.Hidden;
        }

        private void ВыгрузитьВXml_Click(object sender, RoutedEventArgs e)
        {
            ТаблицаСпецификации.ItemsSource = _bomFormat.ListXmlBomData(@"C:\Program Files\SW-Complex\SP-Temp.xml");
            _bomFormat.InsertBomTable();

            var i = 1;
            foreach (var bomDatas in _bomFormat.BomByPage())
            {
                ТаблицаСпецификации.ItemsSource = bomDatas;
                MessageBox.Show("Страница " + i);
                i++;
            }
            
        }
     



    }
}
