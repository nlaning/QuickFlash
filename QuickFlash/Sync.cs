using System.Collections.Generic;
using System.ComponentModel;
using static FileTools;
namespace QuickFlash
{
    public partial class MainWindow
    {
        public void EUsyncBiosFolder(object sender, DoWorkEventArgs e) 
        {
            int delay = System.Convert.ToInt32(serverPollingRate);
            while (true)
            {
                copyDirectory(EUpath, path);
                System.Threading.Thread.Sleep(delay);
            }
            //all child folders to continue forward onto
            //List<string> childFolders = union(strippedDestinationDirectories, strippedSourceDirectories); 
            
        }

        public List<string> removeAllText(List<string> list,string text)
        {
            List<string> strippedList = new List<string>();
            foreach (string item in list)
            {
                strippedList.Add(item.Replace(text, ""));
            }
            return strippedList;
        }

        public List<E> symmetricDifference<E>(List<E> A, List<E> B)
        {
            List<E> result = new List<E>();
            foreach(E item in A)
            {
                if (!B.Contains(item)) result.Add(item); 
            }
            foreach (E item in B)
            {
                if (!A.Contains(item)) result.Add(item);
            }
            return result;
        }

        public List<E> union<E>(List<E> A, List<E> B)
        {
            List<E> result = new List<E>();
            foreach(E item in A)
            {
                if( B.Contains(item)) result.Add(item);
            }

            return result;
        }
    }
}