using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using AirVentsCadWpf.Properties;
using AirVentsCadWpf.Логирование;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

// TODO Добавить при длине от 1200 мм в варианте с ножками третьи ножки посредине

namespace AirVentsCadWpf.AirVentsClasses.UnitsBuilding
{
    public partial class ModelSw
    {

        #region PanelsFrameless

        /// <summary>
        /// Panelses the frameless.
        /// </summary>
        /// <param name="typeOfPanel">The type of panel.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="materialP1">The material p1.</param>
        /// <param name="materialP2">The meterial p2.</param>
        /// <param name="скотч">if set to <c>true</c> [СКОТЧ].</param>
        /// <param name="усиление">if set to <c>true</c> [УСИЛЕНИЕ].</param>
        /// <param name="config">The configuration.</param>
        /// <param name="расположениеПанелей">The РАСПОЛОЖЕНИЕ ПАНЕЛЕЙ.</param>
        /// <param name="покрытие">The ПОКРЫТИЕ.</param>
        /// <param name="расположениеВставок">The РАСПОЛОЖЕНИЕ ВСТАВОК.</param>
        /// <param name="типУсиливающей">The СОСТАВНАЯ.</param>
        public void PanelsFrameless(string[] typeOfPanel, string width, string height, string[] materialP1,
            string[] materialP2, string скотч, bool усиление, string config,
            string расположениеПанелей, string[] покрытие, string расположениеВставок, string типУсиливающей)
        {
            var path = PanelsFramelessStr(typeOfPanel, width, height, materialP1, materialP2,
                скотч, усиление, config,
                расположениеПанелей, покрытие, расположениеВставок, типУсиливающей);
            if (path == "")  return; 

            if (MessageBox.Show(string.Format("Модель находится по пути:\n {0}\n Открыть модель?", new FileInfo(path).Directory),
                string.Format(" {0} ",
                    Path.GetFileNameWithoutExtension(new FileInfo(path).FullName)), MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
            {
                PanelsFramelessStr(typeOfPanel, width, height, materialP1, materialP2, скотч, усиление, config, расположениеПанелей, покрытие, расположениеВставок, типУсиливающей);
            }
        }
        
        #endregion

        #region PanelsFramelessStr

        /// <summary>
        /// Panelses the frameless string.
        /// </summary>
        /// <param name="typeOfPanel">The type of panel.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="materialP1">The material p1.</param>
        /// <param name="materialP2">The material p2.</param>
        /// <param name="скотч">The СКОТЧ.</param>
        /// <param name="усиление">if set to <c>true</c> [УСИЛЕНИЕ].</param>
        /// <param name="config">configuration</param>
        /// <param name="расположениеПанелей">РасположениеПанелей</param>
        /// <param name="покрытие">ПОКРЫТИЕ</param>
        /// <param name="расположениеВставок">РАСПОЛОЖЕНИЕ ВСТАВОК</param>
        /// <param name="типУсиливающей">СОСТАВНАЯ</param>
        /// <returns></returns>
        public string PanelsFramelessStr(string[] typeOfPanel, string width, string height, string[] materialP1, string[] materialP2, string скотч,
            bool усиление, string config, string расположениеПанелей, string[] покрытие, string расположениеВставок, string типУсиливающей)
        {
            InValPanels.StringValue(расположениеПанелей);

            ValProfils.StringValue(расположениеВставок);

          // MessageBox.Show(ValProfils.PsTy1, ValProfils.PsTy2);
//            return "-";

            #region Начальные проверки и пути

            if (IsConvertToInt(new[] { width, height }) == false) return "-";

            var panelsUpDownConfigString = (typeOfPanel[0] != "04" & typeOfPanel[0] != "05" & typeOfPanel[0] != "01")
                ? InValPanels.InValUpDown()
                : "";
            
            #region Обозначение ДЕТАЛЕЙ и СБОРКИ из БАЗЫ

            #region Покрытие

            if (покрытие[0] == "Без покрытия")
            {
                покрытие[1] = "0";
                покрытие[2] = "0";
            }
            if (покрытие[3] == "Без покрытия")
            {
                покрытие[4] = "0";
                покрытие[5] = "0";
            }

            #endregion

            #region Задание наименований

            var sqlBaseData = new SqlBaseData();
            var newId = sqlBaseData.PanelNumber() + 1;

            var partIds = new List<int>();

            #region панельВнешняя, панельВнутренняя

            var панельВнешняя =
                new AddingPanel
                {
                    PanelTypeName = typeOfPanel[1],
                    ElementType = 1,
                    Width = Convert.ToInt32(width),
                    Height = Convert.ToInt32(height),
                    PartThick = 40,
                    PartMat = Convert.ToInt32(materialP1[0]),
                    PartMatThick = Convert.ToDouble(materialP1[1].Replace('.', ',')),
                    Reinforcing = усиление,
                    Ral = покрытие[0],
                    CoatingType = покрытие[1],
                    CoatingClass = Convert.ToInt32(покрытие[2]),
                    Mirror = config.Contains("01"),
                    StickyTape = скотч.Contains("Со скотчем"),
                    StepInsertion = расположениеВставок
                };
            var id = панельВнешняя.AddPart();
            partIds.Add(id);
            панельВнешняя.NewName = "02-" + typeOfPanel[0] + "-1-" + id;
            
            var панельВнутренняя =
                new AddingPanel
                {
                    PanelTypeName = typeOfPanel[1],
                    ElementType = 2,
                    Width = Convert.ToInt32(width),
                    Height = Convert.ToInt32(height),
                    PartThick = 40,
                    PartMat = Convert.ToInt32(materialP2[0]),
                    PartMatThick = Convert.ToDouble(materialP2[1].Replace('.', ',')),
                    Reinforcing = усиление,
                    Ral = покрытие[3],
                    CoatingType = покрытие[4],
                    CoatingClass = Convert.ToInt32(покрытие[5]),
                    Mirror = config.Contains("01"),
                    Step = расположениеПанелей,
                    StickyTape = скотч.Contains("Со скотчем"),
                    StepInsertion = расположениеВставок
                };
            id = панельВнутренняя.AddPart();
            partIds.Add(id);
            панельВнутренняя.NewName = "02-" + typeOfPanel[0] + "-2-" + id;

            #endregion

            #region теплоизоляция, cкотч, усиливающаяРамкаПоШирине, усиливающаяРамкаПоВысоте

            var теплоизоляция =
                new AddingPanel
                {
                    PanelTypeName = typeOfPanel[1],
                    ElementType = 3,
                    Width = Convert.ToInt32(width),
                    Height = Convert.ToInt32(height),
                    PartThick = 40,
                    PartMat = 4900,

                    PartMatThick = 1,
                    Ral = "Без покрытия",
                    CoatingType = "0",
                    CoatingClass = Convert.ToInt32("0"),
                };
            id = теплоизоляция.AddPart();
            partIds.Add(id);
            теплоизоляция.NewName = "02-" + id;

            var cкотч =
                new AddingPanel
                {
                    PanelTypeName = typeOfPanel[1],
                    ElementType = 4,
                    Width = Convert.ToInt32(width),
                    Height = Convert.ToInt32(height),
                    PartThick = 40,
                    PartMat = 14800,

                    PartMatThick = 1,
                    Ral = "Без покрытия",
                    CoatingType = "0",
                    CoatingClass = Convert.ToInt32("0"),
                };
            id = cкотч.AddPart();
            partIds.Add(id);
            cкотч.NewName = "02-" + id;

            var pes =
                new AddingPanel
                {
                    PanelTypeName = typeOfPanel[1],
                    ElementType = 5,
                    Width = Convert.ToInt32(width),
                    Height = Convert.ToInt32(height),
                    PartThick = 40,
                    PartMat = 6700,

                    PartMatThick = 1,
                    Ral = "Без покрытия",
                    CoatingType = "0",
                    CoatingClass = Convert.ToInt32("0")
                };
            id = pes.AddPart();
            partIds.Add(id);
            pes.NewName = "02-" + id;

            var усиливающаяРамкаПоШирине =
               new AddingPanel
               {
                   PanelTypeName = typeOfPanel[1],
                   ElementType = 6,
                   Height = 40,
                   Width = Convert.ToInt32(width),
                   PartThick = 40,
                   PartMat = 1800,
                   PartMatThick = Convert.ToDouble("1".Replace('.', ',')),
                   Mirror = config.Contains("01"),
                   Step = расположениеПанелей,
                   StepInsertion = расположениеВставок,

                   
                   Ral = "Без покрытия",
                   CoatingType = "0",
                   CoatingClass = Convert.ToInt32("0")
               };
            id = усиливающаяРамкаПоШирине.AddPart();
            partIds.Add(id);
            усиливающаяРамкаПоШирине.NewName = "02-" + id;

            var усиливающаяРамкаПоВысоте =
                new AddingPanel
                {
                    PanelTypeName = typeOfPanel[1],
                    ElementType = 7,
                    Height = Convert.ToInt32(height),
                    Width = 40,
                    PartThick = 40,
                    PartMat = 1800,
                    PartMatThick = Convert.ToDouble("1".Replace('.', ',')),
                    Mirror = config.Contains("01"),
                    Step = расположениеПанелей,
                    StepInsertion = расположениеВставок,

                    
                    Ral = "Без покрытия",
                    CoatingType = "0",
                    CoatingClass = Convert.ToInt32("0"),
                };
            id = усиливающаяРамкаПоВысоте.AddPart();
            partIds.Add(id);
            усиливающаяРамкаПоВысоте.NewName = "02-" + id;

            var кронштейнДверной = new AddingPanel
            {
                PanelTypeName = typeOfPanel[1],
                ElementType = 9,
                Height = Convert.ToInt32(height),
                Width = 20,
                PartThick = 40,
                PartMat = 1800,
                PartMatThick = Convert.ToDouble("1".Replace('.', ',')),
                Mirror = config.Contains("01"),
                Step = расположениеПанелей,
                StepInsertion = расположениеВставок,


                Ral = "Без покрытия",
                CoatingType = "0",
                CoatingClass = Convert.ToInt32("0"),
            };
            id = кронштейнДверной.AddPart();
            partIds.Add(id);

            кронштейнДверной.NewName = "02-" + id;

            #endregion

            #region Сборка панели

            var idAsm = 0;
            foreach (var сборка in partIds.Select(partId => new AddingPanel
            {
                PartId = partId,

                PanelTypeName = typeOfPanel[1],
                ElementType = 1,
                Width = Convert.ToInt32(width),
                Height = Convert.ToInt32(height),

                PanelMatOut = Convert.ToInt32(materialP1[0]),
                PanelMatIn = Convert.ToInt32(materialP2[0]),
                PanelThick = 40,
                PanelMatThickOut = Convert.ToDouble(materialP1[1].Replace('.', ',')),
                PanelMatThickIn = Convert.ToDouble(materialP2[1].Replace('.', ',')),
                RalOut = покрытие[0],
                RalIn = покрытие[0],
                CoatingTypeOut = покрытие[1],
                CoatingTypeIn = покрытие[1],
                CoatingClassOut = Convert.ToInt32(покрытие[2]),
                CoatingClassIn = Convert.ToInt32(покрытие[2]),

                Mirror = config.Contains("01"),
                Step = расположениеПанелей,
                StepInsertion = расположениеВставок,
                Reinforcing = усиление,
                StickyTape = скотч.Contains("Со скотчем"),

                PanelNumber = newId
            }))
            {
                idAsm =  сборка.Add();
            }

            #endregion

            #region

            //var панельВнешняя =
            //   new AddingPanel
            //   {
            //       PanelTypeName = typeOfPanel[1],
            //       ElementType = 1,
            //       Width = Convert.ToInt32(width),
            //       Height = Convert.ToInt32(height),

            //       PartMat = Convert.ToInt32(materialP1[0]),
            //       PartThick = 40,
            //       PartMatThick = Convert.ToDouble(materialP1[1].Replace('.', ',')),
            //       Ral = покрытие[0],
            //       CoatingType = покрытие[1],
            //       CoatingClass = Convert.ToInt32(покрытие[2]),

            //       PanelMatOut = Convert.ToInt32(materialP1[0]),
            //       PanelMatIn = Convert.ToInt32(materialP2[0]),
            //       PanelThick = 40,
            //       PanelMatThickOut = Convert.ToDouble(materialP1[1].Replace('.', ',')),
            //       PanelMatThickIn = Convert.ToDouble(materialP2[1].Replace('.', ',')),
            //       RalOut = покрытие[0],
            //       RalIn = покрытие[0],
            //       CoatingTypeOut = покрытие[1],
            //       CoatingTypeIn = покрытие[1],
            //       CoatingClassOut = Convert.ToInt32(покрытие[2]),
            //       CoatingClassIn = Convert.ToInt32(покрытие[2]),

            //       Mirror = Config.Contains("01"),
            //       Step = расположениеПанелей,
            //       StepInsertion = расположениеВставок,
            //       Reinforcing = усиление,

            //       PanelNumber = newId
            //   };
            //панельВнешняя.Add();



            //var панельВнутренняя =
            //   new AddingPanel
            //   {
            //       PanelTypeName = typeOfPanel[1],
            //       ElementType = 2,
            //       Width = Convert.ToInt32(width),
            //       Height = Convert.ToInt32(height),

            //       PartMat = Convert.ToInt32(materialP2[0]),
            //       PartThick = 40,
            //       PartMatThick = Convert.ToDouble(materialP2[1].Replace('.', ',')),
            //       Ral = покрытие[3],
            //       CoatingType = покрытие[4],
            //       CoatingClass = Convert.ToInt32(покрытие[5]),

            //       PanelMatOut = Convert.ToInt32(materialP1[0]),
            //       PanelMatIn = Convert.ToInt32(materialP2[0]),
            //       PanelThick = 40,
            //       PanelMatThickOut = Convert.ToDouble(materialP1[1].Replace('.', ',')),
            //       PanelMatThickIn = Convert.ToDouble(materialP2[1].Replace('.', ',')),
            //       RalOut = покрытие[0],
            //       RalIn = покрытие[3],
            //       CoatingTypeOut = покрытие[1],
            //       CoatingTypeIn = покрытие[4],
            //       CoatingClassOut = Convert.ToInt32(покрытие[2]),
            //       CoatingClassIn = Convert.ToInt32(покрытие[5]),


            //       Mirror = Config.Contains("01"),
            //       Step = расположениеПанелей,
            //       StepInsertion = расположениеВставок,
            //       Reinforcing = усиление,


            //       PanelNumber = newId
            //   };
            //панельВнутренняя.Add();

            //var теплоизоляция =
            //  new AddingPanel
            //  {
            //      PanelTypeName = typeOfPanel[1],
            //      ElementType = 3,
            //      Width = Convert.ToInt32(width),
            //      Height = Convert.ToInt32(height),

            //      PartMat = 4900,
            //      PartThick = 40,
            //      PartMatThick = Convert.ToDouble(materialP2[1].Replace('.', ',')),
            //      Ral = null,
            //      CoatingType = null,
            //      CoatingClass = null,

            //      PanelMatOut = Convert.ToInt32(materialP1[0]),
            //      PanelMatIn = Convert.ToInt32(materialP2[0]),
            //      PanelThick = 40,
            //      PanelMatThickOut = Convert.ToDouble(materialP1[1].Replace('.', ',')),
            //      PanelMatThickIn = Convert.ToDouble(materialP2[1].Replace('.', ',')),
            //      RalOut = покрытие[0],
            //      RalIn = покрытие[3],
            //      CoatingTypeOut = покрытие[1],
            //      CoatingTypeIn = покрытие[4],
            //      CoatingClassOut = Convert.ToInt32(покрытие[2]),
            //      CoatingClassIn = Convert.ToInt32(покрытие[5]),


            //      Mirror = Config.Contains("01"),
            //      Step = расположениеПанелей,
            //      StepInsertion = расположениеВставок,
            //      Reinforcing = усиление,

            //      PanelNumber = newId
            //  };
            //теплоизоляция.Add();

            //var котч =
            //  new AddingPanel
            //  {
            //      PanelTypeName = typeOfPanel[1],
            //      ElementType = 4,
            //      Width = Convert.ToInt32(width),
            //      Height = Convert.ToInt32(height),

            //      PartMat = 14800,
            //      PartThick = 40,
            //      PartMatThick = Convert.ToDouble(materialP2[1].Replace('.', ',')),
            //      Ral = null,
            //      CoatingType = null,
            //      CoatingClass = null,

            //      PanelMatOut = Convert.ToInt32(materialP1[0]),
            //      PanelMatIn = Convert.ToInt32(materialP2[0]),
            //      PanelThick = 40,
            //      PanelMatThickOut = Convert.ToDouble(materialP1[1].Replace('.', ',')),
            //      PanelMatThickIn = Convert.ToDouble(materialP2[1].Replace('.', ',')),
            //      RalOut = покрытие[0],
            //      RalIn = покрытие[3],
            //      CoatingTypeOut = покрытие[1],
            //      CoatingTypeIn = покрытие[4],
            //      CoatingClassOut = Convert.ToInt32(покрытие[2]),
            //      CoatingClassIn = Convert.ToInt32(покрытие[5]),


            //      Mirror = Config.Contains("01"),
            //      Step = расположениеПанелей,
            //      StepInsertion = расположениеВставок,
            //      Reinforcing = усиление,

            //      PanelNumber = newId
            //  };
            //котч.Add();

            //var pes =
            //  new AddingPanel
            //  {
            //      PanelTypeName = typeOfPanel[1],
            //      ElementType = 5,
            //      Width = Convert.ToInt32(width),
            //      Height = Convert.ToInt32(height),

            //      PartMat = 6700,
            //      PartThick = 40,
            //      PartMatThick = Convert.ToDouble(materialP2[1].Replace('.', ',')),
            //      Ral = null,
            //      CoatingType = null,
            //      CoatingClass = null,

            //      PanelMatOut = Convert.ToInt32(materialP1[0]),
            //      PanelMatIn = Convert.ToInt32(materialP2[0]),
            //      PanelThick = 40,
            //      PanelMatThickOut = Convert.ToDouble(materialP1[1].Replace('.', ',')),
            //      PanelMatThickIn = Convert.ToDouble(materialP2[1].Replace('.', ',')),
            //      RalOut = покрытие[0],
            //      RalIn = покрытие[3],
            //      CoatingTypeOut = покрытие[1],
            //      CoatingTypeIn = покрытие[4],
            //      CoatingClassOut = Convert.ToInt32(покрытие[2]),
            //      CoatingClassIn = Convert.ToInt32(покрытие[5]),


            //      Mirror = Config.Contains("01"),
            //      Step = расположениеПанелей,
            //      StepInsertion = расположениеВставок,
            //      Reinforcing = усиление,

            //      PanelNumber = newId
            //  };
            //pes.Add();

            //var усиливающаяРамкаПоШирине =
            //  new AddingPanel
            //  {
            //      PanelTypeName = typeOfPanel[1],
            //      ElementType = 6,
            //      Width = Convert.ToInt32(width),
            //      Height = Convert.ToInt32(height),

            //      PartMat = 1800,
            //      PartThick = 40,
            //      PartMatThick = Convert.ToDouble("1".Replace('.', ',')),
            //      Ral = null,
            //      CoatingType = null,
            //      CoatingClass = null,

            //      PanelMatOut = Convert.ToInt32(materialP1[0]),
            //      PanelMatIn = Convert.ToInt32(materialP2[0]),
            //      PanelThick = 40,
            //      PanelMatThickOut = Convert.ToDouble(materialP1[1].Replace('.', ',')),
            //      PanelMatThickIn = Convert.ToDouble(materialP2[1].Replace('.', ',')),
            //      RalOut = покрытие[0],
            //      RalIn = покрытие[3],
            //      CoatingTypeOut = покрытие[1],
            //      CoatingTypeIn = покрытие[4],
            //      CoatingClassOut = Convert.ToInt32(покрытие[2]),
            //      CoatingClassIn = Convert.ToInt32(покрытие[5]),


            //      Mirror = Config.Contains("01"),
            //      Step = расположениеПанелей,
            //      StepInsertion = расположениеВставок,
            //      Reinforcing = усиление,

            //      PanelNumber = newId
            //  };
            //усиливающаяРамкаПоШирине.Add();

            //var усиливающаяРамкаПоВысоте =
            //   new AddingPanel
            //   {
            //       PanelTypeName = typeOfPanel[1],
            //       ElementType = 7,
            //       Width = Convert.ToInt32(width),
            //       Height = Convert.ToInt32(height),

            //       PartMat = 1800,
            //       PartThick = 40,
            //       PartMatThick = Convert.ToDouble("1".Replace('.', ',')),
            //       Ral = null,
            //       CoatingType = null,
            //       CoatingClass = null,

            //       PanelMatOut = Convert.ToInt32(materialP1[0]),
            //       PanelMatIn = Convert.ToInt32(materialP2[0]),
            //       PanelThick = 40,
            //       PanelMatThickOut = Convert.ToDouble(materialP1[1].Replace('.', ',')),
            //       PanelMatThickIn = Convert.ToDouble(materialP2[1].Replace('.', ',')),
            //       RalOut = покрытие[0],
            //       RalIn = покрытие[3],
            //       CoatingTypeOut = покрытие[1],
            //       CoatingTypeIn = покрытие[4],
            //       CoatingClassOut = Convert.ToInt32(покрытие[2]),
            //       CoatingClassIn = Convert.ToInt32(покрытие[5]),


            //       Mirror = Config.Contains("01"),
            //       Step = расположениеПанелей,
            //       StepInsertion = расположениеВставок,
            //       Reinforcing = усиление,


            //       PanelNumber = newId
            //   };
            //усиливающаяРамкаПоВысоте.Add();

            //return "";

            ////AddingPanels.Add(панельВнешняя);
            ////AddingPanels.Add(панельВнутренняя);
            ////AddingPanels.Add(теплоизоляция);
            ////AddingPanels.Add(cкотч);
            ////AddingPanels.Add(усиливающаяРамкаПоШирине);
            ////AddingPanels.Add(усиливающаяРамкаПоВысоте);

            ////partIdsInAsm.Add(панельВнешняя.PartId);
            ////partIdsInAsm.Add(панельВнутренняя.PartId);
            ////partIdsInAsm.Add(теплоизоляция.PartId);
            ////partIdsInAsm.Add(cкотч.PartId);
            ////partIdsInAsm.Add(усиливающаяРамкаПоШирине.PartId);
            ////partIdsInAsm.Add(усиливающаяРамкаПоВысоте.PartId);

            ////var asm = new AddingPanel
            ////   {
            ////       PartInAsmIds = partIdsInAsm
            ////   };
            //////asm.AddAsm();
            //////MessageBox.Show(asm.NameById);
            //////MessageBox.Show(partIdsInAsm.Aggregate("", (current, panel) => current + panel + "\n"));

#endregion
            
            var обозначениеНовойПанели = "02-" + typeOfPanel[0] + "-" + idAsm;

            //MessageBox.Show(string.Format("{0}\n{1}\n{2}\n{3}\n",
            //    панельВнешняя.NewName,
            //    панельВнутренняя.NewName,
            //    усиливающаяРамкаПоВысоте.NewName,
            //    усиливающаяРамкаПоШирине.NewName),
            //    обозначениеНовойПанели + "  " + скотч.Contains("Со скотчем"));
            //return null;
            
            #region

            //var обозначениеНовойПанели = String.Format(
            //    "{0}-{1}-{2}-40-{3}-{4}-MW{5}{6}",
            //    modelName,
            //    width,
            //    height,
            //    materialP1[0],
            //    materialP2[0],
            //    скотч == "Со скотчем" ? "-ST" : "",
            //    panelsUpDownConfigString);

            #endregion
            
            #endregion

            #endregion

            switch (typeOfPanel[0])
            {
                case "04":
                    DestinationFolder = Panels0204;
                    break;
                case "05":
                    DestinationFolder = Panels0205;
                    break;
                default:
                    DestinationFolder = Panels0201;
                    break;
            }
            
            var newFramelessPanelPath = String.Format(@"{0}{1}\{2}.SLDASM",
                Settings.Default.DestinationFolder,
                DestinationFolder,
                обозначениеНовойПанели);

            if (!InitializeSw(true)) return "-";

            if (File.Exists(newFramelessPanelPath))
            {
                GetLastVersionPdm(new FileInfo(newFramelessPanelPath).FullName, Settings.Default.TestPdmBaseName);
                _swApp.OpenDoc6(newFramelessPanelPath, (int)swDocumentTypes_e.swDocASSEMBLY,
                    (int)swOpenDocOptions_e.swOpenDocOptions_LoadModel, "00", 0, 0);
                return newFramelessPanelPath;
            }

            const string modelPanelsPath = FrameLessFolder;
            var sourceFolder = Settings.Default.SourceFolder;
            const string nameAsm = "02-11-40-1";
            var modelPanelAsmbly = String.Format(@"{0}{1}\{2}.SLDASM",
                sourceFolder,
                modelPanelsPath,
                nameAsm);

            #endregion

            #region Получение последней версии и открытие сборки

            GetLastVersionPdm(
                new[]{modelPanelAsmbly,
                    String.Format(@"{0}{1}\{2}", sourceFolder, modelPanelsPath,"02-11-01-40-.SLDPRT"),
                    String.Format(@"{0}{1}\{2}", sourceFolder, modelPanelsPath,"02-11-02-40-.SLDPRT"),
                    String.Format(@"{0}{1}\{2}", sourceFolder, modelPanelsPath,"02-11-03-40-.SLDPRT"),
                    String.Format(@"{0}{1}\{2}", sourceFolder, modelPanelsPath,"02-11-04-40-.SLDPRT"),
                    String.Format(@"{0}{1}\{2}", sourceFolder, modelPanelsPath,"02-11-05-40-.SLDPRT"),
                    String.Format(@"{0}{1}{2}", sourceFolder, @"\Библиотека проектирования\Прочие изделия\Крепежные изделия\", "Заглушка пластикова для т.о. 16х13 сір..SLDPRT"),
                    String.Format(@"{0}{1}{2}", sourceFolder, @"\Библиотека проектирования\Стандартные изделия\", "Винт саморез DIN 7504 K.SLDPRT"),
                    String.Format(@"{0}{1}{2}", sourceFolder, @"\Библиотека проектирования\Стандартные изделия\", "Rivet Bralo.sldprt")},
                Settings.Default.PdmBaseName);

            var swDoc = _swApp.OpenDoc6(modelPanelAsmbly, (int)swDocumentTypes_e.swDocASSEMBLY,
               (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", 0, 0);
            _swApp.Visible = true;

            var swAsm = (AssemblyDoc)swDoc;
            swAsm.ResolveAllLightWeightComponents(false);

            #endregion

            #region Расчетные данные, переменные и константы

            #region Габариты

            var ширинаПанели = Convert.ToDouble(width);
            var высотаПанели = Convert.ToDouble(height);


            double количествоВинтов = 2000;

            switch (typeOfPanel[0])
            {
                case "04":
                case "05":
                    double расстояниеL;
                    СъемнаяПанель(ширинаПанели, высотаПанели, out ширинаПанели, out высотаПанели, out расстояниеL, out количествоВинтов);
                    break;
            }

            #endregion

            #region Расчет шага для саморезов и заклепок

            double шагСаморезВинтШирина = 200;
            var шагСаморезВинтВысота = шагСаморезВинтШирина;

            if (ширинаПанели < 600)
            {
                шагСаморезВинтШирина = 150;
            }
            if (высотаПанели < 600)
            {
                шагСаморезВинтВысота = 150;
            }

            var колСаморезВинтШирина = (Math.Truncate(ширинаПанели / шагСаморезВинтШирина) + 1) * 1000;
            var колСаморезВинтВысота = (Math.Truncate(высотаПанели / шагСаморезВинтВысота) + 1) * 1000;
            var колСаморезВинтШирина2 = колСаморезВинтШирина;

            switch (typeOfPanel[0])
            {
                case "04":
                case "05":
                    колСаморезВинтШирина = количествоВинтов;
                    break;
            }

            //// Шаг для саморезов
            //const double stepscrewV = 160;
            //var screwWv = (Math.Truncate(widthD / stepscrewV) + 1) * 1000;
            //var screwHv = (Math.Truncate(heightD / stepscrewV) + 1) * 1000;

            // Шаг заклепок
            double колЗаклепокВысота;

            switch (typeOfPanel[0])
            {
                case "04":
                case "05":
                    const double шагЗаклепокВысота = 125;
                    колЗаклепокВысота = (Math.Truncate(высотаПанели / шагЗаклепокВысота) + 1) * 1000;
                    break;
                default:
                    колЗаклепокВысота = колСаморезВинтВысота + 1000;
                    if (Convert.ToInt32(height) > 1000)
                    {
                        колЗаклепокВысота = колСаморезВинтВысота + 3000;
                    }
                    break;
            }

            var колЗаклепокШирина = колСаморезВинтШирина + 1000;
            //var колЗаклепокШирина = (Math.Truncate(ширинаПанели / шагЗаклепокШирина) + 1) * 1000;

            #endregion

            #region Оступы для отверстий заклепок, саморезов и винтов

            var отступОтветныхОтверстийШирина = 8;
            var осьСаморезВинт = 9.0;
            var осьОтверстийСаморезВинт = typeOfPanel[0] == "04" || typeOfPanel[0] == "05" ? 12.0 : 11.0; // 7.5;// 5.3;
            var осьПоперечныеОтверстия = 10.1;//8.9;

            if (скотч == "Со скотчем")
            {
                осьПоперечныеОтверстия =  10.1;// 10.4;
            }

            switch (typeOfPanel[0])
            {
                case "04":
                case "05": break;

                default:
                    отступОтветныхОтверстийШирина = 47;
                    осьСаморезВинт = 9.70;
                    осьОтверстийСаморезВинт = 10.3; //8.5;//  7.3;
                    break;
            }

            // Поправка на толщину
            //if (typeOfPanel != "04" && typeOfPanel != "05")
            //{
            //    отступОтветныхОтверстийШирина = 45;
            //    осьСаморезВинт = 10;
            //    осьОтверстийСаморезВинт = 7.3;
            //}

            #endregion

            #region Коэффициенты и радиусы гибов

            var sbSqlBaseData = new SqlBaseData();
            string[] bendParams;
            double bendRadius;
            double kFactor;

            #endregion

            #region Диаметры отверстий

            var диамЗаглушкаВинт = 13.1;
            var диамСаморезВинт = 3.3;

            switch (typeOfPanel[0])
            {
                case "04":
                case "05":
                    диамЗаглушкаВинт = 11;
                    диамСаморезВинт = 7;
                    break;
            }

            #endregion

            #region Расчет расстояния межу ручками в съемной панели

            var растояниеМеждуРучками = ширинаПанели / 4;
            if (ширинаПанели < 1000)
            {
                растояниеМеждуРучками = ширинаПанели * 0.5 * 0.5;
            }
            if (ширинаПанели >= 1000)
            {
                растояниеМеждуРучками = ширинаПанели * 0.45 * 0.5;
            }
            if (ширинаПанели >= 1300)
            {
                растояниеМеждуРучками = ширинаПанели * 0.4 * 0.5;
            }
            if (ширинаПанели >= 1700)
            {
                растояниеМеждуРучками = ширинаПанели * 0.35 * 0.5;
            }

            #endregion

            #endregion

            #region Типы панелей - Удаление ненужного

            const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed +
                                                (int)swDeleteSelectionOptions_e.swDelete_Children;
            var swDocExt = swDoc.Extension;

            //if (ValProfils.PsTy1 == "-")
            //{
            //    if (config.Contains("01"))
            //    {

            //    }
            //    if (config.Contains("02"))
            //    {

            //    }
            //}
            //else
            //{

            //    //MessageBox.Show("Удаление ненужного");

            //    swDocExt.SelectByID2("Эскиз60@02-11-01-40--1@" + nameAsm, "SKETCH", 0, 0, 0, true, 0, null, 0); swDoc.EditSuppress2();
            //    swDocExt.SelectByID2("Эскиз61@02-11-01-40--1@" + nameAsm, "SKETCH", 0, 0, 0, true, 0, null, 0); swDoc.EditSuppress2();
            //    swDocExt.SelectByID2("Эскиз91@02-11-02-40--1@" + nameAsm, "SKETCH", 0, 0, 0, true, 0, null, 0); swDoc.EditSuppress2();

            //    swDocExt.SelectByID2("Эскиз64@02-11-01-40--1@" + nameAsm, "SKETCH", 0, 0, 0, true, 0, null, 0); swDoc.EditSuppress2();
            //    swDocExt.SelectByID2("Эскиз65@02-11-01-40--1@" + nameAsm, "SKETCH", 0, 0, 0, true, 0, null, 0); swDoc.EditSuppress2();
            //    swDocExt.SelectByID2("Эскиз93@02-11-02-40--1@" + nameAsm, "SKETCH", 0, 0, 0, true, 0, null, 0); swDoc.EditSuppress2();

            //    #region
            //    //swDocExt.SelectByID2("U31@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
            //    //swDoc.EditDelete();
            //    //swDocExt.SelectByID2("U32@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
            //    //swDoc.EditDelete();
            //    //swDocExt.SelectByID2("U33@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
            //    //swDoc.EditDelete();
            //    //swDocExt.SelectByID2("U51@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
            //    //swDoc.EditDelete();
            //    //swDocExt.SelectByID2("U52@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
            //    //swDoc.EditDelete();
            //    //swDocExt.SelectByID2("U53@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
            //    //swDoc.EditDelete(); 
            //    #endregion

            //}

            //if (ValProfils.PsTy2 != "-")
            //{
            //    if (config.Contains("01"))
            //    {

            //    }
            //    if (config.Contains("02"))
            //    {

            //    }
            //}
            //else
            //{

            //    //MessageBox.Show("Удаление ненужного 2");

            //    swDocExt.SelectByID2("Эскиз62@02-11-01-40--1@" + nameAsm, "SKETCH", 0, 0, 0, true, 0, null, 0); swDoc.EditSuppress2();
            //    swDocExt.SelectByID2("Эскиз63@02-11-01-40--1@" + nameAsm, "SKETCH", 0, 0, 0, true, 0, null, 0); swDoc.EditSuppress2();
            //    swDocExt.SelectByID2("Эскиз77@02-11-02-40--1@" + nameAsm, "SKETCH", 0, 0, 0, true, 0, null, 0); swDoc.EditSuppress2();

            //    swDocExt.SelectByID2("Эскиз66@02-11-01-40--1@" + nameAsm, "SKETCH", 0, 0, 0, true, 0, null, 0); swDoc.EditSuppress2();
            //    swDocExt.SelectByID2("Эскиз67@02-11-01-40--1@" + nameAsm, "SKETCH", 0, 0, 0, true, 0, null, 0); swDoc.EditSuppress2();
            //    swDocExt.SelectByID2("Эскиз94@02-11-02-40--1@" + nameAsm, "SKETCH", 0, 0, 0, true, 0, null, 0); swDoc.EditSuppress2();

            //    #region
            //    //swDocExt.SelectByID2("U41@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
            //    //swDoc.EditDelete();
            //    //swDocExt.SelectByID2("U42@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
            //    //swDoc.EditDelete();
            //    //swDocExt.SelectByID2("U43@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
            //    //swDoc.EditDelete();
            //    //swDocExt.SelectByID2("U61@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
            //    //swDoc.EditDelete();
            //    //swDocExt.SelectByID2("U62@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
            //    //swDoc.EditDelete();
            //    //swDocExt.SelectByID2("U63@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
            //    //swDoc.EditDelete(); 
            //    #endregion

            //}

            switch (typeOfPanel[0])
            {
                #region 04 05 - Съемные панели

                case "04":
                case "05":

                    if (Convert.ToInt32(width) > 750)
                    {
                        swDocExt.SelectByID2("Handel-1", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditDelete();
                        swDocExt.SelectByID2("Threaded Rivets-25@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDocExt.SelectByID2("Ручка MLA 120-3@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDocExt.SelectByID2("SC GOST 17475_gost-5@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDocExt.SelectByID2("SC GOST 17475_gost-6@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDocExt.SelectByID2("Threaded Rivets-26@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDocExt.SelectByID2("Threaded Rivets-26@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditDelete();

                        swDoc.Extension.SelectByID2("Вырез-Вытянуть14@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.Extension.DeleteSelection2(deleteOption);
                    }
                    else
                    {
                        swDocExt.SelectByID2("Handel-2", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditDelete();
                        swDocExt.SelectByID2("Threaded Rivets-21@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDocExt.SelectByID2("Threaded Rivets-22@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDocExt.SelectByID2("Ручка MLA 120-1@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDocExt.SelectByID2("SC GOST 17475_gost-1@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDocExt.SelectByID2("SC GOST 17475_gost-2@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDoc.EditDelete();

                        swDocExt.SelectByID2("Вырез-Вытянуть15@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDocExt.DeleteSelection2(deleteOption);
                    }

                    swDocExt.SelectByID2("Threaded Rivets-60@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                    swDocExt.SelectByID2("Threaded Rivets-61@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();

                    swDocExt.SelectByID2("1-1@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("1-2@02-11-01-40--1@02-11-40-1", "SKETCH", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("1-3@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("1-4@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("1-4@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();

                    swDocExt.SelectByID2("3-1@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("3-2@02-11-01-40--1@02-11-40-1", "SKETCH", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("3-3@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("3-4@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("3-4@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();

                    swDocExt.SelectByID2("1-1-1@02-11-01-40--1@02-11-40-1", "SKETCH", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("3-1-1@02-11-01-40--1@02-11-40-1", "SKETCH", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditDelete();

                    swDocExt.SelectByID2("Hole1", "FTRFOLDER", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDocExt.SelectByID2("Hole2", "FTRFOLDER", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    
                    swDoc.Extension.SelectByID2("D1@2-2@02-11-01-40--1@02-11-40-1", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    ((Dimension)(swDoc.Parameter("D1@2-2@02-11-01-40-.Part"))).SystemValue = 0.065;
                    swDoc.EditRebuild3();

                    swDocExt.SelectByID2("D1@1-2@02-11-02-40--1@02-11-40-1", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    ((Dimension)(swDoc.Parameter("D1@1-2@02-11-02-40-.Part"))).SystemValue = typeOfPanel[0] == "04" || typeOfPanel[0] == "05" ? 0.044 : 0.045;
                    swDoc.EditRebuild3();

                    swDocExt.SelectByID2("Hole2", "FTRFOLDER", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDocExt.SelectByID2("Вырез-Вытянуть1@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("Эскиз27@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("Кривая1@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("Кривая1@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDocExt.SelectByID2("Эскиз26@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();

                    swDocExt.SelectByID2("Hole3", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDocExt.SelectByID2("Вырез-Вытянуть3@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("Эскиз31@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("Кривая3@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("Кривая3@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDocExt.SelectByID2("Эскиз30@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    swDocExt.SelectByID2("Эскиз30@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();

                    // Удаление торцевых отверстий под саморезы
                    swDocExt.SelectByID2("Вырез-Вытянуть6@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    swDocExt.DeleteSelection2(deleteOption);
                    swDocExt.SelectByID2("Вырез-Вытянуть7@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    swDocExt.DeleteSelection2(deleteOption);

                    // Удаление торцевых отверстий под клепальные гайки
                    swDoc.Extension.SelectByID2("Под клепальные гайки", "FTRFOLDER", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Эскиз49@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Панель1", "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Панель2", "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Панель3", "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Панель3", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();

                    // Удаление отверстий под монтажную рамку
                    swDocExt.SelectByID2("Вырез-Вытянуть9@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    swDocExt.DeleteSelection2(deleteOption);
                    swDocExt.SelectByID2("Вырез-Вытянуть10@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    swDocExt.DeleteSelection2(deleteOption);

                    swDocExt.SelectByID2("Вырез-Вытянуть12@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    swDocExt.DeleteSelection2(deleteOption);
                    swDocExt.SelectByID2("Вырез-Вытянуть4@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    swDocExt.DeleteSelection2(deleteOption);
                    swDocExt.SelectByID2("Вырез-Вытянуть5@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    swDocExt.DeleteSelection2(deleteOption);
                    swDocExt.SelectByID2("Вырез-Вытянуть6@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    swDocExt.DeleteSelection2(deleteOption);

                    // Одна ручка
                    if (Convert.ToInt32(height) < 825)
                    {
                        swDocExt.SelectByID2("SC GOST 17473_gost-12@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                        swDocExt.SelectByID2("SC GOST 17473_gost-13@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                        swDoc.Extension.SelectByID2("Вырез-Вытянуть19@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.Extension.DeleteSelection2(deleteOption);
                        swDoc.Extension.SelectByID2("Вырез-Вытянуть11@02-11-02-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.Extension.DeleteSelection2(deleteOption);
                    }
                    break;

                #endregion

                #region Удаление элементов съемной панели

                case "01":
                case "21":
                case "22":
                case "23":
                case "30":
                case "31":
                case "32":
                    swDocExt.SelectByID2("Handel-1", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDocExt.SelectByID2("Threaded Rivets-25@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("Ручка MLA 120-3@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("SC GOST 17475_gost-5@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("SC GOST 17475_gost-6@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("Threaded Rivets-26@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("Threaded Rivets-26@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDocExt.SelectByID2("Handel-2", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDocExt.SelectByID2("Threaded Rivets-21@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("Threaded Rivets-22@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("Ручка MLA 120-1@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("SC GOST 17475_gost-1@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocExt.SelectByID2("SC GOST 17475_gost-2@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditDelete();
                    swDocExt.SelectByID2("Вырез-Вытянуть14@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDocExt.DeleteSelection2(deleteOption);
                    swDocExt.SelectByID2("Вырез-Вытянуть15@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDocExt.DeleteSelection2(deleteOption);
                    
                    swDocExt.SelectByID2("SC GOST 17473_gost-12@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditDelete();
                    swDocExt.SelectByID2("SC GOST 17473_gost-13@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Вырез-Вытянуть19@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.Extension.DeleteSelection2(deleteOption);
                    swDoc.Extension.SelectByID2("Вырез-Вытянуть11@02-11-02-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.Extension.DeleteSelection2(deleteOption);

                    if (typeOfPanel[0] != "01")
                    {

//                        MessageBox.Show("Start");

                        //#region
                        
                        //swDoc.Extension.SelectByID2("Эскиз59@02-11-01-40--1@02-11-40-1", "SKETCH", 0, 0, 0, true, 0, null, 0);
                        //swDoc.EditUnsuppress2();
                        //swDocExt.SelectByID2("U32@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        //swDoc.EditUnsuppress2();
                        //swDocExt.SelectByID2("U31@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        //swDoc.EditUnsuppress2();


                        //swDocExt.SelectByID2("U52@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        //swDoc.EditUnsuppress2();
                        //swDocExt.SelectByID2("U51@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        //swDoc.EditUnsuppress2();

                        //swDocExt.SelectByID2("U33@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        //swDoc.EditUnsuppress2();
                        //swDocExt.SelectByID2("U53@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        //swDoc.EditUnsuppress2();

                        //swDocExt.SelectByID2("U42@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        //swDoc.EditUnsuppress2();
                        //swDocExt.SelectByID2("U41@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        //swDoc.EditUnsuppress2();

                        //swDocExt.SelectByID2("U62@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        //swDoc.EditUnsuppress2();
                        //swDocExt.SelectByID2("U61@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        //swDoc.EditUnsuppress2();

                        //swDocExt.SelectByID2("U43@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        //swDoc.EditUnsuppress2();
                        //swDocExt.SelectByID2("U63@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        //swDoc.EditUnsuppress2();

                        //}

                        //if (ValProfils.PsTy2 != "-")
                        //{
                        //    if (config.Contains("01"))
                        //    {

                        //    }
                        //    if (config.Contains("02"))
                        //    {

                        //    }
                        //}
                        //else
                        //{
                        
//                      MessageBox.Show("End");

                        #endregion

                    }

                    if (typeOfPanel[0] == "01")
                    {
                        swDocExt.SelectByID2("D1@1-2@02-11-02-40--1@02-11-40-1", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                        ((Dimension)(swDoc.Parameter("D1@1-2@02-11-02-40-.Part"))).SystemValue = 0.047;
                        swDoc.EditRebuild3();

                        swDocExt.SelectByID2("D1@2-2@02-11-01-40--1@02-11-40-1", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                        ((Dimension)(swDoc.Parameter("D1@2-2@02-11-01-40-.Part"))).SystemValue = 0.067;
                        swDoc.EditRebuild3();

                        //ToDo 30mm

                        if (ValProfils.PsTy1 != "-")
                        {

                            swDocExt.SelectByID2("D1@2-2@02-11-01-40--1@02-11-40-1", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                            ((Dimension)(swDoc.Parameter("D1@2-2@02-11-01-40-.Part"))).SystemValue = 0.03;
                            swDoc.EditRebuild3();

                            swDocExt.SelectByID2("D1@1-2@02-11-02-40--1@02-11-40-1", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                            ((Dimension)(swDoc.Parameter("D1@1-2@02-11-02-40-.Part"))).SystemValue = 0.01;
                            swDoc.EditRebuild3();

                        }
                    }
                
                //MessageBox.Show("End 2");

                    break;

                #endregion
            }

            if (typeOfPanel[0] != "01")
            {
                swDocExt.SelectByID2("Вырез-Вытянуть18@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDocExt.DeleteSelection2(deleteOption);
            }

            if (typeOfPanel[0] == "01")
            {
                // Удаление торцевых отверстий под клепальные гайки

                swDoc.Extension.SelectByID2("Под клепальные гайки", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("Эскиз49@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Панель1", "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Панель2", "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Панель3", "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Панель3", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
            }

            if (typeOfPanel[0] != "32")
            {
                swDocExt.SelectByID2("Threaded Rivets-5@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDocExt.SelectByID2("Threaded Rivets-6@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDocExt.SelectByID2("Threaded Rivets-7@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDocExt.SelectByID2("Threaded Rivets-8@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDocExt.SelectByID2("Threaded Rivets-9@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDocExt.SelectByID2("Threaded Rivets-10@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDocExt.SelectByID2("Threaded Rivets-11@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDocExt.SelectByID2("Threaded Rivets-12@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDocExt.SelectByID2("Threaded Rivets-13@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDocExt.SelectByID2("Threaded Rivets-14@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDocExt.SelectByID2("Threaded Rivets-15@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDocExt.SelectByID2("Threaded Rivets-16@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDocExt.SelectByID2("Threaded Rivets-17@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDocExt.SelectByID2("Threaded Rivets-18@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDocExt.SelectByID2("Threaded Rivets-19@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDocExt.SelectByID2("Threaded Rivets-20@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDocExt.SelectByID2("Threaded Rivets-20@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDocExt.SelectByID2("Ножка", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDocExt.SelectByID2("Вырез-Вытянуть11@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                swDocExt.DeleteSelection2(deleteOption);
                swDocExt.SelectByID2("Зеркальное отражение2@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
            }

            if (typeOfPanel[0] != "22" && typeOfPanel[0] != "31")
            {
                swDocExt.SelectByID2("Threaded Rivets-33@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDocExt.SelectByID2("Threaded Rivets-34@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDocExt.SelectByID2("Threaded Rivets-35@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDocExt.SelectByID2("Threaded Rivets-36@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.EditDelete();
                swDocExt.SelectByID2("Up", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDocExt.SelectByID2("Вырез-Вытянуть16@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                swDocExt.DeleteSelection2(deleteOption);
            }

            if (typeOfPanel[0] != "04" && typeOfPanel[0] != "05")
            {
                swDocExt.SelectByID2("SC GOST 17473_gost-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("SC GOST 17473_gost-2@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();

                swDocExt.SelectByID2("Threaded Rivets-60@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Threaded Rivets-61@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Threaded Rivets-59@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Threaded Rivets-62@" + nameAsm, "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
            }

            if (config.Contains("00"))
            {
                swDocExt.SelectByID2("Threaded Rivets-37@02-11-40-1", "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Threaded Rivets-38@02-11-40-1", "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();

                swDocExt.SelectByID2("Threaded Rivets-47@02-11-40-1", "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Threaded Rivets-52@02-11-40-1", "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Threaded Rivets-48@02-11-40-1", "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Threaded Rivets-51@02-11-40-1", "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
            }

            if (config.Contains("01"))
            {
                swDocExt.SelectByID2("Threaded Rivets-38@02-11-40-1", "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Threaded Rivets-48@02-11-40-1", "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Threaded Rivets-51@02-11-40-1", "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();

                swDocExt.SelectByID2("4-1@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                swDoc.EditSuppress2(); swDoc.EditRebuild3();
                swDocExt.SelectByID2("1-0@02-11-02-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                swDoc.EditSuppress2(); swDoc.EditRebuild3();
                
                // Погашение отверстий под клепальные гайки
                swDoc.Extension.SelectByID2("Вырез-Вытянуть20@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.Extension.SelectByID2("Вырез-Вытянуть18@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Вырез-Вытянуть16@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                swDoc.EditSuppress2();

                swDoc.Extension.SelectByID2("Вырез-Вытянуть7@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress2();
            }

            if (config.Contains("02"))
            {
                swDocExt.SelectByID2("Threaded Rivets-37@02-11-40-1", "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Threaded Rivets-47@02-11-40-1", "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Threaded Rivets-52@02-11-40-1", "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();

                swDocExt.SelectByID2("2-1@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                swDoc.EditSuppress2(); swDoc.EditRebuild3();
                swDocExt.SelectByID2("1-1@02-11-02-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                swDoc.EditSuppress2(); swDoc.EditRebuild3();

                // Погашение отверстий под клепальные гайки
                swDoc.Extension.SelectByID2("Вырез-Вытянуть19@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.Extension.SelectByID2("Вырез-Вытянуть17@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Вырез-Вытянуть15@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                swDoc.EditSuppress2();

                swDoc.Extension.SelectByID2("Вырез-Вытянуть6@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress2();
            }

            #region Удаление усиления для типоразмера меньше AV09

            if (!усиление)
            {
                foreach (var component in new[]
                {
                    "02-11-06-40--1", "02-11-06-40--4", "02-11-07-40--1", "02-11-07-40--2",
                    "Rivet Bralo-37", "Rivet Bralo-38", "Rivet Bralo-39", "Rivet Bralo-40",
                    "Rivet Bralo-41", "Rivet Bralo-42", "Rivet Bralo-43", "Rivet Bralo-44",
                    "Rivet Bralo-45", "Rivet Bralo-46", "Rivet Bralo-47", "Rivet Bralo-48",
                    "Rivet Bralo-49", "Rivet Bralo-50", "Rivet Bralo-51", "Rivet Bralo-52",
                    "Rivet Bralo-52"
                })
                {
                    swDocExt.SelectByID2(component + "@02-11-40-1", "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                }

                swDoc.ShowConfiguration2("Вытяжная заклепка 3,0х6 (ст ст. с пл. гол.)");
                swDocExt.SelectByID2("Усиление", "FTRFOLDER", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();

                swDocExt.SelectByID2("Hole7@02-11-02-40--1@" + nameAsm, "FTRFOLDER", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Эскиз45@02-11-02-40--1@" + nameAsm, "SKETCH", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Вырез-Вытянуть13@02-11-02-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Кривая7@02-11-02-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Hole8@02-11-02-40--1@" + nameAsm, "FTRFOLDER", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Эскиз47@02-11-02-40--1@" + nameAsm, "SKETCH", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Вырез-Вытянуть14@02-11-02-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Кривая9@02-11-02-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();

                swDocExt.SelectByID2("Rivet Bralo-53@02-11-40-1", "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Rivet Bralo-54@02-11-40-1", "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Rivet Bralo-55@02-11-40-1", "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Rivet Bralo-56@02-11-40-1", "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                swDoc.ClearSelection2(true);
            }

            #endregion

            #region Отверстия под панели L2 L3

            if (Convert.ToInt32(OutValPanels.L2) == 28)
            {
                swDocExt.SelectByID2("Threaded Rivets-47@02-11-40-1", "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Threaded Rivets-48@02-11-40-1", "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();

                swDoc.Extension.SelectByID2("Вырез-Вытянуть17@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Вырез-Вытянуть18@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Кривая11@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Кривая11@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();

                swDoc.ClearSelection2(true);
            }

            if (Convert.ToInt32(OutValPanels.L3) == 28)
            {
                swDocExt.SelectByID2("Threaded Rivets-51@02-11-40-1", "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("Threaded Rivets-52@02-11-40-1", "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                swDoc.Extension.SelectByID2("Панель3", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();

                swDoc.Extension.SelectByID2("Вырез-Вытянуть19@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Вырез-Вытянуть20@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Кривая12@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Кривая12@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();

                swDoc.ClearSelection2(true);
            }

            #endregion

            #region Панели усиливающие

            string типКрепежнойЧастиУсиливающейПанели = null;
            var типТорцевойЧастиУсиливающейПанели = "T";

            try
            {
                типТорцевойЧастиУсиливающейПанели = типУсиливающей.Remove(1).Contains("T") ? "T" : "E";

                if (типУсиливающей.Remove(0, 1).Contains("E"))
                {
                    типКрепежнойЧастиУсиливающейПанели = "E";
                }
                if (типУсиливающей.Remove(0, 1).Contains("D"))
                {
                    типКрепежнойЧастиУсиливающейПанели = "D";
                }
                if (типУсиливающей.Remove(0, 1).Contains("E"))
                {
                    типКрепежнойЧастиУсиливающейПанели = "E";
                }
                if (типУсиливающей.Remove(0, 1).Contains("Z"))
                {
                    типКрепежнойЧастиУсиливающейПанели = "Z";
                } 
            }
            catch (Exception exception)
            {
              //  MessageBox.Show(exception.ToString());
            }
            

            if (Convert.ToInt32(height) < 825)
            {
                swDoc.Extension.SelectByID2("UpperAV09@02-11-09-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0); swDoc.EditSuppress();
            }

            if (типТорцевойЧастиУсиливающейПанели == "E")
            {
                //MessageBox.Show("Удаление саморезов торцевых ");

                swDocExt.SelectByID2("1-1@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("1-3@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("1-1-1@02-11-01-40--1@02-11-40-1", "SKETCH", 0, 0, 0, true, 0, null, 0);swDoc.EditDelete();

                swDocExt.SelectByID2("Эскиз42@02-11-02-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0); swDoc.EditSuppress();
                swDocExt.SelectByID2("Hole1@02-11-01-40--1@" + nameAsm, "FTRFOLDER", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
            }
            if (типКрепежнойЧастиУсиливающейПанели != "D")
            {
                foreach (var component in new[]
                {
                    "02-11-09-40--1", 
                    "Threaded Rivets с насечкой-1", "Threaded Rivets с насечкой-2",
                    "Threaded Rivets с насечкой-3", "Threaded Rivets с насечкой-4"
                })
                {
                    swDocExt.SelectByID2(component + "@02-11-40-1", "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                }
                swDocExt.SelectByID2("Кронштейн", "FTRFOLDER", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("U10@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0); swDoc.EditSuppress();
            }

            if (типКрепежнойЧастиУсиливающейПанели != "Z")
            {
                swDocExt.SelectByID2("U20@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0); swDoc.EditSuppress();

                foreach (var component in new[]
                {
                    "Threaded Rivets с насечкой-5", "Threaded Rivets с насечкой-6"
                })
                {
                    swDocExt.SelectByID2(component + "@02-11-40-1", "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                }
            }

            //else
            //{
            //   // MessageBox.Show("Удаление саморезов крепежных 1", типКрепежнойЧастиУсиливающейПанели);

            //    swDocExt.SelectByID2("3-2@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
            //    swDocExt.SelectByID2("3-1@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
            //    swDocExt.SelectByID2("3-1-1@02-11-01-40--1@02-11-40-1", "SKETCH", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();

            //    swDocExt.SelectByID2("Эскиз41@02-11-02-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0); swDoc.EditSuppress();
            //    swDocExt.SelectByID2("Hole3@02-11-01-40--1@" + nameAsm, "FTRFOLDER", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
            //}

            if (типКрепежнойЧастиУсиливающейПанели == "Z" || типКрепежнойЧастиУсиливающейПанели == "D" || типКрепежнойЧастиУсиливающейПанели == "E")
            {
                //MessageBox.Show("Удаление саморезов крепежных 2", типКрепежнойЧастиУсиливающейПанели);

                //swDocExt.SelectByID2("1-1@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                //swDocExt.SelectByID2("1-3@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                //swDocExt.SelectByID2("1-1-1@02-11-01-40--1@02-11-40-1", "SKETCH", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();

                //swDocExt.SelectByID2("Эскиз42@02-11-02-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0); swDoc.EditSuppress();
                //swDocExt.SelectByID2("Hole1@02-11-01-40--1@" + nameAsm, "FTRFOLDER", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                
                swDocExt.SelectByID2("3-2@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("3-1@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                swDocExt.SelectByID2("3-1-1@02-11-01-40--1@02-11-40-1", "SKETCH", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();

                swDocExt.SelectByID2("Эскиз41@02-11-02-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0); swDoc.EditSuppress();
                swDocExt.SelectByID2("Hole3@02-11-01-40--1@" + nameAsm, "FTRFOLDER", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();

                swDocExt.SelectByID2("Эскиз56@02-11-02-40--1@" + nameAsm, "SKETCH", 0, 0, 0, true, 0, null, 0); swDoc.EditSuppress();
                //swDocExt.SelectByID2("Hole6@02-11-01-40--1@" + nameAsm, "FTRFOLDER", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
            }

            #endregion

            #region Вставки внутренние

            if (ValProfils.Tp1 == "01" || ValProfils.Tp1 == "00")
            {
                if (config.Contains("01"))
                {
                    swDoc.Extension.SelectByID2("Вырез-ВытянутьTp1R@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                    swDoc.Extension.SelectByID2("Эскиз80@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                }
                if (config.Contains("02"))
                {
                    swDoc.Extension.SelectByID2("Вырез-ВытянутьTp1L@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                    swDoc.Extension.SelectByID2("Эскиз81@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                }
            }
            else
            {
                swDoc.Extension.SelectByID2("Вырез-ВытянутьTp1R@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
                swDoc.Extension.SelectByID2("Вырез-ВытянутьTp1L@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
            }

            if (ValProfils.Tp2 == "01" || ValProfils.Tp2 == "00")
            {
                if (config.Contains("01"))
                {
                    swDoc.Extension.SelectByID2("Вырез-ВытянутьTp2R@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                    swDoc.Extension.SelectByID2("Эскиз61@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                }
                if (config.Contains("02"))
                {
                    swDoc.Extension.SelectByID2("Вырез-ВытянутьTp2L@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                    swDoc.Extension.SelectByID2("Эскиз62@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                }
            }
            else
            {
                swDoc.Extension.SelectByID2("Вырез-ВытянутьTp2R@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
                swDoc.Extension.SelectByID2("Вырез-ВытянутьTp2L@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
            }

            if (ValProfils.Tp3 == "01" || ValProfils.Tp3 == "00")
            {
                if (config.Contains("01"))
                {
                    swDoc.Extension.SelectByID2("Вырез-ВытянутьTp3R@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                    swDoc.Extension.SelectByID2("Эскиз63@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                }
                if (config.Contains("02"))
                {
                    swDoc.Extension.SelectByID2("Вырез-ВытянутьTp3L@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                    swDoc.Extension.SelectByID2("Эскиз64@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                }
            }
            else
            {
                swDoc.Extension.SelectByID2("Вырез-ВытянутьTp3R@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
                swDoc.Extension.SelectByID2("Вырез-ВытянутьTp3L@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
            }

            if (ValProfils.Tp4 == "01" || ValProfils.Tp4 == "00")
            {
                if (config.Contains("01"))
                {
                    swDoc.Extension.SelectByID2("Вырез-ВытянутьTp4R@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                    swDoc.Extension.SelectByID2("Эскиз82@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                }
                if (config.Contains("02"))
                {
                    swDoc.Extension.SelectByID2("Вырез-ВытянутьTp4L@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                    swDoc.Extension.SelectByID2("Эскиз83@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                }
            }
            else
            {
                swDoc.Extension.SelectByID2("Вырез-ВытянутьTp4R@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
                swDoc.Extension.SelectByID2("Вырез-ВытянутьTp4L@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
            }

            if (ValProfils.Tp1 == "02")
            {
                if (config.Contains("01"))
                {
                    swDoc.Extension.SelectByID2("Тип-02-1R@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                }
                if (config.Contains("02"))
                {
                    swDoc.Extension.SelectByID2("Тип-02-1L@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                }
            }
            else
            {
                swDoc.Extension.SelectByID2("Тип-02-1R@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
                swDoc.Extension.SelectByID2("Тип-02-1L@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
            }

            if (ValProfils.Tp2 == "02")
            {
                if (config.Contains("01"))
                {
                    swDoc.Extension.SelectByID2("Тип-02-2R@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                }
                if (config.Contains("02"))
                {
                    swDoc.Extension.SelectByID2("Тип-02-2L@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                }
            }
            else
            {
                swDoc.Extension.SelectByID2("Тип-02-2R@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
                swDoc.Extension.SelectByID2("Тип-02-2L@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
            }

            if (ValProfils.Tp3 == "02")
            {
                if (config.Contains("01"))
                {
                    swDoc.Extension.SelectByID2("Тип-02-3R@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                }
                if (config.Contains("02"))
                {
                    swDoc.Extension.SelectByID2("Тип-02-3L@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                }
            }
            else
            {
                swDoc.Extension.SelectByID2("Тип-02-3R@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
                swDoc.Extension.SelectByID2("Тип-02-3L@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
            }


            if (ValProfils.Tp4 == "02")
            {
                if (config.Contains("01"))
                {
                    swDoc.Extension.SelectByID2("Тип-02-4R@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                }
                if (config.Contains("02"))
                {
                    swDoc.Extension.SelectByID2("Тип-02-4L@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress();
                }
            }
            else
            {
                swDoc.Extension.SelectByID2("Тип-02-4R@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
                swDoc.Extension.SelectByID2("Тип-02-4L@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
            }

            // Полупанель внутрення

            if (ValProfils.Tp1 == "05")
            {

            }
            else
            {
                swDoc.Extension.SelectByID2("Тип-05-1@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
                swDoc.Extension.SelectByID2("Эскиз88@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
            }

            if (ValProfils.Tp2 == "05")
            {
               
            }
            else
            {
                swDoc.Extension.SelectByID2("Тип-05-2@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
                swDoc.Extension.SelectByID2("Эскиз66@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
            }

            if (ValProfils.Tp3 == "05")
            {

            }
            else
            {
                swDoc.Extension.SelectByID2("Тип-05-3@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
                swDoc.Extension.SelectByID2("Эскиз67@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
            }

            if (ValProfils.Tp4 == "05")
            {

            }
            else
            {
                swDoc.Extension.SelectByID2("Тип-05-4@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
                swDoc.Extension.SelectByID2("Эскиз89@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress();
            }

            //if (ValProfils.Tp2 != "02" & ValProfils.Tp3 != "02")
            //{
            //    swDoc.Extension.SelectByID2("Эскиз65@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, false, 0, null, 0);
            //    swDoc.EditDelete();
            //    swDoc.Extension.SelectByID2("Тип02", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
            //    swDoc.EditDelete();    
            //}

            //if (ValProfils.Tp2 != "01" & ValProfils.Tp3 != "01")
            //{
            //    swDoc.Extension.SelectByID2("Эскиз60@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, false, 0, null, 0);
            //    swDoc.EditDelete();
            //    swDoc.Extension.SelectByID2("Тип01", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
            //    swDoc.EditDelete();
            //}

            //if (ValProfils.Tp2 == "00" & ValProfils.Tp3 == "00")
            //{
            //    swDoc.Extension.SelectByID2("Эскиз59@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, false, 0, null, 0);
            //    swDoc.EditDelete();
            //    swDoc.Extension.SelectByID2("Вставки", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
            //    swDoc.EditDelete();
            //}

            #endregion


            #region 

            #region Отверстия под усиливающие панели

            if (typeOfPanel[0] == "21" || typeOfPanel[0] == "22" || typeOfPanel[0] == "23")
            {
                    
                swDoc.Extension.SelectByID2("Эскиз59@02-11-01-40--1@02-11-40-1", "SKETCH", 0, 0, 0, true, 0, null, 0);
                swDoc.EditUnsuppress2();
                swDoc.Extension.SelectByID2("Эскиз73@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, true, 0, null, 0);
                swDoc.EditUnsuppress2();

                if (ValProfils.PsTy1 != "-")
                {
                    if (config.Contains("02"))
                    {
                        swDocExt.SelectByID2("U32@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDocExt.SelectByID2("U31@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        
                        swDocExt.SelectByID2("U33@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDocExt.SelectByID2("U34@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();

                        //swDocExt.SelectByID2("4-1@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                        //swDoc.EditSuppress2(); 
                    }
                    if (config.Contains("01"))
                    {
                        swDocExt.SelectByID2("U52@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDocExt.SelectByID2("U51@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();

                        swDocExt.SelectByID2("U53@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDocExt.SelectByID2("U54@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                    }
                }
                if (ValProfils.PsTy2 != "-")
                {
                    if (config.Contains("02"))
                    {
                        swDocExt.SelectByID2("U42@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDocExt.SelectByID2("U41@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();

                        swDocExt.SelectByID2("U43@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDocExt.SelectByID2("U44@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        
                        //swDocExt.SelectByID2("4-1@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                        //swDoc.EditSuppress2(); 
                    }
                    if (config.Contains("01"))
                    {
                        swDocExt.SelectByID2("U62@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDocExt.SelectByID2("U61@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                     
                        swDocExt.SelectByID2("U63@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDocExt.SelectByID2("U64@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                    }
                }
            }


            if (typeOfPanel[0] == "30" || typeOfPanel[0] == "31")
            {

                swDoc.Extension.SelectByID2("Эскиз59@02-11-01-40--1@02-11-40-1", "SKETCH", 0, 0, 0, true, 0, null, 0);
                swDoc.EditUnsuppress2();
                swDoc.Extension.SelectByID2("Эскиз73@02-11-02-40--1@02-11-40-1", "SKETCH", 0, 0, 0, true, 0, null, 0);
                swDoc.EditUnsuppress2();

                if (ValProfils.PsTy1 != "-")
                {
                    if (config.Contains("02"))
                    {
                        swDocExt.SelectByID2("U32@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDocExt.SelectByID2("U31@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();

                        swDocExt.SelectByID2("U33@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDocExt.SelectByID2("U34@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();

                        //swDocExt.SelectByID2("4-1@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                        //swDoc.EditSuppress2(); 
                    }
                    if (config.Contains("01"))
                    {
                        swDocExt.SelectByID2("U52@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDocExt.SelectByID2("U51@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();

                        swDocExt.SelectByID2("U53@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDocExt.SelectByID2("U54@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                    }
                }
                if (ValProfils.PsTy2 != "-")
                {
                    if (config.Contains("02"))
                    {
                        swDocExt.SelectByID2("U42@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDocExt.SelectByID2("U41@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();

                        swDocExt.SelectByID2("U43@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDocExt.SelectByID2("U44@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();

                        //swDocExt.SelectByID2("4-1@02-11-01-40--1@" + nameAsm, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                        //swDoc.EditSuppress2(); 
                    }
                    if (config.Contains("01"))
                    {
                        swDocExt.SelectByID2("U62@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDocExt.SelectByID2("U61@02-11-01-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();

                        swDocExt.SelectByID2("U63@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDocExt.SelectByID2("U64@02-11-02-40--1@02-11-40-1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                    }
                }
            }

            #endregion


            #region Со скотчем "ребро-кромка"

            if (скотч != "Со скотчем")
            {
                swDocExt.SelectByID2("02-11-04-40--1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDocExt.SelectByID2("D1@Расстояние1@02-11-40-1.SLDASM", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                ((Dimension)(swDoc.Parameter("D1@Расстояние1"))).SystemValue = 0; // p1Deep = 19.2;
            }

            #endregion

            #endregion

            #region Изменение деталей

            #region  Панель внешняя
            
            var newName = панельВнешняя.NewName;

            #region
            //var newName = String.Format("{0}{1}{2}",
            //    modelName + "-01-" + width + "-" + height + "-",
            //    "40-" + materialP1[0],
            //     скотч == "CheckBox" ? "-ST" : "");
            #endregion

            var newPartPath = String.Format(@"{0}\{1}\{2}.SLDPRT",
                Settings.Default.DestinationFolder,
                DestinationFolder, newName);

            if (File.Exists(newPartPath))
            {
                swDoc = ((ModelDoc2)(_swApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                swDoc.Extension.SelectByID2("02-11-01-40--1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swAsm.ReplaceComponents(newPartPath, "", false, true);
                _swApp.CloseDoc("02-11-01-40-.SLDPRT");
            }
            else if (File.Exists(newPartPath) != true)
            {
                //bendParams = sbSqlBaseData.BendTable(materialP1[1].Replace('.', ','));
                //bendRadius = Convert.ToDouble(bendParams[0]);
                //kFactor = Convert.ToDouble(bendParams[1]);

                SwPartParamsChangeWithNewName("02-11-01-40-",
                        String.Format(@"{0}\{1}\{2}", Settings.Default.DestinationFolder, DestinationFolder, newName),
                        new[,]
                        {
                            // Габариты
                            {"D1@Эскиз1", Convert.ToString(ширинаПанели)},
                            {"D2@Эскиз1", Convert.ToString(высотаПанели)},
                            
                            {"D1@1-4", Convert.ToString(колСаморезВинтВысота)},
                            
                            {"D1@2-4",  typeOfPanel[0] == "01" ?  Convert.ToString(колСаморезВинтШирина-1000 < 2000 ? 2000 : колСаморезВинтШирина - 1000) 
                                : Convert.ToString(колСаморезВинтШирина < 2000 ? 2000 : колСаморезВинтШирина)},    
                            {"D1@3-4", Convert.ToString(колСаморезВинтВысота)},

                            {"D2@2-2", Convert.ToString(осьСаморезВинт)},

                            {"D4@Эскиз47", Convert.ToString(растояниеМеждуРучками)},
                            
                            {"D1@Эскиз50", Convert.ToString(диамСаморезВинт)},
                            {"D1@2-3-1", Convert.ToString(диамСаморезВинт)},
                            
                            {"D1@Эскиз52", типКрепежнойЧастиУсиливающейПанели == null ?  Convert.ToString(30) : Convert.ToString(20)},
                            {"D2@Эскиз52", Convert.ToString(осьПоперечныеОтверстия)},
                            {"D1@Кривая3", typeOfPanel[0] == "01" ?  Convert.ToString(колСаморезВинтШирина-1000 < 2000 ? 2000 : колСаморезВинтШирина - 1000) 
                                : Convert.ToString(колСаморезВинтШирина < 2000 ? 2000 : колСаморезВинтШирина)},
                                

                            {"D3@2-1-1", Convert.ToString(диамЗаглушкаВинт)},
                            {"D1@Эскиз49", Convert.ToString(диамЗаглушкаВинт)},
                            
                            {"D1@Кривая1", Convert.ToString(колЗаклепокШирина)},
                            {"D1@Кривая2", typeOfPanel[0] == "01" || typeOfPanel[0] == "04" || typeOfPanel[0] == "05" ? Convert.ToString(колЗаклепокВысота) : Convert.ToString(колЗаклепокВысота + 1000)},

                            {"D7@Ребро-кромка1", скотч == "Со скотчем" ? Convert.ToString(17.7) : Convert.ToString(19.2)},

                            // Параметры листового металла
                            //{"D7@Листовой металл3", materialP1[1].Replace('.', ',')}
                            {"Толщина@Листовой металл", materialP1[1].Replace('.', ',')}
                            //{"D1@Листовой металл3", Convert.ToString(bendRadius)},
                            //{"D2@Листовой металл3", Convert.ToString(kFactor*1000)}
                        });
                VentsMatdll(materialP1, new[] { покрытие[6], покрытие[1], покрытие[2] }, newName);
                _swApp.CloseDoc(newName);
            }

            #endregion

            #region  Панель внутреняя

            newName = панельВнутренняя.NewName;
            
            //newName = modelname2 + "-02-" + width + "-" + height + "-" + "40-" + materialP2[0] + strenghtP + panelsUpDownConfigString;

            newPartPath = String.Format(@"{0}\{1}\{2}.SLDPRT", Settings.Default.DestinationFolder,
                DestinationFolder, newName);

            if (File.Exists(newPartPath))
            {
                swDoc = ((ModelDoc2)(_swApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                swDoc.Extension.SelectByID2("02-11-02-40--1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swAsm.ReplaceComponents(newPartPath, "", false, true);
                _swApp.CloseDoc("02-11-02-40-.SLDPRT");
            }
            else if (File.Exists(newPartPath) != true)
            {
                SwPartParamsChangeWithNewName("02-11-02-40-",
                        String.Format(@"{0}\{1}\{2}", Settings.Default.DestinationFolder, DestinationFolder, newName),
                        new[,]
                        {
                            {"D1@Эскиз1", typeOfPanel[0] == "04" || typeOfPanel[0] == "05" ? Convert.ToString(ширинаПанели-42) : Convert.ToString(ширинаПанели-40)},
                            {"D2@Эскиз1", typeOfPanel[0] == "04" || typeOfPanel[0] == "05" ? Convert.ToString(высотаПанели-42) : Convert.ToString(высотаПанели-40)},
                            
                            {"D1@1-3", typeOfPanel[0] == "01" ?  Convert.ToString(колСаморезВинтШирина-1000 < 2000 ? 2000 :колСаморезВинтШирина - 1000) : 
                                Convert.ToString(колСаморезВинтШирина < 2000 ? 2000 :колСаморезВинтШирина)},
                            
                            {"D1@1-4", Convert.ToString(колСаморезВинтВысота)},
                            
                            {"D1@Кривая5", typeOfPanel[0] != "01" ?  Convert.ToString(колСаморезВинтШирина-1000 < 2000 ? 2000 : колСаморезВинтШирина - 1000) 
                                : Convert.ToString(колСаморезВинтШирина2 < 2000 ? 2000 : колСаморезВинтШирина)},
                            {"D1@Кривая6", Convert.ToString(колСаморезВинтВысота)},
                            {"D1@Кривая4", typeOfPanel[0] == "04" || typeOfPanel[0] == "05" ? Convert.ToString(колСаморезВинтВысота) : Convert.ToString(колСаморезВинтВысота - 1000)},

                            {"D2@Эскиз32", typeOfPanel[0] == "01" ? Convert.ToString(77.5) : Convert.ToString(158.12)},
                            
                            {"D4@Эскиз47", Convert.ToString(растояниеМеждуРучками)},
                            
                            {"D1@Эскиз38", Convert.ToString(диамСаморезВинт)},
                            {"D3@1-1-1", Convert.ToString(диамСаморезВинт)},
                            
                            {"D2@1-2", Convert.ToString(осьОтверстийСаморезВинт)},

                            {"D1@2-3", Convert.ToString(колЗаклепокШирина)},

                            {"D1@Кривая1", Convert.ToString(колЗаклепокШирина)},
                            {"D1@Кривая2", typeOfPanel[0] == "01" || typeOfPanel[0] == "04" || typeOfPanel[0] == "05" ? Convert.ToString(колЗаклепокВысота) : Convert.ToString(колЗаклепокВысота + 1000)},

                            {"D3@2-1-1", typeOfPanel[0] == "04" || typeOfPanel[0] == "05" ? Convert.ToString(54.0) : Convert.ToString(55.0)},
                            {"D2@Эскиз29", typeOfPanel[0] == "04" || typeOfPanel[0] == "05" ? Convert.ToString(84.0) : Convert.ToString(85.0)},
                             
                            {"D2@Эскиз43", typeOfPanel[0] == "04" || typeOfPanel[0] == "05" ? Convert.ToString(11.0) : Convert.ToString(10.0)},
                            
                            {"D1@Эскиз29", typeOfPanel[0] == "04" || typeOfPanel[0] == "05" ? Convert.ToString(11.3) : Convert.ToString(10.3)},
                            {"D1@2-1-1", typeOfPanel[0] == "04" || typeOfPanel[0] == "05" ? Convert.ToString(11.3) : Convert.ToString(10.3)},
                            {"D2@Эскиз39", typeOfPanel[0] == "04" || typeOfPanel[0] == "05" ? Convert.ToString(11.3) : Convert.ToString(10.3)},
                            {"D1@Эскиз39", typeOfPanel[0] == "04" || typeOfPanel[0] == "05" ? Convert.ToString(5.0) : Convert.ToString(4.0)},
                            
                            //Рамка усиливающая
                            {"D1@Кривая9", typeOfPanel[0] == "01" ?  Convert.ToString(колСаморезВинтШирина-1000) : 
                                Convert.ToString(колСаморезВинтШирина)},
                            {"D1@Кривая7", Convert.ToString(колЗаклепокВысота)},

                            {"D3@Эскиз56", Convert.ToString(отступОтветныхОтверстийШирина)},
                            
                            //Размеры для отверсти под клепальные гайки под съемные панели
                            {"G0@Эскиз49", Convert.ToString(OutValPanels.G0)},
                            {"G1@Эскиз49", Convert.ToString(OutValPanels.G1)},
                            {"G2@Эскиз49", Convert.ToString(OutValPanels.G2)},
                            {"G3@Эскиз49", Convert.ToString(OutValPanels.G0)},

                            //Convert.ToString(количествоВинтов)
                            {"L1@Эскиз49", Convert.ToString(OutValPanels.L1)},
                            {"D1@Кривая10", Convert.ToString(OutValPanels.D1)},
                            {"L2@Эскиз49", Convert.ToString(OutValPanels.L2)},
                            {"D1@Кривая11", Convert.ToString(OutValPanels.D2)},
                            {"L3@Эскиз49", Convert.ToString(OutValPanels.L3)},
                            {"D1@Кривая12", Convert.ToString(OutValPanels.D3)},
                            
                            //Размеры промежуточных профилей
                            {"Wp1@Эскиз59", Math.Abs(ValProfils.Wp1) < 1 ?  "10" : Convert.ToString(ValProfils.Wp1)},
                            {"Wp2@Эскиз59", Math.Abs(ValProfils.Wp2) < 1 ?  "10" : Convert.ToString(ValProfils.Wp2)},
                            {"Wp3@Эскиз59", Math.Abs(ValProfils.Wp3) < 1 ?  "10" : Convert.ToString(ValProfils.Wp3)},
                            {"Wp4@Эскиз59", Math.Abs(ValProfils.Wp4) < 1 ?  "10" : Convert.ToString(ValProfils.Wp4)},
                            
                            //todo Для промежуточной панели отверстия
                            {"D1@Кривая14", Convert.ToString(колЗаклепокВысота*2)},

                            {"Толщина@Листовой металл", materialP2[1].Replace('.', ',')}
                        });
                VentsMatdll(materialP1, new[] { покрытие[7], покрытие[4], покрытие[5] }, newName);
                _swApp.CloseDoc(newName);

            }

            #endregion

            #region Усиливающие рамки

            if (усиление)
            {
                const string thiknessF = "1";
                bendParams = sbSqlBaseData.BendTable(thiknessF);
                bendRadius = Convert.ToDouble(bendParams[0]);
                kFactor = Convert.ToDouble(bendParams[1]);

                const double heightF = 38.0;
                //if (скотч == "Со скотчем")
                //{
                //    heightF = 39.5;
                //}

                #region  Усиливающая рамка по ширине
                
                newName = усиливающаяРамкаПоШирине.NewName;
                
                //newName = modelName + "-06-" + width + "-" + "40-" + materialP2[0] + скотч;

                newPartPath = String.Format(@"{0}\{1}\{2}.SLDPRT", Settings.Default.DestinationFolder,
                    DestinationFolder, newName);

                if (File.Exists(newPartPath))
                {
                    swDoc = ((ModelDoc2)(_swApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("02-11-06-40--1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", true, true);
                    _swApp.CloseDoc("02-11-06-40-.SLDPRT");
                }
                else if (File.Exists(newPartPath) != true)
                {
                    //SetMeterial(materialP2[0], _swApp.ActivateDoc2("02-11-06-40-.SLDPRT", true, 0), "");
                    SwPartParamsChangeWithNewName("02-11-06-40-",
                            String.Format(@"{0}\{1}\{2}", Settings.Default.DestinationFolder, DestinationFolder, newName),
                            new[,]
                        {
                            {"D2@Эскиз1", typeOfPanel[0] == "04" || typeOfPanel[0] == "05" ? Convert.ToString(ширинаПанели-48) : Convert.ToString(ширинаПанели-46)},
                            
                            {"D1@Эскиз1", Convert.ToString(heightF)},

                            {"D1@Кривая3", Convert.ToString(колСаморезВинтШирина2-1000) == "1" ?  "0" :  Convert.ToString(колСаморезВинтШирина2-1000)},
                            {"D1@Кривая2", Convert.ToString(колСаморезВинтШирина)},

                            {"Толщина@Листовой металл", thiknessF},     
                            {"D1@Листовой металл", Convert.ToString(bendRadius)},
                            {"D2@Листовой металл", Convert.ToString(kFactor*1000)}
                        });
                    _swApp.CloseDoc(newName);
                }

                #endregion

                #region  Усиливающая рамка по высоте

                newName = усиливающаяРамкаПоВысоте.NewName;

                //newName = modelName + "-07-" + height + "-" + "40-" + materialP2[0] + скотч;
                newPartPath = String.Format(@"{0}\{1}\{2}.SLDPRT", Settings.Default.DestinationFolder,
                    DestinationFolder, newName);

                if (File.Exists(newPartPath))
                {
                    swDoc = ((ModelDoc2)(_swApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("02-11-07-40--1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", true, true);
                    _swApp.CloseDoc("02-11-07-40-.SLDPRT");
                }

                else if (File.Exists(newPartPath) != true)
                {
                    // SetMeterial(materialP2[0], _swApp.ActivateDoc2("02-11-07-40-.SLDPRT", true, 0), "");
                    SwPartParamsChangeWithNewName("02-11-07-40-",
                            String.Format(@"{0}\{1}\{2}", Settings.Default.DestinationFolder, DestinationFolder, newName),
                            new[,]
                        {
                            // Габарит
                            {"D3@Эскиз1", typeOfPanel[0] == "04" || typeOfPanel[0] == "05" ? Convert.ToString(высотаПанели-2) : Convert.ToString(высотаПанели)},
                            {"D1@Эскиз1", Convert.ToString(heightF)},
                            // Отверстия
                            {"D1@Кривая2", Convert.ToString(колСаморезВинтВысота)},
                            {"D1@Кривая1", Convert.ToString(колЗаклепокВысота)},
                            // Х-ки листа
                            {"Толщина@Листовой металл", thiknessF},
                            {"D1@Листовой металл", Convert.ToString(bendRadius)},
                            {"D2@Листовой металл", Convert.ToString(kFactor*1000)}
                        });
                    _swApp.CloseDoc(newName);
                }
                #endregion
            }

            #endregion

            #region Теплоизоляция
            
            //6700  Лента уплотнительная Pes20x3/25 A/AT-B
            //14800  Лента двохсторонняя акриловая HSA 19х2
            //4900  Материал теплоизол. Сlassik TWIN50

            //newName = теплоизоляция.Name;
            newName = теплоизоляция.NewName;

            //newName = modelName + "-03-" + width + "-" + height + "-" + "40";
            newPartPath = String.Format(@"{0}\{1}\{2}.SLDPRT", Settings.Default.DestinationFolder,
               DestinationFolder, newName);
            if (File.Exists(newPartPath))
            {
                swDoc = ((ModelDoc2)(_swApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                swDoc.Extension.SelectByID2("02-11-03-40--1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swAsm.ReplaceComponents(newPartPath, "", false, true);
                _swApp.CloseDoc("02-11-03-40-.SLDPRT");
            }
            else if (File.Exists(newPartPath) != true)
            {
                SwPartParamsChangeWithNewName("02-11-03-40-",
                        String.Format(@"{0}\{1}\{2}", Settings.Default.DestinationFolder, DestinationFolder, newName),
                        new[,]
                        {
                            {"D2@Эскиз1", Convert.ToString(высотаПанели-1)},
                            {"D1@Эскиз1", Convert.ToString(ширинаПанели-2)}
                        });
                _swApp.CloseDoc(newName);
            }

            #endregion

            #region Скотч

            const double rizn = 3;//5.4

            if (скотч == "Со скотчем")
            {
                //newName = cкотч.Name;
                newName = cкотч.NewName;
                //Скотч
                //newName = modelName + "-04-" + width + "-" + height;
                newPartPath = String.Format(@"{0}\{1}\{2}.SLDPRT", Settings.Default.DestinationFolder,
                    DestinationFolder, newName);
                if (File.Exists(newPartPath))
                {
                    swDoc = ((ModelDoc2)(_swApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("02-11-04-40--1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", false, true);
                    _swApp.CloseDoc("02-11-04-40-.SLDPRT");
                }
                else if (File.Exists(newPartPath) != true)
                {
                    SwPartParamsChangeWithNewName("02-11-04-40-",
                        String.Format(@"{0}\{1}\{2}", Settings.Default.DestinationFolder, DestinationFolder,
                            newName),
                        new[,]
                        {
                            {"D2@Эскиз1", Convert.ToString(высотаПанели - rizn)},
                            {"D1@Эскиз1", Convert.ToString(ширинаПанели - rizn)}
                        });
                    _swApp.CloseDoc(newName);
                }
            }

            #endregion

            #region  Pes 20x3/25 A/AT-BT 538x768
            
            //newName = pes.Name;
            newName = pes.NewName;
            //newName = modelName + "-05-" + width + "-" + height;

            newPartPath = String.Format(@"{0}\{1}\{2}.SLDPRT",
                Settings.Default.DestinationFolder,
                DestinationFolder,
                newName);

            if (File.Exists(newPartPath))
            {
                swDoc = ((ModelDoc2)(_swApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                swDoc.Extension.SelectByID2("02-11-05-40--1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swAsm.ReplaceComponents(newPartPath, "", false, true);
                _swApp.CloseDoc("02-11-05-40-.SLDPRT");
            }
            else if (File.Exists(newPartPath) != true)
            {
                SwPartParamsChangeWithNewName("02-11-05-40-",
                        String.Format(@"{0}\{1}\{2}", Settings.Default.DestinationFolder, DestinationFolder, newName),
                        new[,]
                        {
                            {"D2@Эскиз1", Convert.ToString(высотаПанели - rizn)},
                            {"D1@Эскиз1", Convert.ToString(ширинаПанели - rizn)}
                        });
                _swApp.CloseDoc(newName);
            }

            #endregion

            #region Кронштейн усиливающей панели

            if (типКрепежнойЧастиУсиливающейПанели == "D")
            {
                newName = кронштейнДверной.NewName;
                
                //newName = "02-11-09-40-" + height;

                newPartPath = String.Format(@"{0}\{1}\{2}.SLDPRT",
                    Settings.Default.DestinationFolder,
                    DestinationFolder,
                    newName);

                if (File.Exists(newPartPath))
                {
                    swDoc = ((ModelDoc2)(_swApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("02-11-09-40--1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", false, true);
                    _swApp.CloseDoc("02-11-09-40-.SLDPRT");
                }
                else if (File.Exists(newPartPath) != true)
                {
                    SwPartParamsChangeWithNewName("02-11-09-40-",
                            String.Format(@"{0}\{1}\{2}", Settings.Default.DestinationFolder, DestinationFolder, newName),
                            new[,]
                        {
                            {"D2@Эскиз1", Convert.ToString(высотаПанели - 45)},
                            {"D1@Эскиз1", скотч == "Со скотчем" ? Convert.ToString(16.0) : Convert.ToString(17.5)},
                            {"D1@Кривая1", Convert.ToString(колЗаклепокВысота)}  
                        });
                    _swApp.CloseDoc(newName);
                }
            }

            #endregion

            #endregion

            #region Задание имени сборки (description Наименование)

            switch (typeOfPanel[0])
            {
                case "Несъемная":
                case "Съемная":
                    typeOfPanel[0] = typeOfPanel[0] + " панель";
                    break;
            }

            swDoc = ((ModelDoc2)(_swApp.ActivateDoc2(nameAsm, true, 0)));
            //var swModelDocExt = swDoc.Extension;
            //var swCustPropForDescription = swModelDocExt.CustomPropertyManager[""];

            GabaritsForPaintingCamera(swDoc);
            //swCustPropForDescription.Set("Наименование", typeOfPanel[0]);
            //swCustPropForDescription.Set("Description", typeOfPanel[0]);

            #endregion

            #region Сохранение и регистрация сборки в базе

            swDoc.EditRebuild3();
            swDoc.ForceRebuild3(true);
            swDoc.SaveAs2(newFramelessPanelPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
            NewComponents.Add(new FileInfo(newFramelessPanelPath));
            _swApp.CloseDoc(new FileInfo(newFramelessPanelPath).Name);

            CheckInOutPdm(NewComponents, true, Settings.Default.TestPdmBaseName);

            foreach (var newComponent in NewComponents)
            {
                // MessageBox.Show(newComponent.Name);
                 PartInfoToXml(newComponent.FullName);
            }

            #endregion

            return newFramelessPanelPath;
        }


        /// <summary>
        /// Profils the specified height.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public string Profil(double height, string type)
        {
            const string modelName = "02-11-08";
            double width;
            string config;

            switch (type)
            {
                case "00":
                    config = "00";
                    width = 39;
                    break;
                case "01":
                    config = "01";
                    width = 39;
                    break;
                case "02":
                    config = "02";
                    width = 40;
                    break;
                case "03":
                    config = "03";
                    width = 35;
                    break;
                default:
                    config = "00";
                    width = 39;
                    break;
            }

            if (type == "-" || type == "05") return null;
            

            var newName = modelName + "-" + Convert.ToString(height - 40) + "-" + type;
            
            var newPartPath = String.Format(@"{0}\{1}\{2}.SLDPRT",
                Settings.Default.DestinationFolder,
                Profil021108Destination, newName);

            if (!InitializeSw(true)) return null;

            if (File.Exists(newPartPath))
            {
                return newPartPath;
            }

            var pdmFolder = Settings.Default.SourceFolder;

            var components = new[]
            {
                String.Format(@"{0}{1}\{2}", pdmFolder, Profil021108Destination,"02-11-08-40-.SLDPRT")
            };
            GetLastVersionPdm(components, Settings.Default.PdmBaseName);

            var fileName = String.Format(@"{0}{1}\{2}.SLDPRT", Settings.Default.SourceFolder, Profil021108, "02-11-08-40-");

            var swDoc = _swApp.OpenDoc6(fileName, (int)swDocumentTypes_e.swDocPART,
                (int)swOpenDocOptions_e.swOpenDocOptions_LoadModel, "00", 0, 0);

            swDoc.ShowConfiguration2(config);
            
            string[] configs = swDoc.GetConfigurationNames();
            foreach (var s in configs)
            {
                try
                {
                    swDoc.DeleteConfiguration2(s);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
            }
            swDoc.ConfigurationManager.ActiveConfiguration.Name = "00";

            #region Погашения

            if (height < 750)
            {
                swDoc.Extension.SelectByID2("Вырез-Вытянуть5", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress2();
            }
                
            #endregion

            var колЗаклепокВысота = (Math.Truncate((height + 38) / 125) + 1) * 1000;

            SwPartParamsChangeWithNewName("02-11-08-40-",
                String.Format(@"{0}\{1}\{2}", Settings.Default.DestinationFolder, Profil021108Destination, newName),
                new[,]
                {
                    // Габариты
                    {"D1@Эскиз1", Convert.ToString(width)},
                    {"D2@Эскиз1", Convert.ToString(height)},
                    {"D1@Кривая1", Convert.ToString(колЗаклепокВысота)}
                });
            _swApp.CloseDoc(newName);

            return newPartPath;
        }
        
        #endregion

        #region Naming of AddingPanel


        /// <summary>
        /// Gets or sets the adding panels.
        /// </summary>
        /// <value>
        /// The adding panels.
        /// </value>
        public List<AddingPanel> AddingPanels = new List<AddingPanel>(); 

        /// <summary>
        /// 
        /// </summary>
        public class AddingPanel : FramelessPanelVault
        {
            /// <summary>
            /// Adds the part.
            /// </summary>
            public int AddPart()
            {
                var id = 0;
                try
                {
                    Логгер.Отладка("Добавление панели: " + Name, "", "AddPart", "AddingPanel");
                    id = AirVents_AddPartOfPanel();
                    Логгер.Отладка("Добавление панели прошло успешно!", "", "AddPart", "AddingPanel");
                }
                catch (Exception exception)
                {
                    Логгер.Ошибка("Ошибка во время добавления панели:" + exception.Message, exception.StackTrace, "Add", "AddingPanel");
                }
                return id;
            }

            /// <summary>
            /// Adds this instance.
            /// </summary>
            public int Add()
            {
                var id = 0;
                try
                {
                    Логгер.Отладка("Добавление панели: " + Name, "", "Add", "AddingPanel");
                    id = AirVents_AddPanel();
                    Логгер.Отладка("Добавление панели прошло успешно!", "", "Add", "AddingPanel");
                }
                catch (Exception exception)
                {
                    Логгер.Ошибка("Ошибка во время добавления панели:" + exception.Message, exception.StackTrace, "Add", "AddingPanel");
                }
                return id;
            }

            /// <summary>
            /// Adds the asm.
            /// </summary>
            public void AddAsm()
            {
                try
                {
                    Логгер.Отладка("Добавление сборки панели: " + Name, "", "Add", "AddingPanel");
                    AirVents_AddPanelAsm();
                    Логгер.Отладка("Добавление панели прошло успешно!", "", "Add", "AddingPanel");
                }
                catch (Exception exception)
                {
                    Логгер.Ошибка("Ошибка во время добавления панели:" + exception.Message, exception.StackTrace, "Add", "AddingPanel");
                }
            }
        }
        
        // ReSharper disable once UnusedMember.Local
        class Dimensions : AddingPanel
        {
// ReSharper disable UnusedMember.Local
            protected List<Dimensions> DimensionsCollection
                // ReSharper restore UnusedMember.Local
            {
                get {return new List<Dimensions>();}
            }
        }

        /// <summary>
        /// Хранилище бескаркасных панелей
        /// </summary>
        public class FramelessPanelVault
        {
            /// <summary>
            /// Gets or sets the new name.
            /// </summary>
            /// <value>
            /// The new name.
            /// </value>
            public string NewName { get; set; }
            
            /// <summary>
            /// Gets or sets the part in asm ids.
            /// </summary>
            /// <value>
            /// The part in asm ids.
            /// </value>
            public List<int> PartInAsmIds { get; set; }

            /// <summary>
            /// Gets or sets the part identifier.
            /// </summary>
            /// <value>
            /// The part identifier.
            /// </value>
            public int PartId { get; set; }

            /// <summary>
            /// Имя типа детали
            /// </summary>
            /// <value>
            /// The name of the panel type.
            /// </value>
            public string PanelTypeName { get; set; }

            /// <summary>
            /// Тип детали в сборке
            /// </summary>
            /// <value>
            /// The type of the element.
            /// </value>
            public int ElementType { get; set; }

            /// <summary>
            /// Ширина панели
            /// </summary>
            /// <value>
            /// The width.
            /// </value>
            public int? Width { get; set; }

            /// <summary>
            /// Высота панели 
            /// </summary>
            /// <value>
            /// The height.
            /// </value>
            public int? Height { get; set; }

            /// <summary>
            /// Код материала детали
            /// </summary>
            /// <value>
            /// The panel mat.
            /// </value>
            public int? PartMat { get; set; }
            
            /// <summary>
            /// Gets or sets the part thick.
            /// </summary>
            /// <value>
            /// The part thick.
            /// </value>
            public int PartThick { get; set; }

            /// <summary>
            /// Толщина материала детали
            /// </summary>
            /// <value>
            /// The panel mat thick.
            /// </value>
            public double? PartMatThick { get; set; }

            /// <summary>
            /// Наличие усиления
            /// </summary>
            /// <value>
            /// The Reinforcing.
            /// </value>
            public bool Reinforcing { get; set; }
            
            /// <summary>
            /// Gets or sets the sticky tape.
            /// </summary>
            /// <value>
            /// The sticky tape.
            /// </value>
            public bool StickyTape { get; set; }
            
            /// <summary>
            /// Цвет покраски RAL
            /// </summary>
            /// <value>
            /// The ral.
            /// </value>
            public string Ral { get; set; }

            /// <summary>
            ///Тип покрытия
            /// </summary>
            /// <value>
            /// The type of the coating.
            /// </value>
            public string CoatingType { get; set; }

            /// <summary>
            /// Gets or sets the coating class.
            /// </summary>
            /// <value>
            /// The coating class.
            /// </value>
            public int? CoatingClass { get; set; }

            /// <summary>
            /// Зеркальность детали или сборки
            /// </summary>
            /// <value>
            /// The mirror.
            /// </value>
            public bool? Mirror { get; set; }

            /// <summary>
            /// Описание шагов дл ясъемных панелей
            /// </summary>
            /// <value>
            /// The step.
            /// </value>
            public string Step { get; set; }

            /// <summary>
            /// Описание шагов для вставок
            /// </summary>
            /// <value>
            /// The step insertion.
            /// </value>
            public string StepInsertion { get; set; }

            /// <summary>
            /// Gets or sets the coating type out.
            /// </summary>
            /// <value>
            /// The coating type out.
            /// </value>
            public string CoatingTypeOut { get; set; }

            /// <summary>
            /// Gets or sets the coating class out.
            /// </summary>
            /// <value>
            /// The coating class out.
            /// </value>
            public int? CoatingClassOut { get; set; }

            /// <summary>
            /// Gets or sets the coating type in.
            /// </summary>
            /// <value>
            /// The coating type in.
            /// </value>
            public string CoatingTypeIn { get; set; }

            /// <summary>
            /// Gets or sets the coating class in.
            /// </summary>
            /// <value>
            /// The coating class in.
            /// </value>
            public int? CoatingClassIn { get; set; }

            /// <summary>
            /// Gets or sets the ral out.
            /// </summary>
            /// <value>
            /// The ral out.
            /// </value>
            public string RalOut { get; set; }

            /// <summary>
            /// Gets or sets the ral in.
            /// </summary>
            /// <value>
            /// The ral in.
            /// </value>
            public string RalIn { get; set; }

            /// <summary>
            /// Gets or sets the part mat thick out.
            /// </summary>
            /// <value>
            /// The part mat thick out.
            /// </value>
            public double? PanelMatThickOut { get; set; }

            /// <summary>
            /// Gets or sets the part mat thick in.
            /// </summary>
            /// <value>
            /// The part mat thick in.
            /// </value>
            public double? PanelMatThickIn { get; set; }
            
            /// <summary>
            /// Gets or sets the panel thick.
            /// </summary>
            /// <value>
            /// The panel thick.
            /// </value>
            public int PanelThick { get; set; }

            /// <summary>
            /// Gets or sets the part mat out.
            /// </summary>
            /// <value>
            /// The part mat out.
            /// </value>
            public int? PanelMatOut { get; set; }

            /// <summary>
            /// Gets or sets the part mat in.
            /// </summary>
            /// <value>
            /// The part mat in.
            /// </value>
            public int? PanelMatIn { get; set; }
            
            /// <summary>
            /// Gets or sets the panel number.
            /// </summary>
            /// <value>
            /// The panel number.
            /// </value>
            public int PanelNumber { get; set; }
            
            /// <summary>
            /// Получение имени детали либо сборки
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            public string Name
            {
                get
                {
                    return String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}",
                        "02-",
                        string.IsNullOrEmpty(PanelTypeName) ? null : PanelTypeName,
                        "-0" + ElementType,
                        Width == null ? null: "-" + Width,
                        Height == null ? null : "-" + Height,
                        PanelThick == 0 ? null : "-" + PanelThick,
                        PartMat == null ? null : "-" + PartMat,
                        PartMatThick == null ? null : "-" + PartMatThick,
                        Reinforcing ? "-Y" : null,
                        string.IsNullOrEmpty(Ral) ? null : "-" + Ral,
                        string.IsNullOrEmpty(CoatingType) ? null : "-" + CoatingType,
                        CoatingClass == 0 ? null : "-" + CoatingClass,
                        Mirror == true ? "-M":null,
                        Step,
                        StepInsertion);
                }
            }

            /// <summary>
            /// Gets the name by identifier.
            /// </summary>
            /// <value>
            /// The name by identifier.
            /// </value>
            public string NameById
            {
                get
                {
                    return String.Format("{0}{1}{2}",
                        "02-00",
                        PartId,
                        "-0" + ElementType
                        );
                }
            }

            /// <summary>
            /// The parts list
            /// </summary>
            public List<FramelessPanelVault> PartsParamsList;

            /// <summary>
            /// Airs the vents_ add panel full.
            /// </summary>
            public void AirVents_AddPanelFull()
            {
                try
                {
                    var sqlBaseData = new SqlBaseData();
                    sqlBaseData.AirVents_AddPanelFull(ConvertToDataTable(PartsParamsList));
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.StackTrace);
                }
            }

            /// <summary>
            /// Converts to data table.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="data">The data.</param>
            /// <returns></returns>
            public DataTable ConvertToDataTable<T>(IList<T> data)
            {
                var propertyDescriptorCollection =
                   TypeDescriptor.GetProperties(typeof(T));
                var table = new DataTable();
                foreach (PropertyDescriptor prop in propertyDescriptorCollection)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                foreach (T item in data)
                {
                    var row = table.NewRow();
                    foreach (PropertyDescriptor prop in propertyDescriptorCollection)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    table.Rows.Add(row);
                }
                return table;
            }

            /// <summary>
            /// Запрос на добавление детали бескаркасной панели
            /// </summary>
            protected int AirVents_AddPanel()
            {
                var id = 0;
                try
                {
                    var sqlBaseData = new SqlBaseData();
                    id = sqlBaseData.AirVents_AddPanel(
                        partId: PartId,
                        panelTypeName : PanelTypeName,
                        width : Width,
                        height : Height,
                        panelMatOut : PanelMatOut,
                        panelMatIn : PanelMatIn,
                        panelThick : PanelThick,

                        panelMatThickOut : PanelMatThickOut,
                        panelMatThickIn : PanelMatThickIn,
                        #region Покраска
                        //ralOut : RalOut,
                        //ralIn : RalIn,
                        //coatingTypeOut : CoatingTypeOut,
                        //coatingTypeIn : CoatingTypeIn,
                        //coatingClassOut : CoatingClassOut,
                        //coatingClassIn : CoatingClassIn,
                        #endregion
                        mirror: Mirror.HasValue ? Mirror : false,
                        step : string.IsNullOrEmpty(Step) ? "0" : Step,
                        stepInsertion :  string.IsNullOrEmpty(StepInsertion) ? "0" : StepInsertion,
                        reinforcing01: Reinforcing,

                        stickyTape: StickyTape,
                        
                        panelNumber :PanelNumber);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.StackTrace);
                }
                return id;
            }

            /// <summary>
            /// Airs the vents_ add part of panel.
            /// </summary>
            protected int AirVents_AddPartOfPanel()
            {
                var id = 0;
                try
                {
                    var sqlBaseData = new SqlBaseData();
                    id = sqlBaseData.AirVents_AddPartOfPanel
                        (
                        panelTypeName: PanelTypeName,
                        elementType: ElementType,
                        width: Width,
                        height: Height,
                        partThick: PartThick,
                        partMat: PartMat,
                        partMatThick: PartMatThick,
                        reinforcing: Reinforcing,
                        stickyTape: StickyTape,
                        #region Покраска
                        //ral: Ral,
                        //coatingType: CoatingType,
                        //coatingClass: CoatingClass,
                    #endregion
                        mirror: Mirror.HasValue ? Mirror:false,
                        step: string.IsNullOrEmpty(Step) ? "0" : Step,
                        stepInsertion: string.IsNullOrEmpty(StepInsertion) ? "0" : StepInsertion
                        );
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.StackTrace);
                }
                return id;
            }

            /// <summary>
            /// Airs the vents_ add panel asm.
            /// </summary>
            protected void AirVents_AddPanelAsm()
            {
                try
                {
                    var sqlBaseData = new SqlBaseData();
                    int partId;
                    sqlBaseData.AirVents_AddPanelAsm(
                        PartInAsmIds, 
                        out partId);
                    PartId = partId;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.StackTrace);
                }
            }
        }

        #endregion

        #region Съемные панели и перегородки

        static void СъемнаяПанель(double посадочнаяШирина, double посадочнаяВысота,
                                    out double ширина, out double высота,
                                    out double расстояниеL, out double количествоВинтов)
        {
            ширина = посадочнаяШирина - 2;
            высота = посадочнаяВысота - 2;
            расстояниеL = посадочнаяШирина - 132;

            количествоВинтов = 5000;

            if (посадочнаяШирина < 1100)
            {
                количествоВинтов = 4000;
            }
            if (посадочнаяШирина < 700)
            {
                количествоВинтов = 3000;
            }
            if (посадочнаяШирина < 365)
            {
                количествоВинтов = 2000;
            }
        }

        /// <summary>
        /// Входящие параметры для определения отверстий верхней и нижней панелей
        /// </summary>
        struct InValPanels
        {
            //Зазоры
            static public double G0;
            static public double G1;
            static public double G2;

            //Панели
            static public double B1;
            static public double B2;
            static public double B3;

            /// <summary>
            /// Преобразование строки расположение панелей в значения необходимые для построения
            /// </summary>
            /// <param name="values">The values.</param>
            public static void StringValue(string values)
            {
                G0 = 0;
                G1 = 0;
                G2 = 0;
                B1 = 0;
                B2 = 0;
                B3 = 0;

                try
                {
                    var val = values.Split(';');
                    var lenght = val.Length;

                    if (lenght < 1) return;
                    if (val[0] == "") return;
                    G0 = Convert.ToDouble(val[0]);

                    if (lenght < 2) return;
                    if (val[1] == "") return;
                    B1 = Convert.ToDouble(val[1]);

                    if (lenght < 3) return;
                    if (val[2] == "") return;
                    G1 = Convert.ToDouble(val[2]);

                    if (lenght < 4) return;
                    if (val[3] == "") return;
                    B2 = Convert.ToDouble(val[3]);

                    if (lenght < 5) return;
                    if (val[4] == "") return;
                    G2 = Convert.ToDouble(val[4]);

                    if (lenght < 6) return;
                    if (val[5] == "") return;
                    B3 = Convert.ToDouble(val[5]);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("StringValue " + exception.Message);
                }
                finally
                {
                    //MessageBox.Show(InValUpDown() + " ==  " + OutValPanels.OutValUpDown());
                    ВерхняяНижняяПанельОтверстияПодСъемную();
                    //MessageBox.Show(InValUpDown() + " ==  " + OutValPanels.OutValUpDown());
                }
            }

            public static string InValUpDown()
            {
                return String.Format("_({0}{1}{2}{3}{4}{5})",
                    Convert.ToInt32(G0) != 0 ? Convert.ToString(G0) : "",
                    Convert.ToInt32(B1) != 0 ? "-" + Convert.ToString(B1) : "",
                    Convert.ToInt32(G1) != 0 ? "_" + Convert.ToString(G1) : "",
                    Convert.ToInt32(B2) != 0 ? "-" + Convert.ToString(B2) : "",
                    Convert.ToInt32(G2) != 0 ? "_" + Convert.ToString(G2) : "",
                    Convert.ToInt32(B3) != 0 ? "-" + Convert.ToString(B3) : ""
                    );
            }
        }

        /// <summary>
        /// Исходящие параметры для определения отверстий верхней и нижней панелей
        /// </summary>
        struct OutValPanels
        {
            //Зазоры
            static public double G0;
            static public double G1;
            static public double G2;
            // Панель1
            public static double L1;
            public static double D1;
            // Панель2
            static public double L2;
            public static double D2;
            // Панель3
            static public double L3;
            public static double D3;

            //public static string OutValUpDown()
            //{
            //    return String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
            //        Convert.ToInt32(G0) != 0 ? Convert.ToString(G0) : "",
            //        Convert.ToInt32(L1) != 0 ? ";" + Convert.ToString(L1) : "",
            //        Convert.ToInt32(D1) != 0 ? ";" + Convert.ToString(D1) : "",
            //        Convert.ToInt32(G1) != 0 ? ";" + Convert.ToString(G1) : "",
            //        Convert.ToInt32(L2) != 0 ? ";" + Convert.ToString(L2) : "",
            //        Convert.ToInt32(D2) != 0 ? ";" + Convert.ToString(D2) : "",
            //        Convert.ToInt32(G2) != 0 ? ";" + Convert.ToString(G2) : "",
            //        Convert.ToInt32(L3) != 0 ? ";" + Convert.ToString(L3) : "",
            //        Convert.ToInt32(D3) != 0 ? ";" + Convert.ToString(D3) : "");
            //}
        }

        static void ВерхняяНижняяПанельОтверстияПодСъемную()
        {
            #region Зазоры

            // Зазоры по умолчанию
            OutValPanels.G0 = 46;
            OutValPanels.G1 = 132;
            OutValPanels.G2 = 132;

            if (Math.Abs(InValPanels.G0) > 0)
            {
                OutValPanels.G0 = InValPanels.G0;
            }
            if (Math.Abs(InValPanels.G1) > 0)
            {
                OutValPanels.G1 = InValPanels.G1;
            }
            if (Math.Abs(InValPanels.G2) > 0)
            {
                OutValPanels.G2 = InValPanels.G2;
            }

            #endregion

            #region Отверстия под панель

            double ширина;
            double высота;
            double расстояниеL;
            double количествоВинтов;

            СъемнаяПанель(InValPanels.B1, 0, out ширина, out высота, out расстояниеL, out количествоВинтов);
            OutValPanels.L1 = расстояниеL;
            OutValPanels.D1 = количествоВинтов;

            OutValPanels.L2 = 28;
            OutValPanels.D2 = 2000;
            OutValPanels.L3 = 28;
            OutValPanels.D3 = 2000;

            if (Math.Abs(InValPanels.B2) > 0)
            {
                СъемнаяПанель(InValPanels.B2, 0, out ширина, out высота, out расстояниеL, out количествоВинтов);
                OutValPanels.L2 = расстояниеL;
                OutValPanels.D2 = количествоВинтов;
            }

            if (!(Math.Abs(InValPanels.B3) > 0)) return;
            СъемнаяПанель(InValPanels.B3, 0, out ширина, out высота, out расстояниеL, out количествоВинтов);
            OutValPanels.L3 = расстояниеL;
            OutValPanels.D3 = количествоВинтов;

            #endregion
        }

        struct ValProfils
        {
            //Посадочные размеры для пормежуточных профилей
            static public double Wp1;
            static public double Wp2;
            static public double Wp3;
            static public double Wp4;

            static public double PsSize1;
            static public double PsSize2;

            //Типы промежуточных профилей
            static public string Tp1;
            static public string Tp2;
            static public string Tp3;
            static public string Tp4;

            static public string PsTy1;
            static public string PsTy2;

            public static void StringValue(string values)
            {
                try
                {
                    //MessageBox.Show(values);

                    var val = values.Split(';');

                    var p1 = val[0].Split('_');
                    Tp1 = p1[0];
                    Double.TryParse(p1[1], out Wp1);

                    var p2 = val[1].Split('_');
                    Tp2 = p2[0];
                    Double.TryParse(p2[1], out Wp2);

                    var p3 = val[2].Split('_');
                    Tp3 = p3[0];
                    Double.TryParse(p3[1], out Wp3);

                    var p4 = val[3].Split('_');
                    Tp4 = p4[0];
                    Double.TryParse(p4[1], out Wp4);

                    var ps1 = val[4].Split('_');
                    PsTy1 = ps1[0];
                    Double.TryParse(ps1[1], out PsSize1);

                    var ps2 = val[5].Split('_');
                    PsTy2 = ps2[0];
                    Double.TryParse(ps2[1], out PsSize2);

                    //MessageBox.Show(p1[0] + p1[1] + p2[0] + p2[1] + p3[0] + p3[1]);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("StringValue " + exception.Message);
                }
            }
            
            public string InValUpDown()
            {
                return String.Format("П1 - {0}\n П2 - {1}\n П3 - {2}\n П4 - {3}\n П5 - {4}\n П6 - {5}",
                    Tp1 + "-" + Wp1,
                    Tp2 + "-" + Wp2,
                    Tp3 + "-" + Wp3,
                    Tp4 + "-" + Wp4,
                    PsTy1 + "-" + PsSize1,
                    PsTy2 + "-" + PsSize2);
            }
        }

        #endregion
        
    }
}
