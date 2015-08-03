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

        //const string ModelName = "01 - Frameless Design 40mm.SLDASM";
        const string ModelName = "Frameless Design 40mm.SLDASM";
        
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
        /// <param name="съемныеПанели">The СЪЕМНЫЕ ПАНЕЛИ.</param>
        /// <param name="промежуточныеСтойки">The ПРОМЕЖУТОЧНЫЕ СТОЙКИ.</param>
        /// <param name="height">The height.</param>
        public void FramelessBlock(string size, string order, string side, string section, string pDown, string pFixed, string pUp, string[] съемныеПанели, string[] промежуточныеСтойки, string height)
        {
            Логгер.Информация(String.Format("Бескаркасная установка {0}-{1}  {2}", size, order, section), "", "FramelessBlock", "FramelessBlock");
            
            if (!InitializeSw(true)) return;

            var path = FramelessBlockStr(size, order, side, section, pDown, pFixed, pUp, съемныеПанели, промежуточныеСтойки, height);

            if (path == "") return;

            if (MessageBox.Show(
                string.Format("Модель находится по пути:\n {0}\n Открыть модель?", new FileInfo(path).Directory),
                string.Format(" {0} ", Path.GetFileNameWithoutExtension(new FileInfo(path).FullName)),
                MessageBoxButton.YesNoCancel) != MessageBoxResult.Yes) return;
            
            try
            {
                CloseSldAsm(pDown);
                CloseSldAsm(pFixed);
                CloseSldAsm(pUp);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            
            #region to delete
            //_swApp.CloseDoc(Path.GetFileName(pDown));
            //_swApp.CloseDoc(Path.GetFileName(pFixed));
            //_swApp.CloseDoc(Path.GetFileName(pUp));
            #endregion

            foreach (var панель in съемныеПанели)
            {
                try
                {
                    CloseSldAsm(панель);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }               

                #region to delete
                //_swApp.CloseDoc(Path.GetFileName(панель));    
                #endregion
            }

            FramelessBlockStr(size, order, side, section, pDown, pFixed, pUp, съемныеПанели, промежуточныеСтойки, height);
            Логгер.Информация("Блок згенерирован", "" , "FramelessBlock", "FramelessBlock");
        }

        void CloseSldAsm(string pDown)
        {
            var fileName = Path.GetFileName(pDown);
            var namePrt = fileName != null && fileName.ToLower().Contains(".sldasm")
                ? Path.GetFileName(pDown)
                : Path.GetFileName(pDown) + ".sldasm";
            _swApp.CloseDoc(namePrt);
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
        /// <param name="промежуточныеСтойки">The ПРОМЕЖУТОЧНЫЕ СТОЙКИ.</param>
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
            string[] промежуточныеСтойки,
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

            var framelessBlockNewPath =  new FileInfo(String.Format(@"{0}\{1}.SLDASM",
                orderFolder,
                framelessBlockNewName)).FullName;

            if (File.Exists(framelessBlockNewPath))
            {
                GetLastVersionPdm(new FileInfo(framelessBlockNewPath).FullName, Settings.Default.TestPdmBaseName);
                _swApp.OpenDoc6(framelessBlockNewPath, (int)swDocumentTypes_e.swDocASSEMBLY, (int)swOpenDocOptions_e.swOpenDocOptions_LoadModel, "00", 0, 0);
                return framelessBlockNewPath;
            }

            var pdmFolder = Settings.Default.SourceFolder;

            //var components = new[]
            //{
            //    unitAsMmodel,
            //    String.Format(@"{0}{1}{2}", pdmFolder, @"\Проекты\AirVents\AirVents 30\ВНС-901.92.000\","ВНС-901.92.001.SLDPRT"),
            //    String.Format(@"{0}{1}{2}", pdmFolder, @"\Проекты\Blauberg\10 - Рама монтажная\", "10-3-980-700.SLDASM"),
            //    String.Format(@"{0}{1}{2}", pdmFolder, @"\Библиотека проектирования\Прочие изделия\Крепежные изделия\Замки и ручки\", "M8-Panel block-one side.SLDPRT"),
            //    String.Format(@"{0}{1}{2}", pdmFolder, @"\Библиотека проектирования\Стандартные изделия\", "Threaded Rivets с насечкой.SLDPRT")
            //};

            //GetLastVersionPdm(components, Settings.Default.PdmBaseName);

            GetLatestVersionAsmPdm(unitAsMmodel, Settings.Default.PdmBaseName);

            var swDoc = _swApp.OpenDoc6(unitAsMmodel, (int)swDocumentTypes_e.swDocASSEMBLY, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", 0, 0);
            _swApp.Visible = true;
            var swAsm = (AssemblyDoc)swDoc;
            swAsm.ResolveAllLightWeightComponents(false);

            #endregion
            
            #region Метизы

            switch (панелиСъемные[3])
            {
                case "Со скотчем":
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-11@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-8@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-12@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-7@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-1@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-2@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    break;
                default:
                    swDoc.Extension.SelectByID2("Threaded Rivets-11@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets-8@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets-12@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets-7@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets-1@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets-2@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    break;
            }
            
            switch (side)
            {
                case "левая":

                    #region Верхняя и нижняя панели
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-18@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-53@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-53@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Право", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    #endregion

                    #region Передняя и задняя панели

                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-74@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-39@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-75@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-40@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-40@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
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
                        swDoc.Extension.SelectByID2("Третья панель право", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditDelete();
                        swDoc.Extension.SelectByID2("SC GOST 17473_gost-11@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("Washer 11371_gost-41@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("Threaded Rivets Increased-11@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("Threaded Rivets-11@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("Threaded Rivets-11@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditDelete();

                        if (String.IsNullOrEmpty(панелиСъемные[1]))
                        {
                            // Удаление 2-й панели
                            swDoc.Extension.SelectByID2("Вторая панель право", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                            swDoc.EditDelete();
                            swDoc.Extension.SelectByID2("SC GOST 17473_gost-7@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            swDoc.Extension.SelectByID2("Washer 11371_gost-37@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            swDoc.Extension.SelectByID2("Threaded Rivets Increased-7@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            swDoc.Extension.SelectByID2("Threaded Rivets-7@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            swDoc.Extension.SelectByID2("Threaded Rivets-7@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                            swDoc.EditDelete();
                        }
                    }

                    swDoc.Extension.SelectByID2("Washer 11371_gost-38@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-8@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-8@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();

                    swDoc.Extension.SelectByID2("Вторая панель лево", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("SC GOST 17473_gost-8@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditDelete();

                    swDoc.Extension.SelectByID2("Washer 11371_gost-38@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-8@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.ShowConfiguration2("Рифленая клепальная гайка М8");
                    swDoc.Extension.SelectByID2("Threaded Rivets-8@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Вторая панель лево", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("SC GOST 17473_gost-12@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets-12@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-12@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Washer 11371_gost-42@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Третья панель лево", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("SC GOST 17473_gost-2@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets-2@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-2@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Washer 11371_gost-32@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
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
                        swDoc.Extension.SelectByID2("Третья панель лево", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditDelete();
                        swDoc.Extension.SelectByID2("SC GOST 17473_gost-12@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("Threaded Rivets-12@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("Threaded Rivets Increased-12@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("Washer 11371_gost-42@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("Washer 11371_gost-42@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditDelete();

                        if (String.IsNullOrEmpty(панелиСъемные[1]))
                        {
                            // Удаление 2-й панели
                            swDoc.Extension.SelectByID2("Вторая панель лево", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                            swDoc.EditDelete();
                            swDoc.Extension.SelectByID2("SC GOST 17473_gost-8@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            swDoc.Extension.SelectByID2("Washer 11371_gost-38@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            swDoc.Extension.SelectByID2("Threaded Rivets Increased-8@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            swDoc.Extension.SelectByID2("Threaded Rivets-8@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            swDoc.Extension.SelectByID2("Threaded Rivets-8@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                            swDoc.EditDelete();
                        }
                    }

                    swDoc.Extension.SelectByID2("SC GOST 17473_gost-1@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-1@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Washer 11371_gost-31@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets-1@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets-1@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Первая панель право", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("SC GOST 17473_gost-7@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Washer 11371_gost-37@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-7@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets-7@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Вторая панель право", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("SC GOST 17473_gost-11@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Washer 11371_gost-41@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets Increased-11@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Threaded Rivets-11@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
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

                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-37@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-2@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-2@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Лево", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();

                    #endregion

                    #region Передняя и задняя панели

                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-37@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-72@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-73@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-38@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-38@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
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

            #region Панели || панелиСъемные[0, 1, 2, 3] - Съемные панели || панелиСъемные[4, 5] - Усиливающие панели

            #region Сторона обслуживания

            var panelLeft1 = side != "правая" ? панельНесъемная : панелиСъемные[0];
            var panelRight1 = side != "правая" ? панелиСъемные[0] : панельНесъемная;

            var panelLeft2 = панелиСъемные.Length > 1 ? панелиСъемные[1] : "-";
            var panelLeft3 = панелиСъемные.Length > 2 ? панелиСъемные[2] : "-";

            var panelRight2 = панелиСъемные.Length > 1 ? панелиСъемные[1] : "-";
            var panelRight3 = панелиСъемные.Length > 2 ? панелиСъемные[2] : "-";

            #endregion

            if (panelLeft2 == "-")
            {
                swDoc.Extension.SelectByID2("Вторая панель право зерк", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);swDoc.EditSuppress2();
                swDoc.Extension.SelectByID2("Вторая панель право массив", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);swDoc.EditSuppress2();
            }

            if (panelLeft3 == "-")
            {
                swDoc.Extension.SelectByID2("Третья панель право зерк", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);swDoc.EditSuppress2();
                swDoc.Extension.SelectByID2("Третья панель право массив", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);swDoc.EditSuppress2();
            }

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

            if (panelLeft2 != "-")
            {
                swDoc.Extension.SelectByID2("02-11-40-1-5@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0,
                    false, 0, null, 0);
                if (side != "правая")
                {
                    swAsm.ReplaceComponents(panelLeft2, null, false, true);
                }
                else
                {
                    swDoc.EditDelete();
                }

                swDoc.Extension.SelectByID2("02-11-40-1-7@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0,
                    false, 0, null, 0);

                if (side == "правая")
                {
                    swAsm.ReplaceComponents(panelRight2, null, false, true);
                }
                else
                {
                    swDoc.EditDelete();
                }
            }
            else
            {
                swDoc.Extension.SelectByID2("02-11-40-1-5@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0,
                    false, 0, null, 0);swDoc.EditDelete();
                swDoc.Extension.SelectByID2("02-11-40-1-7@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0,
                    false, 0, null, 0);swDoc.EditDelete();
            }

            // Если три съемных панели

            if (panelLeft3 != "-")
            {
                swDoc.Extension.SelectByID2("02-11-40-1-6@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                if (side != "правая")
                {
                    swAsm.ReplaceComponents(panelLeft3, null, false, true);
                }
                else
                {
                    swDoc.EditDelete();
                }
                
                swDoc.Extension.SelectByID2("02-11-40-1-8@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                if (side == "правая")
                {
                    swAsm.ReplaceComponents(panelRight3, null, false, true);
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

            #region Промежуточные стойки

            #region Первая

            if (промежуточныеСтойки[0] != "-" || промежуточныеСтойки[0] != "05")
            {
                //MessageBox.Show(промежуточныеСтойки[1]);

                swDoc.Extension.SelectByID2("02-11-08-40--1@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                if (side != "правая")
                {
                    swAsm.ReplaceComponents(промежуточныеСтойки[0], null, false, true);
                }
                else
                {
                    swDoc.EditDelete();
                }

                swDoc.Extension.SelectByID2("02-11-08-40--5@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                if (side == "правая")
                {
                    swAsm.ReplaceComponents(промежуточныеСтойки[0], null, false, true);
                }
                else
                {
                    swDoc.EditDelete();
                }
            }
            else
            {
                swDoc.Extension.SelectByID2("02-11-08-40--1@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);swDoc.EditDelete();
                swDoc.Extension.SelectByID2("02-11-08-40--5@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);swDoc.EditDelete();
            }

            if (промежуточныеСтойки[0] == "-" || string.IsNullOrEmpty(промежуточныеСтойки[0]))
            {
                swDoc.Extension.SelectByID2("02-11-08-40--1@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);swDoc.EditDelete();
                swDoc.Extension.SelectByID2("02-11-08-40--5@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);swDoc.EditDelete();
            }

            #endregion

            #region Вторая

            if (промежуточныеСтойки[1] != "-" || промежуточныеСтойки[1] != "05")
            {
                //MessageBox.Show(промежуточныеСтойки[1]);

                swDoc.Extension.SelectByID2("02-11-08-40--2@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                if (side != "правая")
                {
                    swAsm.ReplaceComponents(промежуточныеСтойки[1], "", false, true);
                }
                else
                {
                    swDoc.EditDelete();
                }

                swDoc.Extension.SelectByID2("02-11-08-40--6@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                if (side == "правая")
                {
                    swAsm.ReplaceComponents(промежуточныеСтойки[1], "", false, true);
                }
                else
                {
                    swDoc.EditDelete();
                }
            }
            else
            {
                swDoc.Extension.SelectByID2("02-11-08-40--2@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("02-11-08-40--6@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
            }

            if (промежуточныеСтойки[1] == "-" || string.IsNullOrEmpty(промежуточныеСтойки[1]))
            {
                swDoc.Extension.SelectByID2("02-11-08-40--2@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("02-11-08-40--6@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
            }

            #endregion

            #region Третья

            if (промежуточныеСтойки[2] != "-" || промежуточныеСтойки[2] != "05")
            {
               
                //MessageBox.Show(промежуточныеСтойки[2]);

                swDoc.Extension.SelectByID2("02-11-08-40--3@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                if (side != "правая")
                {
                    swAsm.ReplaceComponents(промежуточныеСтойки[2], "", false, true);
                }
                else
                {
                    swDoc.EditDelete();
                }

                swDoc.Extension.SelectByID2("02-11-08-40--7@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                if (side == "правая")
                {
                    swAsm.ReplaceComponents(промежуточныеСтойки[2], "", false, true);
                }
                else
                {
                    swDoc.EditDelete();
                }
            }
            else
            {
                swDoc.Extension.SelectByID2("02-11-08-40--3@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("02-11-08-40--7@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
            }

            if (промежуточныеСтойки[2] == "-" || string.IsNullOrEmpty(промежуточныеСтойки[2]))
            {
                swDoc.Extension.SelectByID2("02-11-08-40--3@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("02-11-08-40--7@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
            }

            #endregion

            #region Четвертая

            if (промежуточныеСтойки[3] != "-" || промежуточныеСтойки[3] != "05")
            {

                //MessageBox.Show(промежуточныеСтойки[2]);

                swDoc.Extension.SelectByID2("02-11-08-40--4@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                if (side != "правая")
                {
                    swAsm.ReplaceComponents(промежуточныеСтойки[3], "", false, true);
                }
                else
                {
                    swDoc.EditDelete();
                }

                swDoc.Extension.SelectByID2("02-11-08-40--8@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                if (side == "правая")
                {
                    swAsm.ReplaceComponents(промежуточныеСтойки[3], "", false, true);
                }
                else
                {
                    swDoc.EditDelete();
                }
            }
            else
            {
                swDoc.Extension.SelectByID2("02-11-08-40--4@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("02-11-08-40--8@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
            }

            if (промежуточныеСтойки[3] == "-" || string.IsNullOrEmpty(промежуточныеСтойки[3]))
            {
                swDoc.Extension.SelectByID2("02-11-08-40--4@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("02-11-08-40--8@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
            }

            //MessageBox.Show(@" 1 - " + промежуточныеСтойки[0] + "\n 2 - " + промежуточныеСтойки[1] + "\n 3 - " + промежуточныеСтойки[2] + "\n 4 - " + промежуточныеСтойки[3]);

            #endregion

            #endregion

            #region  Сохранение

            swDoc.Extension.SelectByID2("17-AV06-3C-L-3@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);swDoc.EditDelete();
            swDoc.Extension.SelectByID2("16-AV06-2H-1@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);swDoc.EditDelete();
            
            //MessageBox.Show("Сохранение");

            _swApp.IActivateDoc2(ModelName, true, 0);
            swDoc.ForceRebuild3(false);

            #region Услиливающие панели

            if (!string.IsNullOrEmpty(панелиСъемные[4]))
            {
                swDoc.Extension.SelectByID2("02-11-40-1-9@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                if (side == "правая")
                {
                    swAsm.ReplaceComponents(панелиСъемные[4], "", false, true);

                    swDoc.Extension.SelectByID2("ПУВ9", "FTRFOLDER", 0, 0, 0, false, 0, null, 0); swDoc.EditUnsuppress2();
                    swDoc.Extension.SelectByID2("ПУ9", "FTRFOLDER", 0, 0, 0, false, 0, null, 0); swDoc.EditUnsuppress2();

                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-158@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditUnsuppress2();
                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-159@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditUnsuppress2();
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-123@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditUnsuppress2();
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-124@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditUnsuppress2();

                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-159@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditUnsuppress2();
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-123@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditUnsuppress2();
                }
                else
                {
                    swDoc.EditDelete();

                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-158@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-159@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-123@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-124@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-159@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-123@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("ПУВ9", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("ПУ9", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();

                    swDoc.Extension.SelectByID2("МПУ9", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); swDoc.EditSuppress2();
                    swDoc.Extension.SelectByID2("МПУВ9", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); swDoc.EditSuppress2();
                }

                swDoc.Extension.SelectByID2("02-11-40-1-11@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                if (side != "правая")
                {
                    swAsm.ReplaceComponents(панелиСъемные[4], "", false, true);

                    swDoc.Extension.SelectByID2("ПУВ11", "FTRFOLDER", 0, 0, 0, false, 0, null, 0); swDoc.EditUnsuppress2();
                    swDoc.Extension.SelectByID2("ПУ11", "FTRFOLDER", 0, 0, 0, false, 0, null, 0); swDoc.EditUnsuppress2();

                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-138@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditUnsuppress2();
                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-139@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditUnsuppress2();
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-103@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditUnsuppress2();
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-104@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditUnsuppress2();

                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-180@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditUnsuppress2();
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-145@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditUnsuppress2();
                }
                else
                {
                    swDoc.EditDelete();

                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-138@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-139@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-103@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-104@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-180@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-145@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("ПУВ11", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("ПУ11", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();

                    swDoc.Extension.SelectByID2("МПУ11", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); swDoc.EditSuppress2();
                    swDoc.Extension.SelectByID2("МПУВ11", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); swDoc.EditSuppress2();
                }
            }
            if (string.IsNullOrEmpty(панелиСъемные[4]))
            {
                swDoc.Extension.SelectByID2("02-11-40-1-9@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("02-11-40-1-11@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();

                swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-158@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-159@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-123@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-124@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-159@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-123@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ПУВ9", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("ПУ9", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();

                swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-138@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-139@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-103@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-104@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-180@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-145@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ПУВ11", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("ПУ11", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();

                swDoc.Extension.SelectByID2("МПУ9", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); swDoc.EditSuppress2();
                swDoc.Extension.SelectByID2("МПУВ9", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); swDoc.EditSuppress2();
                swDoc.Extension.SelectByID2("МПУ11", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); swDoc.EditSuppress2();
                swDoc.Extension.SelectByID2("МПУВ11", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); swDoc.EditSuppress2();
            }

            if (!string.IsNullOrEmpty(панелиСъемные[5]))
            {
                swDoc.Extension.SelectByID2("02-11-40-1-10@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                if (side == "правая")
                {
                    swAsm.ReplaceComponents(панелиСъемные[5], "", false, true);

                    swDoc.Extension.SelectByID2("ПУВ10", "FTRFOLDER", 0, 0, 0, false, 0, null, 0); swDoc.EditUnsuppress2();
                    swDoc.Extension.SelectByID2("ПУ10", "FTRFOLDER", 0, 0, 0, false, 0, null, 0); swDoc.EditUnsuppress2();

                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-168@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditUnsuppress2();
                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-169@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditUnsuppress2();
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-133@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditUnsuppress2();
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-134@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditUnsuppress2();

                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-179@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditUnsuppress2();
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-144@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditUnsuppress2();
                }
                else
                {
                    swDoc.EditDelete();

                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-168@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-169@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-133@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-134@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-179@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-144@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("ПУВ10", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("ПУ10", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();

                    swDoc.Extension.SelectByID2("МПУ10", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); swDoc.EditSuppress2();
                    swDoc.Extension.SelectByID2("МПУВ10", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); swDoc.EditSuppress2();
                }

                swDoc.Extension.SelectByID2("02-11-40-1-12@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                if (side != "правая")
                {
                    swAsm.ReplaceComponents(панелиСъемные[5], "", false, true);

                    swDoc.Extension.SelectByID2("ПУВ12", "FTRFOLDER", 0, 0, 0, false, 0, null, 0); swDoc.EditUnsuppress2();
                    swDoc.Extension.SelectByID2("ПУ12", "FTRFOLDER", 0, 0, 0, false, 0, null, 0); swDoc.EditUnsuppress2();

                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-148@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditUnsuppress2();
                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-149@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditUnsuppress2();
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-113@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditUnsuppress2();
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-114@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditUnsuppress2();

                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-181@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditUnsuppress2();
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-146@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditUnsuppress2();
                }
                else
                {
                    swDoc.EditDelete();

                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-148@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-149@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-113@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-114@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-181@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-146@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("ПУВ12", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("ПУ12", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();

                    swDoc.Extension.SelectByID2("МПУ12", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); swDoc.EditSuppress2();
                    swDoc.Extension.SelectByID2("МПУВ12", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); swDoc.EditSuppress2();
                }
            }
            else if (string.IsNullOrEmpty(панелиСъемные[5]))
            {
                swDoc.Extension.SelectByID2("02-11-40-1-10@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("02-11-40-1-12@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();

                swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-168@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-169@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-133@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-134@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-179@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-144@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ПУВ10", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("ПУ10", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();

                swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-148@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-149@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-113@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-114@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("Заглушка пластикова для т.о. 16х13 сір.-181@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Винт саморез DIN 7504 K-146@" + ModelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ПУВ12", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("ПУ12", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();

                swDoc.Extension.SelectByID2("МПУ10", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); swDoc.EditSuppress2();
                swDoc.Extension.SelectByID2("МПУВ10", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); swDoc.EditSuppress2();
                swDoc.Extension.SelectByID2("МПУ12", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); swDoc.EditSuppress2();
                swDoc.Extension.SelectByID2("МПУВ12", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); swDoc.EditSuppress2();
            }

            #endregion

            swDoc.Extension.SelectByID2("Расстояние5", "MATE", 0, 0, 0, false, 0, null, 0);
            swDoc.ActivateSelectedFeature();
            swDoc.Extension.SelectByID2("D1@Расстояние5@" + ModelName.Replace(".SLDASM", ""), "DIMENSION", 0, 0, 0, true, 0, null, 0);
            ((Dimension)(swDoc.Parameter("D1@Расстояние5"))).SystemValue = ((Convert.ToDouble(height) - 80) / 1000);

            swDoc = ((ModelDoc2)(_swApp.ActiveDoc));
            //MessageBox.Show("GabaritsForPaintingCamera");
            GabaritsForPaintingCamera(swDoc);
            //MessageBox.Show("ForceRebuild3");
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
