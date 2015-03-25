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
        }

        private void ВыгрузитьВXml_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ТаблицаСпецификации.ItemsSource = _bomFormat.ListXmlBomData(@"C:\Program Files\SW-Complex\SP-Temp.xml");
            _bomFormat.InsertBomTable();
        }
     



    }
}
