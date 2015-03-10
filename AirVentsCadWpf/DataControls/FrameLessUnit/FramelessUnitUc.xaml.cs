using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
//using AirVentsCadWpf.AirVentsCadService;
using AirVentsCadWpf.AirVentsClasses;
using AirVentsCadWpf.Properties;
using AirVentsCadWpf.Логирование;
using JetBrains.Annotations;
using VentsMaterials;
using ModelSw = AirVentsCadWpf.AirVentsClasses.UnitsBuilding.ModelSw;

namespace AirVentsCadWpf.DataControls.FrameLessUnit
{
    /// <summary>
    /// Interaction logic for FramelessUnitUc.xaml
    /// </summary>
    public partial class FramelessUnitUc
    {
        readonly SqlBaseData _sqlBaseData = new SqlBaseData();
        readonly SetMaterials _setMaterials = new SetMaterials();
        readonly ToSQL _toSql = new ToSQL();

        


        bool _isDeveloper = false;

        private readonly DispatcherTimer _dispatcherTimer;
       
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FramelessUnitUc"/> class.
        /// </summary>
        public FramelessUnitUc()
        {
            InitializeComponent();

            ToSQL.Conn = Settings.Default.ConnectionToSQL;

            Усиления.ItemsSource = new List<ComboBoxItem>
            {
                new ComboBoxItem {ToolTip = "Отсутствует", Content = "-"},
                new ComboBoxItem {ToolTip = "Нижней", Content = "Н"},
                new ComboBoxItem {ToolTip = "Нижней\nВерхней", Content = "НВ"},
                new ComboBoxItem {ToolTip = "Нижней\nЗадней", Content = "НЗ"},
                new ComboBoxItem {ToolTip = "Нижней\nВерхней\nЗадней", Content = "НВЗ"},
                new ComboBoxItem {ToolTip = "Нижней\nВерхней\nЗадней\nСъемной(ых)", Content = "НВЗС"}
            };

            //ТипУстановкиПромежуточныеВставки.ItemsSource = new List<ComboBoxItem>
            //{
            //    new ComboBoxItem {ToolTip = "Отсутствует", Content = "-"},
            //    new ComboBoxItem {ToolTip = "Нижней", Content = "Усиления по бокам"},
            //    new ComboBoxItem {ToolTip = "Нижней\nВерхней", Content = "НВ"},
            //    new ComboBoxItem {ToolTip = "Нижней\nЗадней", Content = "НЗ"},
            //    new ComboBoxItem {ToolTip = "Нижней\nВерхней\nЗадней", Content = "НВЗ"},
            //    new ComboBoxItem {ToolTip = "Нижней\nВерхней\nЗадней\nСъемной(ых)", Content = "НВЗС"}
            //};


            ТипПанели.ItemsSource = new List<ComboBoxItem>
            {
                new ComboBoxItem {ToolTip = "Верхняя (для внутренней установки)", Content = "21"},
                new ComboBoxItem {ToolTip = "Верхняя (для установки с односкатной крышей)", Content = "22"},
                new ComboBoxItem {ToolTip = "Верхняя (для установки с двускатной крышей)", Content = "23"},
                new ComboBoxItem {ToolTip = "Съемная (панель простая)", Content = "04"},
                new ComboBoxItem {ToolTip = "Съемная (панель теплообменника)", Content = "05"},
                new ComboBoxItem {ToolTip = "Несъемная (панель простая)", Content = "01"},
                new ComboBoxItem {ToolTip = "Нижняя (для внутренней установки)", Content = "30"},
                new ComboBoxItem {ToolTip = "Нижняя (для установки с монтажной рамой)", Content = "31"},
                new ComboBoxItem {ToolTip = "Нижняя (для установки с монтажными ножками)", Content = "32"}
            }.OrderBy(x=>x.Content);

            ТипУстановки.ItemsSource = new List<ComboBoxItem>
            {
                new ComboBoxItem {ToolTip = "01", Content = "Одна съемная панель"},
                new ComboBoxItem {ToolTip = "02", Content = "Две съемных панели"},
                new ComboBoxItem {ToolTip = "03", Content = "Три съемных панели"},
            };//.OrderBy(x => x.Content);

            КолвоПанелей.ItemsSource = new List<ComboBoxItem>
            {
                new ComboBoxItem {Content = "1"},
                new ComboBoxItem {Content = "2"},
                new ComboBoxItem {Content = "3"}
            };

            ТолщинаВнешней.ItemsSource = new List<ComboBoxItem>
            {
                new ComboBoxItem {Content = "0.5"},
                new ComboBoxItem {Content = "0.6"},
                new ComboBoxItem {Content = "0.8"},
                new ComboBoxItem {Content = "1.0"},
                new ComboBoxItem {Content = "1.2"}
            };
            ТолщинаВнешней.SelectedIndex = 2;

            ТолщинаВннутренней.ItemsSource = new List<ComboBoxItem>
            {
                new ComboBoxItem {Content = "0.5"},
                new ComboBoxItem {Content = "0.6"},
                new ComboBoxItem {Content = "0.8"},
                new ComboBoxItem {Content = "1.0"},
                new ComboBoxItem {Content = "1.2"}
            };
            ТолщинаВннутренней.SelectedIndex = 2;

            SectionTextBox.ItemsSource = new[]
            {
                "A", "B", "C", "D", "E", "F", "G", "H",
                "I", "J", "K", "L", "M", "N", "O", "P",
                "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
            };

            MaterialP1.ItemsSource = ((IListSource)_sqlBaseData.MaterialsTable()).GetList();
            MaterialP1.DisplayMemberPath = "MaterialsName";
            MaterialP1.SelectedValuePath = "LevelID";
            MaterialP1.SelectedIndex = 0;

            MaterialP2.ItemsSource = ((IListSource)_sqlBaseData.MaterialsTable()).GetList();
            MaterialP2.DisplayMemberPath = "MaterialsName";
            MaterialP2.SelectedValuePath = "LevelID";
            MaterialP2.SelectedIndex = 0;
            

            Ral1.ItemsSource = ((IListSource)_toSql.RalTable()).GetList();
            Ral1.DisplayMemberPath = "RAL";
            Ral1.SelectedValuePath = "Hex";
            Ral1.SelectedIndex = 0;

            Ral2.ItemsSource = ((IListSource)_toSql.RalTable()).GetList();
            Ral2.DisplayMemberPath = "RAL";
            Ral2.SelectedValuePath = "Hex";
            Ral2.SelectedIndex = 0;

            Ral1.Visibility = Visibility.Hidden;
            Ral2.Visibility = Visibility.Hidden;


            Table.ItemsSource = ((IListSource)_toSql.RalTable()).GetList();

            CoatingType1.ItemsSource = ((IListSource)_setMaterials.CoatingTypeDt()).GetList();
            CoatingType1.DisplayMemberPath = "Name";
            CoatingType1.SelectedValuePath = "Code";
            CoatingType1.SelectedIndex = 0;

            CoatingType2.ItemsSource = ((IListSource)_setMaterials.CoatingTypeDt()).GetList();
            CoatingType2.DisplayMemberPath = "Name";
            CoatingType2.SelectedValuePath = "Code";
            CoatingType2.SelectedIndex = 0;

            CoatingClass1.ItemsSource = _setMaterials.CoatingListClass();
            CoatingClass2.ItemsSource = _setMaterials.CoatingListClass();

            Шумоизоляция.ItemsSource = ((IListSource)_sqlBaseData.PanelsTable("23")).GetList();
            Шумоизоляция.DisplayMemberPath = "PanelTypeName";
            Шумоизоляция.SelectedValuePath = "PanelTypeCode";
            Шумоизоляция.SelectedIndex = 0;

            КришаТип.ItemsSource = ((IListSource)_sqlBaseData.PanelsTable("3")).GetList();
            КришаТип.DisplayMemberPath = "PanelTypeName";
            КришаТип.SelectedValuePath = "PanelTypeCode";
            КришаТип.SelectedIndex = 0;

            ОпорнаяЧастьТип.ItemsSource = ((IListSource)_sqlBaseData.PanelsTable("1")).GetList();
            ОпорнаяЧастьТип.DisplayMemberPath = "PanelTypeName";
            ОпорнаяЧастьТип.SelectedValuePath = "PanelTypeCode";
            ОпорнаяЧастьТип.SelectedIndex = 0;

            ТипСъемнойПанели1.ItemsSource = ((IListSource)_sqlBaseData.PanelsTable("2")).GetList();
            ТипСъемнойПанели1.DisplayMemberPath = "PanelTypeName";
            ТипСъемнойПанели1.SelectedValuePath = "PanelTypeCode";
            ТипСъемнойПанели1.SelectedIndex = 0;

            ТипСъемнойПанели2.ItemsSource = ((IListSource)_sqlBaseData.PanelsTable("2")).GetList();
            ТипСъемнойПанели2.DisplayMemberPath = "PanelTypeName";
            ТипСъемнойПанели2.SelectedValuePath = "PanelTypeCode";
            ТипСъемнойПанели2.SelectedIndex = 0;

            ТипСъемнойПанели3.ItemsSource = ((IListSource)_sqlBaseData.PanelsTable("2")).GetList();
            ТипСъемнойПанели3.DisplayMemberPath = "PanelTypeName";
            ТипСъемнойПанели3.SelectedValuePath = "PanelTypeCode";
            ТипСъемнойПанели3.SelectedIndex = 0;

            ТипНесъемнойПанели.ItemsSource = ((IListSource)_sqlBaseData.PanelsTable("2")).GetList();
            ТипНесъемнойПанели.DisplayMemberPath = "PanelTypeName";
            ТипНесъемнойПанели.SelectedValuePath = "PanelTypeCode";
            ТипНесъемнойПанели.SelectedIndex = 3;

            ПрименениеСкотча.ItemsSource = ((IListSource)_sqlBaseData.PanelsTable("25")).GetList();
            ПрименениеСкотча.DisplayMemberPath = "PanelTypeName";
            ПрименениеСкотча.SelectedValuePath = "PanelTypeCode";
            ПрименениеСкотча.SelectedIndex = 0;

            #region UnitFramelessFULL

            Lenght.MaxLength = 5;
            WidthU.MaxLength = 5;
            WidthU.IsReadOnly = true;
            HeightU.MaxLength = 5;
            HeightU.IsReadOnly = true;
            Lenght.MaxLength = 5;
            
            SizeOfUnit.ItemsSource = ((IListSource) _sqlBaseData.AirVentsStandardSize()).GetList();
            SizeOfUnit.DisplayMemberPath = "Type"; //SizeID//Type
            //SizeOfUnit.SelectedValuePath = "Type";
            SizeOfUnit.SelectedIndex = 0;

            #endregion

            WidthU.IsEnabled = false;
            HeightU.IsEnabled = false;


            Table.Visibility = Visibility.Collapsed;
            ПанелиУстановки.Visibility = Visibility.Collapsed;

            ДлинаLabel.Visibility = Visibility.Collapsed;

            #region Кол-во панелей

            Ширина2.Visibility = Visibility.Collapsed;
            Ширина3.Visibility = Visibility.Collapsed;

            ШиринаПанели2.Visibility = Visibility.Collapsed;
            ШиринаПанели3.Visibility = Visibility.Collapsed;

            Отступ2L.Visibility = Visibility.Collapsed;
            Отступ3L.Visibility = Visibility.Collapsed;

            Отступ2.Visibility = Visibility.Collapsed;
            Отступ3.Visibility = Visibility.Collapsed;

            КолвоПанелей11.Visibility = Visibility.Hidden;
            КолвоПанелей1.Visibility = Visibility.Hidden;

            #endregion

            Левая.IsChecked = true;


            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += (dispatcherTimer_Tick);
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            Visibilyty = 1;
            // _dispatcherTimer.Start();
        }

        private int Visibilyty {

            get;
            set;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //Visibilyty = Visibilyty + 1;
            //_thisControl.Opacity = Convert.ToDouble(Visibilyty/100);
            //if (Visibilyty == 100)
            //{
            //    _dispatcherTimer.Stop();
            //    MessageBox.Show("Stop");
            //}
        }

        void SizeOfUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SizeOfUnit == null || SizeOfUnit.SelectedItem == null) return;

            // var item = ((DataRowView)SizeOfUnit.SelectedItem)["Type"].ToString();
            var id = Convert.ToInt32(((DataRowView) SizeOfUnit.SelectedItem)["SizeID"].ToString());
            var sqlBaseData = new SqlBaseData();
            var standartUnitSizes = sqlBaseData.StandartSize(id, 6);

            if (WidthU == null || HeightU == null)
            {
                return;
            }
            WidthU.Text = standartUnitSizes[0];
            HeightU.Text = standartUnitSizes[1];

           

        }

        void nonstandard_Checked(object sender, RoutedEventArgs e)
        {
            WidthU.IsReadOnly = false;
            HeightU.IsReadOnly = false;
            WidthU.IsEnabled = true;
            HeightU.IsEnabled = true;
            WidthLabel.Visibility = Visibility.Visible;
            WidthU.Visibility = Visibility.Visible;
            HeightLabel.Visibility = Visibility.Visible;
            HeightU.Visibility = Visibility.Visible;
        }

        void nonstandard_Unchecked(object sender, RoutedEventArgs e)
        {
            SizeOfUnit_SelectionChanged(null,null);
            WidthU.IsReadOnly = true;
            HeightU.IsReadOnly = true;
            WidthU.IsEnabled = false;
            HeightU.IsEnabled = false;
        }

        void BUILDING_Click(object sender, RoutedEventArgs e)
        {

            if (ШиринаСъемнойПанели2.Visibility == Visibility.Visible)
            {
                if (ШиринаСъемнойПанели2.Text == "")
                {
                    MessageBox.Show("Введите ширину 2-й съемной панели");
                    return;
                }
                if (ШиринаСъемнойПанели2.Text.Contains("-"))
                {
                    MessageBox.Show("Ширина 2-й съемной панели не может быть отрицательной!");
                    return;
                }
                if (Convert.ToInt32(ШиринаСъемнойПанели2.Text) < 100)
                {
                    MessageBox.Show("Ширина 2-й съемной панели не может быть меньше 100 мм!"); return;
                }
            }


            if (ШиринаСъемнойПанели3.Visibility == Visibility.Visible)
            {
                if (ШиринаСъемнойПанели3.Text == "")
                {
                    MessageBox.Show("Введите ширину 3-й съемной панели"); return;
                }
                if (ШиринаСъемнойПанели3.Text.Contains("-"))
                {
                    MessageBox.Show("Ширина 3-й съемной панели не может быть отрицательной!"); return;
                }
                if (Convert.ToInt32(ШиринаСъемнойПанели3.Text) < 100)
                {
                    MessageBox.Show("Ширина 3-й съемной панели не может быть меньше 100 мм!"); return;
                }
            }

            if (Convert.ToInt32(ШиринаСъемнойПанели1.Text) < 100)
            {
                MessageBox.Show("Ширина съемной панели не может быть меньше 100 мм!"); return;
            }

            Логгер.Информация(
                "Начато построение бескаркасной установки",
                "",
                "Сохранение настроек программы",
                "FramelessUnitUc");

            try
            {
                var modelSw = new ModelSw();

                var width = Convert.ToDouble(WidthU.Text);
                var height = Convert.ToDouble(HeightU.Text);
                var lenght = Convert.ToDouble(Lenght.Text);

                #region Панели

                var materialP1 = new[] { MaterialP1.SelectedValue.ToString(), ТолщинаВнешней.Text };
                var materialP2 = new[] { MaterialP2.SelectedValue.ToString(), ТолщинаВннутренней.Text,
                    Шумоизоляция.SelectedValue.ToString(),
                ПрименениеСкотча.SelectedValue.ToString()};
               
                var panelUp = "";
                var panelDown = "";
                var panelFixed = "";
                
                var config = Левая.IsChecked == true ? "01" : "02";
                var configDown = Левая.IsChecked == true ? "02" : "01";
                var сторонаОбслуживания = Левая.IsChecked == true ? "левая" : "правая";

                const string noBrush = "Без покрытия";

                try
                {
                    //const string paneltype = "Панель верхняя";
                    panelUp = modelSw.PanelsFramelessStr(
                        typeOfPanel: new[] { ТипВерхнейПанели.Text, КришаТип.Text },
                        width: Convert.ToString(lenght),
                        height: Convert.ToString(width),
                        materialP1: materialP1, 
                        materialP2: materialP2,
                        скотч: ПрименениеСкотча.Text,// == "Со скотчем",
                        усиление: PanelUpChk.IsChecked == true,
                        config: config,
                        расположениеПанелей: PanelsConfig(),
                        покрытие: new[]
                        {
                            Ral1.Text, Ral1.Text != noBrush ? CoatingType1.Text : "0", Ral1.Text != noBrush ? CoatingClass1.Text : "0",
                            Ral2.Text, Ral2.Text != noBrush ? CoatingType2.Text : "0", Ral2.Text != noBrush ? CoatingClass2.Text : "0",
                            Ral1.SelectedValue != null ? Ral1.SelectedValue.ToString():noBrush,
                            Ral2.SelectedValue != null ? Ral2.SelectedValue.ToString():noBrush
                        },
                        расположениеВставок: ProfilsConfig());
                }
                catch (Exception ex)
                {
                    Логгер.Ошибка(
                    ex.Message,
                    ex.StackTrace,
                    ex.TargetSite.ToString(),
                    "FramelessUnitUc");
                }


                try
                {
                    //const string paneltype = "Панель нижняя под монтажные ножки";
                    panelDown = modelSw.PanelsFramelessStr(
                        typeOfPanel: new[] { ТипПанелиНижней.Text, ОпорнаяЧастьТип.Text },
                        width: Convert.ToString(lenght),
                        height: Convert.ToString(width),
                        materialP1: materialP1,
                        materialP2: materialP2,
                        скотч: ПрименениеСкотча.Text,// == "Со скотчем",
                        усиление: PanelDownChk.IsChecked == true,
                        config: configDown,
                        расположениеПанелей: PanelsConfig(),
                        покрытие: new[]
                        {
                            Ral1.Text, Ral1.Text != noBrush ? CoatingType1.Text : "0", Ral1.Text != noBrush ? CoatingClass1.Text : "0",
                            Ral2.Text, Ral2.Text != noBrush ? CoatingType2.Text : "0", Ral2.Text != noBrush ? CoatingClass2.Text : "0",
                            Ral1.SelectedValue != null ? Ral1.SelectedValue.ToString():noBrush,
                            Ral2.SelectedValue != null ? Ral2.SelectedValue.ToString():noBrush
                        },
                        расположениеВставок: ProfilsConfig());
                }
                catch (Exception ex)
                {
                    Логгер.Ошибка(
                    ex.Message,
                    ex.StackTrace,
                    ex.TargetSite.ToString(),
                    "FramelessUnitUc");
                }
                
                var panelRemovable = new []{""};

                try
                {
                    //"Съемная";
                    var panelRemovable1 = modelSw.PanelsFramelessStr(
                        typeOfPanel: new[] { ТипСъемнойПанели1.SelectedValue.ToString(), ТипСъемнойПанели1.Text },
                        width: Convert.ToString(ШиринаСъемнойПанели1.Text),
                        height: Convert.ToString(height - 40),
                        materialP1: materialP1,
                        materialP2: materialP2,
                        скотч: ПрименениеСкотча.Text,
                        усиление: УсилениеСъемнойПанели1.IsChecked == true,
                        config: "00",
                        расположениеПанелей: "",
                        покрытие: new[]
                        {
                            Ral1.Text, Ral1.Text != noBrush ? CoatingType1.Text : "0", Ral1.Text != noBrush ? CoatingClass1.Text : "0",
                            Ral2.Text, Ral2.Text != noBrush ? CoatingType2.Text : "0", Ral2.Text != noBrush ? CoatingClass2.Text : "0",
                            Ral1.SelectedValue != null ? Ral1.SelectedValue.ToString():noBrush,
                            Ral2.SelectedValue != null ? Ral2.SelectedValue.ToString():noBrush
                        },
                        расположениеВставок: ProfilsConfig());

                    var panelRemovable2 = modelSw.PanelsFramelessStr(
                        typeOfPanel: new[] { ТипСъемнойПанели2.SelectedValue.ToString(), ТипСъемнойПанели2.Text },
                        width: Convert.ToString(ШиринаСъемнойПанели2.Text), height: Convert.ToString(height - 40),
                        materialP1: materialP1,
                        materialP2: materialP2,
                        скотч: ПрименениеСкотча.Text,
                        усиление: УсилениеСъемнойПанели2.IsChecked == true,
                        config: "00",
                        расположениеПанелей: "",
                        покрытие: new[]
                        {
                            Ral1.Text, Ral1.Text != noBrush ? CoatingType1.Text : "0", Ral1.Text != noBrush ? CoatingClass1.Text : "0",
                            Ral2.Text, Ral2.Text != noBrush ? CoatingType2.Text : "0", Ral2.Text != noBrush ? CoatingClass2.Text : "0",
                            Ral1.SelectedValue != null ? Ral1.SelectedValue.ToString():noBrush,
                            Ral2.SelectedValue != null ? Ral2.SelectedValue.ToString():noBrush
                        },
                        расположениеВставок: ProfilsConfig());

                    var panelRemovable3 = modelSw.PanelsFramelessStr(
                        typeOfPanel: new[] { ТипСъемнойПанели3.SelectedValue.ToString(), ТипСъемнойПанели3.Text },
                        width: Convert.ToString(ШиринаСъемнойПанели3.Text),
                        height: Convert.ToString(height - 40),
                        materialP1: materialP1, materialP2: materialP2,
                        скотч: ПрименениеСкотча.Text,
                        усиление: УсилениеСъемнойПанели3.IsChecked == true,
                        config: "00",
                        расположениеПанелей: "",
                        покрытие: new[]
                        {
                            Ral1.Text, Ral1.Text != noBrush ? CoatingType1.Text : "0", Ral1.Text != noBrush ? CoatingClass1.Text : "0",
                            Ral2.Text, Ral2.Text != noBrush ? CoatingType2.Text : "0", Ral2.Text != noBrush ? CoatingClass2.Text : "0",
                            Ral1.SelectedValue != null ? Ral1.SelectedValue.ToString():noBrush,
                            Ral2.SelectedValue != null ? Ral2.SelectedValue.ToString():noBrush
                        },
                        расположениеВставок: ProfilsConfig());

                    panelRemovable = new[]
                    {
                        panelRemovable1,
                        panelRemovable2,
                        panelRemovable3
                    };


                }
                catch (Exception ex)
                {
                    Логгер.Ошибка(
                        logText: ex.Message,
                        код: ex.StackTrace,
                        функция: ex.TargetSite.ToString(),
                        className: "FramelessUnitUc");
                }
                
                try
                {
                    //"Несъемная";
                    panelFixed = modelSw.PanelsFramelessStr(
                        typeOfPanel: new[] { ТипНесъемнойПанели.SelectedValue.ToString(), ТипНесъемнойПанели.Text },
                        width: Convert.ToString(lenght),
                        height: Convert.ToString(height - 40),
                        materialP1: materialP1,
                        materialP2: materialP2,
                        скотч: ПрименениеСкотча.Text,
                        усиление: PanelUnremChk.IsChecked == true,
                        config: "00",
                        расположениеПанелей: "",
                        покрытие: new[]
                        {
                            Ral1.Text, Ral1.Text != noBrush ? CoatingType1.Text : "0", Ral1.Text != noBrush ? CoatingClass1.Text : "0",
                            Ral2.Text, Ral2.Text != noBrush ? CoatingType2.Text : "0", Ral2.Text != noBrush ? CoatingClass2.Text : "0",
                            Ral1.SelectedValue != null ? Ral1.SelectedValue.ToString():noBrush,
                            Ral2.SelectedValue != null ? Ral2.SelectedValue.ToString():noBrush
                        },
                        расположениеВставок: ProfilsConfig());
                }
                catch (Exception ex)
                {
                    Логгер.Ошибка(
                        ex.Message,
                        ex.StackTrace,
                        ex.TargetSite.ToString(),
                        "FramelessUnitUc");
                }

                #endregion

                //Parts.ItemsSource = modelSw.AddingPanels;
                //MessageBox.Show("Что видно?");

                modelSw.FramelessBlock(
                    size: ((DataRowView) SizeOfUnit.SelectedItem)["Type"].ToString(),
                    order: OrderTextBox.Text,
                    side: сторонаОбслуживания,
                    section: SectionTextBox.Text,
                    pDown: panelDown,
                    pFixed: panelFixed,
                    pUp: panelUp,
                    съемныеПанели: panelRemovable,
                    height: Convert.ToString(height));
            }
            catch (Exception ex)
            {
                Логгер.Ошибка(
                    logText: ex.Message,
                    код: ex.StackTrace,
                    функция: ex.TargetSite.ToString(),
                    className: "FramelessUnitUc");
            }

            Логгер.Информация(
                String.Format("Построение бескаркасной уставновки завершено - {0}",""),
                "", "", "FramelessUnitUc");
        }

        void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        void HeightU_KeyDown(object sender, KeyEventArgs e)
        {
            StartBuildingBlock(e);
        }

        void StartBuildingBlock(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BUILDING_Click(this, new RoutedEventArgs());
            }
        }

        void WidthU_KeyDown(object sender, KeyEventArgs e)
        {
            StartBuildingBlock(e);
        }

        void Lenght_KeyDown(object sender, KeyEventArgs e)
        {
            StartBuildingBlock(e);
        }

        void LenghtRoof_KeyDown(object sender, KeyEventArgs e)
        {
            StartBuildingBlock(e);
        }

        void WidthRoof_KeyDown(object sender, KeyEventArgs e)
        {
            StartBuildingBlock(e);
        }

        void ПостроитьПанель_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //var добавитьПанель = _sqlBaseData.AirVents_AddPanel(
                //    ТипСъемнойПанели1.SelectedValue.ToString(),
                //    Convert.ToInt32(WidthU.Text),
                //    Convert.ToInt32(HeightU.Text),
                //    40,
                //    Convert.ToInt32(MaterialP1.SelectedValue),
                //    Convert.ToInt32(MaterialP2.SelectedValue),
                //    Convert.ToDouble(ТолщинаВнешней.Text.Replace('.',',')),
                //    Convert.ToDouble(ТолщинаВннутренней.Text.Replace('.', ',')),
                //    Convert.ToInt32(Шумоизоляция.SelectedValue),
                //    ПрименениеСкотча.Text == "Со скотчем",
                //    УсилениеСъемнойПанели1.IsChecked == true,
                //    Ral1.Text,
                //    CoatingType1.Text,
                //    Convert.ToInt32(CoatingClass1.Text));

                //modelSw.PanelsFramelessStr(
                //        "01",
                //        Convert.ToString(lenght),
                //        Convert.ToString(height - 40),
                //        materialP1,
                //        materialP2,
                //        скотч,
                //        "00" + strengthUnrem,
                //        "",
                //        new[] {Ral1.Text, CoatingType1.Text, CoatingClass1.Text});

              //  MessageBox.Show(добавитьПанель.ToString());
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            
            try
            {
                //var modelSw = new ModelSw();

                //string config;

                //switch (ТипПанели.Text)
                //{
                //    case "01":
                //    case "04":
                //    case "05":
                //        config = "00";
                //        break;
                //    default:
                //        config = "01";
                //        break;
                //}

                //var strengthUp = УсилениеПанели.IsChecked == true;

                //modelSw.PanelsFramelessStr(
                //    ТипПанели.Text,
                //    ШиринаПанели.Text,
                //    ВысотаПанели.Text,
                //    new[] { МатериалВнешней.Text, ТолщинаВнешней.Text },
                //    new[] { МатериалВнутренней.Text, ТолщинаВннутренней.Text },
                //    TypeOfPanelF1.Text == "Со скотчем" ? true : false,
                //    strengthUp,
                //    config,
                //    PanelsConfig(),
                //    new[] { Ral1.Text, CoatingType1.Text, CoatingClass1.Text });
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        string PanelsConfig()
        {
            return String.Format("{0}{1}{2}{3}{4}{5}",
                Отступ1.Visibility == Visibility.Visible ? Отступ1.Text : "",
                ШиринаПанели1.Visibility == Visibility.Visible ? ";" + ШиринаПанели1.Text : "",
                Отступ2.Visibility == Visibility.Visible ? ";" + Отступ2.Text : "",
                ШиринаПанели2.Visibility == Visibility.Visible ? ";" + ШиринаПанели2.Text : "",
                Отступ3.Visibility == Visibility.Visible ? ";" + Отступ3.Text : "",
                ШиринаПанели3.Visibility == Visibility.Visible ? ";" + ШиринаПанели3.Text : "");
        }

        string ProfilsConfig()
        {

            //return String.Format("{0}{1}{2}{3}{4}{5}",
            //    Отступ1.Visibility == Visibility.Visible ? Отступ1.Text : "",
            //    ШиринаПанели1.Visibility == Visibility.Visible ? ";" + ШиринаПанели1.Text : "",
            //    Отступ2.Visibility == Visibility.Visible ? ";" + Отступ2.Text : "",
            //    ШиринаПанели2.Visibility == Visibility.Visible ? ";" + ШиринаПанели2.Text : "",
            //    Отступ3.Visibility == Visibility.Visible ? ";" + Отступ3.Text : "",
            //    ШиринаПанели3.Visibility == Visibility.Visible ? ";" + ШиринаПанели3.Text : "");

            return String.Format("{0}{1}{2}",
                Отступ1.Visibility == Visibility.Visible ? Отступ1.Text : "",
                Отступ2.Visibility == Visibility.Visible ? ";" + Отступ2.Text : "",
                Отступ3.Visibility == Visibility.Visible ? ";" + Отступ3.Text : "");

        }

        void ПотроитьПанель(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ПостроитьПанель_Click(this, new RoutedEventArgs());
            } 
        }

        void ШиринаПанели_KeyDown(object sender, KeyEventArgs e)
        {
            ПотроитьПанель(e);
        }

        void ВысотаПанели_KeyDown(object sender, KeyEventArgs e)
        {
            ПотроитьПанель(e);
        }

        void КолвоПанелейS(object sender, SelectionChangedEventArgs e)
        {
            КолВоПанелей();
        }

        void КолВоПанелей()
        {
            if (Ширина2 == null || Ширина3 == null || ШиринаПанели2 == null || ШиринаПанели3 == null ||
                Отступ2L == null || Отступ3L == null || Отступ2 == null || Отступ3 == null) return;

            Ширина2.Visibility = Visibility.Collapsed;
            Ширина3.Visibility = Visibility.Collapsed;

            ШиринаПанели2.Visibility = Visibility.Collapsed;
            ШиринаПанели3.Visibility = Visibility.Collapsed;

            Отступ2L.Visibility = Visibility.Collapsed;
            Отступ3L.Visibility = Visibility.Collapsed;

            Отступ2.Visibility = Visibility.Collapsed;
            Отступ3.Visibility = Visibility.Collapsed;

            ШиринаПанели1.IsEnabled = true;
            ШиринаПанели2.IsEnabled = true;
            ШиринаПанели3.IsEnabled = true;

            var item = КолвоПанелей.SelectedValue as ComboBoxItem;

            if (item == null) return;
            switch (item.Content.ToString())
            {
                case "1":
                    ШиринаПанели1.IsEnabled = false;
                    ШиринаПанели3.Text = "";
                    ШиринаПанели2.Text = "";
                    break;

                case "2":
                    ШиринаПанели2.IsEnabled = false;
                    ШиринаПанели3.Text = "";

                    Ширина2.Visibility = Visibility.Visible;
                    ШиринаПанели2.Visibility = Visibility.Visible;
                    Отступ2L.Visibility = Visibility.Visible;
                    Отступ2.Visibility = Visibility.Visible;
                    break;

                case "3":
                    ШиринаПанели3.IsEnabled = false;

                    Ширина2.Visibility = Visibility.Visible;
                    ШиринаПанели2.Visibility = Visibility.Visible;
                    Отступ2L.Visibility = Visibility.Visible;
                    Отступ2.Visibility = Visibility.Visible;

                    Ширина3.Visibility = Visibility.Visible;
                    ШиринаПанели3.Visibility = Visibility.Visible;
                    Отступ3L.Visibility = Visibility.Visible;
                    Отступ3.Visibility = Visibility.Visible;
                    break;
                default:
                    Ширина3.Visibility = Visibility.Visible;
                    ШиринаПанели3.Visibility = Visibility.Visible;
                    Отступ3L.Visibility = Visibility.Visible;
                    Отступ3.Visibility = Visibility.Visible;
                    break;
            }
        }

        void RiznPanel(TextBox ширинаПанели, TextBox панель1, TextBox панель2, TextBox панель3)
        {
            //if (ширинаПанели == null || панель1 == null || панель2 == null || панель3 == null) return;
            
            панель1.SelectionChanged += ШиринаПанели1_SelectionChanged;
            панель2.SelectionChanged += ШиринаПанели2_SelectionChanged;
            панель3.SelectionChanged += ШиринаПанели3_SelectionChanged;
            
            var посадочнаяШиринаПанели = ширинаПанели.Text != "" ? Convert.ToInt32(ширинаПанели.Text) : 0;
            var посадочнаяШиринаПанели1 = панель1.Text != "" ? Convert.ToInt32(панель1.Text) : 0;
            var посадочнаяШиринаПанели2 = панель2.Text != "" ? Convert.ToInt32(панель2.Text) : 0;
            
            switch (КолвоПанелей.Text)
            {
                case "1":
                    панель1.Text = ширинаПанели.Text;
                    панель1.SelectionChanged -= ШиринаПанели1_SelectionChanged;
                    break;
                case "2":
                    панель2.Text = Convert.ToString(посадочнаяШиринаПанели - посадочнаяШиринаПанели1);
                    панель2.SelectionChanged -= ШиринаПанели2_SelectionChanged;
                    break;
                case "3":
                    панель3.Text = Convert.ToString(посадочнаяШиринаПанели - посадочнаяШиринаПанели1 - посадочнаяШиринаПанели2);
                    панель3.SelectionChanged -= ШиринаПанели3_SelectionChanged;
                    break;
            }
        }

        void ПостроитьПанель1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(PanelsConfig());
        }
        
        void ШиринаПанели_SelectionChanged(object sender, RoutedEventArgs e)
        {
            RiznPanel(ШиринаПанели, ШиринаПанели1, ШиринаПанели2, ШиринаПанели3);
        }

        void ШиринаПанели1_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (!ШиринаПанели1.IsEnabled) return;
            RiznPanel(ШиринаПанели, ШиринаПанели1, ШиринаПанели2, ШиринаПанели3);
        }

        void ШиринаПанели2_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ШиринаСъемнойПанели2.Text = ШиринаПанели2.Text;
            if (!ШиринаПанели2.IsEnabled) return;
            RiznPanel(ШиринаПанели, ШиринаПанели1, ШиринаПанели2, ШиринаПанели3);
        }

        void ШиринаПанели3_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ШиринаСъемнойПанели3.Text = ШиринаПанели3.Text;
            if (!ШиринаПанели3.IsEnabled) return;
            RiznPanel(ШиринаПанели, ШиринаПанели1, ШиринаПанели2, ШиринаПанели3);
        }

        void RemovablePanels_Initialized(object sender, EventArgs e)
        {
            ШиринаПанели1.IsEnabled = false;
        }

        void КолвоПанелей_LayoutUpdated(object sender, EventArgs e)
        {
            КолВоПанелей();
        }

        void ТипУстановки_LayoutUpdated(object sender, EventArgs e)
        {
            const string picturePath = @"\DataControls\Pictures\Бескаркасная установка\";

            if (ТипУстановки == null) return;
            var item = (ComboBoxItem) ТипУстановки.SelectedItem;
            if (item == null) return;
            var pictureName = "01 - Frameless Design 40mm - " + item.ToolTip + ".jpg";
            
            var mapLoader = new BitmapImage();
            mapLoader.BeginInit();
            mapLoader.UriSource = new Uri(picturePath + pictureName, UriKind.RelativeOrAbsolute);

            mapLoader.EndInit();
            if (PicturePanel != null) PicturePanel.Source = mapLoader;
        }

        void Левая_Checked(object sender, RoutedEventArgs e)
        {
            PicturePanel.RenderTransformOrigin = new Point(0.5, 0.5);
            var flipTrans = new ScaleTransform {ScaleX = 1};
            //  flipTrans.ScaleY = -1;
            PicturePanel.RenderTransform = flipTrans;
        }

        void Правая_Checked(object sender, RoutedEventArgs e)
        {
            //PicturePanel.RenderTransformOrigin = new Point(0.5, 0.5);
            //var flipTrans = new ScaleTransform {ScaleX = -1};
            //PicturePanel.RenderTransform = flipTrans;
        }
        
        void КришаТип_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ТипВерхнейПанели.Text = КришаТип.SelectedValue.ToString();
        }

        void ОпорнаяЧастьТип_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ТипПанелиНижней.Text = ОпорнаяЧастьТип.SelectedValue.ToString();
        }

        void ШиринаСъемнойПанели1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (ТипУстановки == null) return;
            var regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);

            //var item = (ComboBoxItem)ТипУстановки.SelectedItem;
            //var handled = item.ToolTip.ToString() != "01" & regex.IsMatch(e.Text);
            //e.Handled = handled;
        }

        void ШиринаСъемнойПанели2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
            //ШиринаПанели2.Text = ШиринаСъемнойПанели2.Text;
        }

        void Lenght_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (ТипУстановки == null) return;
            
            var item = (ComboBoxItem)ТипУстановки.SelectedValue;
            
            if (item.ToolTip.ToString() == "01")
            {
                ШиринаПанели.Text = Lenght.Text;
                ШиринаСъемнойПанели1.Text = Lenght.Text;
                ШиринаПанели1.Text = Lenght.Text;
            }
            else
            {
                ШиринаПанели.Text = Lenght.Text;
            }
        }

        void ТипСъемнойПанели1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ТипСъемнойПанели1.ToolTip = ТипСъемнойПанели1.SelectedValue;
        }

        void ТипУстановки_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ТипУстановки == null) return;
            var item = (ComboBoxItem)ТипУстановки.SelectedItem;

            ШиринаСъемнойПанели1.IsEnabled = false;
            ШиринаСъемнойПанели2.IsEnabled = false;
            ШиринаСъемнойПанели3.IsEnabled = false;

            switch (item.ToolTip.ToString())
            {
                case "01":
                    КолвоПанелей.Text = "1";
                    ШиринаСъемнойПанели1.IsEnabled = false;
                    ШиринаСъемнойПанели1.Text = Lenght.Text;

                    Grid.SetColumn(Lenght, 5);
                    Grid.SetColumn(ШиринаСъемнойПанели1, 5);
                    ШиринаСъемнойПанели1.Visibility = Visibility.Visible;
                    ШиринаСъемнойПанели2.Visibility = Visibility.Collapsed;
                    ШиринаСъемнойПанели3.Visibility = Visibility.Collapsed;

                    break;
                case "02":
                    КолвоПанелей.Text = "2";
                    ШиринаСъемнойПанели1.IsEnabled = true;
                    Grid.SetColumn(Lenght, 6);
                    Grid.SetColumn(ШиринаСъемнойПанели1, 5);
                    Grid.SetColumn(ШиринаСъемнойПанели2, 7);
                    Grid.SetColumn(ШиринаСъемнойПанели3, 9);
                    ШиринаСъемнойПанели1.Visibility = Visibility.Visible;
                    ШиринаСъемнойПанели2.Visibility = Visibility.Visible;
                    ШиринаСъемнойПанели3.Visibility = Visibility.Collapsed;

                    break;
                case "03":
                    КолвоПанелей.Text = "3";
                    ШиринаСъемнойПанели1.IsEnabled = true;
                    ШиринаСъемнойПанели2.IsEnabled = true;
                    Grid.SetColumn(Lenght, 5);
                    Grid.SetColumn(ШиринаСъемнойПанели1, 3);
                    Grid.SetColumn(ШиринаСъемнойПанели2, 5);
                    Grid.SetColumn(ШиринаСъемнойПанели3, 8);
                    ШиринаСъемнойПанели1.Visibility = Visibility.Visible;
                    ШиринаСъемнойПанели2.Visibility = Visibility.Visible;
                    ШиринаСъемнойПанели3.Visibility = Visibility.Visible;
                    
                    break;
            }
        }

        void ШиринаСъемнойПанели1_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ШиринаПанели1.Text = ШиринаСъемнойПанели1.Text;
        }

        void ШиринаСъемнойПанели2_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ШиринаПанели2.Text = ШиринаСъемнойПанели2.Text;
            ШиринаСъемнойПанели2.Foreground = ШиринаСъемнойПанели2.Text.Contains("-") ? Brushes.Red : Brushes.Black;
        }

        void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (!Settings.Default.Developer)
            {
                _isDeveloper = false;
            }
            if (_isDeveloper)
            {
                НесъемныеПанели.Visibility = Visibility.Visible;
                ПанельБескаркаснойУстановки.Visibility = Visibility.Visible;

                УсилениеСъемнойПанели1Chk.Visibility = Visibility.Visible;
                УсилениеСъемнойПанели2Chk.Visibility = Visibility.Visible;
                УсилениеСъемнойПанели3Chk.Visibility = Visibility.Visible;

                Parts.Visibility = Visibility.Visible;
            }
            else
            {
                НесъемныеПанели.Visibility = Visibility.Collapsed;
                ПанельБескаркаснойУстановки.Visibility = Visibility.Collapsed;

                Parts.Visibility = Visibility.Collapsed;

                УсилениеСъемнойПанели1Chk.Visibility = Visibility.Collapsed;
                УсилениеСъемнойПанели2Chk.Visibility = Visibility.Collapsed;
                УсилениеСъемнойПанели3Chk.Visibility = Visibility.Collapsed;
            }
        }

        void Grid_Loaded_2(object sender, RoutedEventArgs e)
        {
            // Длина
            if (Lenght.Parent != null)
            {
                var parent = (Panel)Lenght.Parent;
                parent.Children.Remove(Lenght);
            }
            MyPanel.Children.Add(Lenght);
            Grid.SetRow(Lenght, 8);

            // Панель 1
            if (ШиринаСъемнойПанели1.Parent != null)
            {
                var parent = (Panel)ШиринаСъемнойПанели1.Parent;
                parent.Children.Remove(ШиринаСъемнойПанели1);
            }
            MyPanel.Children.Add(ШиринаСъемнойПанели1);
            Grid.SetRow(ШиринаСъемнойПанели1, 1);


            // Панель 2
            if (ШиринаСъемнойПанели2.Parent != null)
            {
                var parent = (Panel)ШиринаСъемнойПанели2.Parent;
                parent.Children.Remove(ШиринаСъемнойПанели2);
            }
            MyPanel.Children.Add(ШиринаСъемнойПанели2);
            Grid.SetRow(ШиринаСъемнойПанели2, 1);

            // Панель 3
            if (ШиринаСъемнойПанели3.Parent != null)
            {
                var parent = (Panel)ШиринаСъемнойПанели3.Parent;
                parent.Children.Remove(ШиринаСъемнойПанели3);
            }
            MyPanel.Children.Add(ШиринаСъемнойПанели3);
            Grid.SetRow(ШиринаСъемнойПанели3, 1);
        }

        void Усиления_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ComboBoxItem)Усиления.SelectedItem;
            if (item == null) return;

            УсилениеСъемнойПанели1.IsChecked = false;
            УсилениеСъемнойПанели2.IsChecked = false;
            УсилениеСъемнойПанели3.IsChecked = false;
            PanelUpChk.IsChecked = false;
            PanelUnremChk.IsChecked = false;
            PanelDownChk.IsChecked = false;

            switch (item.Content.ToString())
            {
                case "":
                    break;
                case "Н":
                    PanelDownChk.IsChecked = true;
                    break;
                case "НВ":
                    PanelDownChk.IsChecked = true;
                    PanelUpChk.IsChecked = true;
                    break;
                case "НЗ":
                    PanelDownChk.IsChecked = true;
                    PanelUnremChk.IsChecked = true;
                    break;
                case "НВЗ":
                    PanelDownChk.IsChecked = true;
                    PanelUnremChk.IsChecked = true;
                    PanelUpChk.IsChecked = true;
                    break;
                case "НВЗС":
                    УсилениеСъемнойПанели1.IsChecked = true;
                    УсилениеСъемнойПанели2.IsChecked = true;
                    УсилениеСъемнойПанели3.IsChecked = true;
                    PanelDownChk.IsChecked = true;
                    PanelUnremChk.IsChecked = true;
                    PanelUpChk.IsChecked = true;
                    break;
            }
        }

        private void Ral1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ral1.ToolTip = Ral1.SelectedValue;

           
        }

        private void Ral2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ral2.ToolTip = Ral2.SelectedValue;

        }

        private void Grid_Loaded_3(object sender, RoutedEventArgs e)
        {
            //var doubleAnimation = new DoubleAnimation
            //{
            //    From = 0.1,
            //    To = 1,
            //    Duration = new Duration(TimeSpan.FromSeconds(1))
            //};
            //BeginAnimation(OpacityProperty, doubleAnimation);
        }

        private void ШиринаСъемнойПанели3_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ШиринаСъемнойПанели3.Foreground = ШиринаСъемнойПанели3.Text.Contains("-") ? Brushes.Red : Brushes.Black;
        }
        
        private void Ral1_LayoutUpdated(object sender, EventArgs e)
        {
            if (Ral1.Text == "Без покрытия")
            {
                CoatingType1.Visibility = Visibility.Collapsed;
                CoatingClass1.Visibility = Visibility.Collapsed;
            }
            else
            {
                CoatingType1.Visibility = Visibility.Visible;
                CoatingClass1.Visibility = Visibility.Visible;
            }
        }

        private void Ral2_LayoutUpdated(object sender, EventArgs e)
        {

            if (Ral2.Text == "Без покрытия")
            {
                CoatingType2.Visibility = Visibility.Collapsed;
                CoatingClass2.Visibility = Visibility.Collapsed;
            }
            else
            {
                CoatingType2.Visibility = Visibility.Visible;
                CoatingClass2.Visibility = Visibility.Visible;
            }
        }

        private void MaterialP1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MaterialP1.SelectedIndex == 0)
            {
                ТолщинаВнешнейLbl.Visibility = Visibility.Hidden;
                ТолщинаВнешней.Visibility = Visibility.Hidden;
            }
            else
            {
                ТолщинаВнешнейLbl.Visibility = Visibility.Visible;
                ТолщинаВнешней.Visibility = Visibility.Visible;
            }
        }

        private void MaterialP2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MaterialP2.SelectedIndex == 0)
            {
                ТолщинаВннутреннейLbl.Visibility = Visibility.Hidden;
                ТолщинаВннутренней.Visibility = Visibility.Hidden;
            }
            else
            {
                ТолщинаВннутреннейLbl.Visibility = Visibility.Visible;
                ТолщинаВннутренней.Visibility = Visibility.Visible;
            }
        }
    }
}
