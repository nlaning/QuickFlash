using System;

using System.IO;
using System.Windows.Forms;
namespace QuickFlash
{
    public partial class MainWindow
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////// FORMS //////////////////////////////////////////////////////
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
        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            if (!searchBox.Text.Equals("Click to Search..."))
            {
                searchFileViewer(searchBox.Text);
            }
        }
        private void SearchBox_LostFocus(object sender, EventArgs e)
        {
            if (searchBox.Text.Equals("")) searchBox.Text = "Click to Search...";
        }
        private void SearchBox_GotFocus(object sender, EventArgs e)
        {
            if (searchBox.Text.Equals("Click to Search..."))
            {
                searchBox.Text = "";
            }
            else
            {
                searchBox.SelectAll();
            }
        }
        private void listDirectory(TreeView treeView, string path)
        {
            try
            {
                treeView.Nodes.Clear();
                var rootDirectoryInfo = new DirectoryInfo(path);
                directoryList = createDirectoryNode(rootDirectoryInfo);
                foreach (TreeNode node in directoryList.Nodes) treeView.Nodes.Add(node);
            }
            catch { };

        }

        private void searchFileViewer(string text)
        {
            fileViewer.Nodes.Clear();
            fileDirectoryMissingPath = "";
            TreeNode newNode = searchDirectory(directoryList, text);
            while (newNode.Nodes.Count == 1)
            {
                TreeNode tempNode = (TreeNode)newNode.Nodes[0].Clone();
                fileDirectoryMissingPath += tempNode.Text + '\\';
                newNode.Nodes.Clear();
                foreach (TreeNode node in tempNode.Nodes) newNode.Nodes.Add((TreeNode)node.Clone());
            }
            foreach (TreeNode node in newNode.Nodes) fileViewer.Nodes.Add(node);


        }

        private TreeNode searchDirectory(TreeNode node, string text)
        {
            TreeNode newNode = new TreeNode();
            newNode.Text = node.Text;
            foreach (TreeNode treenode in node.Nodes)
            {
                if (treenode.Text.ToLower().Contains(text.ToLower()))
                {
                    newNode.Nodes.Add((TreeNode)treenode.Clone());
                }
                else
                {
                    TreeNode directory = searchDirectory(treenode, text);
                    if (directory.Nodes.Count > 0)
                    {
                        newNode.Nodes.Add(directory);
                    }
                }
            }
            return newNode;
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