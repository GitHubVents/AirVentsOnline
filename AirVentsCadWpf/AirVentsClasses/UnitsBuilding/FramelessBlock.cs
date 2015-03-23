using System;
using System.IO;
using System.Windows;
using AirVentsCadWpf.Properties;
using AirVentsCadWpf.Логирование;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace AirVentsCadWpf.AirVentsClasses.UnitsBuilding
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ModelSw
    {
        #region Fields

        /// <summary>
        ///  Папка с исходной моделью "Панель бескаркасной установки". 
        /// </summary>
        const string FrameLessFolder = @"\Библиотека проектирования\DriveWorks\01 - Frameless Design 40mm"; 
        
        /// <summary>
        /// The unit frameless oreders
        /// </summary>
        const string UnitFramelessOreders = @"\Заказы AirVents Frameless";

        // const string modelName = "01 - Frameless Design 40mm.SLDASM";
        const string ModelName = "01 - Frameless Design 40mm.SLDASM";
        
        #endregion

        #region FramelessBlock

        /// <summary>
        /// Framelesses the block.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="order">The order.</param>
        /// <param name="side">The side.</param>
        /// <param name="section">The section.</param>
        /// <param name="pDown">The p down.</param>
        /// <param name="pFixed">The p fixed.</param>
        /// <param name="pUp">The p up.</param>
        /// <param name="съемныеПанели"></param>
        /// <param name="height">The height.</param>
        public void FramelessBlock(string size, string order, string side, string section, string pDown, string pFixed, string pUp, string[] съемныеПанели, string height)
        {
            Логгер.Информация(String.Format("Бескаркасная установка {0}-{1}  {2}", size, order, section), "", "FramelessBlock", "FramelessBlock");
            
            if (!InitializeSw(true))  return;

            var path = FramelessBlockStr(size, order, side, section, pDown, pFixed, pUp, съемныеПанели, height);

            if (path == "") return;

            if (MessageBox.Show(
                string.Format("Модель находится по пути:\n {0}\n Открыть модель?", new FileInfo(path).Directory),
                string.Format(" {0} ", Path.GetFileNameWithoutExtension(new FileInfo(path).FullName)),
                MessageBoxButton.YesNoCancel) != MessageBoxResult.Yes) return;

            _swApp.CloseDoc(Path.GetFileName(pDown));
            _swApp.CloseDoc(Path.GetFileName(pFixed));
            _swApp.CloseDoc(Path.GetFileName(pUp));
            foreach (var панель in съемныеПанели)
            {
                _swApp.CloseDoc(Path.GetFileName(панель));    
            }
            FramelessBlockStr(size, order, side, section, pDown, pFixed, pUp, съемныеПанели, height);
            Логгер.Информация("Блок згенерирован", "" , "FramelessBlock", "FramelessBlock");
        }


        /// <summary>
        /// Framelesses the block string.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="order">The order.</param>
        /// <param name="side">The side.</param>
        /// <param name="section">The section.</param>
        /// <param name="панельНижняя">The p down.</param>
        /// <param name="панельНесъемная">The p fixed.</param>
        /// <param name="панельВерхняя">The p up.</param>
        /// <param name="панелиСъемные">The ПАНЕЛИ СЪЕМНЫЕ.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public string FramelessBlockStr(
            string size,
            string order,
            string side,
            string section,
            string панельНижняя,
            string панельНесъемная,
            string панельВерхняя,
            string[] панелиСъемные,
            string height)
        {
            #region Start


            var unitAsMmodel = String.Format(@"{0}{1}\{2}", Settings.Default.SourceFolder, FrameLessFolder, ModelName);

            var framelessBlockNewName = String.Format
                (
                "{0} {1}B Section {2}",
                size,
                order,
                section
                );

            var orderFolder = String.Format(@"{0}\{1}\{2} {3}B",
                Settings.Default.DestinationFolder,
                UnitFramelessOreders,
                size,
                order);

            CreateDistDirectory(orderFolder);

            var framelessBlockNewPath = String.Format(@"{0}\{1}.SLDASM",
                orderFolder,
                framelessBlockNewName);

            if (File.Exists(framelessBlockNewPath))
            {
                GetLastVersionPdm(new FileInfo(framelessBlockNewPath).FullName, Settings.Default.TestPdmBaseName);
                _swApp.OpenDoc6(framelessBlockNewPath, (int)swDocumentTypes_e.swDocASSEMBLY, (int)swOpenDocOptions_e.swOpenDocOptions_LoadModel, "00", 0, 0);
                return framelessBlockNewPath;
            }

            var pdmFolder = Settings.Default.SourceFolder;
            var components = new[]
            {
                unitAsMmodel,
                String.Format(@"{0}{1}{2}", pdmFolder, @"\Проекты\AirVents\AirVents 30\ВНС-901.92.000\","ВНС-901.92.001.SLDPRT"),
                String.Format(@"{0}{1}{2}", pdmFolder, @"\Проекты\Blauberg\10 - Рама монтажная\", "10-3-980-700.SLDASM"),
                String.Format(@"{0}{1}{2}", pdmFolder, @"\Библиотека проектирования\Прочие изделия\Крепежные изделия\Замки и ручки\", "M8-Panel block-one side.SLDPRT"),
                String.Format(@"{0}{1}{2}", pdmFolder, @"\Библиотека проектирования\Стандартные изделия\", "Threaded Rivets с насечкой.SLDPRT")
            };

            GetLastVersionPdm(components, Settings.Default.PdmBaseName);
            var swDoc = _swApp.OpenDoc6(unitAsMmodel, (int)swDocumentTypes_e.swDocASSEMBLY, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", 0, 0);
            _swApp.Visible = true;
            var swAsm = (AssemblyDoc)swDoc;
            swAsm.ResolveAllLightWeightComponents(false);

            #endregion

            #region Метизы

            switch (панелиСъемные[3])
            {
                case "Со скотчем":
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-11@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-8@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-12@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-7@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-1@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-2@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    break;
                default:
                    swDoc.Extension.SelectByID2("Threaded Rivets-11@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets-8@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets-12@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets-7@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets-1@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets-2@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    break;
            }


            switch (side)
            {
                case "левая":

                    #region Верхняя и нижняя панели
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-18@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-53@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-53@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Право", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    #endregion

                    #region Передняя и задняя панели

                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-74@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-39@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-75@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-40@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-40@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Винт и заглушка правая панель", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("ЗеркальныйКомпонент8", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Винт и заглушка правая панель массив", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();

                    #endregion

                    #region Съемные панели

                    if (String.IsNullOrEmpty(панелиСъемные[2]))
                    {
                        // Удаление 3-й панели

                        //MessageBox.Show("Удаление 3-й панели", "левая");
                        swDoc.Extension.SelectByID2("Третья панель право", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditDelete();
                        swDoc.Extension.SelectByID2("SC GOST 17473_gost-11@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("Washer 11371_gost-41@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("Threaded Rivets Increased-11@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("Threaded Rivets-11@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("Threaded Rivets-11@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditDelete();

                        if (String.IsNullOrEmpty(панелиСъемные[1]))
                        {
                            // Удаление 2-й панели

                      //      MessageBox.Show("Удаление 2-й панели", "левая");
                            swDoc.Extension.SelectByID2("Вторая панель право", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                            swDoc.EditDelete();
                            swDoc.Extension.SelectByID2("SC GOST 17473_gost-7@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            swDoc.Extension.SelectByID2("Washer 11371_gost-37@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            swDoc.Extension.SelectByID2("Threaded Rivets Increased-7@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            swDoc.Extension.SelectByID2("Threaded Rivets-7@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            swDoc.Extension.SelectByID2("Threaded Rivets-7@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                            swDoc.EditDelete();
                        }
                    }


                    swDoc.Extension.SelectByID2("Washer 11371_gost-38@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-8@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-8@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();

                    swDoc.Extension.SelectByID2("Вторая панель лево", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("SC GOST 17473_gost-8@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditDelete();

                    swDoc.Extension.SelectByID2("Washer 11371_gost-38@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-8@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.ShowConfiguration2("Рифленая клепальная гайка М8");
                    swDoc.Extension.SelectByID2("Threaded Rivets-8@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Вторая панель лево", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("SC GOST 17473_gost-12@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets-12@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-12@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Washer 11371_gost-42@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Третья панель лево", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("SC GOST 17473_gost-2@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets-2@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-2@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Washer 11371_gost-32@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Первая панель лево", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Третья панель лево зерк", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();



                    swDoc.Extension.SelectByID2("DerivedCrvPattern6", "COMPPATTERN", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("DerivedCrvPattern7", "COMPPATTERN", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("DerivedCrvPattern7", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();



                    #endregion

                    break;
                case "правая":

                    #region Съемные панели

                    if (String.IsNullOrEmpty(панелиСъемные[2]))
                    {
                        // Удаление 3-й панели

//                        MessageBox.Show("Удаление 3-й панели", "правая");
                        swDoc.Extension.SelectByID2("Третья панель лево", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditDelete();
                        swDoc.Extension.SelectByID2("SC GOST 17473_gost-12@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("Threaded Rivets-12@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("Threaded Rivets Increased-12@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("Washer 11371_gost-42@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("Washer 11371_gost-42@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditDelete();

                        if (String.IsNullOrEmpty(панелиСъемные[1]))
                        {
                            // Удаление 2-й панели

                          //  MessageBox.Show("Удаление 2-й панели", "правая");

                            swDoc.Extension.SelectByID2("Вторая панель лево", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                            swDoc.EditDelete();
                            swDoc.Extension.SelectByID2("SC GOST 17473_gost-8@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            swDoc.Extension.SelectByID2("Washer 11371_gost-38@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            swDoc.Extension.SelectByID2("Threaded Rivets Increased-8@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            swDoc.Extension.SelectByID2("Threaded Rivets-8@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            swDoc.Extension.SelectByID2("Threaded Rivets-8@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                            swDoc.EditDelete();
                        }
                    }

                    swDoc.Extension.SelectByID2("SC GOST 17473_gost-1@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-1@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Washer 11371_gost-31@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets-1@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets-1@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Первая панель право", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("SC GOST 17473_gost-7@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Washer 11371_gost-37@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-7@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets-7@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Вторая панель право", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("SC GOST 17473_gost-11@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Washer 11371_gost-41@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-11@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets-11@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Третья панель право", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Первая панель право зерк", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Третья панель право зерк", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();


                    swDoc.Extension.SelectByID2("DerivedCrvPattern4", "COMPPATTERN", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("DerivedCrvPattern5", "COMPPATTERN", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("DerivedCrvPattern5", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();

                    #endregion

                    #region Верхняя и нижняя панели

                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-37@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-2@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-2@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Лево", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    #endregion

                    #region Передняя и задняя панели

                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-37@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-72@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-73@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-38@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-38@01 - Frameless Design 40mm", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Винт и заглушка левая панель", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("ЗеркальныйКомпонент6", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт и заглушка левая панель массив", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("ЗеркальныйКомпонент6", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();

                    #endregion

                    break;
            }

            #endregion

            #region Панели

            #region Сторона обслуживания

            var panelLeft1 = панельНесъемная;
            var panelRight1 = панелиСъемные[0];

            var panelLeft2 = панелиСъемные.Length > 1 ? панелиСъемные[1] : "";
            var panelLeft3 = панелиСъемные.Length > 2 ? панелиСъемные[2] : "";

            var panelRight2 = панелиСъемные.Length > 1 ? панелиСъемные[1] : "";
            var panelRight3 = панелиСъемные.Length > 2 ? панелиСъемные[2] : "";

            if (side == "правая")
            {
                panelLeft1 = панелиСъемные[0];
                panelRight1 = панельНесъемная;
            }

            #endregion

            swDoc.Extension.SelectByID2("02-11-40-1-1@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
            swAsm.ReplaceComponents(панельНижняя, "", false, true);
            swDoc.Extension.SelectByID2("02-11-40-1-3@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
            swAsm.ReplaceComponents(панельВерхняя, "", false, true);

            swDoc.Extension.SelectByID2("02-11-40-1-2@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
            swAsm.ReplaceComponents(panelLeft1, "", false, true);
            swDoc.Extension.SelectByID2("02-11-40-1-4@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
            swAsm.ReplaceComponents(panelRight1, "", false, true);

            if (side == "правая")
            {
                swDoc.Extension.SelectByID2("Совпадение1709", "MATE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress2();
                swDoc.ClearSelection2(true);
                swDoc.Extension.SelectByID2("Совпадение9", "MATE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditUnsuppress2();
                swDoc.ClearSelection2(true);
            }
            else
            {
                swDoc.Extension.SelectByID2("Совпадение1710", "MATE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress2();
                swDoc.ClearSelection2(true);
                swDoc.Extension.SelectByID2("Совпадение4", "MATE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditUnsuppress2();
                swDoc.ClearSelection2(true);
            }

            // Если две съемных панели

            if (panelLeft2 != "")
            {
                swDoc.Extension.SelectByID2("02-11-40-1-5@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0,
                    false, 0, null, 0);
                if (side != "правая")
                {
                    swAsm.ReplaceComponents(panelLeft2, "", false, true);
                }
                else
                {
                    swDoc.EditDelete();
                }

                swDoc.Extension.SelectByID2("02-11-40-1-7@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0,
                    false, 0, null, 0);

                if (side == "правая")
                {
                    swAsm.ReplaceComponents(panelRight2, "", false, true);
                }
                else
                {
                    swDoc.EditDelete();
                }
            }
            else
            {
                swDoc.Extension.SelectByID2("02-11-40-1-5@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0,
                    false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("02-11-40-1-7@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0,
                    false, 0, null, 0);
                swDoc.EditDelete();
            }

            // Если три съемных панели

            if (panelLeft3 != "")
            {
                swDoc.Extension.SelectByID2("02-11-40-1-6@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                if (side != "правая")
                {
                    swAsm.ReplaceComponents(panelLeft3, "", false, true);
                }
                else
                {
                    swDoc.EditDelete();
                }
                
                swDoc.Extension.SelectByID2("02-11-40-1-8@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                if (side == "правая")
                {
                    swAsm.ReplaceComponents(panelRight3, "", false, true);
                }
                else
                {
                    swDoc.EditDelete();
                }
            }
            else
            {
                swDoc.Extension.SelectByID2("02-11-40-1-6@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("02-11-40-1-8@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
            }

            #endregion

            #region  Сохранение

            _swApp.IActivateDoc2(ModelName, false, 0);

            swDoc.Extension.SelectByID2("Расстояние5", "MATE", 0, 0, 0, false, 0, null, 0);
            swDoc.ActivateSelectedFeature();
            swDoc.Extension.SelectByID2("D1@Расстояние5@" + ModelName.Replace(".SLDASM", ""), "DIMENSION", 0, 0, 0, true, 0, null, 0);
            ((Dimension)(swDoc.Parameter("D1@Расстояние5"))).SystemValue = ((Convert.ToDouble(height) - 80) / 1000);

            swDoc = ((ModelDoc2)(_swApp.ActiveDoc));

            GabaritsForPaintingCamera(swDoc);

            swDoc.ForceRebuild3(true);
            swDoc.SaveAs2(framelessBlockNewPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
            NewComponents.Add(new FileInfo(framelessBlockNewPath));
            _swApp.CloseDoc(new FileInfo(framelessBlockNewPath).Name);
            CheckInOutPdm(NewComponents, true, Settings.Default.TestPdmBaseName);
            
            #endregion

            return framelessBlockNewPath;
        }

        #endregion
    }
}
