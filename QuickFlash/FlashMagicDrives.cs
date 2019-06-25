using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using static FileTools;
namespace QuickFlash
{
    public partial class MainWindow
    {
        public void createFileStructure(string driveLetter)
        {
            List<string> fileLocations = new List<string>();
            //create File Structer of files/folders
            foreach (var row in bioses_data)
            {
                string type = row[5].ToString(),
                    thumbdriveFolderStructure = "",
                    filePath = row[8].ToString(),
                    taskID = row[2].ToString(),
                    version = row[7].ToString(),
                    TATask = row[8].ToString(),
                    manufacturer = row[4].ToString(),
                    files = row[9].ToString();
                List<string> filesToRun = new List<string>();
                files = files.Replace("\"", "");
                files = files.Replace("]", "");
                files = files.Replace("[", "");
                filesToRun.AddRange(files.Split(','));
                List<string> SKUS = new List<string>();
                foreach (var task in bios_task_and_products_data)
                {
                    try
                    {
                        //get data, skips if data is broken
                        string SKU = task[2].ToString();
                        //if task ID's Match
                        if (task[0].Equals(taskID))
                        {
                            //add sku
                            SKUS.Add(SKU);
                        }

                    }
                    catch (Exception E)
                    {
                        if (fullLogButton.Checked) console.Text += "Exception Caught: " + E.ToString() + "\n";
                    }
                }
                foreach (string SKU in SKUS) {
                    switch (type.ToUpper())
                    {

                        case ("DOS"):
                            thumbdriveFolderStructure = driveLetter+"\\"+type + '\\' + taskID;
                            generateSKUToBIOSFile(filePath,type, SKU, taskID, TATask, manufacturer, version, filesToRun, driveLetter);
                            break;
                        case ("INSTANT"):
                            string ID = row[0].ToString();
                            string notes = row[10].ToString();
                            thumbdriveFolderStructure = driveLetter + "\\"+type + '\\' + ID + '_' + version + '_' + notes+"\\";
                            foreach (string file in filesToRun)
                            {
                                FileInfo thumbdriveCurrentFile = new FileInfo(thumbdriveFolderStructure + file);

                                if (!thumbdriveCurrentFile.Exists)
                                {

                                    string[] copy = { "del " + thumbdriveFolderStructure, "xcopy \"" + filePath + "\\*" + "\" " + "\"" + thumbdriveFolderStructure + "\"  /h /i /y /c" };
                                    createFile(file.Split('.')[0] + "_" + driveLetter.Remove(1) + ".bat", copy);
                                    runBATFile(file.Split('.')[0] + "_" + driveLetter.Remove(1) + ".bat", false);
                                }
                                else
                                {
                                    if (!compareFiles(thumbdriveFolderStructure + file, filePath + "\\" + file))
                                    {
                                        string[] copy = { "xcopy \"" + filePath + "\\" + file + "\" " + "\"" + thumbdriveFolderStructure + "\"  /h /i /y /c" };
                                        createFile(file.Split('.')[0] + "_" + driveLetter.Remove(1) + ".bat", copy);
                                        runBATFile(file.Split('.')[0] + "_" + driveLetter.Remove(1) + ".bat", false);
                                    }
                                }
                            }
                            break;
                        case ("EFI"):
                            thumbdriveFolderStructure = driveLetter + "\\"+type + '\\' + taskID;
                            generateSKUToBIOSFile(filePath,type, SKU, taskID, TATask, manufacturer, version, filesToRun,driveLetter);
                            break;
                    }

                }
            }

        }

        public void generateSKUToBIOSFile(string filePath,string type, string SKU, string taskID, string TATask, string manufacturer, string version,List<string> filesToRun,string driveLetter)
        {
            string[] userInterface= { };
            string[] runFiles= { };
            string extension="";
            
            switch (type)
            {
                case ("EFI"):
                    extension = ".nsh";
                    userInterface = new string[]{
                        "@echo -off", 
                        "cls", 
                        "echo =======================================================================", 
                        "echo \"LOGIC SUPPLY BIOS UTILITY\"", 
                        "echo \"\"",
                        "echo \""+TATask+"\"",
                        "echo \"Manufacturer:"+ manufacturer+"\"", 
                        "echo \"Product:"+SKU+"\"",
                        "echo \"Version: "+version+"\"", 
                        "echo \"\"", 
                        "echo \"If this is the correct BIOS, press ENTER. Otherwise, press Q\"", 
                        "echo =======================================================================", 
                        "pause", 
                        "echo \"\"",  
                        "echo \"\"", 
                        "echo Loading BIOS confirmation window...", 
                        "echo \"\"",  
                        "cd "+taskID,
                        taskID+".nsh", 
                        "echo \"\"", 
                        "echo \"\"", 
                        ":END", 
                        ":END"
                    };
                    List<string> nshCreation = new List<string>();
                    nshCreation.Add("@echo -off");
                    nshCreation.Add("cls");
                    nshCreation.Add("echo =======================================================================");
                    nshCreation.Add("echo \"LOGIC SUPPLY BIOS UTILITY\"");
                    nshCreation.Add("echo \"\"");
                    nshCreation.Add("echo \"Task Name: "+ TATask + "\"");
                    nshCreation.Add("echo \"Manufacturer:"+ manufacturer+"\"");
                    nshCreation.Add("echo \"Version:"+ version+"\"");
                    nshCreation.Add("echo \"\"");
                    nshCreation.Add("echo ==----------Update-Script-Details-------------==");
                    foreach(string file in filesToRun)
                    {
                        nshCreation.Add("type "+file);
                    }
                    nshCreation.Add("echo \"\"");
                    nshCreation.Add("echo ==--------------------------------------------==");
                    nshCreation.Add("echo \"\"");
                    nshCreation.Add("echo \"\"");
                    nshCreation.Add("echo \"If you don't want to update, press 'q', else press any key to update!\"");
                    nshCreation.Add("echo =======================================================================");
                    nshCreation.Add("pause");
                    nshCreation.Add("echo \"\"");
                    nshCreation.Add("echo \"\"");
                    nshCreation.Add("echo Starting EFI Firmware update...");
                    nshCreation.Add("echo \"\"");
                    foreach (string file in filesToRun)
                    {
                        nshCreation.Add(file);
                    }
                    nshCreation.Add("echo \"\"");
                    nshCreation.Add("echo \"\"");
                    nshCreation.Add("echo \"\"");
                    nshCreation.Add(":END");
                    nshCreation.Add(":END");
                    nshCreation.Add("echo "+ version);
                    runFiles = new string[nshCreation.Count];
                    for (int i= 0;i < runFiles.Length; i ++){
                        runFiles[i] = nshCreation[i];
                    }
                    break;

                case ("DOS"):
                    extension = ".bat";
                    userInterface = new string[]{
                        "@ECHO OFF",
                        "CLS",
                        "ECHO.",
                        "ECHO =======================================================================",
                        "ECHO      LOGIC SUPPLY BIOS UTILITY",
                        "ECHO.",
                        "ECHO      TA Task: "+TATask,
                        "ECHO      Manufacturer: "+manufacturer,
                        "ECHO      Product: "+SKU,
                        "ECHO      BIOS Version: "+version,
                        "ECHO =======================================================================",
                        "ECHO.",
                        "pause",
                        "cd "+taskID,
                        taskID+".bat",
                    };
                    runFiles = new string[filesToRun.Count+1];
                    for(int i = 0; i < filesToRun.Count; i++)
                    {
                        runFiles[i] = filesToRun[i];
                    }
                    runFiles[filesToRun.Count] = "rem " + version;
                    magicDrivesWorker.ReportProgress(0, SKU);
                    break;
            }
            string mainDirectoryPath = SKU.Split('.')[0] + "_" + driveLetter.Remove(1)
                , drivePath = driveLetter + "\\" + type + "\\";
            createFile( taskID+ extension, runFiles);
            createFile(SKU + extension, userInterface);
            string[] copy = { "xcopy \"" + SKU + extension + "\" " + "\"" + drivePath + "\"  /h /i /y /c" };
            createFile(SKU.Split('.')[0] + "_" + driveLetter.Remove(1) + ".bat", copy);
            runBATFile(SKU.Split('.')[0] + "_" + driveLetter.Remove(1) + ".bat", false);
           
            List<string> currentRunningFile = getFilesLines(driveLetter + "\\" + type + "\\" + taskID + "\\" + taskID + extension);
            string thumbdriveVersion = currentRunningFile[currentRunningFile.Count - 1];
            try
            {
                thumbdriveVersion = thumbdriveVersion.Split(' ')[1];
            }catch(IndexOutOfRangeException)
            {
                thumbdriveVersion = "NULL";
            }
            if (!thumbdriveVersion.Equals(version))
            {
                copy = new string[] { "xcopy \"" + filePath + "\\*\" " + "\"" + drivePath +taskID+ "\\"+ "\" /e /h /i /y /c",
                                        "xcopy \""+ workingDirectory+ taskID + extension+ "\" " + "\"" + driveLetter + "\\" + type + "\\" + taskID + "\\" + "\" /h /i /y /c"};
                createFile(taskID + "_" + driveLetter.Remove(1) + ".bat", copy);
                runBATFile(taskID + "_" + driveLetter.Remove(1) + ".bat", false);
            }

            //return fileData;

        }

        /*Flash Magic Drives
         *
         * Moves necessary files to magic drives, DOES NOT format them, in other words
         * this will not create new magic drives, just updates current ones. this is done to save immense time since
         * creating a new one everytime would require formatting and diskpart doesnt like running in multiple instances 
         * therefor the slowdown would be immense
         * 
         * Furthermore the directory is manually placed into the preferences file (pref.txt)
         * this is to prevent tampering as theoretically this directory will never change
         * But does prompt the user if either the current folder does not exist or the preferences file is not saved
         * 
         * */
        private void flashMagicFlashDrives(object sender, DoWorkEventArgs e)
        {
            int progress = 5;
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            magicDrivesWorker.ReportProgress(progress, "Gathering Magic Flash Drives...");
            int drive = 0;
            foreach (DriveInfo D in allDrives)
            {
                //determine if thumbdrive
                if (D.DriveType.ToString().Equals("Removable"))
                {
                    //gather info            
                    string driveLetter = D.Name.ToString().Remove(2);
                    magicDrivesWorker.ReportProgress(progress, "loading Magic Drive " + driveLetter + " with necessary components...");
                    //loading drive with components


                    createFileStructure(driveLetter);
                    //List<string> excludedFiles = new List<string>();
                    //List<string> driveFiles = getFilePath(driveLetter + @"\");
                    //foreach (string file in driveFiles)
                    //{
                    //    if (compareFiles(file, magicDrivePath + @"\" + file.Remove(0, 2)))
                    //    {
                    //        magicDrivesWorker.ReportProgress(progress, magicDrivePath + @"\" + file.Remove(0, 2));
                    //        string[] fullFilePath = file.Split('\\');
                    //        excludedFiles.Add(fullFilePath[fullFilePath.Length - 1]);
                    //        magicDrivesWorker.ReportProgress(progress, fullFilePath[fullFilePath.Length - 1]);
                    //    }
                    //}
                    //loadDrive(driveLetter, "INSTANT", drive.ToString());
                    //if (fullLogButton.Checked) magicDrivesWorker.ReportProgress(progress, "moving files from " + fullPath + " ...");
                    //string[] cmd = { @"xcopy " + '"' + magicDrivePath + @"\" + "*" + '"' + " " + driveLetter + @"\ /e /h /i /y /c /EXCLUDE:exclude" + driveLetter.Split(':')[0] + ".txt > log" + driveLetter.Split(':')[0] + ".txt" };
                    //createFile("copy" + driveLetter.Split(':')[0] + ".bat", cmd);
                    //string[] excludedFilesArray = new string[excludedFiles.Count];
                    //for (int i = 0; i < excludedFiles.Count; i++)
                    //{
                    //    excludedFilesArray[i] = excludedFiles[i];
                    //}
                    //createFile("exclude" + driveLetter.Split(':')[0] + ".txt", excludedFilesArray);
                    //runBATFile("copy" + driveLetter.Split(':')[0] + ".bat", false);

                }
                drive++;
            }
            progress += 5;
            magicDrivesWorker.ReportProgress(progress, "Waiting for all files to transfer...");
            //finishing up, waits for cmd to stop proccessing
            waitForFinish(progress, magicDrivesWorker);
            //magicDrivesWorker.ReportProgress(progress, "Removing old Files...");
            //removeOldFiles(magicDrivePath, "none", progress, magicDrivesWorker);
            //waitForFinish(progress, magicDrivesWorker);
        }

        private void magicDrivesClick(object sender, EventArgs e)
        {
            buttonEnabler(false);
            console.Text += "building pathway\n";
            //if empty prompt for a new directory
            //if (magicDrivePath.Equals("") || !Directory.Exists(magicDrivePath))
            //{
            //    DialogResult result = folderBrowser.ShowDialog();
            //    if (result == DialogResult.OK)
            //    {
            //        console.Text += "New Magic Drive directory selected at:\n" + folderBrowser.SelectedPath + "\n";
            //        magicDrivePath = folderBrowser.SelectedPath;
            //    }
            //    else
            //    {
            //        buttonEnabler(true);
            //        return;
            //    }
            //}

            magicDrivesWorker.RunWorkerAsync();
        }
    }

}