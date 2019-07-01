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
                //gathering all necessary info
                string type = row[5].ToString(),
                    thumbdriveFolderStructure = "",
                    filePath = row[8].ToString(),
                    taskID = row[2].ToString(),
                    version = row[7].ToString(),
                    TATask = row[8].ToString(),
                    manufacturer = row[4].ToString(),
                    files = row[9].ToString();
                List<string> filesToRun = new List<string>();
                //cleans data for easier entry ["bios1","bios2"] -> bios1, bios2
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
                            switch (type.ToUpper())
                            {

                                case ("DOS"):
                                    thumbdriveFolderStructure = driveLetter + "\\" + type + '\\' + taskID;
                                    generateSKUToBIOSFile(filePath, type, SKU, taskID, TATask, manufacturer, version, filesToRun, driveLetter);
                                    break;
                                case ("INSTANT"):
                                    string ID = row[0].ToString();
                                    string notes = row[10].ToString();
                                    thumbdriveFolderStructure = driveLetter + "\\" + type + '\\' + ID + '_' + version + '_' + notes + "\\";
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
                                    thumbdriveFolderStructure = driveLetter + "\\" + type + '\\' + taskID;
                                    generateSKUToBIOSFile(filePath, type, SKU, taskID, TATask, manufacturer, version, filesToRun, driveLetter);
                                    break;
                            }
                        }

                    }
                    catch (Exception E)
                    {
                        if (fullLogButton.Checked) console.Text += "Exception Caught: " + E.ToString() + "\n";
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
                    //adds version to file
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

                }
                drive++;
            }
            progress += 5;
            magicDrivesWorker.ReportProgress(progress, "Waiting for all files to transfer...");
            //finishing up, waits for cmd to stop proccessing
            waitForFinish(progress, magicDrivesWorker);
        }

        private void magicDrivesClick(object sender, EventArgs e)
        {
            buttonEnabler(false);
            getSheetData();
            console.Text += "building pathway\n";
            magicDrivesWorker.RunWorkerAsync();
        }
    }

}