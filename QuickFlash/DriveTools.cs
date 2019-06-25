using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using static FileTools;
namespace QuickFlash {
public partial class MainWindow
    {
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
        public string determineType()
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
        public void DOSBoot(string drive)
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
        public void activateDrive(string drive, string number)
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

        public void formatDrive(string letter, string number)
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


        }

        /* Clean Drive
         * Cleans all files off of the drive in question, this can be helpful when
         * loading two different versions of the same bios on one drive and is instigated
         * using the "always clean" button
         * 
         * */
        public void cleanDrive(string drive)
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
        public void loadDrive(string drive, string Type, string number)
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
            copySelectedFile(drive,fullPath);

        }

        /* Format Drives
         * Simply formats all drives, a nice feature just to have available for various reasons
         * 
         * */
        public void FormatDrives(object sender, EventArgs e)
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
                    overrideAllRights(D.Name);
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
    }

    
}