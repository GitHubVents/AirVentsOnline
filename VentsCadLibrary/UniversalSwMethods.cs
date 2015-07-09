using EdmLib;
using MakeDxfUpdatePartData;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

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

        internal void GetLastVersionAsmPdm(string path, string pdmBase)
        {            
            try
            {
                LoggerInfo(string.Format("Получение последней версии по пути {0}\nБаза - {1}", path, pdmBase), "", "GetLastVersionPdm");
                var vaultSource = new EdmVault5();
                IEdmFolder5 oFolder;
                vaultSource.LoginAuto(pdmBase, 0);
                var edmFile5 = vaultSource.GetFileFromPath(path, out oFolder);
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

        public class VentsCadFiles
        {
            public int PartIdSql { get; set; }

            public string PartName { get; set; }

            public int PartIdPdm { get; set; }

            public FileInfo LocalPartFileInfo { get; set; }
        }
        
        public void CheckInOutPdm(List<VentsCadFiles> filesList, bool registration, string pdmBase, out List<VentsCadFiles> newFilesList)
        {
            vault1.LoginAuto("Tets_debag", 0);

           // BatchAddFiles(filesList);
           // MessageBox.Show("Файлы добавлены?");

            //BatchUnLock(filesList);

            BatchUnLock();
           // MessageBox.Show("Файл зарегистрирован");
                       

            newFilesList = new List<VentsCadFiles>();

            return;

            foreach (var file in filesList)
            {                
                try
                {
                    #region
                    //    var vault1 = new EdmVault5();
                    //    IEdmFolder5 oFolder;
                    //    vault1.LoginAuto("Tets_debag", 0);
                    //    var attempts = 1;
                    //m1:
                    //    Thread.Sleep(350);
                    //    var edmFile5 = vault1.GetFileFromPath(file.LocalPartFileInfo.FullName, out oFolder);

                    //   // MessageBox.Show(file.LocalPartFileInfo.FullName);

                    //    if (oFolder == null)
                    //    {
                    //        attempts++;
                    //        if (attempts > 25)
                    //        {
                    //            MessageBox.Show(file.LocalPartFileInfo.FullName, "Не взят");
                    //            goto m2;
                    //        }
                    //        goto m1;
                    //    }
                    //m2:
                    //    // Разрегистрировать
                    //    if (registration == false)
                    //    {
                    //        edmFile5.GetFileCopy(0, 0, oFolder.ID, (int)EdmGetFlag.EdmGet_Simple);
                    //        edmFile5.LockFile(oFolder.ID, 0);
                    //    }
                    //    // Зарегистрировать
                    //    if (registration)
                    //    {
                    //        edmFile5.UnlockFile(oFolder.ID, "");
                    //    }                    
                    //    LoggerInfo(string.Format("В базе PDM - {1}, зарегестрирован документ по пути {0}", file.LocalPartFileInfo.FullName, pdmBase), "", "CheckInOutPdm");
                    #endregion
                }
                catch (Exception exception)
                {
                    LoggerError(string.Format("Во время регистрации документа по пути {0} возникла ошибка\nБаза - {1}. {2}", file.LocalPartFileInfo.FullName, pdmBase, exception.Message), "", "CheckInOutPdm");
                }
                finally
                {
                    string FileName;
                    int FileIdPdm;
                    GetIdPdm(file.LocalPartFileInfo.FullName, out FileName, out FileIdPdm);
                    newFilesList.Add(new VentsCadFiles
                    {
                        PartName = FileName,
                        PartIdPdm = FileIdPdm,
                        LocalPartFileInfo = new FileInfo(file.LocalPartFileInfo.FullName),
                        PartIdSql = file.PartIdSql
                    });                  
                }
            }
        }

        public void PartInfoToXml(string filePath)
        {
            //try
            //{
            //    if (filePath == "") return;
            //    var extension = Path.GetExtension(filePath);
            //    if (extension == null) return;
            //    if (extension.ToLower() != ".sldprt") return;
            //    var @class = new MakeDxfExportPartDataClass
            //    {
            //        PdmBaseName = VaultName
            //    };
            //    bool isErrors;
            //    string newEdrwFileName;
            //    @class.CreateFlattPatternUpdateCutlistAndEdrawing(filePath, out newEdrwFileName, out isErrors, false, false, true);
            //    if (!isErrors)
            //    {
            //        LoggerInfo("Закончена обработка " + Path.GetFileName(filePath), "", "PartInfoToXml");
            //    }
            //    else
            //    {
            //        var List = new List<VentsCadFiles>();
            //        CheckInOutPdm(new List<VentsCadFiles> { new VentsCadFiles { LocalPartFileInfo = new FileInfo(newEdrwFileName) }  }, true, VaultName, out List);
            //        LoggerError("Закончена обработка детали " + Path.GetFileName(filePath) + " с ошибками", "", "PartInfoToXml");
            //    }
            //}
            //catch (Exception exception)
            //{
            //    LoggerError("Ошибка: " + exception.StackTrace, GetHashCode().ToString("X"), "OnTaskRun");
            //}
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
            catch (Exception){}
        }

        public EdmVault5 vault1 = new EdmVault5();
        

        public void BatchAddFiles(List<VentsCadFiles> filesList)
        {
            try
            {
                poAdder = (IEdmBatchAdd2)vault1.CreateUtility(EdmUtility.EdmUtil_BatchAdd);
                //var newFolder = vault.RootFolderPath + "\\Новая папка (2)";                
                //poAdder.AddFolderPath(newFolder, 0, (int)EdmBatchAddFolderFlag.Ebaff_Nothing);

             //   MessageBox.Show(vault1.Name);
                //poAdder.AddFileFromPathToPath(file.LocalPartFileInfo.FullName, file.LocalPartFileInfo.DirectoryName + "\\", 0);
                poAdder.AddFileFromPathToPath(@"C:\12-20-1977.SLDPRT", @"E:\Tets_debag\", 0);
                poAdder.AddFileFromPathToPath(@"C:\12-20-1978.SLDPRT", @"E:\Tets_debag\", 0);
                poAdder.AddFileFromPathToPath(@"C:\12-20-1979.SLDPRT", @"E:\Tets_debag\", 0);
                poAdder.AddFileFromPathToPath(@"C:\12-20-1980.SLDPRT", @"E:\Tets_debag\", 0);
                poAdder.AddFileFromPathToPath(@"C:\12-20-1981.SLDPRT", @"E:\Tets_debag\", 0);
                poAdder.AddFileFromPathToPath(@"C:\12-20-1982.SLDPRT", @"E:\Tets_debag\", 0);
                poAdder.AddFileFromPathToPath(@"C:\12-20-1983.SLDPRT", @"E:\Tets_debag\", 0);

                //foreach (var file in filesList)
                //{
                //    MessageBox.Show(vault1.Name);
                //    //poAdder.AddFileFromPathToPath(file.LocalPartFileInfo.FullName, file.LocalPartFileInfo.DirectoryName + "\\", 0);
                //    poAdder.AddFileFromPathToPath(@"C:\12-20-1977.SLDPRT", @"E:\Tets_debag\", 0);                    
                //}

                poAdder.CommitAdd(0, null, 0);
                
                #region
                //var edmFileInfo = new EdmFileInfo[0];

                //var ppoAddedFiles = new EdmFileInfo[10];
                //poAdder.CommitAdd(0, ref ppoAddedFiles);

                ////Display the returned information in a message box

                //int idx;
                //idx = ppoAddedFiles.GetLowerBound(0);
                //string msg = "";

                //while (idx <= ppoAddedFiles.GetUpperBound(0))
                //{
                //    string row = null;
                //    row = "(" + ppoAddedFiles[idx].mbsPath + ") arg = " + Convert.ToString(ppoAddedFiles[idx].mlArg);

                //    if (ppoAddedFiles[idx].mhResult == 0)
                //    {
                //        row = row + " status = OK ";
                //    }
                //    else
                //    {
                //        string oErrName = "";
                //        string oErrDesc = "";

                //        vault1.GetErrorString(ppoAddedFiles[idx].mhResult, out oErrName, out oErrDesc);
                //        row = row + " status = " + oErrName;
                //    }

                //    idx = idx + 1;
                //    msg = msg + Constants.vbLf + row;

                //}

                //Interaction.MsgBox(msg);
                #endregion
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.StackTrace);   
            }            
        }

        public void BatchUnLock()
        {
            var vault1 = new EdmVault5();
            vault1.LoginAuto("Tets_debag", 0);
            batchUnlocker = (IEdmBatchUnlock2)vault1.CreateUtility(EdmUtility.EdmUtil_BatchUnlock);
            EdmSelItem[] ppoSelection = new EdmSelItem[1];

            //aFile = vault1.GetFileFromPath(file.LocalPartFileInfo.FullName, out ppoRetParentFolder);
            //MessageBox.Show(@"E:\Tets_debag\12-20-19" + num + ".SLDPRT");
            

                aFile = vault1.GetFileFromPath(@"E:\Tets_debag\12-20-1977.SLDPRT", out ppoRetParentFolder);
            aPos = aFile.GetFirstFolderPosition();
            aFolder = aFile.GetNextFolder(aPos);

            ppoSelection[0] = new EdmSelItem();
            ppoSelection[0].mlDocID = aFile.ID;
            ppoSelection[0].mlProjID = aFolder.ID;
        

            // Add selections to the batch of files to check in
            batchUnlocker.AddSelection(vault1, ppoSelection);

            if ((batchUnlocker != null))
            {
                batchUnlocker.CreateTree(0, (int)EdmUnlockBuildTreeFlags.Eubtf_ShowCloseAfterCheckinOption + (int)EdmUnlockBuildTreeFlags.Eubtf_MayUnlock);

                fileList = (IEdmSelectionList6)batchUnlocker.GetFileList((int)EdmUnlockFileListFlag.Euflf_GetUnlocked + (int)EdmUnlockFileListFlag.Euflf_GetUndoLocked + (int)EdmUnlockFileListFlag.Euflf_GetUnprocessed);
                aPos = fileList.GetHeadPosition();

                while (!(aPos.IsNull))
                {
                    fileList.GetNext2(aPos, out poSel);
                }

                batchUnlocker.UnlockFiles(0, null);
                //batchUnlocker.ShowDlg(this.Handle.ToInt32());
            }
        }

        //public void BatchUnLock()
        //{
        //    batchUnlocker = (IEdmBatchUnlock2)vault1.CreateUtility(EdmUtility.EdmUtil_BatchUnlock);            
        //    EdmSelItem[] ppoSelection = new EdmSelItem[6];
        //    for (int i = 0; i < 6; i++)
        //    {
        //        int num = 77 + i;
        //        //aFile = vault1.GetFileFromPath(file.LocalPartFileInfo.FullName, out ppoRetParentFolder);
        //        //MessageBox.Show(@"E:\Tets_debag\12-20-19" + num + ".SLDPRT");

        //        aFile = vault1.GetFileFromPath(@"E:\Tets_debag\12-20-19" + num + ".SLDPRT", out ppoRetParentFolder);
        //        aPos = aFile.GetFirstFolderPosition();
        //        aFolder = aFile.GetNextFolder(aPos);

        //        ppoSelection[i] = new EdmSelItem();
        //        ppoSelection[i].mlDocID = aFile.ID;
        //        ppoSelection[i].mlProjID = aFolder.ID;               
        //    }

        //    // Add selections to the batch of files to check in
        //    batchUnlocker.AddSelection(vault1, ppoSelection);

        //    if ((batchUnlocker != null))
        //    {
        //        batchUnlocker.CreateTree(0, (int)EdmUnlockBuildTreeFlags.Eubtf_ShowCloseAfterCheckinOption + (int)EdmUnlockBuildTreeFlags.Eubtf_MayUnlock);
        //        fileList = (IEdmSelectionList6)batchUnlocker.GetFileList((int)EdmUnlockFileListFlag.Euflf_GetUnlocked + (int)EdmUnlockFileListFlag.Euflf_GetUndoLocked + (int)EdmUnlockFileListFlag.Euflf_GetUnprocessed);
        //        aPos = fileList.GetHeadPosition();

        //        while (!(aPos.IsNull))
        //        {
        //            fileList.GetNext2(aPos, out poSel);
        //        }
        //    }
        //}



        //public void BatchUnLock(List<VentsCadFiles> filesList)
        //{   
        //    batchUnlocker = (IEdmBatchUnlock2)vault1.CreateUtility(EdmUtility.EdmUtil_BatchUnlock);
        //    int i = 0;
        //    EdmSelItem[] ppoSelection = new EdmSelItem[filesList.Count];
        //    foreach (var file in filesList)
        //    {
        //        //aFile = vault1.GetFileFromPath(file.LocalPartFileInfo.FullName, out ppoRetParentFolder);
        //        aFile = vault1.GetFileFromPath(@"E:\Tets_debag\12-20-1977.SLDPRT", out ppoRetParentFolder);
        //        aPos = aFile.GetFirstFolderPosition();
        //        aFolder = aFile.GetNextFolder(aPos);                            

        //        ppoSelection[i] = new EdmSelItem();
        //        ppoSelection[i].mlDocID = aFile.ID;
        //        ppoSelection[i].mlProjID = aFolder.ID;
        //        i++;
        //    }

        //    // Add selections to the batch of files to check in
        //    batchUnlocker.AddSelection(vault1, ppoSelection);

        //    if ((batchUnlocker != null))
        //    {
        //        batchUnlocker.CreateTree(0, (int)EdmUnlockBuildTreeFlags.Eubtf_ShowCloseAfterCheckinOption + (int)EdmUnlockBuildTreeFlags.Eubtf_MayUnlock);
        //        fileList = (IEdmSelectionList6)batchUnlocker.GetFileList((int)EdmUnlockFileListFlag.Euflf_GetUnlocked + (int)EdmUnlockFileListFlag.Euflf_GetUndoLocked + (int)EdmUnlockFileListFlag.Euflf_GetUnprocessed);
        //        aPos = fileList.GetHeadPosition();

        //        while (!(aPos.IsNull))
        //        {
        //            fileList.GetNext2(aPos, out poSel);
        //        }
        //    }
        //}

        internal void GetIdPdm(string path, out string FileName, out int FileIdPdm)
        {
            FileName = null;
            FileIdPdm = 0;
            try
            {                
                //LoggerInfo(string.Format("Получение последней версии по пути {0}\nБаза - {1}", path, pdmBase), "", "GetLastVersionPdm");               
                IEdmFolder5 oFolder;              
                int tries = 1;
                m1:
                Thread.Sleep(500);
                var edmFile5 = vault1.GetFileFromPath(path, out oFolder);
                if (oFolder == null)
                {
                    tries++;
                    if (tries > 20)
                    {
                        return;
                    }
                    goto m1;
                }
                edmFile5.GetFileCopy(0, 0, oFolder.ID, (int)EdmGetFlag.EdmGet_RefsVerLatest);
                FileName = edmFile5.Name;
                FileIdPdm = edmFile5.ID;
            }
            catch (Exception exception)
            {
                //LoggerError(string.Format("Во время получения последней версии по пути {0} возникла ошибка!\nБаза - {1}. {2}", path, pdmBase, exception.Message), exception.StackTrace, "GetLastVersionPdm");
            }
        }
    }
}
