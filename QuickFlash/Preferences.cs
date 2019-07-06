using System;
using System.Windows.Forms;

namespace QuickFlash
{
    public partial class Preferences : Form
    {
        public string newBiosLocation { get; set; }
        public string EUpath { get; set; }
        public long newByteLimit{ get; set; }
        public int serverPollingRate { get; set; }
        public Preferences(MainWindow currentWindow)
        {
           
            InitializeComponent();
            directoryLimitBox.Text = currentWindow.getlimit().ToString();
            rootDirectoryBox.Text = currentWindow.getfullPath().ToString();
            EUpath_TEXTBOX.Text = currentWindow.getEUPath().ToString();
            SERVER_POLL_TEXTBOX.Text = currentWindow.getserverPollingRate().ToString();
            if (EUpath_TEXTBOX.Text.Equals("")) EUMode.Checked = false; 
        }

        private void applyChanges(object sender, EventArgs e)
        {
            try
            {
                
                newByteLimit= Convert.ToInt64(directoryLimitBox.Text);
                newBiosLocation = rootDirectoryBox.Text;
                EUpath = EUpath_TEXTBOX.Text;
                if (!EUMode.Checked) EUpath = "";
                serverPollingRate = Convert.ToInt32(SERVER_POLL_TEXTBOX.Text);
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

        private void EUMode_CheckedChanged(object sender, EventArgs e)
        {
            if (EUMode.Checked)
            {
                SERVER_POLL_TEXTBOX.Enabled = true;
                EUpath_TEXTBOX.Enabled = true;
                SERVER_POLL_LABEL.ForeColor = System.Drawing.SystemColors.ButtonFace;
                USPath_LABEL.ForeColor = System.Drawing.SystemColors.ButtonFace;
                EUpath_TEXTBOX.BackColor = System.Drawing.SystemColors.Window;
                SERVER_POLL_TEXTBOX.BackColor = System.Drawing.SystemColors.Window;
               
            }
            else
            {
                SERVER_POLL_TEXTBOX.Enabled = false;
                EUpath_TEXTBOX.Enabled = false;
                SERVER_POLL_LABEL.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
                USPath_LABEL.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
                EUpath_TEXTBOX.BackColor = System.Drawing.SystemColors.ScrollBar;
                SERVER_POLL_TEXTBOX.BackColor = System.Drawing.SystemColors.ScrollBar;
            }
        }
    }
}
