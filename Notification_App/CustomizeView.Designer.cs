namespace Notification_App
{
    partial class CustomizeView
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
			System.Windows.Forms.PictureBox pictureBox1;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomizeView));
			this.RefID = new System.Windows.Forms.Label();
			this.labUname = new System.Windows.Forms.Label();
			this.checkCurrentID = new System.Windows.Forms.CheckBox();
			this.checkUseNewID = new System.Windows.Forms.CheckBox();
			this.textCurrentID = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.labAutoID = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.btnApply = new System.Windows.Forms.Button();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.checkBoxAlradyEx = new System.Windows.Forms.CheckBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.linkBack = new System.Windows.Forms.LinkLabel();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.textAppID = new System.Windows.Forms.TextBox();
			this.cmGroup = new System.Windows.Forms.ComboBox();
			this.labelGroup = new System.Windows.Forms.Label();
			this.labelExcicution = new System.Windows.Forms.Label();
			this.textExcicution = new System.Windows.Forms.TextBox();
			pictureBox1 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
			pictureBox1.BackColor = System.Drawing.Color.Transparent;
			pictureBox1.Image = global::CCL_Notification.Properties.Resources.Court;
			pictureBox1.Location = new System.Drawing.Point(3, 193);
			pictureBox1.Margin = new System.Windows.Forms.Padding(0);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new System.Drawing.Size(182, 26);
			pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			pictureBox1.TabIndex = 13;
			pictureBox1.TabStop = false;
			// 
			// RefID
			// 
			this.RefID.AutoSize = true;
			this.RefID.Location = new System.Drawing.Point(9, 1);
			this.RefID.Name = "RefID";
			this.RefID.Size = new System.Drawing.Size(13, 13);
			this.RefID.TabIndex = 0;
			this.RefID.Text = "0";
			this.RefID.Visible = false;
			// 
			// labUname
			// 
			this.labUname.AutoSize = true;
			this.labUname.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labUname.ForeColor = System.Drawing.Color.FloralWhite;
			this.labUname.Location = new System.Drawing.Point(103, 30);
			this.labUname.Name = "labUname";
			this.labUname.Size = new System.Drawing.Size(14, 16);
			this.labUname.TabIndex = 3;
			this.labUname.Text = "_";
			// 
			// checkCurrentID
			// 
			this.checkCurrentID.AutoSize = true;
			this.checkCurrentID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.checkCurrentID.ForeColor = System.Drawing.SystemColors.ControlLight;
			this.checkCurrentID.Location = new System.Drawing.Point(90, 19);
			this.checkCurrentID.Name = "checkCurrentID";
			this.checkCurrentID.Size = new System.Drawing.Size(120, 19);
			this.checkCurrentID.TabIndex = 6;
			this.checkCurrentID.Text = "Use Current ID";
			this.checkCurrentID.UseVisualStyleBackColor = true;
			this.checkCurrentID.CheckedChanged += new System.EventHandler(this.checkCurrentID_CheckedChanged);
			// 
			// checkUseNewID
			// 
			this.checkUseNewID.AutoSize = true;
			this.checkUseNewID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.checkUseNewID.Location = new System.Drawing.Point(91, 52);
			this.checkUseNewID.Name = "checkUseNewID";
			this.checkUseNewID.Size = new System.Drawing.Size(101, 19);
			this.checkUseNewID.TabIndex = 7;
			this.checkUseNewID.Text = "Use New ID";
			this.checkUseNewID.UseVisualStyleBackColor = true;
			this.checkUseNewID.CheckedChanged += new System.EventHandler(this.checkUseNewID_CheckedChanged);
			// 
			// textCurrentID
			// 
			this.textCurrentID.Location = new System.Drawing.Point(6, 19);
			this.textCurrentID.Name = "textCurrentID";
			this.textCurrentID.Size = new System.Drawing.Size(73, 20);
			this.textCurrentID.TabIndex = 8;
			this.textCurrentID.Visible = false;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.textCurrentID);
			this.groupBox1.Controls.Add(this.checkUseNewID);
			this.groupBox1.Controls.Add(this.checkCurrentID);
			this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlLight;
			this.groupBox1.Location = new System.Drawing.Point(208, 1);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(219, 15);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "connection";
			this.groupBox1.Visible = false;
			// 
			// labAutoID
			// 
			this.labAutoID.AutoSize = true;
			this.labAutoID.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labAutoID.ForeColor = System.Drawing.Color.FloralWhite;
			this.labAutoID.Location = new System.Drawing.Point(106, 8);
			this.labAutoID.Name = "labAutoID";
			this.labAutoID.Size = new System.Drawing.Size(14, 16);
			this.labAutoID.TabIndex = 10;
			this.labAutoID.Text = "0";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.FloralWhite;
			this.label2.Location = new System.Drawing.Point(12, 29);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(92, 16);
			this.label2.TabIndex = 11;
			this.label2.Text = "User Name   :";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.ForeColor = System.Drawing.Color.FloralWhite;
			this.label3.Location = new System.Drawing.Point(11, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(92, 16);
			this.label3.TabIndex = 12;
			this.label3.Text = "ID                 :";
			this.label3.Click += new System.EventHandler(this.label3_Click);
			// 
			// btnApply
			// 
			this.btnApply.BackColor = System.Drawing.Color.Transparent;
			this.btnApply.BackgroundImage = global::CCL_Notification.Properties.Resources.Apply_removebg_preview_removebg_preview;
			this.btnApply.Enabled = false;
			this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnApply.ForeColor = System.Drawing.SystemColors.Control;
			this.btnApply.Image = global::CCL_Notification.Properties.Resources.Apply_removebg_preview_removebg_preview;
			this.btnApply.Location = new System.Drawing.Point(352, 193);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(75, 26);
			this.btnApply.TabIndex = 1;
			this.btnApply.UseVisualStyleBackColor = false;
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel1.Font = new System.Drawing.Font("Segoe UI Variable Static Small", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.flowLayoutPanel1.ForeColor = System.Drawing.SystemColors.Info;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(15, 89);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(403, 102);
			this.flowLayoutPanel1.TabIndex = 14;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.panel1.Controls.Add(this.checkBox2);
			this.panel1.Controls.Add(this.checkBoxAlradyEx);
			this.panel1.Location = new System.Drawing.Point(74, 84);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(293, 85);
			this.panel1.TabIndex = 15;
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.Font = new System.Drawing.Font("Bernard MT Condensed", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.checkBox2.ForeColor = System.Drawing.SystemColors.Control;
			this.checkBox2.Location = new System.Drawing.Point(168, 25);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(107, 22);
			this.checkBox2.TabIndex = 1;
			this.checkBox2.Text = "Create New ID";
			this.checkBox2.UseVisualStyleBackColor = true;
			this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
			// 
			// checkBoxAlradyEx
			// 
			this.checkBoxAlradyEx.AutoSize = true;
			this.checkBoxAlradyEx.Font = new System.Drawing.Font("Bernard MT Condensed", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.checkBoxAlradyEx.ForeColor = System.Drawing.SystemColors.Control;
			this.checkBoxAlradyEx.Location = new System.Drawing.Point(24, 25);
			this.checkBoxAlradyEx.Name = "checkBoxAlradyEx";
			this.checkBoxAlradyEx.Size = new System.Drawing.Size(115, 22);
			this.checkBoxAlradyEx.TabIndex = 0;
			this.checkBoxAlradyEx.Text = "Already Exist ID";
			this.checkBoxAlradyEx.UseVisualStyleBackColor = true;
			this.checkBoxAlradyEx.CheckedChanged += new System.EventHandler(this.checkBoxAlradyEx_CheckedChanged);
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.Transparent;
			this.panel2.Controls.Add(this.linkBack);
			this.panel2.Controls.Add(this.linkLabel1);
			this.panel2.Controls.Add(this.label1);
			this.panel2.Controls.Add(this.textAppID);
			this.panel2.Location = new System.Drawing.Point(77, 81);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(293, 85);
			this.panel2.TabIndex = 16;
			this.panel2.Visible = false;
			// 
			// linkBack
			// 
			this.linkBack.AutoSize = true;
			this.linkBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.linkBack.Location = new System.Drawing.Point(60, 60);
			this.linkBack.Name = "linkBack";
			this.linkBack.Size = new System.Drawing.Size(90, 16);
			this.linkBack.TabIndex = 3;
			this.linkBack.TabStop = true;
			this.linkBack.Text = "<<<<< BACK";
			this.linkBack.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkBack_LinkClicked);
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.linkLabel1.LinkColor = System.Drawing.Color.Red;
			this.linkLabel1.Location = new System.Drawing.Point(149, 60);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(91, 16);
			this.linkLabel1.TabIndex = 2;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "NEXT >>>>>";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.label1.Location = new System.Drawing.Point(80, 1);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(135, 18);
			this.label1.TabIndex = 1;
			this.label1.Text = "APPLICATION ID";
			// 
			// textAppID
			// 
			this.textAppID.BackColor = System.Drawing.SystemColors.InactiveCaption;
			this.textAppID.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textAppID.Location = new System.Drawing.Point(100, 31);
			this.textAppID.Name = "textAppID";
			this.textAppID.Size = new System.Drawing.Size(91, 24);
			this.textAppID.TabIndex = 0;
			this.textAppID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// cmGroup
			// 
			this.cmGroup.Cursor = System.Windows.Forms.Cursors.Hand;
			this.cmGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmGroup.Font = new System.Drawing.Font("Cambria", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmGroup.FormattingEnabled = true;
			this.cmGroup.Location = new System.Drawing.Point(323, 22);
			this.cmGroup.Name = "cmGroup";
			this.cmGroup.Size = new System.Drawing.Size(95, 23);
			this.cmGroup.TabIndex = 27;
			this.cmGroup.SelectedIndexChanged += new System.EventHandler(this.cmGroup_SelectedIndexChanged);
			this.cmGroup.SelectedValueChanged += new System.EventHandler(this.cmGroup_SelectedValueChanged);
			// 
			// labelGroup
			// 
			this.labelGroup.AutoSize = true;
			this.labelGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelGroup.ForeColor = System.Drawing.SystemColors.Control;
			this.labelGroup.Location = new System.Drawing.Point(268, 27);
			this.labelGroup.Name = "labelGroup";
			this.labelGroup.Size = new System.Drawing.Size(49, 13);
			this.labelGroup.TabIndex = 28;
			this.labelGroup.Text = "Group :";
			// 
			// labelExcicution
			// 
			this.labelExcicution.AutoSize = true;
			this.labelExcicution.BackColor = System.Drawing.Color.Transparent;
			this.labelExcicution.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelExcicution.ForeColor = System.Drawing.SystemColors.Control;
			this.labelExcicution.Location = new System.Drawing.Point(217, 56);
			this.labelExcicution.Name = "labelExcicution";
			this.labelExcicution.Size = new System.Drawing.Size(102, 13);
			this.labelExcicution.TabIndex = 29;
			this.labelExcicution.Text = "Execution Time :";
			// 
			// textExcicution
			// 
			this.textExcicution.Location = new System.Drawing.Point(323, 52);
			this.textExcicution.Name = "textExcicution";
			this.textExcicution.Size = new System.Drawing.Size(34, 20);
			this.textExcicution.TabIndex = 30;
			this.textExcicution.Text = "1";
			// 
			// CustomizeView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(439, 223);
			this.Controls.Add(this.textExcicution);
			this.Controls.Add(this.labelExcicution);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.labelGroup);
			this.Controls.Add(this.cmGroup);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(pictureBox1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.labAutoID);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.labUname);
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.RefID);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "CustomizeView";
			this.Text = "CustomizeView";
			this.Load += new System.EventHandler(this.CustomizeView_Load);
			((System.ComponentModel.ISupportInitialize)(pictureBox1)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label RefID;
        private System.Windows.Forms.Label labUname;
        private System.Windows.Forms.CheckBox checkCurrentID;
        private System.Windows.Forms.CheckBox checkUseNewID;
        private System.Windows.Forms.TextBox textCurrentID;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labAutoID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBoxAlradyEx;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textAppID;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkBack;
        private System.Windows.Forms.ComboBox cmGroup;
        private System.Windows.Forms.Label labelGroup;
        private System.Windows.Forms.Label labelExcicution;
        private System.Windows.Forms.TextBox textExcicution;
    }
}