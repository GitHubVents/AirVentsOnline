using EdmLib;
using MakeDxfUpdatePartData;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

            public string PartWithoutExtension { get { return LocalPartFileInfo.Substring(LocalPartFileInfo.LastIndexOf('\\')); }  }

            public int PartIdPdm { get; set; }

            public string LocalPartFileInfo { get; set; }
        }
        
        public void CheckInOutPdm(List<VentsCadFiles> filesList, bool registration, string pdmBase, out List<VentsCadFiles> newFilesList)
        {
            vault1.LoginAuto("Tets_debag", 0);

            BatchAddFiles(filesList);            

            try
            {
                BatchUnLock(filesList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }                                   

            newFilesList = new List<VentsCadFiles>();                      

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
                    LoggerError(string.Format("Во время регистрации документа по пути {0} возникла ошибка\nБаза - {1}. {2}", file.LocalPartFileInfo, pdmBase, exception.Message), "", "CheckInOutPdm");
                }
                finally
                {
                    string FileName;
                    int FileIdPdm;
                    GetIdPdm(file.LocalPartFileInfo, out FileName, out FileIdPdm);
                    newFilesList.Add(new VentsCadFiles
                    {
                        PartName = FileName,
                        PartIdPdm = FileIdPdm,
                        LocalPartFileInfo = file.LocalPartFileInfo,
                        PartIdSql = file.PartIdSql
                    });
                }
            }
        }

        public bool ExistInBase(string fileName, int? idPdm, int? fileType, int? typeOfSpigot, int? height, int? width)
        {
            var status = false;
            using (var con = new SqlConnection(ConnectionToSQL))
            {
                try
                {
                    con.Open();

                    var sqlCommand = new SqlCommand("AirVents.Spigot", con) { CommandType = CommandType.StoredProcedure };
                    var sqlParameter = sqlCommand.Parameters;

                    if (fileName == null)
                    {
                        sqlParameter.AddWithValue("@Filename", DBNull.Value);
                    }
                    else
                    {
                        sqlParameter.AddWithValue("@Filename", fileName);
                    }

                    if (idPdm == null)
                    {
                        sqlParameter.AddWithValue("@IDPDM", DBNull.Value);
                    }
                    else
                    {
                        sqlParameter.AddWithValue("@IDPDM", idPdm);
                    }

                    if (fileType == null)
                    {
                        sqlParameter.AddWithValue("@FileType", DBNull.Value);
                    }
                    else
                    {
                        sqlParameter.AddWithValue("@FileType", fileType);
                    }

                    if (typeOfSpigot == null)
                    {
                        sqlParameter.AddWithValue("@Type", DBNull.Value);
                    }
                    else
                    {
                        sqlParameter.AddWithValue("@Type", typeOfSpigot);
                    }

                    if (height == null)
                    {
                        sqlParameter.AddWithValue("@Hight", DBNull.Value);
                    }
                    else
                    {
                        sqlParameter.AddWithValue("@Hight", height);
                    }

                    if (width == null)
                    {
                        sqlParameter.AddWithValue("@Width", DBNull.Value);
                    }
                    else
                    {
                        sqlParameter.AddWithValue("@Width", width);
                    }                                       

                    sqlCommand.Parameters.Add("@status", SqlDbType.Bit);
                    sqlCommand.Parameters["@status"].Direction = ParameterDirection.ReturnValue;
                    sqlCommand.ExecuteNonQuery();

                    status = Convert.ToBoolean(sqlCommand.Parameters["@status"].Value);

                    MessageBox.Show(string.Format("fileName - {0} idPdm - {1} fileType - {2} typeOfSpigot - {3} height - {4} width - {5}", fileName, idPdm, fileType, typeOfSpigot, height, width));
                    MessageBox.Show(status ? "Деталь есть в базе" : "Детали нет в базе", fileName);
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
            // todo
            return status;
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

                foreach (var file in filesList)
                {
                    //MessageBox.Show(directoryName);

                    //MessageBox.Show(file.LocalPartFileInfo, "file.LocalPartFileInfo");
                    //MessageBox.Show(file.PartName, "file.PartName");
                    //MessageBox.Show(file.PartWithoutExtension, "file.PartWithoutExtension");

                    var directoryName = file.LocalPartFileInfo.Replace(file.PartWithoutExtension, "");

                    //MessageBox.Show(directoryName, "directoryName");

                    poAdder.AddFileFromPathToPath(file.LocalPartFileInfo, directoryName, 0);

                    //poAdder.AddFileFromPathToPath(file.LocalPartFileInfo.FullName, file.LocalPartFileInfo.DirectoryName + "\\", 0);
                    //poAdder.AddFileFromPathToPath(@"C:\12-20-1977.SLDPRT", @"E:\Tets_debag\", 0);
                }
                poAdder.CommitAdd(0, null, 0);               
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "BatchAddFiles");
            }            
        }
                
        public void BatchUnLock(List<VentsCadFiles> filesList)
        {
            batchUnlocker = (IEdmBatchUnlock2)vault1.CreateUtility(EdmUtility.EdmUtil_BatchUnlock);
            int i = 0;
            EdmSelItem[] ppoSelection = new EdmSelItem[filesList.Count];
            foreach (var file in filesList)
            {
                aFile = vault1.GetFileFromPath(file.LocalPartFileInfo, out ppoRetParentFolder);                
                aPos = aFile.GetFirstFolderPosition();
                aFolder = aFile.GetNextFolder(aPos);

                ppoSelection[i] = new EdmSelItem();
                ppoSelection[i].mlDocID = aFile.ID;
                ppoSelection[i].mlProjID = aFolder.ID;
                i++;
            }

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
            }
        }

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
