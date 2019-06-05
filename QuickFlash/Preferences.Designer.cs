using System;

namespace QuickFlash
{
    partial class Preferences
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
            this.rootDirectoryBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.magicDirectoryBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.directoryLimitBox = new System.Windows.Forms.TextBox();
            this.applyButton = new System.Windows.Forms.Button();
            this.limitLabel = new System.Windows.Forms.Label();
            this.selectDirectoryButton = new System.Windows.Forms.Button();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // rootDirectoryBox
            // 
            this.rootDirectoryBox.Location = new System.Drawing.Point(180, 22);
            this.rootDirectoryBox.Name = "rootDirectoryBox";
            this.rootDirectoryBox.Size = new System.Drawing.Size(112, 20);
            this.rootDirectoryBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(23, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Root Directory";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(23, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Magic Flash Drive Directory";
            // 
            // magicDirectoryBox
            // 
            this.magicDirectoryBox.Location = new System.Drawing.Point(180, 63);
            this.magicDirectoryBox.Name = "magicDirectoryBox";
            this.magicDirectoryBox.Size = new System.Drawing.Size(143, 20);
            this.magicDirectoryBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(23, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Directory Byte Limit";
            // 
            // directoryLimitBox
            // 
            this.directoryLimitBox.Location = new System.Drawing.Point(180, 104);
            this.directoryLimitBox.Name = "directoryLimitBox";
            this.directoryLimitBox.Size = new System.Drawing.Size(144, 20);
            this.directoryLimitBox.TabIndex = 5;
            this.directoryLimitBox.TextChanged += new System.EventHandler(this.limitText);
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(99, 147);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(167, 23);
            this.applyButton.TabIndex = 6;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyChanges);
            // 
            // limitLabel
            // 
            this.limitLabel.AutoSize = true;
            this.limitLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.limitLabel.ForeColor = System.Drawing.Color.Red;
            this.limitLabel.Location = new System.Drawing.Point(119, 127);
            this.limitLabel.Name = "limitLabel";
            this.limitLabel.Size = new System.Drawing.Size(127, 13);
            this.limitLabel.TabIndex = 7;
            this.limitLabel.Text = "Whole Number Required!";
            // 
            // selectDirectoryButton
            // 
            this.selectDirectoryButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.selectDirectoryButton.FlatAppearance.BorderSize = 0;
            this.selectDirectoryButton.Location = new System.Drawing.Point(297, 22);
            this.selectDirectoryButton.Name = "selectDirectoryButton";
            this.selectDirectoryButton.Size = new System.Drawing.Size(26, 21);
            this.selectDirectoryButton.TabIndex = 8;
            this.selectDirectoryButton.Text = "...";
            this.selectDirectoryButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.selectDirectoryButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.selectDirectoryButton.UseVisualStyleBackColor = true;
            this.selectDirectoryButton.Click += new System.EventHandler(SelectDirectory);
            // 
            // folderBrowser
            // 
            this.folderBrowser.RootFolder = System.Environment.SpecialFolder.LocalizedResources;
            // 
            // Preferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(351, 188);
            this.Controls.Add(this.selectDirectoryButton);
            this.Controls.Add(this.limitLabel);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.directoryLimitBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.magicDirectoryBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rootDirectoryBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Preferences";
            this.Text = "Preferences";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void limitText(object sender, EventArgs e)
        {
            limitLabel.Visible = false;
        }



        #endregion

        private System.Windows.Forms.TextBox rootDirectoryBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox magicDirectoryBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox directoryLimitBox;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Label limitLabel;
        private System.Windows.Forms.Button selectDirectoryButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
    }
}