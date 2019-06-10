using System;

namespace QuickFlash
{
    partial class QueryDriveType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryDriveType));
            this.DOS = new System.Windows.Forms.RadioButton();
            this.UEFI = new System.Windows.Forms.RadioButton();
            this.INSTANT = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.CONTINUE = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DOS
            // 
            this.DOS.AutoSize = true;
            this.DOS.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.DOS.Location = new System.Drawing.Point(98, 47);
            this.DOS.Name = "DOS";
            this.DOS.Size = new System.Drawing.Size(92, 17);
            this.DOS.TabIndex = 0;
            this.DOS.TabStop = true;
            this.DOS.Text = "DOS bootable";
            this.DOS.UseVisualStyleBackColor = true;
            this.DOS.CheckedChanged += new System.EventHandler(this.DOSChecked);
            // 
            // UEFI
            // 
            this.UEFI.AutoSize = true;
            this.UEFI.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.UEFI.Location = new System.Drawing.Point(98, 70);
            this.UEFI.Name = "UEFI";
            this.UEFI.Size = new System.Drawing.Size(93, 17);
            this.UEFI.TabIndex = 1;
            this.UEFI.TabStop = true;
            this.UEFI.Text = "UEFI bootable";
            this.UEFI.UseVisualStyleBackColor = true;
            this.UEFI.CheckedChanged += new System.EventHandler(this.UEFIChecked);
            // 
            // INSTANT
            // 
            this.INSTANT.AutoSize = true;
            this.INSTANT.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.INSTANT.Location = new System.Drawing.Point(102, 93);
            this.INSTANT.Name = "INSTANT";
            this.INSTANT.Size = new System.Drawing.Size(85, 17);
            this.INSTANT.TabIndex = 2;
            this.INSTANT.TabStop = true;
            this.INSTANT.Text = "Instant Flash";
            this.INSTANT.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.INSTANT.UseVisualStyleBackColor = true;
            this.INSTANT.CheckedChanged += new System.EventHandler(this.InstantChecked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Please select the type of drive you would like to create";
            // 
            // CONTINUE
            // 
            this.CONTINUE.Location = new System.Drawing.Point(107, 125);
            this.CONTINUE.Name = "CONTINUE";
            this.CONTINUE.Size = new System.Drawing.Size(75, 23);
            this.CONTINUE.TabIndex = 4;
            this.CONTINUE.Text = "Continue";
            this.CONTINUE.UseVisualStyleBackColor = true;
            this.CONTINUE.Click += new System.EventHandler(this.continueSelected);
            // 
            // QueryDriveType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(46)))));
            this.ClientSize = new System.Drawing.Size(285, 170);
            this.Controls.Add(this.CONTINUE);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.INSTANT);
            this.Controls.Add(this.UEFI);
            this.Controls.Add(this.DOS);
            this.ForeColor = System.Drawing.SystemColors.MenuText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "QueryDriveType";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Select Drive Type";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }





        #endregion

        private System.Windows.Forms.RadioButton DOS;
        private System.Windows.Forms.RadioButton UEFI;
        private System.Windows.Forms.RadioButton INSTANT;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CONTINUE;
    }
}