using System;

namespace QuickFlash
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node0");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.fileViewer = new System.Windows.Forms.TreeView();
            this.help = new System.Windows.Forms.Button();
            this.fullLogButton = new System.Windows.Forms.CheckBox();
            this.changeRootDirectoryButton = new System.Windows.Forms.Button();
            this.savePref = new System.Windows.Forms.Button();
            this.manualBootSelectButton = new System.Windows.Forms.CheckBox();
            this.alwaysCleanButton = new System.Windows.Forms.CheckBox();
            this.start = new System.Windows.Forms.Button();
            this.console = new System.Windows.Forms.RichTextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.FlashMagicDrives = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.fileViewer);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.FlashMagicDrives);
            this.splitContainer1.Panel2.Controls.Add(this.help);
            this.splitContainer1.Panel2.Controls.Add(this.fullLogButton);
            this.splitContainer1.Panel2.Controls.Add(this.changeRootDirectoryButton);
            this.splitContainer1.Panel2.Controls.Add(this.savePref);
            this.splitContainer1.Panel2.Controls.Add(this.manualBootSelectButton);
            this.splitContainer1.Panel2.Controls.Add(this.alwaysCleanButton);
            this.splitContainer1.Panel2.Controls.Add(this.start);
            this.splitContainer1.Panel2.Controls.Add(this.console);
            this.splitContainer1.Panel2.Controls.Add(this.progressBar);
            this.splitContainer1.Size = new System.Drawing.Size(609, 507);
            this.splitContainer1.SplitterDistance = 355;
            this.splitContainer1.TabIndex = 0;
            // 
            // fileViewer
            // 
            this.fileViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileViewer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.fileViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileViewer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileViewer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.fileViewer.LineColor = System.Drawing.Color.DarkOliveGreen;
            this.fileViewer.Location = new System.Drawing.Point(0, 0);
            this.fileViewer.Name = "fileViewer";
            treeNode1.Name = "Node0";
            treeNode1.Text = "Node0";
            this.fileViewer.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.fileViewer.Size = new System.Drawing.Size(355, 507);
            this.fileViewer.TabIndex = 0;
            // 
            // help
            // 
            this.help.Location = new System.Drawing.Point(155, 214);
            this.help.Name = "help";
            this.help.Size = new System.Drawing.Size(50, 23);
            this.help.TabIndex = 10;
            this.help.Text = "Help";
            this.help.UseVisualStyleBackColor = true;
            this.help.Click += new System.EventHandler(this.displayHelp);
            // 
            // fullLogButton
            // 
            this.fullLogButton.AutoSize = true;
            this.fullLogButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.fullLogButton.Location = new System.Drawing.Point(16, 162);
            this.fullLogButton.Name = "fullLogButton";
            this.fullLogButton.Size = new System.Drawing.Size(93, 17);
            this.fullLogButton.TabIndex = 9;
            this.fullLogButton.Text = "Show Full Log";
            this.fullLogButton.UseVisualStyleBackColor = true;
            // 
            // changeRootDirectoryButton
            // 
            this.changeRootDirectoryButton.Location = new System.Drawing.Point(54, 110);
            this.changeRootDirectoryButton.Name = "changeRootDirectoryButton";
            this.changeRootDirectoryButton.Size = new System.Drawing.Size(144, 23);
            this.changeRootDirectoryButton.TabIndex = 7;
            this.changeRootDirectoryButton.Text = "Change Root Directory";
            this.changeRootDirectoryButton.UseVisualStyleBackColor = true;
            this.changeRootDirectoryButton.Click += new System.EventHandler(this.browseForFolder);
            // 
            // savePref
            // 
            this.savePref.Location = new System.Drawing.Point(45, 214);
            this.savePref.Name = "savePref";
            this.savePref.Size = new System.Drawing.Size(105, 23);
            this.savePref.TabIndex = 6;
            this.savePref.Text = "Save Preferences";
            this.savePref.UseVisualStyleBackColor = true;
            this.savePref.Click += new System.EventHandler(this.savePreferences);
            // 
            // manualBootSelectButton
            // 
            this.manualBootSelectButton.AutoSize = true;
            this.manualBootSelectButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.manualBootSelectButton.Location = new System.Drawing.Point(39, 139);
            this.manualBootSelectButton.Name = "manualBootSelectButton";
            this.manualBootSelectButton.Size = new System.Drawing.Size(160, 17);
            this.manualBootSelectButton.TabIndex = 5;
            this.manualBootSelectButton.Text = "Manually Select Boot Option";
            this.manualBootSelectButton.UseVisualStyleBackColor = true;
            // 
            // alwaysCleanButton
            // 
            this.alwaysCleanButton.AutoSize = true;
            this.alwaysCleanButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.alwaysCleanButton.Location = new System.Drawing.Point(113, 162);
            this.alwaysCleanButton.Name = "alwaysCleanButton";
            this.alwaysCleanButton.Size = new System.Drawing.Size(122, 17);
            this.alwaysCleanButton.TabIndex = 4;
            this.alwaysCleanButton.Text = "Always Clean Drives";
            this.alwaysCleanButton.UseVisualStyleBackColor = true;
            // 
            // start
            // 
            this.start.BackColor = System.Drawing.Color.Linen;
            this.start.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.start.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.start.Location = new System.Drawing.Point(13, 12);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(222, 59);
            this.start.TabIndex = 0;
            this.start.Text = "Flash Drives";
            this.start.UseVisualStyleBackColor = false;
            this.start.Click += new System.EventHandler(this.startClick);
            // 
            // console
            // 
            this.console.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.console.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.console.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.console.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.console.Location = new System.Drawing.Point(1, 243);
            this.console.Name = "console";
            this.console.ReadOnly = true;
            this.console.Size = new System.Drawing.Size(247, 262);
            this.console.TabIndex = 3;
            this.console.Text = "";
            this.console.WordWrap = false;
            this.console.TextChanged += new System.EventHandler(this.consoleTextChanged);
            // 
            // progressBar
            // 
            this.progressBar.ForeColor = System.Drawing.SystemColors.Control;
            this.progressBar.Location = new System.Drawing.Point(13, 77);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(222, 20);
            this.progressBar.TabIndex = 1;
            // 
            // folderBrowser
            // 
            this.folderBrowser.RootFolder = System.Environment.SpecialFolder.LocalizedResources;
            // 
            // FlashMagicDrives
            // 
            this.FlashMagicDrives.Location = new System.Drawing.Point(69, 185);
            this.FlashMagicDrives.Name = "FlashMagicDrives";
            this.FlashMagicDrives.Size = new System.Drawing.Size(116, 23);
            this.FlashMagicDrives.TabIndex = 11;
            this.FlashMagicDrives.Text = "Flash Magic Drives";
            this.FlashMagicDrives.UseVisualStyleBackColor = true;
            this.FlashMagicDrives.Click += new System.EventHandler(this.flashMagicFlashDrives);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(609, 507);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "Quick Flash 0.04";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }





        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView fileViewer;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.RichTextBox console;
        private System.Windows.Forms.CheckBox alwaysCleanButton;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.CheckBox manualBootSelectButton;
        private System.Windows.Forms.CheckBox fullLogButton;
        private System.Windows.Forms.Button changeRootDirectoryButton;
        private System.Windows.Forms.Button savePref;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
        private System.Windows.Forms.Button FlashMagicDrives;
    }
}

