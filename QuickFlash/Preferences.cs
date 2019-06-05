using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuickFlash
{
    public partial class Preferences : Form
    {
        public string newBiosLocation { get; set; }
        public string newMagicLocation { get; set; }
        public long newByteLimit{ get; set; }
        public Preferences(string currentBiosLocation, string currentMagicLocation, long currentByteLimit)
        {
            InitializeComponent();
            magicDirectoryBox.Text = currentMagicLocation;
            directoryLimitBox.Text = currentByteLimit.ToString();
            rootDirectoryBox.Text = currentBiosLocation;
        }

        private void applyChanges(object sender, EventArgs e)
        {
            try
            {
                newByteLimit= Convert.ToInt64(directoryLimitBox.Text);
                newMagicLocation = magicDirectoryBox.Text;
                newBiosLocation = rootDirectoryBox.Text;
                Close();
                DialogResult = DialogResult.OK;
            }
            catch (FormatException)
            {
                limitLabel.Visible = true;
            }
            
        }

        private void SelectDirectory(object sender, EventArgs e)
        {
            DialogResult result = folderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                newBiosLocation = folderBrowser.SelectedPath;
                rootDirectoryBox.Text= newBiosLocation;
            }
            else return;
        }
    }
}
