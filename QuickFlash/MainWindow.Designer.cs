﻿using System;

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
            this.fullLogButton = new System.Windows.Forms.CheckBox();
            this.manualBootSelectButton = new System.Windows.Forms.CheckBox();
            this.alwaysCleanButton = new System.Windows.Forms.CheckBox();
            this.start = new System.Windows.Forms.Button();
            this.console = new System.Windows.Forms.RichTextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeRootDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateMagicFlashDrivesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.formatDrivesFAT32ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.outputConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
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
            this.splitContainer1.Panel2.Controls.Add(this.fullLogButton);
            this.splitContainer1.Panel2.Controls.Add(this.manualBootSelectButton);
            this.splitContainer1.Panel2.Controls.Add(this.alwaysCleanButton);
            this.splitContainer1.Panel2.Controls.Add(this.start);
            this.splitContainer1.Panel2.Controls.Add(this.console);
            this.splitContainer1.Panel2.Controls.Add(this.progressBar);
            this.splitContainer1.Panel2.Controls.Add(this.menuStrip1);
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
            // fullLogButton
            // 
            this.fullLogButton.AutoSize = true;
            this.fullLogButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.fullLogButton.Location = new System.Drawing.Point(13, 188);
            this.fullLogButton.Name = "fullLogButton";
            this.fullLogButton.Size = new System.Drawing.Size(93, 17);
            this.fullLogButton.TabIndex = 9;
            this.fullLogButton.Text = "Show Full Log";
            this.fullLogButton.UseVisualStyleBackColor = true;
            // 
            // manualBootSelectButton
            // 
            this.manualBootSelectButton.AutoSize = true;
            this.manualBootSelectButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.manualBootSelectButton.Location = new System.Drawing.Point(38, 165);
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
            this.alwaysCleanButton.Location = new System.Drawing.Point(117, 188);
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
            this.start.Location = new System.Drawing.Point(13, 41);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(222, 73);
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
            this.console.Location = new System.Drawing.Point(1, 229);
            this.console.Name = "console";
            this.console.ReadOnly = true;
            this.console.Size = new System.Drawing.Size(247, 276);
            this.console.TabIndex = 3;
            this.console.Text = "";
            this.console.WordWrap = false;
            this.console.TextChanged += new System.EventHandler(this.consoleTextChanged);
            // 
            // progressBar
            // 
            this.progressBar.ForeColor = System.Drawing.SystemColors.Control;
            this.progressBar.Location = new System.Drawing.Point(13, 127);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(222, 20);
            this.progressBar.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(250, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.preferencesToolStripMenuItem,
            this.changeRootDirectoryToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.preferencesToolStripMenuItem.Text = "Change Preferences";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.displayPreferences);
            // 
            // changeRootDirectoryToolStripMenuItem
            // 
            this.changeRootDirectoryToolStripMenuItem.Name = "changeRootDirectoryToolStripMenuItem";
            this.changeRootDirectoryToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.changeRootDirectoryToolStripMenuItem.Text = "Change Root Directory";
            this.changeRootDirectoryToolStripMenuItem.Click += new System.EventHandler(this.browseForFolder);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.displayHelp);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateMagicFlashDrivesToolStripMenuItem,
            this.formatDrivesFAT32ToolStripMenuItem,
            this.outputConsoleToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // updateMagicFlashDrivesToolStripMenuItem
            // 
            this.updateMagicFlashDrivesToolStripMenuItem.Name = "updateMagicFlashDrivesToolStripMenuItem";
            this.updateMagicFlashDrivesToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.updateMagicFlashDrivesToolStripMenuItem.Text = "Update Magic Flash Drives";
            this.updateMagicFlashDrivesToolStripMenuItem.Click += new System.EventHandler(this.flashMagicFlashDrives);
            // 
            // formatDrivesFAT32ToolStripMenuItem
            // 
            this.formatDrivesFAT32ToolStripMenuItem.Name = "formatDrivesFAT32ToolStripMenuItem";
            this.formatDrivesFAT32ToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.formatDrivesFAT32ToolStripMenuItem.Text = "Format Drives (FAT32)";
            this.formatDrivesFAT32ToolStripMenuItem.Click += new System.EventHandler(this.FormatDrives);
            // 
            // folderBrowser
            // 
            this.folderBrowser.RootFolder = System.Environment.SpecialFolder.LocalizedResources;
            // 
            // outputConsoleToolStripMenuItem
            // 
            this.outputConsoleToolStripMenuItem.Name = "outputConsoleToolStripMenuItem";
            this.outputConsoleToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.outputConsoleToolStripMenuItem.Text = "Output Console";
            this.outputConsoleToolStripMenuItem.Click += new System.EventHandler(this.outputConsole);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(609, 507);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "Quick Flash 0.04";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeRootDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateMagicFlashDrivesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem formatDrivesFAT32ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outputConsoleToolStripMenuItem;
    }
}

