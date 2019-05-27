using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace QuickFlash
{
    public partial class MainWindow : Form

    {
        double increment = 0;
        String path = @"C:\";
        string magicDrivePath = "";
        List<string> drives = new List<String>();
        List<string> driveType = new List<String>();
        string fullPath = "";
        string preferencesFile = "pref.txt";
        DriveInfo[] allDrives;
        String version = "1.04";
        
        public MainWindow()
        {
            InitializeComponent();
            
            //Manual select button, full log, and help temporarily disabled until implemented
            manualBootSelectButton.Enabled = false;
            fullLogButton.Enabled = false;
            //end
            this.Text = "QuickFlash " + version;
            console.Text += "Welcome to Quick Flash! (" + version + ")\nPlease select a file to begin or press " + '"' + "help" + '"' + "\n";

            loadPreferences("Pref.txt");
            ListDirectory(fileViewer, path);
        }
        //string to bool
        //converts plain text into boolean values
        // importantly it only looks for "true" and assumes all else is false
        private bool stringToBool(string s)
        {
            return s.ToLower().Equals("true");
        }
        /*load preferences
         * loads preferences from specified file in txt format
         * the default for this instance is pref.txt
         * it then uses the data to populate a very specific set of variables
         * this allows any user to alter settings easily outside of the program
         * incase of an in-operable state
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
            CreateFile(preferencesFile, preferences);
        }
        //basic folder browser dialogue to select new root folder
        private void browseForFolder(object sender, EventArgs e)
        {
            DialogResult result = folderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                console.Text += "New directory selected at:\n" + folderBrowser.SelectedPath + "\n";
                path = folderBrowser.SelectedPath;
                ListDirectory(fileViewer, path);
            }
        }

        private void flashMagicFlashDrives(object sender, EventArgs e)
        {
            if (!magicDrivePath.Equals(""))
            {
                allDrives = DriveInfo.GetDrives();
                console.Text += "Gathering Magic Flash Drives...\n";
                foreach (DriveInfo D in allDrives)
                {
                    //determine if thumbdrive
                    if (D.DriveType.ToString().Equals("Removable"))
                    {
                        //gather info
                        string driveLetter = D.Name.ToString().Remove(2);
                        console.Text += "loading Magic Drive " + driveLetter + " with necesary components...\n";
                        //loading drive with components
                        if (fullLogButton.Checked) console.Text += "moving files from " + fullPath + " ...\n";
                        string[] cmd = { @"xcopy " + '"' + magicDrivePath + '\\' + "*" + '"' + " " + driveLetter + @" /e /h /i /y /c >log.txt" };
                        CreateFile("copy.bat", cmd);
                        runBATFile("copy.bat");

                    }
                }
            }
            else { console.Text += "Magic Flash Drive directory is not yet saved\nPlease add the directory path to pref.txt\n"; }
        }


        /*start Click
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
            //defaulting the format type as we only want ntfs if uefi boot is used
            string FormatType = "FAT32";
            //determining the boot option
            string InstallerType = determineType();
            //if no option selected quit operation entirely
            if (InstallerType != "NONE")
            {
                progressBar.Value = 5;
                //if uefi, flash as ntfs bootable
                if (InstallerType == "UEFI") FormatType = "NTFS";
                //gets all thumbdrives and starts the proccess, making vars aswell
                allDrives = DriveInfo.GetDrives();
                console.Text += "Flash Initiated\n";
                int drive = 0;
                increment = allDrives.Length / 95.0;
                if (alwaysCleanButton.Checked) increment /= 2.0;
                //increments through all drives
                foreach (DriveInfo D in allDrives)
                {
                    //determine if thumbdrive
                    if (D.DriveType.ToString().Equals("Removable"))
                    {
                        //gather info
                        string driveLetter = D.Name.ToString().Remove(2);
                        string driveFormat = D.DriveFormat;
                        //if format type doesnt match, flash drive
                        if (driveFormat != FormatType)
                        {
                            console.Text += "Cleaning and Formating Drive " + driveLetter + "...\n";
                            cleanDrive(driveLetter, drive.ToString(), FormatType);
                        }
                        console.Text += "loading drive " + driveLetter + " with necesary components...\n";
                        //loading drive with components
                        loadDrive(driveLetter, InstallerType);
                    }
                    progressBar.Value += (int)increment;
                    Application.DoEvents();
                    drive++;
                }
                //finishing up
                progressBar.Value = 100;
                console.Text += "Process Complete!\n";
            }
            else console.Text += "Incorrect/no boot method selected\n";

        }
        /**
         * determine Type 
         * determines given the file(s) selected how the software 
         * should behave, looking for four main cases:
         * 1: no files or too large of file(s) tripping a request to pick a new directory
         * 2: determines the directory to be of DOS importance
         * 3: determines the directory to be of EFI importance
         * 4: determines the directory to be of INSTANT importance
         * 5: an undetermined state by which the user will need to select the the type
         * these will output strings in the order following above
         * "NONE","DOS","EFI","INSTANT"
         * the final option will be up to the user to determine the type (only operable option currently)
         **/
        private string determineType()
        {
            //browse all files on drive
            //look for EFI(ntfs),BAT(DOS),EXE(DOS) or neither for instant. if both, query a select

            //query
            using (var form = new QueryDriveType())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    return form.returnValue;
                }
            }
            return "NONE";
        }
        private void displayHelp(object sender, EventArgs e)
        {
            var form = new Help();
            form.Show();
        }
        //loads drives differently depending on type and style
        private void loadDrive(string drive, string Type)
        {
            if (alwaysCleanButton.Checked || Type.Equals("INSTANT"))
            {
                clearDrive(drive);
            }
            if (Type.Equals("DOS"))
            {
                DOSBoot(drive);
            }
            if (Type.Equals("UEFI"))
            {
                UEFIBoot(drive);
            }
            copySelectedFile(drive);

        }

        private void copySelectedFile(string drive)
        {
            if (fullLogButton.Checked) console.Text += "moving files from " + fullPath + " ...\n";
            string folderToMove = fullPath;
            string[] cmd = { @"xcopy " + '"' + folderToMove + '\\' + "*" + '"' + " " + drive + @" /e /h /i /y /c >log.txt" };
            CreateFile("copy.bat", cmd);
            runBATFile("copy.bat");

        }
        private void UEFIBoot(string drive)
        {
            //make bootable...
            //copying files....
        }

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
            CreateFile("DOS.bat", batchLines);
            runBATFile("DOS.bat");
        }
        private void cleanDrive(string letter, string number, string type)
        {
            string partitionStyle = "";
            if (type.Equals("FAT32")) {
                partitionStyle = "convert mbr";
            }
            if (type.Equals("NTFS"))
            {
                partitionStyle = "convert gpt";
            }
            string[] batchLines = {
                "pushd %~dp0",
                "diskpart /s clean.txt > cleanlog.txt" };
            string[] textLines = {
                "select disk " + number,
                "clean",
                partitionStyle,
                "create partition primary",
                "select partition 1",
                "active",
                "format fs =" + type + " quick",
                "assign letter=",letter.Remove(1) };
            CreateFile("clean.txt", textLines);
            CreateFile("clean.bat", batchLines);
            runBATFile("clean.bat");
            if (fullLogButton.Checked)
            {
                outputLog("cleanlog.txt", 6);
            }


        }

        private void clearDrive(string drive)
        {
            progressBar.Value += (int)increment;
            Application.DoEvents();
            console.Text += "clearing out files on drive...\n";
            string[] cmd =
            {
                "del "+drive+@"\* /s /q",
                "rmdir /s/q "+drive
            };
            CreateFile("clear.bat", cmd);
            runBATFile("clear.bat");
        }

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
        private void consoleTextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            console.SelectionStart = console.Text.Length;
            // scroll it automatically
            console.ScrollToCaret();
        }
        private void moveFile()
        {

            runBATFile("Copy.bat");

        }

        private void runBATFile(string filename)
        {
            ProcessStartInfo BatchProcess;
            Process process;
            BatchProcess = new ProcessStartInfo("cmd.exe", "/c " + filename);
            BatchProcess.CreateNoWindow = true;
            BatchProcess.UseShellExecute = false;
            process = Process.Start(BatchProcess);
            process.WaitForExit();
        }
        private void CreateFile(String name, String[] lines)
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
        private void ListDirectory(TreeView treeView, string path)
        {
            try
            {
                treeView.Nodes.Clear();
                var rootDirectoryInfo = new DirectoryInfo(path);
                treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo));
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
        private TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            
                var directoryNode = new TreeNode(directoryInfo.Name);
                foreach (var directory in directoryInfo.GetDirectories())
                {
                try
                {
                    directoryNode.Nodes.Add(CreateDirectoryNode(directory));
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
    }

}
