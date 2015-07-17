using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using AirVentsCadWpf.AirVentsClasses;
using AirVentsCadWpf.Properties;
using AirVentsCadWpf.Логирование;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="FramelessUnitUc"/> class.
        /// </summary>
        public FramelessUnitUc()
        {
            InitializeComponent();

            ToSQL.Conn = Settings.Default.ConnectionToSQL;

            #region Профили промежуточные

            ШиринаПрофиля1.MaxLength = 3;
            ШиринаПрофиля2.MaxLength = 3;
            ШиринаПрофиля3.MaxLength = 3;
            ШиринаПрофиля4.MaxLength = 3;

            #endregion

            Усиления.ItemsSource = new List<ComboBoxItem>
            {
                new ComboBoxItem {ToolTip = "Отсутствует", Content = "-"},
                new ComboBoxItem {ToolTip = "Нижней", Content = "Н"},
                new ComboBoxItem {ToolTip = "Нижней\nВерхней", Content = "НВ"},
                new ComboBoxItem {ToolTip = "Нижней\nЗадней", Content = "НЗ"},
                new ComboBoxItem {ToolTip = "Нижней\nВерхней\nЗадней", Content = "НВЗ"},
                new ComboBoxItem {ToolTip = "Нижней\nВерхней\nЗадней\nСъемной(ых)", Content = "НВЗС"}
            };
                                

            DataGrid1.Visibility = Visibility.Collapsed;

            var images =
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Image1", @"\AHU Selection\br_icon_teaser_cad_sr_pos_150(1).ico"),
                    new KeyValuePair<string, string>("Image2", @"D:\Photos\tn-36.jpg"),
                    new KeyValuePair<string, string>("Image3", @"D:\Photos\tn-37.jpg")
                };

            DataGrid1.ItemsSource = images;

            ТипУстановкиПромежуточныеВставки.ItemsSource = new List<ComboBoxItem>
            {
                new ComboBoxItem {ToolTip = "0", Content = "Без усиливающих панелей"},
                new ComboBoxItem {ToolTip = "1", Content = "Две усиливающие панели"},
                new ComboBoxItem {ToolTip = "2", Content = "Первая усиливающая панель"},
                new ComboBoxItem {ToolTip = "3", Content = "Вторая усиливающая панель"}
            };

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

        void SizeOfUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SizeOfUnit == null || SizeOfUnit.SelectedItem == null) return;
            
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

        #region BUILDING

        void BUILDING_Click(object sender, RoutedEventArgs e)
        {
            //return;

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
                    MessageBox.Show("Ширина 2-й съемной панели не может быть меньше 100 мм!");
                    return;
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

                SqlBaseData basedata = new SqlBaseData();

                try
                {
                    //const string paneltype = "Панель верхняя";
                    //MessageBox.Show(ТипВерхнейПанели.Text, КришаТип.Text);
                    panelUp = modelSw.PanelsFramelessStr(
                        typeOfPanel: new[] { ТипВерхнейПанели.Text, КришаТип.Text, Convert.ToString(basedata.PanelsTypeId(ТипВерхнейПанели.Text)) },
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
                        расположениеВставок: ProfilsConfig(),
                        типУсиливающей: "");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());

                    Логгер.Ошибка(
                    ex.Message,
                    ex.StackTrace,
                    ex.TargetSite.ToString(),
                    "FramelessUnitUc");
                }

                //MessageBox.Show(panelUp);
                //return;

                #region Панель нижняя под монтажные ножки

                try
                {
                    //const string paneltype = "Панель нижняя под монтажные ножки";
                    panelDown = modelSw.PanelsFramelessStr(
                        typeOfPanel: new[] { ТипПанелиНижней.Text, ОпорнаяЧастьТип.Text, Convert.ToString(basedata.PanelsTypeId(ТипПанелиНижней.Text)) },
                        width: Convert.ToString(lenght),
                        height: Convert.ToString(width),
                        materialP1: materialP1,
                        materialP2: materialP2,
                        скотч: ПрименениеСкотча.Text, // == "Со скотчем",
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
                        расположениеВставок: ProfilsConfig(),
                        типУсиливающей: "");
                }
                catch (Exception ex)
                {
                    Логгер.Ошибка(
                    ex.Message,
                    ex.StackTrace,
                    ex.TargetSite.ToString(),
                    "FramelessUnitUc");
                }

                //MessageBox.Show(panelDown);
                //return;

                #endregion

                var panelRemovable = new []{""};
                string panelRemovable1 = null;
                string panelRemovable2 = null;
                string panelRemovable3 = null;

                try
                {
                    //"Съемная";
                    try
                    {
                        panelRemovable1 = modelSw.PanelsFramelessStr(
                            typeOfPanel: new[]
                            {
                                ТипСъемнойПанели1.SelectedValue.ToString(),
                                ТипСъемнойПанели1.Text,
                                Convert.ToString(basedata.PanelsTypeId(ТипСъемнойПанели1.SelectedValue.ToString()))
                            },
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
                                Ral1.Text, Ral1.Text != noBrush ? CoatingType1.Text : "0",
                                Ral1.Text != noBrush ? CoatingClass1.Text : "0",
                                Ral2.Text, Ral2.Text != noBrush ? CoatingType2.Text : "0",
                                Ral2.Text != noBrush ? CoatingClass2.Text : "0",
                                Ral1.SelectedValue != null ? Ral1.SelectedValue.ToString() : noBrush,
                                Ral2.SelectedValue != null ? Ral2.SelectedValue.ToString() : noBrush
                            },
                            расположениеВставок: ProfilsConfig(),
                            типУсиливающей: "");
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.ToString());
                    }

                    try
                    {
                       panelRemovable2 = modelSw.PanelsFramelessStr(
                       typeOfPanel: new[] 
                       {
                           ТипСъемнойПанели2.SelectedValue.ToString(),
                           ТипСъемнойПанели2.Text,
                           Convert.ToString(basedata.PanelsTypeId(ТипСъемнойПанели2.SelectedValue.ToString()))
                       },
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
                       расположениеВставок: ProfilsConfig(),
                       типУсиливающей: "");
                    
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.ToString());
                    }

                    try
                    {
                        panelRemovable3 = modelSw.PanelsFramelessStr(
                        typeOfPanel: new[] 
                        {
                            ТипСъемнойПанели3.SelectedValue.ToString(),
                            ТипСъемнойПанели3.Text,
                           Convert.ToString(basedata.PanelsTypeId(ТипСъемнойПанели3.SelectedValue.ToString()))
                        },
                        width: Convert.ToString(ШиринаСъемнойПанели3.Text),
                        height: Convert.ToString(height - 40),
                        materialP1: materialP1,
                        materialP2: materialP2,
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
                        расположениеВставок: ProfilsConfig(),
                        типУсиливающей: "");
                    
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.ToString());
                    }

                    //MessageBox.Show(panelRemovable1);
                    //return;

                    #region Усиливющие панели

                    string panelReinforcing1 = null;
                    string panelReinforcing2 = null;
                    
                    if (ТипУсилПанели1.Visibility == Visibility.Visible)
                    {
                        try
                        {
                            var typeOfPanel = ТипУсилПанели1.SelectedItem as ComboBoxItem;

                            panelReinforcing1 = modelSw.PanelsFramelessStr(
                            typeOfPanel: new[]
                            {
                                ТипНесъемнойПанели.SelectedValue.ToString(),
                                ТипНесъемнойПанели.Text,
                                Convert.ToString(basedata.PanelsTypeId(ТипНесъемнойПанели.SelectedValue.ToString())),
                                ТипУсилПанели1.Text },
                            width: Convert.ToString(130),
                            height: Convert.ToString(height - 40),
                            materialP1: materialP1,
                            materialP2: materialP2,
                            скотч: ПрименениеСкотча.Text,
                            усиление: false,
                            config: "00",
                            расположениеПанелей: "",
                            покрытие: new[]
                        {
                            Ral1.Text, Ral1.Text != noBrush ? CoatingType1.Text : "0", Ral1.Text != noBrush ? CoatingClass1.Text : "0",
                            Ral2.Text, Ral2.Text != noBrush ? CoatingType2.Text : "0", Ral2.Text != noBrush ? CoatingClass2.Text : "0",
                            Ral1.SelectedValue != null ? Ral1.SelectedValue.ToString():noBrush,
                            Ral2.SelectedValue != null ? Ral2.SelectedValue.ToString():noBrush
                        },
                            расположениеВставок: ProfilsConfig() + "_1",
                            типУсиливающей: typeOfPanel.Tag.ToString());
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.ToString());
                        }
                    }

                    if (ТипУсилПанели2.Visibility == Visibility.Visible)
                    {
                        try
                        {
                            var typeOfPanel = ТипУсилПанели2.SelectedItem as ComboBoxItem;

                            panelReinforcing2 = modelSw.PanelsFramelessStr(
                            typeOfPanel: new[]
                            {
                                ТипНесъемнойПанели.SelectedValue.ToString(),
                                ТипНесъемнойПанели.Text,
                                Convert.ToString(basedata.PanelsTypeId(ТипНесъемнойПанели.SelectedValue.ToString())),
                                ТипУсилПанели1.Text },
                            width: Convert.ToString(130),
                            height: Convert.ToString(height - 40),
                            materialP1: materialP1,
                            materialP2: materialP2,
                            скотч: ПрименениеСкотча.Text,
                            усиление: false,
                            config: "00",
                            расположениеПанелей: "",
                            покрытие: new[]
                            {
                                Ral1.Text, Ral1.Text != noBrush ? CoatingType1.Text : "0",
                                Ral1.Text != noBrush ? CoatingClass1.Text : "0",
                                Ral2.Text, Ral2.Text != noBrush ? CoatingType2.Text : "0",
                                Ral2.Text != noBrush ? CoatingClass2.Text : "0",
                                Ral1.SelectedValue?.ToString() ?? noBrush,
                                Ral2.SelectedValue != null ? Ral2.SelectedValue.ToString() : noBrush
                            },
                            расположениеВставок: ProfilsConfig() + "_2",
                            типУсиливающей: typeOfPanel.Tag.ToString());
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.ToString());
                        }
                    }

                    #endregion

                    panelRemovable = new[]
                    {
                        panelRemovable1,
                        panelRemovable2,
                        panelRemovable3,
                        ПрименениеСкотча.Text,
                        panelReinforcing1,
                        panelReinforcing2
                    };
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());

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
                        typeOfPanel: new[]
                        {
                            ТипНесъемнойПанели.SelectedValue.ToString(),
                            ТипНесъемнойПанели.Text,
                            Convert.ToString(basedata.PanelsTypeId(ТипНесъемнойПанели.SelectedValue.ToString())),
                        },
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
                        расположениеВставок: ProfilsConfig(),
                        типУсиливающей: null); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());


                    Логгер.Ошибка(
                        ex.Message,
                        ex.StackTrace,
                        ex.TargetSite.ToString(),
                        "FramelessUnitUc");
                }

                #endregion
                
                if (ТипПроф1.Visibility != Visibility.Visible)
                {
                    ТипПроф1.Text = "-";
                }

                if (ТипПроф2.Visibility != Visibility.Visible)
                {
                    ТипПроф2.Text = "-";
                }

                if (ТипПроф3.Visibility != Visibility.Visible)
                {
                    ТипПроф3.Text = "-";
                }

                if (ТипПроф4.Visibility != Visibility.Visible)
                {
                    ТипПроф4.Text = "-";
                }

                var profils = new[]
                {
                    ТипПроф1.Text == "-" ? "-" : modelSw.Profil(Convert.ToDouble(HeightU.Text) - 80, ТипПроф1.Text),
                    ТипПроф2.Text == "-" ? "-" : modelSw.Profil(Convert.ToDouble(HeightU.Text) - 80, ТипПроф2.Text),
                    ТипПроф3.Text == "-" ? "-" : modelSw.Profil(Convert.ToDouble(HeightU.Text) - 80, ТипПроф3.Text),
                    ТипПроф4.Text == "-" ? "-" : modelSw.Profil(Convert.ToDouble(HeightU.Text) - 80, ТипПроф4.Text)
                };

                modelSw.FramelessBlock(
                    size: ((DataRowView) SizeOfUnit.SelectedItem)["Type"].ToString(),
                    order: OrderTextBox.Text,
                    side: сторонаОбслуживания,
                    section: SectionTextBox.Text,
                    pDown: panelDown,
                    pFixed: panelFixed,
                    pUp: panelUp,
                    съемныеПанели: panelRemovable,
                    промежуточныеСтойки: profils,
                    height: Convert.ToString(height));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

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
        
        #endregion

        #region Построить панель

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

                //string Config;

                //switch (ТипПанели.Text)
                //{
                //    case "01":
                //    case "04":
                //    case "05":
                //        Config = "00";
                //        break;
                //    default:
                //        Config = "01";
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
                //    Config,
                //    PanelsConfig(),
                //    new[] { Ral1.Text, CoatingType1.Text, CoatingClass1.Text });
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
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

        #endregion

        #region Строки для передачи параметров в метод построения панелей (Съемные панели и конфигурации)

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
            // todo ШиринаУсилПанели1 ШиринаПрофиля1
            return String.Format
                (
                "{0};{1};{2};{3};{4};{5}",
                ТипПроф1.Text + "_" + ШиринаПрофиля1.Text,
                ТипПроф2.Text + "_" + ШиринаПрофиля2.Text,
                ТипПроф3.Text + "_" + ШиринаПрофиля3.Text,
                ТипПроф4.Text + "_" + ШиринаПрофиля4.Text,
                ТипУсилПанели1.Text + "_" + ШиринаПрофиля1.Text,
                ТипУсилПанели2.Text + "_" + ШиринаПрофиля1.Text
                );
        }

        void ПостроитьПанель1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(PanelsConfig());
        }

        #endregion
        
        void КолвоПанелейS(object sender, SelectionChangedEventArgs e)
        {
            КолВоПанелей();
        }

        void КолвоПанелей_LayoutUpdated(object sender, EventArgs e)
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

        void RiznPanel(TextBox общаяДлина, TextBox панель1, TextBox панель2, TextBox панель3,
             TextBox профиль1, TextBox профиль2, TextBox профиль3, TextBox профиль4)
        {            
            панель1.SelectionChanged += ШиринаПанели1_SelectionChanged;
            панель2.SelectionChanged += ШиринаПанели2_SelectionChanged;
            панель3.SelectionChanged += ШиринаПанели3_SelectionChanged;
            
            var посадочнаяШиринаПанели = общаяДлина.Text != "" ? Convert.ToInt32(общаяДлина.Text) : 0;
            var посадочнаяШиринаПанели1 = панель1.Text != "" ? Convert.ToInt32(панель1.Text) : 0;
            var посадочнаяШиринаПанели2 = панель2.Text != "" ? Convert.ToInt32(панель2.Text) : 0;
          
            #region Профили

            int prof1;
            int.TryParse(профиль1.Text, out prof1);
            int prof2;
            int.TryParse(профиль2.Text, out prof2);
            int prof3;
            int.TryParse(профиль3.Text, out prof3);
            int prof4;
            int.TryParse(профиль4.Text, out prof4);

            var sumOfProfilsLenght = prof1 + prof2 + prof3 + prof4;
                
            #endregion

            switch (КолвоПанелей.Text)
            {
                case "1":
                    панель1.Text = Convert.ToString(посадочнаяШиринаПанели - sumOfProfilsLenght);
                    панель1.SelectionChanged -= ШиринаПанели1_SelectionChanged;
                    break;
                case "2":
                    панель2.Text = Convert.ToString(посадочнаяШиринаПанели - посадочнаяШиринаПанели1 - sumOfProfilsLenght);
                    панель2.SelectionChanged -= ШиринаПанели2_SelectionChanged;
                    break;
                case "3":
                    панель3.Text = Convert.ToString(посадочнаяШиринаПанели - посадочнаяШиринаПанели1 - посадочнаяШиринаПанели2 - sumOfProfilsLenght);
                    панель3.SelectionChanged -= ШиринаПанели3_SelectionChanged;
                    break;
            }
        }

        void Sum()
        {
            RiznPanel(ШиринаПанели, ШиринаПанели1, ШиринаПанели2, ШиринаПанели3, ШиринаПрофиля1, ШиринаПрофиля2, ШиринаПрофиля3, ШиринаПрофиля4);
        }
        
        void ШиринаПанели_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Sum();
        }
        void ШиринаПанели1_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ШиринаСъемнойПанели1.Text = ШиринаПанели1.Text;
            Sum();
        }

        void ШиринаПанели2_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ШиринаСъемнойПанели2.Text = ШиринаПанели2.Text;
            if (!ШиринаПанели2.IsEnabled) return;
            Sum();
        }

        void ШиринаПанели3_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ШиринаСъемнойПанели3.Text = ШиринаПанели3.Text;
            if (!ШиринаПанели3.IsEnabled) return;
            Sum();
        }

        void ШиринаПрофиля1_TextChanged(object sender, TextChangedEventArgs e)
        {
            Sum();
            int width;
            int.TryParse(ШиринаПрофиля1.Text, out width);
            Отступ1.Text = Convert.ToString(46 + width);
        }

        void ШиринаПрофиля2_TextChanged(object sender, TextChangedEventArgs e)
        {
            Sum();
            int width;
            int.TryParse(ШиринаПрофиля2.Text, out width);
            Отступ2.Text = Convert.ToString(132 + width);
        }

        void ШиринаПрофиля3_TextChanged(object sender, TextChangedEventArgs e)
        {
            Sum();
            int width;
            int.TryParse(ШиринаПрофиля3.Text, out width);
            Отступ3.Text = Convert.ToString(132 + width);
        }

        void ШиринаПрофиля4_TextChanged(object sender, TextChangedEventArgs e)
        {
            Sum();
        }

        void RemovablePanels_Initialized(object sender, EventArgs e)
        {
            ШиринаПанели1.IsEnabled = false;
        }

        #region Картинка установки

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

        // Перенос текстбоксов с размерами на рисунок
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
            Grid.SetRow(ШиринаСъемнойПанели1, 0);


            // Панель 2
            if (ШиринаСъемнойПанели2.Parent != null)
            {
                var parent = (Panel)ШиринаСъемнойПанели2.Parent;
                parent.Children.Remove(ШиринаСъемнойПанели2);
            }
            MyPanel.Children.Add(ШиринаСъемнойПанели2);
            Grid.SetRow(ШиринаСъемнойПанели2, 0);

            // Панель 3
            if (ШиринаСъемнойПанели3.Parent != null)
            {
                var parent = (Panel)ШиринаСъемнойПанели3.Parent;
                parent.Children.Remove(ШиринаСъемнойПанели3);
            }
            MyPanel.Children.Add(ШиринаСъемнойПанели3);
            Grid.SetRow(ШиринаСъемнойПанели3, 0);

            // Профиль 2
            if (ТипПроф2.Parent != null)
            {
                var parent = (Panel)ТипПроф2.Parent;
                parent.Children.Remove(ТипПроф2);
            }
            MyPanel.Children.Add(ТипПроф2);
            Grid.SetRow(ТипПроф2, 0);

            // Профиль 3
            if (ТипПроф3.Parent != null)
            {
                var parent = (Panel)ТипПроф3.Parent;
                parent.Children.Remove(ТипПроф3);
            }
            MyPanel.Children.Add(ТипПроф3);
            Grid.SetRow(ТипПроф3, 0);

            // Тип Усил Панели1 
            if (ТипУсилПанели1.Parent != null)
            {
                var parent = (Panel)ТипУсилПанели1.Parent;
                parent.Children.Remove(ТипУсилПанели1);
            }
            MyPanel.Children.Add(ТипУсилПанели1);
            Grid.SetRow(ТипУсилПанели1, 5);

            // Профиль 1
            if (ТипПроф1.Parent != null)
            {
                var parent = (Panel)ТипПроф1.Parent;
                parent.Children.Remove(ТипПроф1);
            }
            MyPanel.Children.Add(ТипПроф1);
            Grid.SetRow(ТипПроф1, 0);
            
            // Тип Усил Панели2 
            if (ТипУсилПанели2.Parent != null)
            {
                var parent = (Panel)ТипУсилПанели2.Parent;
                parent.Children.Remove(ТипУсилПанели2);
            }
            MyPanel.Children.Add(ТипУсилПанели2);
            Grid.SetRow(ТипУсилПанели2, 5);

            // Профиль 4
            if (ТипПроф4.Parent != null)
            {
                var parent = (Panel)ТипПроф4.Parent;
                parent.Children.Remove(ТипПроф4);
            }
            MyPanel.Children.Add(ТипПроф4);
            Grid.SetRow(ТипПроф4, 0);

            ТипПроф4.Visibility = Visibility.Collapsed;
            ТипПроф1.Visibility = Visibility.Collapsed;

        }

        #endregion

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
        }

        void ШиринаСъемнойПанели2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
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

                    ТипПроф2.SelectedIndex = 0;
                    ТипПроф3.SelectedIndex = 0;
                    
                    ТипПроф2.Visibility = Visibility.Collapsed;
                    ТипПроф3.Visibility = Visibility.Collapsed;

                    break;

                case "02":
                    КолвоПанелей.Text = "2";
                    ШиринаСъемнойПанели1.IsEnabled = true;
                    Grid.SetColumn(Lenght, 6);
                    Grid.SetColumn(ШиринаСъемнойПанели1, 4);
                    
                    Grid.SetColumn(ТипПроф2, 5);

                    Grid.SetColumn(ШиринаСъемнойПанели2, 7);
                    Grid.SetColumn(ШиринаСъемнойПанели3, 9);
                    ШиринаСъемнойПанели1.Visibility = Visibility.Visible;
                    ШиринаСъемнойПанели2.Visibility = Visibility.Visible;
                    ШиринаСъемнойПанели3.Visibility = Visibility.Collapsed;
                    
                    ТипПроф3.SelectedIndex = 0;
                    ТипПроф2.Visibility = Visibility.Visible;
                    ТипПроф3.Visibility = Visibility.Collapsed;

                    break;

                case "03":
                    КолвоПанелей.Text = "3";
                    ШиринаСъемнойПанели1.IsEnabled = true;
                    ШиринаСъемнойПанели2.IsEnabled = true;
                    Grid.SetColumn(Lenght, 5);
                    Grid.SetColumn(ШиринаСъемнойПанели1, 3);
                    Grid.SetColumn(ТипПроф2, 4);
                    Grid.SetColumn(ШиринаСъемнойПанели2, 5);
                    Grid.SetColumn(ТипПроф3, 7);
                    Grid.SetColumn(ШиринаСъемнойПанели3, 8);
                    ШиринаСъемнойПанели1.Visibility = Visibility.Visible;
                    ШиринаСъемнойПанели2.Visibility = Visibility.Visible;
                    ШиринаСъемнойПанели3.Visibility = Visibility.Visible;
                    
                    ТипПроф2.Visibility = Visibility.Visible;
                    ТипПроф3.Visibility = Visibility.Visible;

                    break;
            }
        }

        void ТипУстановкиПромежуточныеВставки_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ТипУстановкиПромежуточныеВставки == null) return;
            var item = (ComboBoxItem)ТипУстановкиПромежуточныеВставки.SelectedItem;

            switch (item.ToolTip.ToString())
            {
                case "0":

                    ТипУсилПанели1.Visibility = Visibility.Collapsed;
                    ТипУсилПанели2.Visibility = Visibility.Collapsed;

                    ТипУсилПанели1.Text = "-";
                    ТипУсилПанели2.Text = "-";

                    break;

                case "1":
                    
                    ТипУсилПанели1.Visibility = Visibility.Visible;
                    ТипУсилПанели2.Visibility = Visibility.Visible;
                    ТипУсилПанели1.SelectedIndex = 0;
                    ТипУсилПанели2.SelectedIndex = 0;

                    Grid.SetColumn(ТипУсилПанели1, 2);
                    Grid.SetColumn(ТипУсилПанели2, 9);

                    break;

                case "2":
                    
                    ТипУсилПанели1.Visibility = Visibility.Visible;
                    ТипУсилПанели2.Visibility = Visibility.Collapsed;
                    ТипУсилПанели1.SelectedIndex = 0;
                    ТипУсилПанели2.Text = "-";

                    Grid.SetColumn(ТипУсилПанели1, 2);
                    Grid.SetColumn(ТипУсилПанели2, 9);

                    break;

                case "3":

                    ТипУсилПанели1.Visibility = Visibility.Collapsed;
                    ТипУсилПанели2.Visibility = Visibility.Visible;
                    ТипУсилПанели1.Text = "-";
                    ТипУсилПанели2.SelectedIndex = 0;

                    Grid.SetColumn(ТипУсилПанели1, 2);
                    Grid.SetColumn(ТипУсилПанели2, 9);

                    break;
            }

            ШиринаПрофиля1.Text = ТипУсилПанели1.Visibility == Visibility.Visible ? "130" : "";
            ШиринаПрофиля4.Text = ТипУсилПанели2.Visibility == Visibility.Visible ? "130" : "";
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

                ПанелиУстановки.Visibility = Visibility.Visible;

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
        
        private void ШиринаСъемнойПанели3_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ШиринаСъемнойПанели3.Foreground = ШиринаСъемнойПанели3.Text.Contains("-") ? Brushes.Red : Brushes.Black;
        }

        #region Покраска

        private void Ral1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ral1.ToolTip = Ral1.SelectedValue;
        }

        private void Ral2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ral2.ToolTip = Ral2.SelectedValue;
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

        #endregion

        #region Материал

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

        #endregion

        #region Visibilyty

        private int Visibilyty { get; set; }
        private readonly DispatcherTimer _dispatcherTimer;
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


        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var modelSw = new ModelSw();
            modelSw.Profil(Convert.ToDouble(HeightU.Text) - 40, "03");
        }

        private void ТипУсилПанели1_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (ТипУсилПанели1 == null) return;

            if (ТипУсилПанели1.Visibility == Visibility.Visible)
            {
                ТипПроф1.Visibility = Visibility.Visible;
                Grid.SetColumn(ТипПроф1, 2);
            }
            else
            {
                ТипПроф1.Visibility = Visibility.Collapsed;
            }
        }

        private void ТипУсилПанели2_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (ТипУсилПанели2 == null) return;

            if (ТипУсилПанели2.Visibility == Visibility.Visible)
            {
                ТипПроф4.Visibility = Visibility.Visible;
                Grid.SetColumn(ТипПроф4, 9);
            }
            else
            {
                ТипПроф4.Visibility = Visibility.Collapsed;
            }
        }

       
    }
}
