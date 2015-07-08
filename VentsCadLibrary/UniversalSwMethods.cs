using EdmLib;
using MakeDxfUpdatePartData;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace VentsCadLibrary
{
    public partial class VentsCad
    {
        static bool IsConvertToInt(IEnumerable<string> newStringParams)
        {
            foreach (var param in newStringParams)
            {
                try
                {
                    // ReSharper disable once UnusedVariable
                    var y = Convert.ToInt32(param);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        internal static void GetLastVersionAsmPdm(string path, string pdmBase)
        {            
            try
            {
                LoggerInfo(string.Format("Получение последней версии по пути {0}\nБаза - {1}", path, pdmBase), "", "GetLastVersionPdm");
                var vault1 = new EdmVault5();
                IEdmFolder5 oFolder;
                vault1.LoginAuto(pdmBase, 0);
                var edmFile5 = vault1.GetFileFromPath(path, out oFolder);
                edmFile5.GetFileCopy(0, 0, oFolder.ID, (int)EdmGetFlag.EdmGet_RefsVerLatest);
            }
            catch (Exception exception)
            {
                LoggerError(string.Format("Во время получения последней версии по пути {0} возникла ошибка!\nБаза - {1}. {2}", path, pdmBase, exception.Message), exception.StackTrace, "GetLastVersionPdm");
            }
        }

        internal bool InitializeSw(bool visible)
        {
            try
            {
                _swApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
            }
            catch (Exception)
            {
                _swApp = new SldWorks { Visible = visible };
            }
            return _swApp != null;
        }

        static void DelEquations(int index, IModelDoc2 swModel)
        {
            try
            {
                LoggerInfo(string.Format("Удаление уравнения #{0} в модели {1}", index, swModel.GetPathName()), "", "DelEquations");
                var myEqu = swModel.GetEquationMgr();
                myEqu.Delete(index);
                swModel.EditRebuild3();                
            }
            catch (Exception exception)
            {
                LoggerError(string.Format("Удаление уравнения #{0} в модели {1}. {2}", index, swModel.GetPathName(), exception.Message), exception.StackTrace, "DelEquations");
            }
        }

        internal static void CheckInOutPdm(List<FileInfo> filesList, bool registration, string pdmBase)
        {
            foreach (var file in filesList)
            {
                try
                {
                    var vault1 = new EdmVault5();
                    IEdmFolder5 oFolder;
                    vault1.LoginAuto(pdmBase, 0);
                    var attempts = 1;
                m1:
                    Thread.Sleep(300);
                    var edmFile5 = vault1.GetFileFromPath(file.FullName, out oFolder);
                    if (oFolder == null)
                    {
                        attempts++;
                        if (attempts > 20)
                        {
                            goto m2;
                        }
                        goto m1;
                    }
                m2:
                    // Разрегистрировать
                    if (registration == false)
                    {
                        edmFile5.GetFileCopy(0, 0, oFolder.ID, (int)EdmGetFlag.EdmGet_Simple);
                        edmFile5.LockFile(oFolder.ID, 0);
                    }
                    // Зарегистрировать
                    if (registration)
                    {
                        edmFile5.UnlockFile(oFolder.ID, "");
                    }

                    LoggerInfo(string.Format("В базе PDM - {1}, зарегестрирован документ по пути {0}", file.FullName, pdmBase), "", "CheckInOutPdm");
                }
                catch (Exception exception)
                {
                    LoggerError(string.Format("Во время регистрации документа по пути {0} возникла ошибка\nБаза - {1}. {2}", file.FullName, pdmBase, exception.Message), "", "CheckInOutPdm");
                }
            }
        }

        public void PartInfoToXml(string filePath)
        {
            try
            {
                if (filePath == "") return;
                var extension = Path.GetExtension(filePath);
                if (extension == null) return;
                if (extension.ToLower() != ".sldprt") return;
                var @class = new MakeDxfExportPartDataClass
                {
                    PdmBaseName = VaultName
                };
                bool isErrors;
                string newEdrwFileName;
                @class.CreateFlattPatternUpdateCutlistAndEdrawing(filePath, out newEdrwFileName, out isErrors, false, false, true);
                if (!isErrors)
                {
                    LoggerInfo("Закончена обработка " + Path.GetFileName(filePath), "", "PartInfoToXml");
                }
                else
                {
                    CheckInOutPdm(new List<FileInfo> { new FileInfo(newEdrwFileName) }, true, VaultName);
                    LoggerError("Закончена обработка детали " + Path.GetFileName(filePath) + " с ошибками", "",
                        "PartInfoToXml");
                }
            }
            catch (Exception exception)
            {
                LoggerError("Ошибка: " + exception.StackTrace, GetHashCode().ToString("X"), "OnTaskRun");
            }
        }

        static void GabaritsForPaintingCamera(IModelDoc2 swmodel)
        {
            try
            {
                const long valueset = 1000;
                const int swDocPart = 1;
                const int swDocAssembly = 2;

                for (int i = 0; i < swmodel.GetConfigurationCount(); i++)
                {
                    i = i + 1;
                    var configname = swmodel.IGetConfigurationNames(ref i);
                    
                    Configuration swConf = swmodel.GetConfigurationByName(configname);
                    if (swConf.IsDerived()) continue;                    
                    swmodel.EditRebuild3();

                    switch (swmodel.GetType())
                    {
                        case swDocPart:
                            {                                
                                var part = (PartDoc)swmodel;
                                var box = part.GetPartBox(true);

                                swmodel.AddCustomInfo3(configname, "Длина", 30, "");
                                swmodel.AddCustomInfo3(configname, "Ширина", 30, "");
                                swmodel.AddCustomInfo3(configname, "Высота", 30, "");
                                
                                swmodel.CustomInfo2[configname, "Длина"] =
                                    Convert.ToString(Math.Round(Convert.ToDecimal((long)(Math.Abs(box[0] - box[3]) * valueset)), 0));
                                swmodel.CustomInfo2[configname, "Ширина"] =
                                    Convert.ToString(Math.Round(Convert.ToDecimal((long)(Math.Abs(box[1] - box[4]) * valueset)), 0));
                                swmodel.CustomInfo2[configname, "Высота"] =
                                    Convert.ToString(Math.Round(Convert.ToDecimal((long)(Math.Abs(box[2] - box[5]) * valueset)), 0));

                            }
                            break;
                        case swDocAssembly:
                            {
                                var swAssy = (AssemblyDoc)swmodel;
                                var boxAss = swAssy.GetBox((int)swBoundingBoxOptions_e.swBoundingBoxIncludeRefPlanes);

                                swmodel.AddCustomInfo3(configname, "Длина", 30, "");
                                swmodel.AddCustomInfo3(configname, "Ширина", 30, "");
                                swmodel.AddCustomInfo3(configname, "Высота", 30, "");

                                swmodel.CustomInfo2[configname, "Длина"] =
                                    Convert.ToString(Math.Round(Convert.ToDecimal((long)(Math.Abs(boxAss[0] - boxAss[3]) * valueset)), 0));
                                swmodel.CustomInfo2[configname, "Ширина"] =
                                    Convert.ToString(Math.Round(Convert.ToDecimal((long)(Math.Abs(boxAss[1] - boxAss[4]) * valueset)), 0));
                                swmodel.CustomInfo2[configname, "Высота"] =
                                    Convert.ToString(Math.Round(Convert.ToDecimal((long)(Math.Abs(boxAss[2] - boxAss[5]) * valueset)), 0));
                            }
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
