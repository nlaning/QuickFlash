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

namespace QuickFlash
{
    public partial class MainWindow : Form

    {
        Random random = new Random();
        double increment = 0;//for use in progress bar
        string path = "";//starting path.
        string magicDrivePath = "";//for use in magic drives
        string fullPath = "";//full path of file being moved
        long limit= 200000000;
        private BackgroundWorker worker;
        string preferencesFile = "pref.txt";//just incase a name change is necessary
        string version = "1.21";//probably should be replaced for proper versioning...
                                /* Main Window
                                * first opening the program, (like main)
                                * */
        public MainWindow()
        {
            clearOutCMDProccesses();
            InitializeComponent();
            this.Text = "QuickFlash " + version;
            console.Text += "Welcome to Quick Flash! (" + version + ")\nPlease select a file to begin or press " + '"' + "help" + '"' + "\n";
            loadPreferences("Pref.txt");
            listDirectory(fileViewer, path);
            worker = new BackgroundWorker
            {
                WorkerReportsProgress = true, WorkerSupportsCancellation = true
            };
            worker.DoWork += driveDisplay;
            worker.ProgressChanged += refreshDriveDisplay;
            worker.RunWorkerAsync();
            //console.
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////CORE FUNCTIONS///////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /* Start Click
         *   ________________
         *  /               /----. 
         * /_______________/____/01101010010100101010001010010110101010101
         * |_______________|----'
         * 
        *  where all the magic happens, executes a series of methods to
        *  flash, clean, and move files onto all plugged in thumbdrives
        **/
        private void startClick(object sender, EventArgs e)
        {

            //disable buttons that should not be pushed during proccess
            buttonEnabler(false);
            //getting actual location of files to verify size
            console.Text += "building pathway\n";
            string[] pathParts = path.Split('\\');
            fullPath = "";
            for (int i = 0; i < pathParts.Length - 1; i++)
            {
                fullPath += pathParts[i];
                fullPath += '\\';
            }
            fullPath += fileViewer.SelectedNode.FullPath;
            //determining the boot option
            //gathering file size
            long fileSize = getFileSize(fullPath);
            string size = "selected folder is ";
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
                console.Text += "\n" + (int)(fileSize / 1000000.0) + "mb is too large!\n" +
                    "Check that you have selected the correct file\n" +
                    "or adjust the limit in pref.txt";
                buttonEnabler(true);
                return;
            }
            if (fullLogButton.Checked) console.Text += size + " in size\n";
            string InstallerType = determineType();
            if (fullLogButton.Checked) console.Text += InstallerType + " Selected as boot type\n";
            //if no option selected quit operation entirely
            progressBar.Value = 5;
            Application.DoEvents();
            //gets all thumbdrives and starts the proccess, making vars aswell
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            console.Text += "Flash Initiated, DO NOT remove thumdrives\n";
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
            console.Text += "Estimated time: " + totalTime + " sec\n";
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
                        console.Text += "Formating Drive " + driveLetter + "...\n";
                        formatDrive(driveLetter, drive.ToString());
                    }
                    //if (alwaysCleanButton.Checked && InstallerType != "DOS")
                    //{
                    //    console.Text += "Cleaning drive " + driveLetter + " as requested\n";
                    //    cleanDrive(driveLetter);
                    //}
                    console.Text += "loading drive " + driveLetter + " with necessary components...\n";
                    //loading drive with components
                    loadDrive(driveLetter, InstallerType, drive.ToString());
                }
                drive++;
            }
            console.Text += "Waiting for files to finish transfering...\n";
            //finishing up, waits for cmd to stop proccessing

            int currentlyRunning = Process.GetProcessesByName("cmd").Length;
            int largestAmount = currentlyRunning;
            while (currentlyRunning > 0)
            {
                //loading bar stuff
                if (currentlyRunning > largestAmount) largestAmount = currentlyRunning;
                progressBar.Value = (int)(5 + (90 - 90 * (currentlyRunning / largestAmount)));
                Application.DoEvents();
                //checking
                currentlyRunning = Process.GetProcessesByName("cmd").Length;
            }
            progressBar.Value = 90;
            Application.DoEvents();
            if (alwaysCleanButton.Checked)
            {
                console.Text += "Removing old Files...\n";
                removeOldFiles(fullPath, InstallerType);
            }
            
            if (fullLogButton.Checked) console.Text += "Elapsed time: " + ((DateTime.Now.Ticks / 10000 - start) / 1000) + " Sec\n";//time in ms

            progressBar.Value = 100;
            buttonEnabler(true);
            console.Text += "Process Complete!\n";

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
        private void flashMagicFlashDrives(object sender, EventArgs e)
        {
            int currentlyRunning = Process.GetProcessesByName("cmd").Length;
            int largestAmount = currentlyRunning;

            buttonEnabler(false);
            //if empty prompt for a new directory
            if (magicDrivePath.Equals("") || !Directory.Exists(magicDrivePath))
            {
                DialogResult result = folderBrowser.ShowDialog();
                if (result == DialogResult.OK)
                {
                    console.Text += "New Magic Drive directory selected at:\n" + folderBrowser.SelectedPath + "\n";
                    magicDrivePath = folderBrowser.SelectedPath;
                }
                else
                {
                    buttonEnabler(true);
                    return;
                } 
            }
            //find folder to copy
            List<string> directories = new List<string>(Directory.EnumerateDirectories(magicDrivePath));
            //string folder = "";
            foreach (string directory in directories)
            {
                string[] dir = directory.Split('\\');
                string name = dir[dir.Length - 1];
                if (name.Split('_').Length > 1) magicDrivePath = directory;
            }
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            console.Text += "Gathering Magic Flash Drives...\n";
            int drive = 0;
            foreach (DriveInfo D in allDrives)
            {
                //determine if thumbdrive
                if (D.DriveType.ToString().Equals("Removable"))
                {
                    //gather info            
                    string driveLetter = D.Name.ToString().Remove(2);
                    
                    console.Text += "loading Magic Drive " + driveLetter + " with necessary components...\n";
                    //loading drive with components
                    List<string> excludedFiles = new List<string>();
                    List<string> driveFiles = getFilePath(driveLetter+@"\");
                    foreach(string file in driveFiles)
                    {
                        if (compareFiles(file, magicDrivePath + @"\" + file.Remove(0, 2)))
                        {
                            string[] fullFilePath = file.Split('\\');
                            excludedFiles.Add(fullFilePath[fullFilePath.Length-1]);
                        }
                    }
                    loadDrive(driveLetter, "INSTANT", drive.ToString());
                    if (fullLogButton.Checked) console.Text += "moving files from " + fullPath + " ...\n";
                    string[] cmd = { @"xcopy " + '"' + magicDrivePath + @"\" + "*" + '"' + " " + driveLetter + @" /e /h /i /y /c /EXCLUDE:exclude"+driveLetter.Split(':')[0]+".txt "/*>log" + driveLetter.Split(':')[0] + ".txt"*/ };
                    createFile("copy" + driveLetter.Split(':')[0] + ".bat", cmd);
                    string[] excludedFilesArray = new string[excludedFiles.Count];
                    for(int i =0; i<excludedFiles.Count;i++)
                    {
                        excludedFilesArray[i] = excludedFiles[i];
                    }
                    createFile("exclude" + driveLetter.Split(':')[0]+".txt", excludedFilesArray);
                    runBATFile("copy" + driveLetter.Split(':')[0] + ".bat", false);

                }
                drive++;
            }
            console.Text += "Waiting for all files to transfer...\n";
            //finishing up, waits for cmd to stop proccessing

            currentlyRunning = Process.GetProcessesByName("cmd").Length;
            while (currentlyRunning > 0)
            {
                currentlyRunning = Process.GetProcessesByName("cmd").Length;
            }
            console.Text += "Removing old Files...\n";
            removeOldFiles(magicDrivePath, "none");
            progressBar.Value = 100;
            console.Text += "Magic Drives ready!\n";

            buttonEnabler(true);
        }

        /* Generate AUTOEXEC
         * Creates an AUTOEXEC batch file to automate or make choosing a file without typing
         * It searches through the directory to determine other batch files that are canidates
         * if the list has none, it errors out, if there is just 1, it runs it, if more then 1
         * it will prompt the user for an answer
         **/
        private void generateAUTOEXEC(string driveLetter)
        {
            if (fullLogButton.Checked)
            {
                console.Text += "Generating AUTOEXEC for DOS bootable\n";
            }
            //default output
            string[] contents = { "@echo off","cls",
                "echo No Batch or Executable Files Found on Drive, type DIR to get a list of the directory as you may need to navigate to a different directory" };
            List<string> files = new List<string>(Directory.EnumerateFiles(fullPath));
            List<string> selectedFiles = new List<string>();
            foreach (string file in files)
            {
                string[] f = file.Split('\\');
                string extension = (f[f.Length - 1].Split('.'))[1].ToLower();
                if (extension.Equals("bat") || extension.Equals("exe")) selectedFiles.Add(f[f.Length - 1].ToLower());
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




        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////FORMS///////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /* Display Help
         * Does exactly what one might think, deploys a help screen in a new window/form
         * */
        private void displayHelp(object sender, EventArgs e)
        {
            var form = new Help();
            form.Show();
        }

        /* Display Preferences
         * Displays preference menu to change aspects and behavior of the program
         * 
         * */
        private void displayPreferences(object sender, EventArgs e)
        {
            using (var form = new Preferences(path, magicDrivePath, limit))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    path = form.newBiosLocation;
                    magicDrivePath = form.newMagicLocation;
                    limit = form.newByteLimit;
                    listDirectory(fileViewer, path);
                    savePreferences(this, null);

                }
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////DRIVE TOOLS/////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /* Determine Type 
         * determines given the file(s) selected how the software 
         * should behave, looking for four main cases:
         * 1: no files or too large of file(s) tripping a request to pick a new directory <--- still working on this!
         * 2: determines the directory to be of DOS importance
         * 3: determines the directory to be of EFI importance
         * 4: determines the directory to be of INSTANT importance
         * 5: an undetermined state by which the user will need to select the the type
         * these will output strings in the order following above
         * "DOS","EFI","INSTANT"
         **/
        private string determineType()
        {
            string returnValue = "Instant";
            bool DOS = false;
            bool UEFI = false;
            //browse all files on drive;
            //look for EFI(uefi),BAT or EXE(DOS), or neither for instant. if both, query a select
            List<string> directories = new List<string>(Directory.EnumerateDirectories(fullPath));
            List<string> files = new List<string>(Directory.EnumerateFiles(fullPath));
            foreach (string directory in directories)
            {
                string[] dir = directory.Split('\\');
                string name = dir[dir.Length - 1].ToLower();
                if (name.Equals("efi")) UEFI = true;
                if (name.ToLower().Equals("dos")) DOS = true;
            }
            foreach (string file in files)
            {
                string[] f = file.Split('\\');
                string extension = (f[f.Length - 1].Split('.'))[1].ToLower();
                if (extension.Equals("bat") || extension.Equals("exe")) DOS = true;
                if (extension.Equals("efi")) UEFI = true;
            }
            if (DOS)
            {
                returnValue = "DOS";
            }
            if (UEFI)
            {
                returnValue = "UEFI";
            }
            if ((DOS && UEFI) || manualBootSelectButton.Checked)
            {
                using (var form = new QueryDriveType())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        returnValue = form.returnValue;
                    }
                }
            }

            return returnValue;
        }
        /* DOS Boot
         * 
         * Deploys DOS and makes it bootable on the drive specified
         * i.e. "D:"
         * 
         * */
        private void DOSBoot(string drive)
        {
            string[] batchLines = {
                "pushd %~dp0",
                @"cd /d syslinux-6.03\bios\win32",
                "syslinux.exe -fma " +drive,
                "cd..",
                "cd..",
                "cd..",
                @"xcopy DOS\* " +drive+@" /e /h /i /y /c"};
            drive = drive.Split(':')[0];
            createFile("DOS" + drive + ".bat", batchLines);
            runBATFile("DOS" + drive + ".bat", true);

        }

        /*Activate Drive 
         * sets the drive to be active (bootable) its an easy enough operation 
         * that its typically run always just in case
        **/
        private void activateDrive(string drive, string number)
        {
            string[] batchLines = {
                "pushd %~dp0",
                "diskpart /s activate"+drive+".txt > activatelog"+drive.Split(':')[0]+".txt" };
            string[] textLines = {
                "select disk " + number,
                "select partition 1",
                "active" };
            drive = drive.Split(':')[0];
            createFile("activate" + drive + ".txt", textLines);
            createFile("activate" + drive + ".bat", batchLines);
            runBATFile("activate" + drive + ".bat", false);
        }

        /*Foramt Drive
         * Formats the drive in question, this will (hopefully) be rarely 
         * necessary as most drives are already fat32's that are mbr
         * takes a drive letter and number i.e. "D:","2"
         * 
         **/

        private void formatDrive(string letter, string number)
        {
            string partitionStyle = "convert mbr";
            string[] batchLines = {
                "pushd %~dp0",
                "diskpart /s clean"+number+".txt > cleanlog"+number+".txt" };
            string[] textLines = {
                "select disk " + number,
                "clean",
                partitionStyle,
                "create partition primary",
                "select partition 1",
                "active",
                "format fs = fat32 quick",
                "assign letter="+letter.Remove(1) };
            createFile("clean" + number + ".txt", textLines);
            createFile("clean" + number + ".bat", batchLines);
            runBATFile("clean" + number + ".bat", true);
            if (fullLogButton.Checked)
            {
                outputLog("cleanlog" + number + ".txt", 6);
            }


        }

        /* Clean Drive
         * Cleans all files off of the drive in question, this can be helpful when
         * loading two different versions of the same bios on one drive and is instigated
         * using the "always clean" button
         * 
         * */
        private void cleanDrive(string drive)
        {
            progressBar.Value += (int)increment;
            Application.DoEvents();
            string[] cmd =
            {
                "del "+drive+@"\* /s /q",
                "rmdir /s/q "+drive
            };
            drive = drive.Split(':')[0];
            createFile("clear" + drive + ".bat", cmd);
            runBATFile("clear" + drive + ".bat", false);
        }

        /*loads drives differently depending on type and style
         * takes a drive, type and drive number, i.e
         * loadDDrive("D:","DOS","2")
         * */
        private void loadDrive(string drive, string Type, string number)
        {
            if (Type.Equals("DOS"))
            {
                activateDrive(drive, number);
                DOSBoot(drive);
                generateAUTOEXEC(drive);
            }
            if (Type.Equals("UEFI"))
            {
                activateDrive(drive, number);
            }
            copySelectedFile(drive);

        }

        /* Format Drives
         * Simply formats all drives, a nice feature just to have available for various reasons
         * 
         * */
        private void FormatDrives(object sender, EventArgs e)
        {
            buttonEnabler(false);
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            console.Text += "Formating all available thumb drives...\n";
            int drive = 0;
            progressBar.Value = 10;
            foreach (DriveInfo D in allDrives)
            {
                //determine if thumbdrive
                if (D.DriveType.ToString().Equals("Removable"))
                {
                    //gather info
                    string driveLetter = D.Name.ToString().Remove(2);
                    console.Text += "Formatting Drive " + driveLetter + " ...\n";
                    formatDrive(driveLetter, drive.ToString());
                }
                drive++;
            }
            progressBar.Value = 100;
            buttonEnabler(true);
            console.Text += "Process Complete!\n";
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
            while (!worker.CancellationPending)
            {
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                string display = "Currently Insterted Drives: ";
                foreach (DriveInfo D in allDrives)
                {
                    //determine if thumbdrive
                    if (D.DriveType.ToString().Equals("Removable"))
                    {
                        display += D.Name.Remove(1) + " ";
                    }
                }
                Thread.Sleep(100);
                worker.ReportProgress(0, display);
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

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////FILE HANDLING///////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /* Comnpare Files
         * Compares files to determine whether or not they are the same
         * currrently only checks size, has the ability to check each byte but is cuurently disabled
         * due to how long it takes. But I left it in here incase more precision is ever needed
         * 
         * */
        public bool compareFiles(string filePath1, string filePath2)
        {
            FileInfo File1 = new FileInfo(filePath1);
            FileInfo File2 = new FileInfo(filePath2);
            if (!File1.Exists || !File2.Exists) return false;
            if (File1.Length != File2.Length) return false;
            //int byteSize = sizeof(long);
            //using (FileStream stream1 = File1.OpenRead())
            //using (FileStream stream2 = File2.OpenRead())
            //{
            //    byte[] byte1 = new byte[byteSize];
            //    byte[] byte2 = new byte[byteSize];

            //    for (int i = 0; i < File1.Length; i++)
            //    {
            //        stream1.Read(byte1, 0, byteSize);
            //        stream2.Read(byte2, 0, byteSize);

            //        if (BitConverter.ToInt64(byte1, 0) != BitConverter.ToInt64(byte2, 0))
            //            return false;
            //    }
            //}
            return true;
        }
        /* Get Files
         * recursively gathers files by path
         * 
         * */
        private List<string> getFilePath(string path)
        {
            
            List<string> allFiles = new List<string>();
            try
            {
                List<string> directories = new List<string>(Directory.EnumerateDirectories(path));
                List<string> files = new List<string>(Directory.EnumerateFiles(path));
                allFiles.AddRange(files);
                foreach (string directory in directories)
                {
                    allFiles.AddRange(getFilePath(directory));
                }
            }
            catch
            {
                console.Text += "Error gathering path: " + path + "\n";
            }
            return allFiles;
        }
        /* Run BAT file
        * Runs a specified BAT file in the background, it is also capable of waiting for completetion
        * for programs like diskpart that dont like to be instanced. there is also a manually placed delay 
        * regardless for similar reasons although cmd has less issues thus the set delay 
        * takes a filename and a boolean as to whether it needs to wait, i.e
        * "flash.BAT",true
        * */
        private void runBATFile(string filename, bool wait)
        {
            ProcessStartInfo BatchProcess;
            Process process;
            BatchProcess = new ProcessStartInfo("cmd.exe", "/c " + filename);
            BatchProcess.CreateNoWindow = true;
            BatchProcess.UseShellExecute = false;
            process = Process.Start(BatchProcess);
            if (wait)
            {
                process.WaitForExit();
            }
            else System.Threading.Thread.Sleep(200);
        }

        /*Create File
         * Creates a file based on the names and the lines it needs
         * the lines are in array form to avoid the necessity to add 
         * \n's to the input allowing for easier visability when using the program
         * takes a name and the lines i.e.
         * "file.txt",{"line 1","line 2"}
         * 
         * */
        private void createFile(String name, String[] lines)
        {

            if (File.Exists(name))
            {
                File.Delete(name);
            }
            StreamWriter writer = new StreamWriter(name);
            foreach (String line in lines)
            {
                writer.WriteLine(line);
            }
            writer.Close();
        }

        /*Remove Old Files
         * Removes files that exist in one directory but not the other, excluding key system files
         * 
         * */
        private void removeOldFiles(string sourceDirectory,string installerType)
        {
            
            List<string> Exclusions = new List<string>();
            Exclusions.Add("IndexerVolumeGuid");
            Exclusions.Add("WPSettings.dat" );
            Exclusions.Add("ldlinux.c32");
            Exclusions.Add("ldlinux.sys");
            if (installerType == "DOS")
            {
                List<string> DOSExclusions = getFilePath("DOS");
                foreach (string DOSFile in DOSExclusions)
                {
                    string[] splitFiles = DOSFile.Split('\\');
                    Exclusions.Add(splitFiles[splitFiles.Length - 1]);
                }
            }
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo D in allDrives)
            {
                //determine if thumbdrive
                if (D.DriveType.ToString().Equals("Removable"))
                {
                    
                    string hostDirectory = D.Name;
                    console.Text += "Cleaning "+ hostDirectory+"\n";
                    List<string> fileList = getFilePath(D.Name);
                    
                    foreach (string file in fileList)
                    {
                        //console.Text += file + "\n";
                        if (!File.Exists(sourceDirectory + file.Remove(0, hostDirectory.Length-1)))
                        {
                            string[] splitFiles = file.Split('\\');
                            if (!Exclusions.Contains(splitFiles[splitFiles.Length - 1]))
                            {
                                try
                                {
                                    File.SetAttributes(file, FileAttributes.Normal);
                                    File.Delete(file);
                                }
                                catch
                                {
                                    console.Text+="Error occured while Deleting " + file + "\n";
                                }
                            }
                        }
                    }
                }
            }
        }

        /* Get Files
         * recursively gathers size information from files, and returns the total size in bytes
         * 
         * */
        private long getFileSize(string path)
        {
            List<string> directories = new List<string>(Directory.EnumerateDirectories(path));
            List<string> files = new List<string>(Directory.EnumerateFiles(path));
            long total = 0;
            foreach (string file in files)
            {
                total += new FileInfo(file).Length;
            }
            foreach (string directory in directories)
            {
                total += getFileSize(directory);
            }
            return total;
        }
        /* Copy Selected File
         * copies the selected path, for excclusive use, could be either improved
         * to be more generic or depricated completely infavor of on use cases
         * takes a Drive Variable (i.e "D:")
         * */
        private void copySelectedFile(string drive)
        {
            if (fullLogButton.Checked) console.Text += "moving files from " + fullPath + " ...\n";
            string folderToMove = fullPath;
            string[] cmd = { @"xcopy " + '"' + folderToMove + '\\' + "*" + '"' + " " + drive + @" /e /h /i /y /c >log" + drive.Split(':')[0] + ".txt" };
            drive = drive.Split(':')[0];
            createFile("copy" + drive + ".bat", cmd);
            runBATFile("copy" + drive + ".bat", false);

        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////MISC////////////////////////////////////////////////////////////////////
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
        /* Output Log
         * outputs a specified log to the console, typically only used when
         * the "show full log" option is selected, and can be used to view progress or errors
         * takes a file and a line starting point to avoid uneccesary headers
         * i.e. "log.txt",4
         * 
         * */
        private void outputLog(string file, int start)
        {
            if (File.Exists(file))
            {
                string[] lines = File.ReadAllLines(file);
                for (int i = start; i < lines.Length; i++)
                {
                    console.Text += lines[i] + "\n";
                }
            }
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
                            if (path.Equals("")) browseForFolder(this, null);
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
                            magicDrivePath = s.Split('?')[1];

                            break;
                        case (6):
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
                    }
                }
            }
            else
            {
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
                "magicDirectory?"+magicDrivePath,
                "Directorylimit?"+limit
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
        private void listDirectory(TreeView treeView, string path)
        {
            try
            {
                treeView.Nodes.Clear();
                var rootDirectoryInfo = new DirectoryInfo(path);
                treeView.Nodes.Add(createDirectoryNode(rootDirectoryInfo));
            }
            catch (ArgumentException)
            {
                //browseForFolder(this, null);
            }
            catch (IOException)
            {
                //browseForFolder(this, null);
            }
        }

        /* Create Directory Node
         * recursively called to populate all available directories (tree)
         * is just passed the current directory location and branches from there
         * 
         * */
        private TreeNode createDirectoryNode(DirectoryInfo directoryInfo)
        {

            var directoryNode = new TreeNode(directoryInfo.Name);
            foreach (var directory in directoryInfo.GetDirectories())
            {
                try
                {
                    TreeNode N = createDirectoryNode(directory);
                    if (N.Text != "EFI")
                    {
                        directoryNode.Nodes.Add(N);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    console.Text += "Access denied to " + directory.Name + "\n";
                }
                catch (Exception)
                {
                    console.Text += "Unknown error at " + directory.Name + "\n";

                }
            }
            return directoryNode;

        }


    }
}
