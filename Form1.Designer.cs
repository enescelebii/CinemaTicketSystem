namespace CinemaTicketSystem
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panelSeats = new System.Windows.Forms.Panel();
            this.KaydetButonu = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SelectedCount = new System.Windows.Forms.Label();
            this.labelSelectedSeats = new System.Windows.Forms.Timer(this.components);
            this.logout = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblTime = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.SystemColors.MenuText;
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.Font = new System.Drawing.Font("Lucida Calligraphy", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.ForeColor = System.Drawing.Color.Red;
            this.treeView1.Indent = 30;
            this.treeView1.ItemHeight = 30;
            this.treeView1.LineColor = System.Drawing.Color.DarkGreen;
            this.treeView1.Location = new System.Drawing.Point(3, 142);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(323, 551);
            this.treeView1.TabIndex = 1;
            // 
            // panelSeats
            // 
            this.panelSeats.AutoSize = true;
            this.panelSeats.BackColor = System.Drawing.Color.Black;
            this.panelSeats.Location = new System.Drawing.Point(270, 108);
            this.panelSeats.Name = "panelSeats";
            this.panelSeats.Size = new System.Drawing.Size(947, 585);
            this.panelSeats.TabIndex = 0;
            this.panelSeats.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelSeats_MouseDown);
            this.panelSeats.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanelSeats_MouseMove);
            this.panelSeats.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelSeats_MouseUp);
            // 
            // KaydetButonu
            // 
            this.KaydetButonu.BackColor = System.Drawing.Color.Orange;
            this.KaydetButonu.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.KaydetButonu.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.KaydetButonu.Location = new System.Drawing.Point(1265, 176);
            this.KaydetButonu.Name = "KaydetButonu";
            this.KaydetButonu.Size = new System.Drawing.Size(127, 50);
            this.KaydetButonu.TabIndex = 5;
            this.KaydetButonu.Text = "Kaydet";
            this.KaydetButonu.UseVisualStyleBackColor = false;
            this.KaydetButonu.Click += new System.EventHandler(this.KaydetButonu_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.BackColor = System.Drawing.Color.IndianRed;
            this.CancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.CancelBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.CancelBtn.Location = new System.Drawing.Point(1265, 120);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(127, 50);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Temizle";
            this.CancelBtn.UseVisualStyleBackColor = false;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // SelectedCount
            // 
            this.SelectedCount.AutoSize = true;
            this.SelectedCount.BackColor = System.Drawing.SystemColors.Desktop;
            this.SelectedCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.SelectedCount.ForeColor = System.Drawing.Color.Red;
            this.SelectedCount.Location = new System.Drawing.Point(1205, 76);
            this.SelectedCount.Name = "SelectedCount";
            this.SelectedCount.Size = new System.Drawing.Size(187, 29);
            this.SelectedCount.TabIndex = 0;
            this.SelectedCount.Text = "Seçili Koltuk: 0";
            // 
            // labelSelectedSeats
            // 
            this.labelSelectedSeats.Enabled = true;
            this.labelSelectedSeats.Interval = 1000;
            // 
            // logout
            // 
            this.logout.BackColor = System.Drawing.Color.Orange;
            this.logout.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.logout.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.logout.Location = new System.Drawing.Point(1265, 232);
            this.logout.Name = "logout";
            this.logout.Size = new System.Drawing.Size(127, 50);
            this.logout.TabIndex = 4;
            this.logout.Text = "Çıkış yap";
            this.logout.UseVisualStyleBackColor = false;
            this.logout.Click += new System.EventHandler(this.logout_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::CinemaTicketSystem.Properties.Resources.welcome;
            this.pictureBox1.Location = new System.Drawing.Point(294, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(839, 106);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.BackColor = System.Drawing.Color.Black;
            this.lblTime.ForeColor = System.Drawing.Color.Red;
            this.lblTime.Location = new System.Drawing.Point(1299, 673);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(51, 20);
            this.lblTime.TabIndex = 6;
            this.lblTime.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1413, 705);
            this.ControlBox = false;
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.KaydetButonu);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.logout);
            this.Controls.Add(this.panelSeats);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.SelectedCount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panelSeats;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Label SelectedCount;
        private System.Windows.Forms.Timer labelSelectedSeats;
        private System.Windows.Forms.Button logout;
        private System.Windows.Forms.Button KaydetButonu;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label lblTime;
    }
}

