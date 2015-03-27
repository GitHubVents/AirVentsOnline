using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AirVentsCadWpf.AirVentsClasses;
using AirVentsCadWpf.AirVentsClasses.UnitsBuilding;
using AirVentsCadWpf.Properties;
using BomPartList.Спецификации;
using EdmLib;
using NLog;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace AirVentsCadWpf.DataControls.Specification
{

    /// <summary>
    /// 
    /// </summary>
    public partial class SpecificationUc
    {
        List<string> AsmsNames { get; set; }
        string SelectedName { get; set; }

        #region Основные поля

        string _путьКСборке;
        List<СпецификацияДляВыгрузкиСборки.ДанныеДляВыгрузки> _спецификация;
        private readonly СпецификацияДляВыгрузкиСборки _работаСоСпецификацией;

        #endregion

        #region Конструктор класса

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecificationUc"/> class.
        /// </summary>
        public SpecificationUc()
        {
          
            InitializeComponent();
            ТаблицаСпецификации.Visibility = Visibility.Collapsed;
            СпецификацияТаблица.Visibility = Visibility.Collapsed;
            Exp1.Visibility = Visibility.Collapsed;
            Exp2.Visibility = Visibility.Collapsed;
            Exp3.Visibility = Visibility.Collapsed;

            ДобавитьТаблицу.Visibility = Visibility.Collapsed;

            ВыгрузитьВXml.IsEnabled = false;

            ПереченьДеталей.Visibility = Visibility.Hidden;

            PartsList.Visibility = Visibility.Hidden;
            XmlParts.Visibility = Visibility.Hidden;

            _работаСоСпецификацией = new СпецификацияДляВыгрузкиСборки
            {
                ИмяХранилища = Settings.Default.PdmBaseName,
                ИмяПользователя = Settings.Default.UserName,
                ПарольПользователя = Settings.Default.Password,
            };

            var thread = new Thread(LoadAsmsList);
         //   thread.Start();

        }

        
        void LoadAsmsList()
        {
            var epdmSearch = new EpdmSearch { VaultName = Settings.Default.PdmBaseName };
            List<string> найденныеФайлы;
            epdmSearch.SearchDoc("900", EpdmSearch.SwDocType.SwDocAssembly, out найденныеФайлы);
            if (найденныеФайлы == null) return;
            AsmsNames = найденныеФайлы.ConvertAll(FileNameWithoutExt);

           // static AutoCompleteBehavior BomByPage;
        }

        void НайтиПолучитьСборкуЕеКонфигурацииПоИмени(string имяСборки)
        {
            //  имяСборки = "ВНС-904.40.300";

            _путьКСборке = _работаСоСпецификацией.ПолучениеПутиКСборке(имяСборки);

            if (_путьКСборке != "")
            {
                Конфигурации.ItemsSource = _работаСоСпецификацией.ПолучениеКонфигураций(_путьКСборке).ToList();
            }
            else
            {
                MessageBox.Show("Сборка не найдена!");
            }

            _спецификация = _работаСоСпецификацией.Спецификация(_путьКСборке, 10, "00");
            ТаблицаСпецификации.ItemsSource = _спецификация;
        }

        #endregion

        #region Logger

        static readonly NLog.Logger Logger = LogManager.GetLogger("SpecificationUc");

        static void LoggerDebug(string logText)
        {
            Logger.Log(LogLevel.Debug, logText);
        }

        static void LoggerError(string logText)
        {
            Logger.Log(LogLevel.Error, logText);
        }

        static void LoggerFatal(string logText)
        {
            Logger.Log(LogLevel.Fatal, logText);
        }

        static void LoggerInfo(string logText)
        {
            Logger.Log(LogLevel.Info, logText);
        }

        static void LoggerTrace(string logText)
        {
            Logger.Log(LogLevel.Trace, logText);
        }

        static void LoggerWarn(string logText)
        {
            Logger.Log(LogLevel.Warn, logText);
        }

        static void LoggerOff(string logText)
        {
            Logger.Log(LogLevel.Off, logText);
        }

        #endregion

        #region ВыгрузитьВXml
        
        void ВыгрузитьВXml_Click(object sender, RoutedEventArgs e)
        {
            LoggerInfo(string.Format("Запуск выгрузки в XML"));
            НайтиПолучитьСборкуЕеКонфигурацииПоИмени(AutoCompleteTextBox1.Text);
            ВыгрузкаСборкиВxml(Convert.ToBoolean(IncludeAsms.IsChecked));
        }
        
        void ИмяСборки_KeyDown(object sender, KeyEventArgs e)
        {
            ВыгрузитьВXmlClickHandler(e);
        }
        
        void ВыгрузитьВXmlClickHandler(KeyEventArgs e)
         {
             if (e.Key == Key.Enter)
             {
                 ВыгрузитьВXml_Click(this, new RoutedEventArgs());
             }
         }
        
        void ВыгрузитьСборку(string путьКСборке)
         {
            
            var имяСборки = Path.GetFileNameWithoutExtension(путьКСборке);
            var списокКонфигурацийСборки = _работаСоСпецификацией.ПолучениеКонфигураций(путьКСборке).ToList().Distinct();

             try
             {
                 const string xmlPath = @"\\srvkb\SolidWorks Admin\XML\";

                  //   const string xmlPath = @"C:\Temp\";

                 var myXml = new System.Xml.XmlTextWriter(xmlPath + имяСборки + ".xml", System.Text.Encoding.UTF8);
                 
                 myXml.WriteStartDocument();
                 myXml.Formatting = System.Xml.Formatting.Indented;
                 myXml.Indentation = 2;
                 
                 // создаем элементы
                 myXml.WriteStartElement("xml");
                 myXml.WriteStartElement("transactions");
                 myXml.WriteStartElement("transaction");

                 myXml.WriteAttributeString("vaultName", "Vents-PDM");
                 myXml.WriteAttributeString("type", "wf_export_document_attributes");
                 myXml.WriteAttributeString("date", "1416475021");

                 // document
                 myXml.WriteStartElement("document");
                 myXml.WriteAttributeString("pdmweid", "73592");
                 myXml.WriteAttributeString("aliasset", "Export To ERP");

                 foreach (var config in списокКонфигурацийСборки)
                 {
                     var спецификация = _работаСоСпецификацией.Спецификация(путьКСборке, config);

                     foreach (var topAsm in спецификация.Where(x => x.Уровень == "0"))
                     {
                         #region XML

                         // Конфигурация
                         myXml.WriteStartElement("configuration");
                         myXml.WriteAttributeString("name", topAsm.Конфиг);

                         // Версия
                         myXml.WriteStartElement("attribute");
                         myXml.WriteAttributeString("name", "Версия");
                         myXml.WriteAttributeString("value", topAsm.Версия);
                         myXml.WriteEndElement();

                         // Масса
                         myXml.WriteStartElement("attribute");
                         myXml.WriteAttributeString("name", "Масса");
                         myXml.WriteAttributeString("value", topAsm.Масса);
                         myXml.WriteEndElement();

                         // Наименование
                         myXml.WriteStartElement("attribute");
                         myXml.WriteAttributeString("name", "Наименование");
                         myXml.WriteAttributeString("value", topAsm.Наименование);
                         myXml.WriteEndElement();

                         // Обозначение
                         myXml.WriteStartElement("attribute");
                         myXml.WriteAttributeString("name", "Обозначение");
                         myXml.WriteAttributeString("value", topAsm.Обозначение);
                         myXml.WriteEndElement();

                         // Раздел
                         myXml.WriteStartElement("attribute");
                         myXml.WriteAttributeString("name", "Раздел");
                         myXml.WriteAttributeString("value", topAsm.Раздел);
                         myXml.WriteEndElement();

                         // ERP code
                         myXml.WriteStartElement("attribute");
                         myXml.WriteAttributeString("name", "ERP code");
                         myXml.WriteAttributeString("value", topAsm.КодERP);
                         myXml.WriteEndElement();

                         // Код_Материала
                         myXml.WriteStartElement("attribute");
                         myXml.WriteAttributeString("name", "Код_Материала");
                         myXml.WriteAttributeString("value", topAsm.КодМатериала);
                         myXml.WriteEndElement();

                         // Код документа
                         myXml.WriteStartElement("attribute");
                         myXml.WriteAttributeString("name", "Код документа");
                         myXml.WriteAttributeString("value", topAsm.КодДокумента);
                         myXml.WriteEndElement();

                         // Кол. Материала
                         myXml.WriteStartElement("attribute");
                         myXml.WriteAttributeString("name", "Кол. Материала");
                         myXml.WriteAttributeString("value", topAsm.КолМатериала);
                         myXml.WriteEndElement();

                         // Состояние
                         myXml.WriteStartElement("attribute");
                         myXml.WriteAttributeString("name", "Состояние");
                         myXml.WriteAttributeString("value", topAsm.Состояние);
                         myXml.WriteEndElement();

                         // Подсчет ссылок
                         myXml.WriteStartElement("attribute");
                         myXml.WriteAttributeString("name", "Подсчет ссылок");
                         myXml.WriteAttributeString("value", topAsm.Количество);
                         myXml.WriteEndElement();

                         // Конфигурация
                         myXml.WriteStartElement("attribute");
                         myXml.WriteAttributeString("name", "Конфигурация");
                         myXml.WriteAttributeString("value", topAsm.Конфиг);
                         myXml.WriteEndElement();

                         // Идентификатор
                         myXml.WriteStartElement("attribute");
                         myXml.WriteAttributeString("name", "Идентификатор");
                         myXml.WriteAttributeString("value", "");
                         myXml.WriteEndElement();

                         // references
                         myXml.WriteStartElement("references");

                         // document
                         myXml.WriteStartElement("document");
                         myXml.WriteAttributeString("pdmweid", "73592");
                         myXml.WriteAttributeString("aliasset", "Export To ERP");

                         foreach (var topLevel in спецификация.Where(x => x.Уровень == "1"))
                         {
                             #region XML

                             // Конфигурация
                             myXml.WriteStartElement("configuration");
                             myXml.WriteAttributeString("name", topLevel.Конфиг);

                             // Версия
                             myXml.WriteStartElement("attribute");
                             myXml.WriteAttributeString("name", "Версия");
                             myXml.WriteAttributeString("value", topLevel.Версия);
                             myXml.WriteEndElement();

                             // Масса
                             myXml.WriteStartElement("attribute");
                             myXml.WriteAttributeString("name", "Масса");
                             myXml.WriteAttributeString("value", topLevel.Масса);
                             myXml.WriteEndElement();

                             // Наименование
                             myXml.WriteStartElement("attribute");
                             myXml.WriteAttributeString("name", "Наименование");
                             myXml.WriteAttributeString("value", topLevel.Наименование);
                             myXml.WriteEndElement();

                             // Обозначение
                             myXml.WriteStartElement("attribute");
                             myXml.WriteAttributeString("name", "Обозначение");
                             myXml.WriteAttributeString("value", topLevel.Обозначение);
                             myXml.WriteEndElement();

                             // Раздел
                             myXml.WriteStartElement("attribute");
                             myXml.WriteAttributeString("name", "Раздел");
                             myXml.WriteAttributeString("value", topLevel.Раздел);
                             myXml.WriteEndElement();

                             // ERP code
                             myXml.WriteStartElement("attribute");
                             myXml.WriteAttributeString("name", "ERP code");
                             myXml.WriteAttributeString("value", topLevel.КодERP);
                             myXml.WriteEndElement();

                             // Код_Материала
                             myXml.WriteStartElement("attribute");
                             myXml.WriteAttributeString("name", "Код_Материала");
                             myXml.WriteAttributeString("value", topLevel.КодМатериала);
                             myXml.WriteEndElement();

                             // Код документа
                             myXml.WriteStartElement("attribute");
                             myXml.WriteAttributeString("name", "Код документа");
                             myXml.WriteAttributeString("value", topLevel.КодДокумента);
                             myXml.WriteEndElement();

                             // Кол. Материала
                             myXml.WriteStartElement("attribute");
                             myXml.WriteAttributeString("name", "Кол. Материала");
                             myXml.WriteAttributeString("value", topLevel.КолМатериала);
                             myXml.WriteEndElement();

                             // Состояние
                             myXml.WriteStartElement("attribute");
                             myXml.WriteAttributeString("name", "Состояние");
                             myXml.WriteAttributeString("value", topLevel.Состояние);
                             myXml.WriteEndElement();

                             // Подсчет ссылок
                             myXml.WriteStartElement("attribute");
                             myXml.WriteAttributeString("name", "Подсчет ссылок");
                             myXml.WriteAttributeString("value", topLevel.Количество);
                             myXml.WriteEndElement();

                             // Конфигурация
                             myXml.WriteStartElement("attribute");
                             myXml.WriteAttributeString("name", "Конфигурация");
                             myXml.WriteAttributeString("value", topLevel.Конфиг);
                             myXml.WriteEndElement();

                             // Идентификатор
                             myXml.WriteStartElement("attribute");
                             myXml.WriteAttributeString("name", "Идентификатор");
                             myXml.WriteAttributeString("value", "");
                             myXml.WriteEndElement();

                             myXml.WriteEndElement(); //configuration

                             #endregion
                         }

                         myXml.WriteEndElement(); // document

                         myXml.WriteEndElement(); // элемент references

                         myXml.WriteEndElement(); // configuration

                         #endregion
                     }
                 }

                 myXml.WriteEndElement(); // ' элемент DOCUMENT
                 myXml.WriteEndElement(); // ' элемент TRANSACTION
                 myXml.WriteEndElement(); // ' элемент TRANSACTIONS
                 myXml.WriteEndElement(); // ' элемент XML
                 // заносим данные в myMemoryStream
                 myXml.Flush();

                 Thread.Sleep(1000);

                 myXml.Close();
             }
             catch (Exception exception)
             {
                 MessageBox.Show(exception.Message);
             }
         }

        void ВыгрузкаСборкиВxml(bool включаяПодсборки)
        {
            var имяСборки = Path.GetFileNameWithoutExtension(_путьКСборке);

            var включаяВсеПодсборки = "";

            if (включаяПодсборки)
            {
                включаяВсеПодсборки = "(включая все подсборки)";
            }

            if (имяСборки == "")
            {
                MessageBox.Show("Сборка не найдена!");
                return;
            }
            
            LoggerInfo(string.Format("Начало выгрузки в XML установки {0}{1}", имяСборки, включаяВсеПодсборки));

            if (включаяПодсборки)
            {
                foreach (var путьКСборке in _спецификация.Where(x => Path.GetExtension(x.Путь).ToLower() == ".sldasm").Where(x => x.Раздел == "" || x.Раздел == "Сборочные единицы").Select(x => x.Путь).Distinct())
                {
                    ВыгрузитьСборку(путьКСборке);
                }
            }
            else
            {
                ВыгрузитьСборку(_путьКСборке);
            }

            LoggerInfo(string.Format("Выгрузка в XML установки {0}{1} завершена", имяСборки, включаяВсеПодсборки));
            MessageBox.Show(string.Format("Выгрузка сборки {0}{1} выполнена.", имяСборки, включаяВсеПодсборки));

        }

         //delegate void ВыгрузитьВXmlClickHandler(object sender, KeyEventArgs e);

         //readonly ВыгрузитьВXmlClickHandler ВыгрузитьВXmlClickHandler = DelegateMethod;

         //void DelegateMethod(object sender, KeyEventArgs e)
         //{
         //    if (e.Key == Key.Enter)
         //    {
         //        ВыгрузитьВXml_Click(this, new RoutedEventArgs());
         //    }
         //}

        #endregion
        
        #region ВыгрузитьСпецификацию

        void ВыгрузитьСпецификацию_Click(object sender, RoutedEventArgs e)
        {
           
        }

        void Конфигурации_LayoutUpdated(object sender, EventArgs e)
        {
            ВыгрузитьСпецификацию.IsEnabled = !Конфигурации.Items.IsEmpty;
        }

        void Поиск_Click(object sender, RoutedEventArgs e)
        {
            НайтиПолучитьСборкуЕеКонфигурацииПоИмени(ИмяСборки1.Text);
        }
        
        void ИмяСборки1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Поиск_Click(this, new RoutedEventArgs());
            }
        }


        private void ИмяСборки1_LayoutUpdated(object sender, EventArgs e)
        {
            if (ИмяСборки1.Text != Path.GetFileNameWithoutExtension(_путьКСборке))
            {
                Конфигурации.ItemsSource = null;
            }
        }

        List<СпецификацияДляВыгрузкиСборки.ДанныеДляВыгрузки> _bomPartsList;
        List<СпецификацияДляВыгрузкиСборки.ДанныеДляВыгрузки> _bomPartsListFiltered;
        List<СпецификацияДляВыгрузкиСборки.ДанныеДляВыгрузки> _bomPartsListOriginal;

        readonly List<СпецификацияДляВыгрузкиСборки.ДанныеДляВыгрузки> _bomListSelectedPrts = new List<СпецификацияДляВыгрузкиСборки.ДанныеДляВыгрузки>();

        readonly SolidColorBrush _orangeColorBrush = new SolidColorBrush(Colors.LavenderBlush);
        readonly SolidColorBrush _whiteColorBrush = new SolidColorBrush(Colors.White);
        readonly SolidColorBrush _lightCyanColorBrush = new SolidColorBrush(Colors.LightCyan);

        private void Table_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var bomCells = (СпецификацияДляВыгрузкиСборки.ДанныеДляВыгрузки)e.Row.DataContext;

            if (bomCells.Errors == "")
            {
                e.Row.Background = e.Row.GetIndex() % 2 == 1 ? _lightCyanColorBrush : _whiteColorBrush;
            }
            else
            {
                e.Row.Background = _orangeColorBrush;
            }
        }

        private void TablePrt_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (BomTablePrt.SelectedValue == null) return;
            var row = ((СпецификацияДляВыгрузкиСборки.ДанныеДляВыгрузки)(BomTablePrt.SelectedValue));
            if (_bomListSelectedPrts.Any(bomListSelectedPrt => row.Обозначение == bomListSelectedPrt.Обозначение)) return;
            _bomListSelectedPrts.Add(row);
            BomTableSelectedPrt.ItemsSource = _bomListSelectedPrts.OrderBy(x => x.Обозначение);
        }

        private void TablePrt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BomTablePrt.SelectedValue == null) return;
            var row = ((СпецификацияДляВыгрузкиСборки.ДанныеДляВыгрузки)(BomTablePrt.SelectedValue));
         //   MaterialsList.SelectedValue = row.Материал;

         //   SelectedPart.ItemsSource = _bomPartsList.Where(x => x.Обозначение == row.ОбозначениеDocMgr);
        }


        private void BeginUpdate1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void УдалитьВыбранную_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearList_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MaterialsList_LayoutUpdated(object sender, EventArgs e)
        {

        }

        private void BeginUpdatePartProp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FilterTextBox_OnSelectionChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BomTableSelectedPrt_LayoutUpdated(object sender, EventArgs e)
        {

        }

        private void BeginUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BomTableSelectedPrt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void UpdateCutList_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AssemblyInfo_LayoutUpdated(object sender, EventArgs e)
        {

        }

        #endregion

        #region Выгрузка eDrawing

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var fileName = PartFile.Text;
            var extension = Path.GetExtension(fileName);
            if (extension == null || extension.ToLower() != ".sldprt") return;
            var cutlistClass = new MakeDxfUpdatePartData.MakeDxfExportPartDataClass
            {
                PdmBaseName = Settings.Default.PdmBaseName
            };
            string file;
            bool isErrors;
            cutlistClass.CreateFlattPatternUpdateCutlistAndEdrawing(fileName, out file, out isErrors, false, false);

            //var thread = new Thread(RegistationEdrw);
            //thread.Start();
            
            //cutlistClass2.RegistrationPdm(@"E:\Vents-PDM\Проекты\AirVents\EmptyPart.eprt");
            //ВыгрузкаeDrawing("");
        }

        void ВыгрузкаeDrawing(string filePath)
        {
            var swApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");

            var swModel = (IModelDoc2)swApp.IActiveDoc;
            
            swApp.SetUserPreferenceIntegerValue((int)swUserPreferenceIntegerValue_e.swEdrawingsSaveAsSelectionOption, (int)swEdrawingSaveAsOption_e.swEdrawingSaveAll);
            swModel.Extension.SaveAs("C:\\Temp\\" + swModel.GetTitle() + ".EPRT",
                (int)swSaveAsVersion_e.swSaveAsCurrentVersion,
                (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, 0, 0);
        }

        #endregion

        void Registration(object sender, RoutedEventArgs e)
        {
            var fileName = PartFile.Text;
            var extension = Path.GetExtension(fileName);
            if (extension == null || extension.ToLower() != ".sldprt") return;
            var cutlistClass = new MakeDxfUpdatePartData.MakeDxfExportPartDataClass
            {
                PdmBaseName = Settings.Default.PdmBaseName
            };
            cutlistClass.RegistrationPdm(fileName);
        }

        void Выгрузить_PDF(object sender, RoutedEventArgs e)
        {
            //E:\Vents-PDM\Заказы AirVents Frameless\AV02 E:\Vents-PDM\Заказы AirVents Frameless\AV02 001002\AV02 001002 Section A.slddrw
            //var fileName = PartFile.Text;
            //var extension = Path.GetExtension(fileName);
            //if (extension == null || extension.ToLower() != ".slddrw") return;
            
            var cutlistClass = new MakeDxfUpdatePartData.MakeDxfExportPartDataClass()
            {
                PdmBaseName = Settings.Default.PdmBaseName
            };
            try
            {
                string file;
                cutlistClass.SaveDrwAsPdf(@"E:\Vents-PDM\Заказы AirVents Frameless\AV02 E:\Vents-PDM\Заказы AirVents Frameless\AV02 001002\AV02 001002 Section A.slddrw", out file);
                MessageBox.Show(file);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.StackTrace);
            }
        }

        void AddToPdm(object sender, RoutedEventArgs e)
        {
            //var cutlistClass = new MakeDxfUpdatePartData.MakeDxfUpdatePartDataClass
            //{
            //    PdmBaseName = Settings.Default.PdmBaseName
            //};
            const string file = @"C:\Tets_debag\ВНС-900.98.0087.pdf";
            //cutlistClass.AddToPdmByPath(file, Settings.Default.PdmBaseName);

            AddToPdmByPath(file, Settings.Default.PdmBaseName);
        }

        static void AddToPdmByPath(string path, string pdmBase)
        {
            try
            {
                LoggerInfo(string.Format("Создание папки по пути {0} для сохранения", path));
                var vault1 = new EdmVault5();
                if (!vault1.IsLoggedIn)
                {
                    vault1.LoginAuto(pdmBase, 0);
                }

                var vault2 = (IEdmVault7)vault1;
                var fileDirectory = new FileInfo(path).DirectoryName;
                var fileFolder = vault2.GetFolderFromPath(fileDirectory);
                var result = fileFolder.AddFile(fileFolder.ID, "", Path.GetFileName(path));


                //var edmFolder6 = (IEdmFolder6)vault2.GetFolderFromPath(fileDirectory);
                //MessageBox.Show(edmFolder6.Name);
                //var edmAddFileInfo = new EdmAddFileInfo
                //{
                //    mbsNewName = "",
                //    mbsPath = path,
                //    mlEdmAddFlags = (int)EdmAddFlag.EdmAdd_Simple,
                //    mlFileID = 0,
                //    mlSrcDocumentID = 0,
                //    mlSrcProjectID = 0
                //};
                //edmFolder6.AddFiles(edmFolder6.ID,ref edmAddFileInfo, null);
                //edmFolder6.AddFile(edmFolder6.ID, "", path);
                
                //directoryInfo = new DirectoryInfo(path);
                //if (directoryInfo.Parent == null) return;
                //var parentFolder = vault2.GetFolderFromPath(directoryInfo.Parent.FullName);
                //parentFolder.AddFolder(0, directoryInfo.Name);
                
                LoggerInfo(string.Format("Получение папки по пути {0} в папке {1} завершено. {1}", fileDirectory, fileFolder.Name));
                
                    //parentFolder.AddFolder(0, directoryInfo.Name);
                 LoggerInfo(string.Format("Создание файла по пути {0} в папке {2} завершено. {1}", path, result, fileFolder.Name));
                //}
            }
            catch (Exception exception)
            {
                LoggerError(string.Format("Не удалось создать файл по пути {0}. Ошибка {1}", path, exception.StackTrace));
            }
        }

        void ПоискEpdm(object sender, RoutedEventArgs e)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var epdmSearch = new EpdmSearch { VaultName = Settings.Default.PdmBaseName };
            List<string> найденныеФайлы;
            epdmSearch.SearchDoc("901.30.1.", EpdmSearch.SwDocType.SwDocPart, out найденныеФайлы);
            if (найденныеФайлы == null) return;
            СписокНайденныхФайлов.ItemsSource = найденныеФайлы.ConvertAll(FileName);
            stopwatch.Stop();
            MessageBox.Show(String.Format(" Поиск {1} файлов {0} мс ", stopwatch.Elapsed, найденныеФайлы.Count));

            #region Проверка на листовую деталь

            //var swApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
            //var model = swApp.IActiveDoc;
            //MessageBox.Show(IsSheetMetalPart((IPartDoc)model).ToString());

            #endregion
        }

        static string FileName(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            return fileName;
        }
        static string FileNameWithoutExt(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            return fileName;
        }

        static bool IsSheetMetalPart(IPartDoc swPart)
        {
            LoggerInfo("Проверка на листовую деталь IsSheetMetalPart()");
            try
            {
                var vBodies = swPart.GetBodies2((int)swBodyType_e.swSolidBody, false);

                foreach (Body2 vBody in vBodies)
                {
                    try
                    {
                        var isSheetMetal = vBody.IsSheetMetal();
                        if (!isSheetMetal) continue;
                        LoggerInfo("Проверка завершена");
                        return true;
                    }
                    catch (Exception exception)
                    {
                        LoggerError(exception.StackTrace);
                        return false;
                    }
                }

                //LoggerInfo("Проверка завершена");
                return false;
            }
            catch (Exception)
            {
                LoggerInfo("Проверка завершена. Деталь не из листового материала.");
                // var swApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
                // swApp.ExitApp();
                return false;
            }
        }

        private void Сборка_TextInput(object sender, TextCompositionEventArgs e)
        {
           // MessageBox.Show(" Поиск {1} файлов {0} мс ");

            //var stopwatch = new Stopwatch();
            //stopwatch.Start();
            //var epdmSearch = new EpdmSearch { VaultName = Settings.Default.PdmBaseName };
            //if (Сборка.Text.Length < 9) return;
            //List<string> найденныеФайлы;
            //epdmSearch.SearchDoc(Сборка.Text, EpdmSearch.SwDocType.SwDocPart, out найденныеФайлы);
            //if (найденныеФайлы == null) return;
            //Сборка.ItemsSource = найденныеФайлы.ConvertAll(FileName);
            //stopwatch.Stop();
            //MessageBox.Show(String.Format(" Поиск {1} файлов {0} мс ", stopwatch.Elapsed, найденныеФайлы.Count));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (AsmsNames!=null)
            {
              //  MessageBox.Show(_AsmsNames.Count.ToString());
                СписокНайденныхФайлов.ItemsSource = AsmsNames;
                
                AutoCompleteTextBox1.ItemsSource = AsmsNames;
               // AutoCompleteTextBox1.SelectedItem = SelectedName;
            }
        }

        private void AutoCompleteTextBox1Reload()
        {
           // if (AsmsNames == null) return;

           // MessageBox.Show(AutoCompleteTextBox1.Text);

            var epdmSearch = new EpdmSearch { VaultName = Settings.Default.PdmBaseName };

            List<string> найденныеФайлы;
            epdmSearch.SearchDoc(AutoCompleteTextBox1.Text, EpdmSearch.SwDocType.SwDocAssembly, out найденныеФайлы);
            if (найденныеФайлы == null) return;
            AsmsNames = найденныеФайлы.ConvertAll(FileNameWithoutExt);
            AutoCompleteTextBox1.ItemsSource = AsmsNames;
            
            //AutoCompleteTextBox1.SelectedItem = SelectedName;

            AutoCompleteTextBox1.FilterMode = AutoCompleteFilterMode.Contains;
        }



        private void AutoCompleteTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click_3(this, new RoutedEventArgs());
            }



         ////   MessageBox.Show(AutoCompleteTextBox1.Text);
         //   if (AutoCompleteTextBox1.Text.Length > 8)
         //   {
         //       MessageBox.Show(AutoCompleteTextBox1.Text.Length.ToString());
         //       AutoCompleteTextBox1Reload();
         //   }
            
            //if (e.Key == Key.Enter)
            //{
            //  //  MessageBox.Show(AutoCompleteTextBox1.Text);
            //  //  AutoCompleteTextBox1Reload();
            //}
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            AutoCompleteTextBox1Reload();

           // AutoCompleteTextBox1.DropDownOpening += AutoCompleteTextBox1_DropDownOpening;


         //   AutoCompleteTextBox1_KeyDown(sender, new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Down));
            

            //AutoCompleteTextBox1_SelectionChanged(sender, null);
         //   AutoCompleteTextBox1_KeyDown(sender, new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Down));
            //AutoCompleteTextBox1_TextChanged(sender, new RoutedEventArgs());

            
            //  AutoCompleteTextBox1.Text = AutoCompleteTextBox1.Text + "";
            //  AutoCompleteTextBox1.TextChanged += AutoCompleteTextBox1_TextChanged;


        }

        void AutoCompleteTextBox1_DropDownOpening(object sender, RoutedPropertyChangingEventArgs<bool> e)
        {
            MessageBox.Show("gvwg");
        }

       

        private void AutoCompleteTextBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ВыгрузитьВXml.IsEnabled = false;
          //  MessageBox.Show("AutoCompleteTextBox1_SelectionChanged");
            ВыгрузитьВXml.IsEnabled = AsmsNames.Contains(AutoCompleteTextBox1.Text);

            
          
        }

        private void AutoCompleteTextBox1_TextChanged(object sender, RoutedEventArgs e)
        {
           // AutoCompleteTextBox1.DropDownClosed;

            
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(System.Environment.UserName);
            MessageBox.Show(System.Net.Dns.GetHostName());

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            AirVents_AddOrder();
            //MessageBox.Show("Hello world " + "\n userName -" + System.Environment.userName + "\n HostName -" + System.Net.Dns.GetHostName());
           // WriteToBase("wqdhsfn", "aqegsh", "adghdn", "qegteht", "aefsd");
        }


        static void AirVents_AddOrder()
        {
            using (var con = new SqlConnection(Settings.Default.ConnectionToSQL))
            {
                try
                {
                    con.Open();
                    var sqlCommand = new SqlCommand("AddErrorLog", con) { CommandType = CommandType.StoredProcedure };

                    var sqlParameter = sqlCommand.Parameters;

                    sqlParameter.AddWithValue("@userName", "hs878fdg");
                    sqlParameter.AddWithValue("@errorModule", "hsf87987dg");
                    sqlParameter.AddWithValue("@errorMessage", "hsfdg");
                    sqlParameter.AddWithValue("@errorCode", "hsfdg");
                    sqlParameter.AddWithValue("@errorTime", DateTime.Now);
                    sqlParameter.AddWithValue("@errorState", "hsfdg");
                    sqlParameter.AddWithValue("@errorFunction", "hsfdg");
                    

                    //var returnParameter = sqlCommand.Parameters.Add("@ProjectNumber", SqlDbType.Int);
                    //returnParameter.Direction = ParameterDirection.ReturnValue;

                   sqlCommand.ExecuteNonQuery();

                    //var result = Convert.ToInt32(returnParameter.Value);

                    //switch (result)
                    //{
                    //    case 0:
                    //        MessageBox.Show("Подбор №" + Номерподбора.Text + " уже существует!");
                    //        break;
                    //}

                }
                catch (Exception exception)
                {
                    MessageBox.Show("Введите корректные данные! " + exception.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        static void WriteToBase(string описание, string тип, string код, string модуль, string функция)
        {
            using (var con = new SqlConnection(Settings.Default.ConnectionToSQL))
            {
                try
                {
                    con.Open();
                    var sqlCommand = new SqlCommand("AirVents_AddErrorLog", con) { CommandType = CommandType.StoredProcedure };
                    //sqlCommand.Parameters.Add("@userName", SqlDbType.NVarChar).Value = System.Environment.userName + " (" + System.Net.Dns.GetHostName() + ")";
                    //sqlCommand.Parameters.Add("@errorModule", SqlDbType.NVarChar).Value = модуль;
                    //sqlCommand.Parameters.Add("@errorMessage", SqlDbType.NVarChar).Value = функция;
                    //sqlCommand.Parameters.Add("@errorCode", SqlDbType.NVarChar).Value = код;
                    //sqlCommand.Parameters.Add("@errorTime", SqlDbType.DateTime).Value = DateTime.Now;
                    //sqlCommand.Parameters.Add("@errorMessage", SqlDbType.NVarChar).Value = описание;
                    //sqlCommand.Parameters.Add("@errorState", SqlDbType.NVarChar).Value = тип;
                    var myDateTime = DateTime.Now;
                    var sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                    sqlCommand.Parameters.Add("@userName", SqlDbType.Char).Value = "asdgh";
                    sqlCommand.Parameters.Add("@errorModule", SqlDbType.Char).Value = "asdgh";
                    sqlCommand.Parameters.Add("@errorFunction", SqlDbType.Char).Value = "asdgh";
                    sqlCommand.Parameters.Add("@errorCode", SqlDbType.Char).Value = "asdgh";
                    sqlCommand.Parameters.Add("@errorTime", SqlDbType.DateTime).Value = DateTime.Now;
                    sqlCommand.Parameters.Add("@errorState", SqlDbType.Char).Value = "asdgh";
                    sqlCommand.Parameters.Add("@errorMessage", SqlDbType.Char).Value = "asdgh";


                    sqlCommand.ExecuteNonQuery();

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.StackTrace);
                    return;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void ДобавитьТаблицу_Click(object sender, RoutedEventArgs e)
        {
            var swApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");


            var swModel = (IModelDoc2)swApp.IActiveDoc;
            TableAnnotation myTable;
            //InsertTable(swModel, out myTable);

            var обозначения = new[] { "AV 0989897", "-01", "-02", "-03", "-04" };

            InsertTableРис(swModel, обозначения);

            swModel.Extension.SelectByID2("Общая таблица7", "GENERALTABLEFEAT", 0, 0, 0, false, 0, null, 0);
            swModel.SelectedFeatureProperties(0, 0, 0, 0, 0, 0, 0, true, true, "Общая таблица4_15");
      
            var myModelView = ((ModelView)(swModel.ActiveView));
            myModelView.TranslateBy(-0.00040965517241379308, 0.00040965517241379308);

            //boolstatus = swDoc.Extension.SelectByID2("Детальный элемент86@Sheet1", "ANNOTATIONTABLES", 0.42799491570140613, 0.28291848892280747, 0, false, 0, null, 0);
            //boolstatus = swDoc.Extension.SelectByID2("Детальный элемент86@Sheet1", "ANNOTATIONTABLES", 0.43157101326023029, 0.28403601940994005, 0, false, 0, null, 0);
//            swModel.ClearSelection2(true);

          
        }

        private static void InsertTable(IModelDoc2 swModel, out TableAnnotation myTable)
        {
            var swDrawing = ((DrawingDoc) (swModel));
            myTable = swDrawing.InsertTableAnnotation(0.417, 0.292, 2, 4, 2);
            if ((myTable == null)) return;
            myTable.BorderLineWeight = 2;
            myTable.GridLineWeight = 1;
            myTable.Text[0, 0] = "Обозначение";
            myTable.SetColumnWidth(0, 0.05, 0);
            myTable.Text[0, 1] = "Рис.";
            myTable.SetColumnWidth(1, 0.02, 0);
            //myTable.InsertRow();

        }

        private static void InsertTableРис(IModelDoc2 swModel, string[] обозначения)
        {
            var swDrawing = ((DrawingDoc)(swModel));
            var enumerable = обозначения as string[] ?? обозначения.ToArray();
            var myTable = swDrawing.InsertTableAnnotation(0.417, 0.292, 2, enumerable.Count()+1, 2);
            if ((myTable == null)) return;
            myTable.Title = "Таблица видов";
            myTable.BorderLineWeight = 2;
            myTable.GridLineWeight = 1;
            myTable.Text[0, 0] = "Обозначение";
           
            myTable.SetColumnWidth(0, 0.05, 0);
            myTable.Text[0, 1] = "Рис.";
            myTable.SetColumnWidth(1, 0.02, 0);
            
            for (var i = 1; i < обозначения.Count()+1; i++)
            {
                myTable.Text[i, 0] = обозначения[i-1];
                if (i>1)
                {
                    myTable.set_CellTextHorizontalJustification(i, 0, (int)swTextJustification_e.swTextJustificationRight);    
                }
                myTable.Text[i, 1] = (i).ToString();
            }
            

        }

        private void AutoCompleteTextBox1_DropDownClosed(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (ВыгрузитьВXml.IsEnabled)
            {
                ИмяСборки1.Text = AutoCompleteTextBox1.Text;
             //   Поиск_Click(null, null);
            }
        }

        private void ПолучитьПереченьДеталей_Click(object sender, RoutedEventArgs e)
        {
            //if (ПолучитьПереченьДеталей.IsChecked == true)
            //{
            //    _путьКСборке = _работаСоСпецификацией.ПолучениеПутиКСборке(AutoCompleteTextBox1.Text);

            //    if (_путьКСборке != "")
            //    {
            //        Конфигурация.ItemsSource = _работаСоСпецификацией.ПолучениеКонфигураций(_путьКСборке).ToList();
            //        Конфигурация.IsEnabled = true;
            //        Конфигурация.SelectedIndex = 0;
            //    }
            //    else
            //    {
            //        MessageBox.Show("Сборка не найдена!");
            //    }
            //}
            //else
            //{
            //    Конфигурация.Text = "";
            //    Конфигурация.IsEnabled = false;
            //}
        }


        class PartsListXml
        {
            public bool Xml { get; set; }
            public string Путь {  get; set; }
            public string Наименование { get { return Path.GetFileNameWithoutExtension(Путь); } }
        }
       

        private void ВыгрузитьВXml_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (ВыгрузитьВXml.IsEnabled)
            {
                ПолучитьПереченьДеталей.IsEnabled = true;
                
            }
            else
            {
                ПолучитьПереченьДеталей.IsEnabled = false;
                ПолучитьПереченьДеталей.IsChecked = false;
            }
        }

        private void Конфигурация_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var partsListXml = new List<PartsListXml>();

            foreach (var данныеДляВыгрузки in _работаСоСпецификацией.Спецификация(_путьКСборке, 10, Конфигурация.Text).Where(x => x.Путь.ToLower().EndsWith("prt")).Where(x => x.Раздел == "Детали" || x.Раздел == "").GroupBy(v => v.Путь))//.Where(g => g.Count() > 1))
            {
                partsListXml.Add(new PartsListXml
                {
                    Путь = данныеДляВыгрузки.Key,
                    Xml = new FileInfo(@"\\srvkb\SolidWorks Admin\XML\" + Path.GetFileNameWithoutExtension(данныеДляВыгрузки.Key) + ".xml").Exists    //  //@"C:\Temp\"
                });
            }

            PartsList.ItemsSource = partsListXml.OrderBy(x => x.Xml).ThenBy(x => x.Наименование);
        }

        private void PartsList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var partsListXml = (PartsListXml)e.Row.DataContext;

            if (partsListXml.Xml)
            {
                e.Row.Background = e.Row.GetIndex() % 2 == 1 ? _lightCyanColorBrush : _whiteColorBrush;
            }
            else
            {
                e.Row.Background = _orangeColorBrush;
            }
        }

        private void XMLParts_Click(object sender, RoutedEventArgs e)
        {
            var modelSw = new ModelSw();

            var list = PartsList.ItemsSource.OfType<PartsListXml>().ToList();
            
            //if (list == null) return;
            foreach (var newComponent in list)
            {
                modelSw.PartInfoToXml(newComponent.Путь);
            }

            Конфигурация_SelectionChanged(null, null);

        }

        private void ПолучитьПереченьДеталей_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
           
        }

        private void Конфигурация_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Конфигурация.Visibility == Visibility.Visible )
            {
                PartsList.Visibility = Visibility.Visible;
            }
            else
            {
                PartsList.Visibility = Visibility.Hidden;
                PartsList.ItemsSource = null;
            }
        }

      

        private void PartsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PartsList.HasItems)
            {
                XmlParts.Visibility = Visibility.Visible;
            }
            else
            {
                XmlParts.Visibility = Visibility.Hidden;
            }
                
        }

        private void ПолучитьПереченьДеталей_Unchecked(object sender, RoutedEventArgs e)
        {
            Конфигурация.Text = "";
            Конфигурация.IsEnabled = false;
            PartsList.ItemsSource = null;
            ПереченьДеталей.Visibility = Visibility.Hidden;


            if (PartsList.HasItems)
            {
                XmlParts.Visibility = Visibility.Visible;
                PartsList.Visibility = Visibility.Visible;
            }
            else
            {
                XmlParts.Visibility = Visibility.Hidden;
                PartsList.Visibility = Visibility.Hidden;
            }
        }

        private void ПолучитьПереченьДеталей_Checked(object sender, RoutedEventArgs e)
        {
            _путьКСборке = _работаСоСпецификацией.ПолучениеПутиКСборке(AutoCompleteTextBox1.Text);

            if (_путьКСборке != "")
            {
                Конфигурация.ItemsSource = _работаСоСпецификацией.ПолучениеКонфигураций(_путьКСборке).ToList();
                Конфигурация.IsEnabled = true;
                Конфигурация.SelectedIndex = 0;

                ПереченьДеталей.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Сборка не найдена!");
            }

            if (PartsList.HasItems)
            {
                XmlParts.Visibility = Visibility.Visible;
                PartsList.Visibility = Visibility.Visible;
            }
            else
            {
                XmlParts.Visibility = Visibility.Hidden;
                PartsList.Visibility = Visibility.Hidden;
            }
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (PartsListXml)PartsList.SelectedItem;
            MessageBox.Show(item.Путь);
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            var item = (PartsListXml)PartsList.SelectedItem;
            Process.Start(@item.Путь);
        }
    }
}
