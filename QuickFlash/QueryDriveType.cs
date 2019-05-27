using System;
using System.Windows.Forms;

namespace QuickFlash
{
    
    public partial class QueryDriveType : Form
    {
        public string returnValue { get; set; }
        public QueryDriveType()
        {
            InitializeComponent();
        }

        private void DOSChecked(object sender, EventArgs e)
        {
            if(DOS.Checked)
            {
                UEFI.Checked = false;
                INSTANT.Checked = false;
            }
        }

        private void continueSelected(object sender, EventArgs e)
        {
            if (DOS.Checked)
            {
                returnValue = "DOS";
                Close();
                DialogResult = DialogResult.OK;
            }
            if (UEFI.Checked)
            {
                returnValue = "UEFI";
                Close();
                DialogResult = DialogResult.OK;
            }
            if (INSTANT.Checked)
            {
                returnValue = "Instant";
                Close();
                DialogResult = DialogResult.OK;
            }
        }

        private void InstantChecked(object sender, EventArgs e)
        {
            if (INSTANT.Checked)
            {
                UEFI.Checked = false;
                DOS.Checked = false;
            }
        }

        private void UEFIChecked(object sender, EventArgs e)
        {
            if (UEFI.Checked)
            {
                DOS.Checked = false;
                INSTANT.Checked = false;
            }
        }
    }
}
