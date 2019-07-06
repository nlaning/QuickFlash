/*
 *
 *                                          /===============\
 *>=====================================:~ /  ~Quick Flash~  \ ~:==========================================
 *                                        /===================\
 *  
 * Quick Flash is a program designed around greatly increasing the amount of time it takes someone to
 * flash multiple thumbdrives for holding a BIOS update. It achieves this in these major ways:
 * 
 * -> Allows a clean user interface to easily select BIOS updates either standard or custom
 * -> Automatatically selects the boot method based on the selected folder structure (DOS, UEFI, or Instant)
 * -> Is capable of recognizing when a drive is need of formatting, and formatting it
 * -> Is capable of doing multiple drives simultaneously to greatly reduce time spent moving files
 * -> Is capable of updating current magic thumbdrives simultaneously
 * 
 * It can achieve all of this by leveraging built in Windows tools, doing away with any need for extermal 
 * sources or programs. It uses CMD and DISKPART, and is capable of creating, running, and gathering logs 
 * from both of these programs. the Majority of the UI was used in the Visual Studio enviornment and much 
 * of the UI code was therefore generated, the majority of my work relies within this file itself. For more
 * in depth of the innerworkings one would need to browse the various methods.
 * 
 * Quick Flash was created and is Property of myself, Nathan Laning, and was soley created for use by 
 * ONLogic, therefor is not for sale and cannot be used outside the scope of ONLogic without direct consent.
 * 
 * Please Email myself directly or whomever may be maintaning this code for any questions or concerns
 **/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using static FileTools;
namespace QuickFlash
{
    public partial class MainWindow : Form{
        string workingDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location.Remove(System.Reflection.Assembly.GetExecutingAssembly().Location.Length-14);
        double increment = 0; //for use in progress bar
        bool EU = false;
        string path  = "", EUpath = ""; //starting path.
        string magicDrivePath = ""; //for use in magic drives
        string fullPath= ""/*, EUpath=""*/; //full path of file being moved
        string fileDirectoryMissingPath= "";
        long limit = 200000000;
        int serverPollingRate = 500;
        TreeNode directoryList = new TreeNode();
        private BackgroundWorker checkDrivesWorker,flashDrivesWorker, magicDrivesWorker, updateBiosFolder;
        string preferencesFile = "pref.txt"; //just incase a name change is necessary
        string version = "1.62"; //probably should be replaced for proper versioning...
        /* Main Window
        * first opening the program, (like main)
        * */
        public MainWindow()
        {
            clearOutCMDProccesses();
            InitializeComponent();
            loadWorkers();
            Text = "QuickFlash " + version;
            console.Text += "Welcome to Quick Flash! (" + version + ")\nPlease select a file to begin or press " + '"' + "help" + '"' + "\n";
            loadPreferences("Pref.txt");
            makeLink(magicDrivePath);
            listDirectory(fileViewer,path);
            getSheetData();
            if(!EUpath.Equals("")) updateBiosFolder.RunWorkerAsync();

        }

        private void flashDrivesClick(object sender, EventArgs e)
        {
            buttonEnabler(false);
            console.Text += "building pathway\n";
            string[] pathParts = path.Split('\\');
            fullPath = "";
            //for (int i = 0; i < pathParts.Length - 1; i++)
            //{
            //    fullPath += pathParts[i];
            //    fullPath += '\\';
            //}

            fullPath = path + '\\'+ fileDirectoryMissingPath+fileViewer.SelectedNode.FullPath;
            flashDrivesWorker.RunWorkerAsync();
        }

       

        /* Flash Drives
        *   ________________
        *  /               /----.
        * /_______________/____/101101010010100101010001010010110101010101
        * |_______________|----'
        * 
        *  where all the magic happens, executes a series of methods to
        *  flash, clean, and move files onto all plugged in thumbdrives
        **/
        private void flashDrives(object sender, DoWorkEventArgs e)
        {
            int progress = 5; //disable buttons that should not be pushed during proccess
            long fileSize = getFileSize(fullPath);
            string size = "";
            if (fileSize > 1000000)
            {
                size += (int)(fileSize / 1000000.0) + "mb";
            }
            else
            {
                size += (int)(fileSize / 1000.0) + "kb";
            }
            //estimate time
            long start = DateTime.Now.Ticks / 10000;//time in ms
            //stop program if file is too large
            if (fileSize > limit)
            {
                flashDrivesWorker.ReportProgress(0, size + " is too large!\n" +
                    "Check that you have selected the correct file\n" +
                    "or adjust the limit in pref.txt");
                return;
            }
            if (fullLogButton.Checked) flashDrivesWorker.ReportProgress(progress, "selected folder is " + size + " in size");
            string InstallerType = determineType();
            if (fullLogButton.Checked) flashDrivesWorker.ReportProgress(progress, InstallerType + " Selected as boot type");
            //gets all thumbdrives and starts the proccess, making vars aswell
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            flashDrivesWorker.ReportProgress(progress, "Flash Initiated, DO NOT remove thumdrives");
            int drive = 0;
            int thumbdrives = 0;
            // estimate time using previously gathered data
            //if cleaning time estimate will be much more eradic, this could be adjusted but general time constraints tend to be more useful
            long bytesPerSecond = 4560000;
            long totalTime = fileSize / bytesPerSecond;
            if (InstallerType == "DOS") totalTime += 1;
            if (alwaysCleanButton.Checked) totalTime += 8;
            totalTime += 1;//adding one since there is a built in delay for drives attached and the average number of drives would most likely be around 1-4
            //increments through all drives

            flashDrivesWorker.ReportProgress(progress, "Estimated time: " + totalTime + " sec");
            if (fullLogButton.Checked) flashDrivesWorker.ReportProgress(progress, "moving files from " + fullPath + " ...");
            foreach (DriveInfo D in allDrives)
            {
                //determine if thumbdrive
                if (D.DriveType.ToString().Equals("Removable"))
                {
                    thumbdrives++;
                    //gather info
                    string driveLetter = D.Name.ToString().Remove(2);
                    string driveFormat = D.DriveFormat;
                    //if format type doesnt match, flash drive
                    if (driveFormat != "FAT32")
                    {
                        flashDrivesWorker.ReportProgress(progress, "Formating Drive " + driveLetter + "...");
                        formatDrive(driveLetter, drive.ToString());
                    }
                    flashDrivesWorker.ReportProgress(progress, "loading drive " + driveLetter + " with necessary components...");
                    //loading drive with components
                    
                    loadDrive(driveLetter, InstallerType, drive.ToString());
                }
                drive++;
            }
            
            //finishing up, waits for cmd to stop proccessing
            
            if (alwaysCleanButton.Checked)
            {
                progress = 90;
                flashDrivesWorker.ReportProgress(progress, "Removing old Files...");
                removeOldFiles(fullPath, InstallerType,progress, flashDrivesWorker);
            }
            flashDrivesWorker.ReportProgress(progress, "Finishing up");
            waitForFinish(progress, flashDrivesWorker);
            if (fullLogButton.Checked) flashDrivesWorker.ReportProgress(progress, "Elapsed time: " + ((DateTime.Now.Ticks / 10000 - start) / 1000) + " Sec");//time in ms

        }

 
        /* Generate AUTOEXEC
         * Creates an AUTOEXEC batch file to automate or make choosing a file without typing
         * It searches through the directory to determine other batch files that are canidates
         * if the list has none, it errors out, if there is just 1, it runs it, if more then 1
         * it will prompt the user for an answer
         **/
        private void generateAUTOEXEC(string driveLetter)
        {
            
            //default output
            string[] contents = { "@echo off","cls",
                "echo No Batch or Executable Files Found on Drive, type DIR to get a list of the directory as you may need to navigate to a different directory" };
            List<string> files = new List<string>(Directory.EnumerateFiles(fullPath));
            List<string> selectedFiles = new List<string>();
            foreach (string file in files)
            {
                string[] f = file.Split('\\');
                string extension = (f[f.Length - 1].Split('.'))[1].ToLower();
                if (extension.Equals("bat") /*|| extension.Equals("exe")*/) selectedFiles.Add(f[f.Length - 1].ToLower());//extension incase its ever needed, seemed like all runnables were bat files.
            }         
            //if more then one file, but less then 10
            if (selectedFiles.Count < 9 && selectedFiles.Count != 0)
            {
                List<string> contentsList = new List<string>();
                contentsList.Add("@echo off");
                contentsList.Add("cls");
                contentsList.Add("echo press the number of the file you would like to run or press q to quit");
                contentsList.Add("echo ===============================");
                string choice = "choice /C:";
                //adds the visable options for user to pick through
                for (int i = 0; i < selectedFiles.Count; i++)
                {
                    choice += (i + 1).ToString();
                    contentsList.Add("echo " + (i + 1).ToString() + ": " + selectedFiles[i]);
                }
                contentsList.Add("echo ===============================");
                contentsList.Add(choice + "q /n");
                //adds the actual options once selected, choice seems to like numbers in reverse order
                for (int i = selectedFiles.Count - 1; i > -1; i--)
                {
                    contentsList.Add("IF %ERRORLEVEL% == " + (i + 1).ToString() + " CALL " + '"' + selectedFiles[i] + '"');
                }
                //convert from List to Array for createfile to accept it
                contents = new string[contentsList.Count];
                for (int i = 0; i < contentsList.Count; i++)
                {
                    contents[i] = contentsList[i];
                }
            }
            //if more then 9, just list options for user to type out
            if (selectedFiles.Count > 9 || selectedFiles.Count==0)
            {
                List<string> contentsList = new List<string>();
                int linesAcross = 3;
                contentsList.Add("@echo off");
                contentsList.Add("cls");
                contentsList.Add("echo Too many files to use choice, please type the file you would like");
                contentsList.Add("echo =================================================================");
                for (int i = 0; i < (selectedFiles.Count / linesAcross) + 1; i++)
                {
                    string line = "echo ";
                    int count = 0;
                    for (int j = i * linesAcross; j < selectedFiles.Count; j++)
                    {
                        count++;
                        line += selectedFiles[j].Split('.')[0] + "  ";
                        if (count == linesAcross) j = selectedFiles.Count;
                    }
                    contentsList.Add(line);
                }

                contentsList.Add("echo =================================================================");
                //convert from List to Array for createfile to accept it
                contents = new string[contentsList.Count];
                for (int i = 0; i < contentsList.Count; i++)
                {
                    contents[i] = contentsList[i];
                }
            }
            createFile(driveLetter + @"\autoexec.bat", contents);



        }

        public long getlimit()
        {
            return limit;
        }
        public string getfullPath()
        {
            return path;
        }
        public string getEUPath()
        {
            return EUpath;
        }
        public long getserverPollingRate()
        {
            return serverPollingRate;
        }
        private void makeLink(string address)
        {
            if (!magicDrivePath.Equals("")) { 
                string[] contents = {"remdir magickLink","mklink /d magicLink " + '"'+ address + '"' };
                createFile("links.bat", contents);
                runBATFile("links.bat",true);
                magicDrivePath = "magicLink";
            }
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////// MISC ///////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /*string to bool
        *converts plain text into boolean values
        * importantly it only looks for "true" and assumes all else is false
        * */
        private bool stringToBool(string s)
        {
            return s.ToLower().Equals("true");
        }
        /*
             * Button Enabler
             * enables or disables buttons that are apart of the flashing/moving proccess
             * this prevents damaging actions 
             * only needs a boolean to determine enabling or disabling
             * 
             * */

        private void buttonEnabler(bool enabled)
        {
            start.Enabled = enabled;
            updateMagicFlashDrivesToolStripMenuItem.Enabled = enabled;
            preferencesToolStripMenuItem.Enabled = enabled;
            saveCurrentPresetToolStripMenuItem.Enabled = enabled;
            updateMagicFlashDrivesToolStripMenuItem.Enabled = enabled;
        }

        /*Output Console
         * 
         * Outputs the console to a txt file 
         * the name is generated using the current time 
         * 
         * */
        private void outputConsole(object Sender, EventArgs e)
        {
            string name = "ConsoleLog_";
            name += System.DateTime.Now.ToFileTime() + ".txt";
            console.Text += "saving console log as:\n" + name + "\n";
            string[] consoleText = console.Text.Split('\n');
            createFile(name, consoleText);
        }

        
        
        //Scrolls console automatically to keep the most relevent info visable
        private void consoleTextChanged(object sender, EventArgs e)
        {
            console.SelectionStart = console.Text.Length;
            console.ScrollToCaret();
        }

        /*load preferences
         * loads preferences from specified file in txt format
         * the default for this instance is pref.txt (see above)
         * it then uses the data to populate a very specific set of variables
         * this allows any user to alter settings easily outside of the program
         * incase of an in-operable state
         * really could use some try/catching incase of broken or misplaced file
         * */
        private void loadPreferences(string file)
        {
            if (File.Exists(file))
            {
                int counter = 0;
                string[] lines = File.ReadAllLines(file);
                foreach (string s in lines)
                {
                    counter++;
                    switch (counter)
                    {
                        case (1):
                            path = s.Split('?')[1];
                            if (path.Equals("")) displayPreferences(this, null);
                            break;
                        case (2):
                            manualBootSelectButton.Checked = stringToBool(s.Split('?')[1]);
                            break;
                        case (3):
                            fullLogButton.Checked = stringToBool(s.Split('?')[1]);
                            break;
                        case (4):
                            alwaysCleanButton.Checked = stringToBool(s.Split('?')[1]);
                            break;
                        case (5):
                            try
                            {
                                limit = Convert.ToInt64(s.Split('?')[1]);
                            }
                            catch (FormatException)
                            {
                                limit = 100000000;
                                console.Text += "Error in preferences file, the limit is not inputed correctly";
                            }
                            break;
                        case (6):
                            EUpath = s.Split('?')[1];
                            break;
                        case (7):
                            try
                            {
                                serverPollingRate = Convert.ToInt32(s.Split('?')[1]);
                            }
                            catch (FormatException)
                            {
                                serverPollingRate = 500;
                                console.Text += "Error in preferences file, the limit is not inputed correctly";
                            }
                            break;
                    }
                }
            }
            else
            {
                //browseForFolder(this, null);
                displayPreferences(this, null);
                savePreferences(this, null);
                
            }

        }

        /*save preferences
        * saves preferences from specified file in txt format
        * the default for this instance is pref.txt
        * it saves a very specific set of variables
        * this allows any user to alter settings easily outside of the program
        * incase of an in-operable state
        * */
        private void savePreferences(object sender, EventArgs e)
        {
            string[] preferences =
            {
                "rootDirectory?"+path,
                "manualBoot?"+manualBootSelectButton.Checked,
                "fullLog?"+fullLogButton.Checked,
                "alwaysClean?"+alwaysCleanButton.Checked,
                "Directorylimit?"+limit,
                 "USpath?"+EUpath,
                 "serverPollingRate?"+serverPollingRate
            };
            createFile(preferencesFile, preferences);
        }

        /*Browse For Folder
         * Basic Folder Browsing dialogue to allow the user to change the root directory
         * 
         * */
        private void browseForFolder(object sender, EventArgs e)
        {
            DialogResult result = folderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                console.Text += "New directory selected at:\n" + folderBrowser.SelectedPath + "\n";
                path = folderBrowser.SelectedPath;
                listDirectory(fileViewer, path);
            }
            else return;
        }

        /* Clear Out CMD Proccesses
         * This forcebly clears out all pending,locked,or even just running command proccesses 
         * this avoids conflicts and lock ups
         * */
        private void clearOutCMDProccesses()
        {
            Process[] CMD = Process.GetProcessesByName("cmd");
            foreach (Process P in CMD)
            {
                P.Kill();
            }
        }

        /* Initiates a node tree to access the full specifed directory and all of its children
         * only cares about directories
         * takes the TreeView it would like to change, and the path of the specified folder
         * */

        
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////// THREADING STUFF //////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //initializes workers 
        private void loadWorkers()
        {
            //drive updating workers init
            checkDrivesWorker = new BackgroundWorker();
            checkDrivesWorker.WorkerReportsProgress = true;
            checkDrivesWorker.DoWork += driveDisplay;
            checkDrivesWorker.ProgressChanged += refreshDriveDisplay;
            checkDrivesWorker.RunWorkerAsync();
            //folder updating workers init
            updateBiosFolder = new BackgroundWorker();
            updateBiosFolder.WorkerReportsProgress = true;
            updateBiosFolder.DoWork += EUsyncBiosFolder;
            //updateBiosFolder.ProgressChanged += refreshDriveDisplay;
            
            //flashing worker init
            flashDrivesWorker = new BackgroundWorker();
            flashDrivesWorker.WorkerReportsProgress = true;
            flashDrivesWorker.DoWork += flashDrives;
            flashDrivesWorker.ProgressChanged += driveProgressChanged;
            flashDrivesWorker.RunWorkerCompleted += driveProgressCompleted ;
            //magic drive worker init
            magicDrivesWorker = new BackgroundWorker();
            magicDrivesWorker.WorkerReportsProgress = true;
            magicDrivesWorker.DoWork += flashMagicFlashDrives;
            magicDrivesWorker.ProgressChanged += driveProgressChanged;
            magicDrivesWorker.RunWorkerCompleted += driveProgressCompleted;
        }

        private void driveProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int percentage = e.ProgressPercentage;
            if (percentage > 100) percentage = 100;
            if (percentage >= 0 ) progressBar.Value = percentage;
            string text = e.UserState as string;
            if (text != null )  console.Text += text + "\n";
        }

        private void driveProgressCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Value = 100;
            buttonEnabler(true);
            console.Text += "Process Complete!\n";
        }



        /* Wait to finish
         * waits for all running (cmd)processes to complete
         * */
        private void waitForFinish(int startingProgress, BackgroundWorker worker)
        {
            int progress = startingProgress;
            int currentlyRunning = Process.GetProcessesByName("cmd").Length;
            int largestAmount = currentlyRunning;
            int currentProgress = startingProgress;
            while (currentlyRunning > 0)
            {
                if (currentlyRunning > largestAmount) largestAmount = currentlyRunning;

                progress = (largestAmount - currentlyRunning) * ((100 - startingProgress) * progress / largestAmount) + startingProgress;
                if (currentProgress != progress)
                {
                    worker.ReportProgress(progress);
                    currentProgress = progress;
                }
                Thread.Sleep(100);
                //checking
                currentlyRunning = Process.GetProcessesByName("cmd").Length;
            }
        }

        /* RefreshDriveDisplay
         * 
         * simply used as a gate of sorts, albiet a basic one, but suffiecent for a simple thread.
         * takes the "thread Progress" although this mainly a workaround, and dispalys the string 
         * associated.
         * 
         * */
        private void refreshDriveDisplay(object sender, ProgressChangedEventArgs e)
        {
            label1.Text = e.UserState as string;

        }


        /* Drive Display
* 
* Gathers all drives continuosly and displays them, this is primarily used
* to determine when any drive is not functioning properly, disconnecting the drives one 
* one by one watching the letters go away.
* 
* This is designed to be used in threading use cases
* */
        public void driveDisplay(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                string display = "Currently Inserted Drives: ";
                foreach (DriveInfo D in allDrives)
                {
                    //determine if thumbdrive
                    if (D.DriveType.ToString().Equals("Removable"))
                    {
                        display += D.Name.Remove(1) + " ";
                    }
                }
                Thread.Sleep(100);
                checkDrivesWorker.ReportProgress(0, display);
            }

        }
    }
}




