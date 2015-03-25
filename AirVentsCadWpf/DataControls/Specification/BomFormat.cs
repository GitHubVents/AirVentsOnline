using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Xml.Linq;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace AirVentsCadWpf.DataControls.Specification
{
    class BomFormat
    {

        //TextFormatClass textFormatClass = new TextFormatClass();

   //     private  ITextFormat iTextFormat = new TextFormatClass();

        public List<BomData> ListXmlBomData(string xmlPath)
        {
            return Parse(xmlPath) ? BomDataList : null;
        }
        
        internal List<BomData> BomDataList;

        bool Parse(string xmlPath)
            {
                if (!xmlPath.EndsWith("xml"))
                {
                    return false;
                }

                var coordinates = XDocument.Load(xmlPath);//"C:\\Q-018657-01-0Bom.xml"

                MessageBox.Show(coordinates.ToString());

                BomDataList = new List<BomData>();

                try
                {
                    foreach (var coordinate in coordinates.Descendants("part"))
                    {
                        
                        var config = coordinate.Element("config");
                        var pathNameComponent = coordinate.Element("PathNameComponent");
                        var row = coordinate.Element("Row");
                        var itemNum = coordinate.Element("ItemNum");
                        var раздел = coordinate.Element("Раздел");
                        var обозначение = coordinate.Element("Обозначение");
                        var наименование = coordinate.Element("Наименование");
                        var формат = coordinate.Element("Формат");
                        var erpCode = coordinate.Element("ErpCode");
                        var кодМатериала = coordinate.Element("КодМатериала");
                        var примечание = coordinate.Element("Примечание");
                        var количество = coordinate.Element("Количество");
                        

                        BomDataList.Add(new BomData
                        {
                            Config = config == null ? "" : config.Value,
                            PathNameComponent = pathNameComponent == null ? "" : pathNameComponent.Value,
                            Row = row == null ? "" : row.Value,
                            ItemNum = itemNum == null ? "" : itemNum.Value,
                            Раздел = раздел == null ? "" : раздел.Value,
                            Обозначение = обозначение == null ? "" : обозначение.Value,
                            Наименование = наименование == null ? "" : наименование.Value,
                            Формат = формат == null ? "" : формат.Value,
                            ErpCode = erpCode == null ? "" : erpCode.Value,
                            КодМатериала = кодМатериала == null ? "" : кодМатериала.Value,
                            Примечание = примечание == null ? "" : примечание.Value,
                            Количество = количество == null ? "" : количество.Value
                        });

                      
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                    return false;
                }

                foreach (var coordinate in coordinates.Descendants("Item"))
                {

                    var config = coordinate.Element("config");
                    var pathNameComponent = coordinate.Element("PathNameComponent");
                    var row = coordinate.Element("Row");
                    var itemNum = coordinate.Element("ItemNum");
                    var раздел = coordinate.Element("Раздел");
                    var обозначение = coordinate.Element("Обозначение");
                    var наименование = coordinate.Element("Наименование");
                    var формат = coordinate.Element("Формат");
                    var erpCode = coordinate.Element("ErpCode");
                    var кодМатериала = coordinate.Element("КодМатериала");
                    var примечание = coordinate.Element("Примечание");
                    var количество = coordinate.Element("Количество");


                    BomDataList.Add(new BomData
                    {
                        Config = config == null ? "" : config.Value,
                        PathNameComponent = pathNameComponent == null ? "" : pathNameComponent.Value,
                        Row = row == null ? "" : row.Value,
                        ItemNum = itemNum == null ? "" : itemNum.Value,
                        Раздел = раздел == null ? "" : раздел.Value,
                        Обозначение = обозначение == null ? "" : обозначение.Value,
                        Наименование = наименование == null ? "" : наименование.Value,
                        Формат = формат == null ? "" : формат.Value,
                        ErpCode = erpCode == null ? "" : erpCode.Value,
                        КодМатериала = кодМатериала == null ? "" : кодМатериала.Value,
                        Примечание = примечание == null ? "" : примечание.Value,
                        Количество = количество == null ? "" : количество.Value
                    });
                }

                return true;
            }
        
        public void InsertBomTable()
        {
            var swApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
            InsertTableOne((IModelDoc2)swApp.IActiveDoc, BomDataList);
        }

         public static class BomSettings
         {
             public static int ИнтервалМеждустрочный = 3;
             public static int КоличествоСтрокНаПервомЛистеА4 = 26;
             public static int КоличествоСтрокНаВторомЛистеА4 = 30;
         }


        private static
            void InsertTableOne(IModelDoc2 swModel, IList<BomData> bomDataList)
        {
            var swDrawing = ((DrawingDoc)(swModel));
            var myTable = swDrawing.InsertTableAnnotation(0.02, 0.292, 1, bomDataList.Count() + 1 + 6 + 9, 7);
            if ((myTable == null)) return;
            myTable.Title = "Таблица видов";
            myTable.BorderLineWeight = 1;
            myTable.GridLineWeight = 1;

            #region Шапка
            //myTable.Text[0, 0] = "Форм.";
            //myTable.Text[0, 1] = "Зона.";
            //myTable.Text[0, 2] = "Поз.";
            //myTable.Text[0, 3] = "Позначення";
            //myTable.Text[0, 4] = "Найменування";
            //myTable.Text[0, 5] = "Кіл.";
            //myTable.Text[0, 6] = "Примітка";

            //myTable.MergeCells(3, 1, 3, 3);
            //myTable.SetCellTextFormat(0, 0, true, new TextFormatClass{ObliqueAngle = 90});
            #endregion

            #region Ширина колонок
            myTable.SetRowHeight(0, 0.015, 0);
            myTable.SetColumnWidth(0, 0.006, 0);
            myTable.SetColumnWidth(1, 0.006, 0);
            myTable.SetColumnWidth(2, 0.008, 0);
            myTable.SetColumnWidth(3, 0.07, 0);
            myTable.SetColumnWidth(4, 0.063, 0);
            myTable.SetColumnWidth(5, 0.010, 0);
            myTable.SetColumnWidth(6, 0.022, 0);
            #endregion

            #region Документация

            myTable.Text[2, 4] = "Документация";
            myTable.Text[4, 4] = bomDataList.First(x => x.Раздел == "").Наименование;

            var position = 5;

            for (var i = 1; i < position; i++)
            {
                myTable.SetRowHeight(i, 0.008, 0);
            }
            
            #endregion

            #region Сборочные единицы

            var razdel = "Сборочные единицы";

            var data = bomDataList.Where(x => x.Раздел == razdel).OrderBy(x=>x.Обозначение).ToList();

            myTable.Text[position + 1, 4] = razdel;


            for (var i = position; i < position + 1 + BomSettings.ИнтервалМеждустрочный; i++)
            {
                myTable.SetRowHeight(i, 0.008, 0);
            }

            position = position + 3;

            for (var i = 0; i < data.Count(); i++)
            {
                myTable.Text[position + i, 0] = data[i].Формат;
                myTable.Text[position + i, 2] = data[i].Row;
                myTable.Text[position + i, 3] = data[i].Обозначение;
                myTable.Text[position + i, 4] = data[i].Наименование;
                myTable.Text[position + i, 5] = data[i].Количество;
                myTable.Text[position + i, 6] = data[i].Примечание;
                myTable.SetRowHeight(position + i, data[i].Наименование.Count() < 35 ? 0.008 : 0.016, 0);
                if (i <= position) continue;
                myTable.set_CellTextHorizontalJustification(position + i, 0, (int)swTextJustification_e.swTextJustificationRight);
            }

            #endregion

            #region Стандартные изделия

            position = position + data.Count();

            razdel = "Стандартные изделия";

            data = bomDataList.Where(x => x.Раздел == razdel).ToList();

            myTable.Text[position + 1, 4] = razdel;


            for (var i = position; i < position + 1 + BomSettings.ИнтервалМеждустрочный; i++)
            {
                myTable.SetRowHeight(i, 0.008, 0);
            }

            position = position + 3;

            for (var i = 0; i < data.Count(); i++)
            {
                myTable.Text[position + i, 0] = data[i].Формат;
                myTable.Text[position + i, 2] = data[i].Row;
                myTable.Text[position + i, 3] = data[i].Обозначение;
                myTable.Text[position + i, 4] = data[i].Наименование;
                myTable.Text[position + i, 5] = data[i].Количество;
                myTable.Text[position + i, 6] = data[i].Примечание;
                myTable.SetRowHeight(position + i, data[i].Наименование.Count() < 35 ? 0.008 : 0.016, 0);
                if (i <= position) continue;
                myTable.set_CellTextHorizontalJustification(position + i, 0, (int)swTextJustification_e.swTextJustificationRight);
            }

            //for (var i = 1; i < bomDataList.Count() + 1; i++)
            //{
            //    myTable.Text[i, 0] = bomDataList[i - 1].Формат;
            //    myTable.Text[i, 2] = bomDataList[i - 1].Row;
            //    myTable.Text[i, 3] = bomDataList[i - 1].Обозначение;
            //    myTable.Text[i, 4] = bomDataList[i - 1].Наименование;
            //    myTable.Text[i, 5] = bomDataList[i - 1].Количество;
            //    myTable.Text[i, 6] = bomDataList[i - 1].Примечание;
            //    myTable.SetRowHeight(i, bomDataList[i - 1].Наименование.Count() < 35 ? 0.008 : 0.016, 0);
            //    if (i <= 1) continue;
            //    //myTable.SetRowHeight(i, 0.008, 0);
            //    myTable.set_CellTextHorizontalJustification(i, 0, (int)swTextJustification_e.swTextJustificationRight);
            //    //myTable.Text[i, 1] = (i).ToString();
            //}

            #endregion
        }
        
        public class BomData
        {
            public string Config { get; set; }
            public string PathNameComponent { get; set; }
            public string Row { get; set; }
            public string ItemNum { get; set; }
            public string Раздел { get; set; }
            public string Обозначение { get; set; }
            public string Наименование { get; set; }
            public string Формат { get; set; }
            public string ErpCode { get; set; }
            public string КодМатериала { get; set; }
            public string Примечание { get; set; }
            public string Количество { get; set; }
        }
      
    }
}
