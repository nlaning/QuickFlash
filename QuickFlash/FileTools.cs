using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
public static class FileTools
{
    /* Compare Files
   * Compares files to determine whether or not they are the same
   * currrently only checks size, has the ability to check each byte but is cuurently disabled
   * due to how long it takes. But I left it in here incase more precision is ever needed
   * 
   * */
    public static bool compareFiles(string filePath1, string filePath2)
    {
        if (filePath1==null || filePath2 == null) return false;
        FileInfo File1 = new FileInfo(filePath1);
        FileInfo File2 = new FileInfo(filePath2);
        if (!File1.Exists || !File2.Exists) return false;
        if (File1.Length != File2.Length) return false;
        if (File1.FullName != File2.FullName) return false;
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
    public static List<string> getFilePath(string path)
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

        }
        return allFiles;
    }

    public static List<string> getFilesLines(string file)
    {
        List<string> fileLines = new List<string>();
        if (File.Exists(file))
        {
            string[] lines = File.ReadAllLines(file);
            for (int i = 0; i < lines.Length; i++)
            {
                fileLines.Add (lines[i]);
            }
        }
        return fileLines;
    }

    /* Run BAT file
    * Runs a specified BAT file in the background, it is also capable of waiting for completetion
    * for programs like diskpart that dont like to be instanced. there is also a manually placed delay 
    * regardless for similar reasons although cmd has less issues thus the set delay 
    * takes a filename and a boolean as to whether it needs to wait, i.e
    * "flash.BAT",true
    * */
    public static void runBATFile(string filename, bool wait)
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
    public static void createFile(String name, String[] lines)
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
    /* Override Rights
     * 
     * 
     * */
    public static void overrideAllRights(string path)
    {

        List<string> fileList = getFilePath(path);
        foreach (string file in fileList)
        {
            try
            {
                File.SetAttributes(file, FileAttributes.Normal);
            }
            catch
            {};
        }
    }
    /*Remove Old Files
     * Removes files that exist in one directory but not the other, excluding key system files
     * 
     * */
    public static void removeOldFiles(string sourceDirectory, string installerType, int progress, BackgroundWorker worker)
    {

        List<string> Exclusions = new List<string>();
        Exclusions.Add("IndexerVolumeGuid");
        Exclusions.Add("WPSettings.dat");
        //Exclusions.Add("ldlinux.c32");
        //Exclusions.Add("ldlinux.sys");
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
                worker.ReportProgress(progress, "Cleaning " + hostDirectory);
                List<string> fileList = getFilePath(D.Name);

                foreach (string file in fileList)
                {
                    //console.Text += file + "\n";
                    if (!File.Exists(sourceDirectory + file.Remove(0, hostDirectory.Length - 1)))
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
                                worker.ReportProgress(progress, "Error occured while Deleting " + file);
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
    public static long getFileSize(string path)
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
    public static void copySelectedFile(string drive,string fullPath)
    {

        string folderToMove = fullPath;
        string[] cmd = { @"xcopy " + '"' + folderToMove + '\\' + "*" + '"' + " " + drive + @" /e /h /i /y /c >log" + drive.Split(':')[0] + ".txt" };
        drive = drive.Split(':')[0];
        createFile("copy" + drive + ".bat", cmd);
        runBATFile("copy" + drive + ".bat", false);

    }

    /*
     * needs to not contain new name, for example
     * 
     * */
    public static void copyDirectory(string sourceDirectory,string destinationDirectory)
    {
        Directory.CreateDirectory(destinationDirectory);
        List<string> directories = new List<string>(Directory.EnumerateDirectories(sourceDirectory));
        List<string> files = new List<string>(Directory.EnumerateFiles(sourceDirectory));
        foreach(string file in files)
        {
            copyFile(file, destinationDirectory);
        }
        foreach(string directory in directories)
        {
            string[] splitSource = directory.Split('\\');
            string DirectoryName = splitSource[splitSource.Length - 1];
            copyDirectory(directory, destinationDirectory +'\\'+ DirectoryName);
        }
    }
    /*
     * needs to not contain new name, for example
     * 
     * */
    public static void copyFile(string sourceFile, string destinationFolder)
    {
        try
        {

            string[] splitSource = sourceFile.Split('\\');
            string fileName = splitSource[splitSource.Length - 1];
            if(!File.Exists(destinationFolder + "\\" + fileName)) File.Copy(sourceFile, destinationFolder + "\\" + fileName);
        }
        catch (UnauthorizedAccessException)
        {
            //File.
            //File.SetAttributes(sourceFile, FileAttributes.Normal);
            //copyFile(sourceFile,  destinationFolder);
        }
    }

}