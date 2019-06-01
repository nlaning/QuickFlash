using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace QuickFlash
{
    public partial class MainWindow : Form

    {
        double increment = 0;//for use in progress bar
        String path = @"C:\";//starting path, probably could be removed...
        string magicDrivePath = "";//for use in magic drives
        string fullPath = "";//full path of file being moved
        string preferencesFile = "pref.txt";//just incase a name change is necessary
        String version = "1.07";//probably should be replaced for proper versioning...
         /* Main Window
         * first opening the program, (like main)
         * */
        public MainWindow()
        {
            InitializeComponent();
            this.Text = "QuickFlash " + version;
            console.Text += "Welcome to Quick Flash! (" + version + ")\nPlease select a file to begin or press " + '"' + "help" + '"' + "\n";
            loadPreferences("Pref.txt");
           listDirectory(fileViewer, path);
        }
        /*start Click
         *   ________________
         *  /               /----. 
         * /_______________/____/01101010010100101010010101
         * |_______________|----'
         * 
        *where all the magic starts, executes a series of methods to
        *flash, clean, and move files onto all plugged in thumbdrives
        **/
        private void startClick(object sender, EventArgs e)
        {

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

            string InstallerType = determineType();
            console.Text += InstallerType + " Selected as boot type\n";
            //if no option selected quit operation entirely
            progressBar.Value = 5;
            Application.DoEvents();
            //gets all thumbdrives and starts the proccess, making vars aswell
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            console.Text += "Flash Initiated, DO NOT remove thumdrives\n";
            int drive = 0;

            int thumbdrives = 0;
            //increments through all drives
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
                    if (alwaysCleanButton.Checked && InstallerType != "DOS")
                    {
                        console.Text += "Cleaning drive " + driveLetter + " as requested\n";
                        cleanDrive(driveLetter);
                    }
                    console.Text += "loading drive " + driveLetter + " with necesary components...\n";
                    //loading drive with components
                    loadDrive(driveLetter, InstallerType, drive.ToString());
                }
                drive++;
            }
            console.Text += "Finishing up...\n";
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
            progressBar.Value = 100;
            console.Text += "Process Complete!\n";

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
                    switch (counter) {
                        case (1):
                            path = s.Split('?')[1];
                            if (path.Equals(null)) browseForFolder(this, null);
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
                    }
                }
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
                "magicDirectory?"+magicDrivePath
                //,
               // "driveNumber?"+driveNumber.ToString()
            };
            createFile(preferencesFile, preferences);
        }
        //basic folder browser dialogue to select new root folder
        private void browseForFolder(object sender, EventArgs e)
        {
            DialogResult result = folderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                console.Text += "New directory selected at:\n" + folderBrowser.SelectedPath + "\n";
                path = folderBrowser.SelectedPath;
                listDirectory(fileViewer, path);
            }
        }
        /*
         * Flash Magic Drives
         *
         * Moves necessary files to magic drives, DOES NOT format them, in other words
         * this will not create new magic drives, just updates current ones. this is done to save immense time since
         * creating a new one everytime would require formatting and diskpart doesnt like running in multiple instances 
         * therefor the slowdown would be immense
         * 
         * Furthermore the directory is manually placed into the preferences file (pref.txt)
         * this is to prevent tampering as theoretically this directory will never change
         * 
         * */
        private void flashMagicFlashDrives(object sender, EventArgs e)
        {
            //checking location exist
            if (!magicDrivePath.Equals(""))
            {
                //find folder to copy
                List<string> directories = new List<string>(Directory.EnumerateDirectories(magicDrivePath));
                string folder = "";
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
                        console.Text += "loading Magic Drive " + driveLetter + " with necesary components...\n";
                        //loading drive with components
                        loadDrive(driveLetter, "DOS", drive.ToString());
                        if (fullLogButton.Checked) console.Text += "moving files from " + fullPath + " ...\n";
                        string[] cmd = { @"xcopy " + '"' + magicDrivePath + '\\' + "*" + '"' + " " + driveLetter + @" /e /h /i /y /c >log"+driveLetter.Split(':')[0] + ".txt" };
                        createFile("copy"+driveLetter.Split(':')[0]+".bat", cmd);
                        runBATFile("copy" + driveLetter.Split(':')[0] +".bat", false);

                    }
                    drive++;
                }
                console.Text += "Finishing up...\n";
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
                progressBar.Value = 100;
                console.Text += "Magic Drives ready!\n";
            }
            else { console.Text += "Magic Flash Drive directory is not yet saved\nPlease add the directory path to pref.txt\n"; }
        }


        
        /*
         * determine Type 
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
            foreach(string directory in directories)
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
                if (extension.Equals("bat")|| extension.Equals("exe")) DOS = true;
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
            if ((DOS && UEFI)||manualBootSelectButton.Checked)
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
        
        /*loads drives differently depending on type and style
         * takes a drive, type and drive number, i.e
         * loadDDrive("D:","DOS","2")
         * */
        private void loadDrive(string drive, string Type,string number)
        {
            if (Type.Equals("DOS"))
            {
                activateDrive(drive,number);
                DOSBoot(drive);
            }
            if (Type.Equals("UEFI"))
            {
                activateDrive(drive,number);
            }
            copySelectedFile(drive);

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
            string[] cmd = { @"xcopy " + '"' + folderToMove + '\\' + "*" + '"' + " " + drive + @" /e /h /i /y /c >log"+drive.Split(':')[0]+".txt" };
            drive = drive.Split(':')[0];
            createFile("copy"+drive+".bat", cmd);
            runBATFile("copy" + drive +".bat", false);

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
            createFile("DOS" + drive +".bat", batchLines);
            runBATFile("DOS" + drive +".bat", false);
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
            createFile("activate" + drive +".txt", textLines);
            createFile("activate" + drive +".bat", batchLines);
            runBATFile("activate" + drive +".bat", false);
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
                "assign letter=",letter.Remove(1) };
            createFile("clean" + number + ".txt", textLines);
            createFile("clean" + number + ".bat", batchLines);
            runBATFile("clean" + number + ".bat",true);
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
            console.Text += "clearing out files on drive "+drive+"...\n";
            string[] cmd =
            {
                "del "+drive+@"\* /s /q",
                "rmdir /s/q "+drive
            };
            drive = drive.Split(':')[0];
            createFile("clear" + drive + ".bat", cmd);
            runBATFile("clear" + drive +".bat",false);
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
        /* Run BAT file
         * Runs a specified BAT file in the background, it is also capable of waiting for completetion
         * for programs like diskpart that dont like to be instanced. there is also a manually placed delay 
         * regardless for similar reasons although cmd has less issues thus the set delay 
         * takes a filename and a boolean as to whether it needs to wait, i.e
         * "flash.BAT",true
         * */
        private void runBATFile(string filename,bool wait)
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
                    console.Text += "Access denied to "+directory.Name+"\n";
                }
                catch (Exception)
                {
                    console.Text += "Unknown error at " + directory.Name + "\n";

                }
            }
            return directoryNode;
            
        }
        //string to bool
        //converts plain text into boolean values
        // importantly it only looks for "true" and assumes all else is false
        private bool stringToBool(string s)
        {
            return s.ToLower().Equals("true");
        }
        /*
         * Display Help
         * Does exactly what one might think, deploys a help screen in a new window/form
         * */
        private void displayHelp(object sender, EventArgs e)
        {
            var form = new Help();
            form.Show();
        }
    }

}
