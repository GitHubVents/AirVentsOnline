﻿using EdmLib;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace VentsCadLibrary
{
    public partial class VentsCad
    {
        public string ConnectionToSql { get; set; }       

        public string VaultName { get; set; }

        public string DestVaultName { get; set; }
        
        static string LocalPath(string vault)
        {
            try
            {
                var edmVault5 = new EdmVault5();
                edmVault5.LoginAuto(vault, 0);
                return edmVault5.RootFolder.LocalPath;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
                LoggerError(string.Format("В базе - {1}, не удалось получить корнувую папку ({0})", exception.Message, vault), "", " LocalPath(string Vault)");
                return null;
            }
        }
        
        SldWorks _swApp;

        public List<VentsCadFiles> NewComponents = new List<VentsCadFiles>();

        #region Fields

        /// <summary>
        ///  Папка с исходной моделью "Вибровставки". 
        /// </summary>
        private const string SpigotFolder = @"\Библиотека проектирования\DriveWorks\12 - Spigot";

        /// <summary>
        ///  Папка для сохранения компонентов "Вибровставки". 
        /// </summary>
        private const string SpigotDestinationFolder = @"Проекты\Blauberg\12 - Вибровставка";

        #endregion
        

        public void Spigot(string type, string width, string height, out string newFile)
        {
            newFile = null;

            if (!IsConvertToInt(new[] { width, height })) return;    

            string modelName = null;

            switch (type)
            {
                case "20":
                    modelName = "12-20";
                    break;
                case "30":
                    modelName = "12-30";
                    break;
            }

            if (string.IsNullOrEmpty(modelName)) return;
            
            var sourceFolder = LocalPath(VaultName);
            var destinationFolder = LocalPath(DestVaultName);

            var newSpigotName = modelName + "-" + width + "-" + height;
            var newSpigotPath = $@"{destinationFolder}\{SpigotDestinationFolder}\{newSpigotName}";
            
            if (ExistInBaseSpigot(newSpigotName, null, null, null, null, null))
            {
                newFile = newSpigotPath + ".SLDDRW";
                return;
            }                       

            var drawing = "12-00";
            if (modelName == "12-30")
            { drawing = modelName; }
            Dimension myDimension;
            var modelSpigotDrw = $@"{sourceFolder}{SpigotFolder}\{drawing}.SLDDRW";

            GetLastVersionAsmPdm(modelSpigotDrw, VaultName);            

            if (!InitializeSw(true)) return;                       

            var swDrwSpigot = _swApp.OpenDoc6(modelSpigotDrw, (int)swDocumentTypes_e.swDocDRAWING,
                (int)swOpenDocOptions_e.swOpenDocOptions_LoadModel, "", 0, 0);

            if (swDrwSpigot == null) return;
            
            ModelDoc2 swDoc = _swApp.ActivateDoc2("12-00", false, 0);
            var swAsm = (AssemblyDoc)swDoc;
            swAsm.ResolveAllLightWeightComponents(false);

            switch (modelName)
            {
                case "12-20":
                    DelEquations(5, swDoc);
                    DelEquations(4, swDoc);
                    DelEquations(3, swDoc);
                    break;
                case "12-30":
                    DelEquations(0, swDoc);
                    DelEquations(0, swDoc);
                    DelEquations(0, swDoc);
                    break;
            }
            swDoc.ForceRebuild3(true);

            string newPartName;
            string newPartPath;
            IModelDoc2 swPartDoc;

            #region Удаление ненужного

            if (type == "20")
            {
                const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed +
                                         (int)swDeleteSelectionOptions_e.swDelete_Children;
                swDoc.Extension.SelectByID2("12-30-001-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-30-001-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-30-002-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-30-002-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.002-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.002-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.002-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.002-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-30-001-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-30-001-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-30-002-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-30-002-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.002-5@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.002-6@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.002-7@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.002-8@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-003-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Клей-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Клей-2@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.Extension.DeleteSelection2(deleteOption);
            }
            if (type == "30")
            {               
                const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed +
                                         (int)swDeleteSelectionOptions_e.swDelete_Children;
                swDoc.Extension.SelectByID2("12-20-001-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-20-001-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-20-002-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-20-002-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.001-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.001-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.001-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.001-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-20-001-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-20-001-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-20-002-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-20-002-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.001-5@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.001-6@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.001-7@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.001-8@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-003-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Клей-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.DeleteSelection2(deleteOption);
            }

            swDoc.Extension.SelectByID2("30", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
            swDoc.EditDelete();
            swDoc.Extension.SelectByID2("20", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
            swDoc.EditDelete();

            #endregion

            #region Сохранение и изменение элементов

            var addDimH = 1;
            if (modelName == "12-30")
            {
                addDimH = 10;
            }

            var w = (Convert.ToDouble(width) - 1) / 1000;
            var h = Convert.ToDouble((Convert.ToDouble(height) + addDimH) / 1000);
            const double step = 50;
            var weldW = Convert.ToDouble((Math.Truncate(Convert.ToDouble(width) / step) + 1));
            var weldH = Convert.ToDouble((Math.Truncate(Convert.ToDouble(height) / step) + 1));

            if (modelName == "12-20")
            {
                //12-20-001
                _swApp.IActivateDoc2("12-20-001", false, 0);
                swPartDoc = _swApp.IActiveDoc2;
                newPartName = $"12-20-{height}.SLDPRT";
                newPartPath = $@"{destinationFolder}\{SpigotDestinationFolder}\{newPartName}";
                
                if (ExistInBaseSpigot(newPartName, null, null, null, null, null))
                {
                    swDoc = ((ModelDoc2)(_swApp.ActivateDoc2("12-00.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("12-20-001-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", true, true);
                    _swApp.CloseDoc("12-20-001.SLDPRT");
                }
                else
                {
                    swDoc.Extension.SelectByID2("D1@Вытянуть1@12-20-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D1@Вытянуть1@12-20-001.Part")));
                    myDimension.SystemValue = h - 0.031;
                    swDoc.Extension.SelectByID2("D1@Кривая1@12-20-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D1@Кривая1@12-20-001.Part")));
                    myDimension.SystemValue = weldH;
                    swPartDoc.SaveAs2(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                    NewComponents.Add(new VentsCadFiles
                    {
                        LocalPartFileInfo = newPartPath
                    });
                    _swApp.CloseDoc(newPartName);
                }

                //12-20-002
                _swApp.IActivateDoc2("12-20-002", false, 0);
                swPartDoc = _swApp.IActiveDoc2;
                newPartName = $"12-20-{width}.SLDPRT";
                newPartPath = $@"{destinationFolder}\{SpigotDestinationFolder}\{newPartName}";
                if (ExistInBaseSpigot(newPartName, null, null, null, null, null))
                {
                    swDoc = ((ModelDoc2)(_swApp.ActivateDoc2("12-00.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("12-20-002-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", true, true);
                    _swApp.CloseDoc("12-20-002.SLDPRT");
                }
                else
                {
                    swDoc.Extension.SelectByID2("D1@Вытянуть1@12-20-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D1@Вытянуть1@12-20-002.Part")));
                    myDimension.SystemValue = w - 0.031;
                    swDoc.Extension.SelectByID2("D1@Кривая1@12-20-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D1@Кривая1@12-20-002.Part")));
                    myDimension.SystemValue = weldW;
                    swPartDoc.SaveAs2(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                    NewComponents.Add(new VentsCadFiles
                    {
                        LocalPartFileInfo = newPartPath
                    });
                    _swApp.CloseDoc(newPartName);
                }

                //12-003
                _swApp.IActivateDoc2("12-003", false, 0);
                swPartDoc = _swApp.IActiveDoc2;
                newPartName = $"12-03-{width}-{height}.SLDPRT";
                newPartPath = $@"{destinationFolder}\{SpigotDestinationFolder}\{newPartName}";
                if (ExistInBaseSpigot(newPartName, null, null, null, null, null))
                {
                    swDoc = ((ModelDoc2)(_swApp.ActivateDoc2("12-00.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("12-003-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", true, true);
                    _swApp.CloseDoc("12-003.SLDPRT");
                }
                else
                {
                    swDoc.Extension.SelectByID2("D3@Эскиз1@12-003-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D3@Эскиз1@12-003.Part")));
                    myDimension.SystemValue = w;
                    swDoc.Extension.SelectByID2("D2@Эскиз1@12-003-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D2@Эскиз1@12-003.Part")));
                    myDimension.SystemValue = h;
                    swDoc.EditRebuild3();
                    swPartDoc.SaveAs2(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                    NewComponents.Add(new VentsCadFiles
                    {
                        LocalPartFileInfo = newPartPath
                    });
                    _swApp.CloseDoc(newPartName);
                }
            }

            if (modelName == "12-30")
            {
                //12-30-001
                _swApp.IActivateDoc2("12-30-001", false, 0);
                swPartDoc = _swApp.IActiveDoc2;
                newPartName = $"12-30-{height}.SLDPRT";
                newPartPath = $@"{destinationFolder}\{SpigotDestinationFolder}\{newPartName}";
                if (ExistInBaseSpigot(newPartName, null, null, null, null, null))
                {
                    swDoc = ((ModelDoc2)(_swApp.ActivateDoc2("12-00.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("12-30-001-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", true, true);
                    _swApp.CloseDoc("12-30-001.SLDPRT");
                }
                else
                {
                    swDoc.Extension.SelectByID2("D1@Вытянуть1@12-30-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D1@Вытянуть1@12-30-001.Part")));
                    myDimension.SystemValue = h - 0.031;
                    swDoc.Extension.SelectByID2("D1@Кривая1@12-30-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D1@Кривая1@12-30-001.Part")));
                    myDimension.SystemValue = weldH;
                    swPartDoc.SaveAs2(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                    NewComponents.Add(new VentsCadFiles
                    {
                        LocalPartFileInfo = newPartPath
                    });
                    _swApp.CloseDoc(newPartName);
                }

                //12-30-002
                _swApp.IActivateDoc2("12-30-002", false, 0);
                swPartDoc = _swApp.IActiveDoc2;
                newPartName = $"12-30-{width}.SLDPRT";
                newPartPath = $@"{destinationFolder}\{SpigotDestinationFolder}\{newPartName}";
                if (ExistInBaseSpigot(newPartName, null, null, null, null, null))
                {
                    swDoc = ((ModelDoc2)(_swApp.ActivateDoc2("12-00.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("12-30-002-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", true, true);
                    _swApp.CloseDoc("12-30-002.SLDPRT");
                }
                else
                {
                    swDoc.Extension.SelectByID2("D1@Вытянуть1@12-30-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D1@Вытянуть1@12-30-002.Part")));
                    myDimension.SystemValue = w - 0.031;
                    swDoc.Extension.SelectByID2("D1@Кривая1@12-30-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D1@Кривая1@12-30-002.Part")));
                    myDimension.SystemValue = weldH;
                    swPartDoc.SaveAs2(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                    NewComponents.Add(new VentsCadFiles
                    {
                        LocalPartFileInfo = newPartPath
                    });
                    _swApp.CloseDoc(newPartName);
                }

                //12-003
                _swApp.IActivateDoc2("12-003", false, 0);
                swPartDoc = _swApp.IActiveDoc2;
                newPartName = $"12-03-{width}-{height}.SLDPRT";
                newPartPath = $@"{destinationFolder}\{SpigotDestinationFolder}\{newPartName}";
                if (ExistInBaseSpigot(newPartName, null, null, null, null, null))
                {
                    swDoc = ((ModelDoc2)(_swApp.ActivateDoc2("12-00.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("12-003-2@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", true, true);
                    _swApp.CloseDoc("12-003.SLDPRT");
                }
                else
                {
                    swDoc.Extension.SelectByID2("D3@Эскиз1@12-003-2@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D3@Эскиз1@12-003.Part")));
                    myDimension.SystemValue = w;
                    swDoc.Extension.SelectByID2("D2@Эскиз1@12-003-2@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D2@Эскиз1@12-003.Part")));
                    myDimension.SystemValue = h;
                    swDoc.EditRebuild3();
                    swPartDoc.SaveAs2(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                    NewComponents.Add(new VentsCadFiles
                    {
                        LocalPartFileInfo = newPartPath
                    });
                    _swApp.CloseDoc(newPartName);
                }
            }

            #endregion

            GabaritsForPaintingCamera(swDoc);

            swDoc.ForceRebuild3(true);
            swDoc.SaveAs2(newSpigotPath + ".SLDASM", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
            _swApp.CloseDoc(newSpigotName + ".SLDASM");
            NewComponents.Add(new VentsCadFiles
            {
                LocalPartFileInfo = newSpigotPath + ".SLDASM"
            });            
            swDrwSpigot.Extension.SelectByID2("DRW1", "SHEET", 0, 0, 0, false, 0, null, 0);
            var drw = (DrawingDoc)(_swApp.IActivateDoc3(drawing + ".SLDDRW", true, 0));
            drw.ActivateSheet("DRW1");
            var m = 5;
            if (Convert.ToInt32(width) > 500 || Convert.ToInt32(height) > 500){ m = 10; }
            if (Convert.ToInt32(width) > 850 || Convert.ToInt32(height) > 850){ m = 15; }
            if (Convert.ToInt32(width) > 1250 || Convert.ToInt32(height) > 1250){ m = 20; }
            drw.SetupSheet5("DRW1", 12, 12, 1, m, true, destinationFolder + @"\Vents-PDM\\Библиотека проектирования\\Templates\\Основные надписи\\A3-A-1.slddrt", 0.42, 0.297, "По умолчанию", false);            
            var errors = 0;
            var warnings = 0;

            swDrwSpigot.SaveAs4(newSpigotPath + ".SLDDRW", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, ref errors, ref warnings);
            
            NewComponents.Add(new VentsCadFiles
            {
                LocalPartFileInfo = newSpigotPath + ".SLDDRW"
            });
            
            //_swApp.CloseDoc(Path.GetFileNameWithoutExtension(new FileInfo(newSpigotPath + ".SLDDRW").FullName) + " - DRW1");

            _swApp.CloseDoc(newSpigotPath);
            _swApp.ExitApp();
            _swApp = null;

            List<VentsCadFiles> newFilesList;
            CheckInOutPdm(NewComponents, true, DestVaultName, out newFilesList);

            var text = "";

            foreach (var item in newFilesList)
            {
                var typeFile = 0;
                if (item.LocalPartFileInfo.ToUpper().Contains(".SLDASM")) { typeFile = 1; }
                if (item.LocalPartFileInfo.ToUpper().Contains(".SLDPRT")) { typeFile = 0; }
                var fileNameWithoutExtension = item.PartName.Remove(item.PartName.LastIndexOf('.'));

                ExistInBaseSpigot(fileNameWithoutExtension, item.PartIdPdm, typeFile, Convert.ToInt32(type), Convert.ToInt32(height), Convert.ToInt32(width));

                text = text + "\n PartIdPdm - " + item.PartIdPdm + " PartName - " + item.PartName;
            }

            //MessageBox.Show(text);

            foreach (var newComponent in NewComponents)
            {
               PartInfoToXml(newComponent.LocalPartFileInfo);
            }

            newFile = newSpigotPath + ".SLDDRW";

        }
    }
}
