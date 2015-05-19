namespace OSF3.Forms
{
	partial class FStickerExporter
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
			this.label1 = new System.Windows.Forms.Label();
			this.MenuP = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.ExporterThread = new System.ComponentModel.BackgroundWorker();
			this.label4 = new System.Windows.Forms.Label();
			this.StatusL = new System.Windows.Forms.Label();
			this.UsernameTB = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.Orange;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(428, 30);
			this.label1.TabIndex = 6;
			this.label1.Text = "Discipline/class type sticker image exporter";
			// 
			// MenuP
			// 
			this.MenuP.Location = new System.Drawing.Point(12, 195);
			this.MenuP.Name = "MenuP";
			this.MenuP.Size = new System.Drawing.Size(600, 60);
			this.MenuP.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(33, 55);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(461, 21);
			this.label2.TabIndex = 7;
			this.label2.Text = "This will export .PNG images for each class type of each discipline.";
			// 
			// ExporterThread
			// 
			this.ExporterThread.WorkerReportsProgress = true;
			this.ExporterThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ExporterThread_DoWork);
			this.ExporterThread.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.ExporterThread_ProgressChanged);
			this.ExporterThread.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ExporterThread_RunWorkerCompleted);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(33, 152);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(55, 21);
			this.label4.TabIndex = 12;
			this.label4.Text = "Status:";
			// 
			// StatusL
			// 
			this.StatusL.AutoSize = true;
			this.StatusL.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.StatusL.Location = new System.Drawing.Point(94, 152);
			this.StatusL.Name = "StatusL";
			this.StatusL.Size = new System.Drawing.Size(59, 21);
			this.StatusL.TabIndex = 13;
			this.StatusL.Text = "Status:";
			this.StatusL.Click += new System.EventHandler(this.StatusL_Click);
			// 
			// UsernameTB
			// 
			this.UsernameTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.UsernameTB.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.UsernameTB.Location = new System.Drawing.Point(126, 113);
			this.UsernameTB.Name = "UsernameTB";
			this.UsernameTB.Size = new System.Drawing.Size(137, 22);
			this.UsernameTB.TabIndex = 15;
			this.UsernameTB.Text = "unknown";
			this.UsernameTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(33, 113);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(87, 21);
			this.label5.TabIndex = 14;
			this.label5.Text = "User name:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(33, 76);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(437, 21);
			this.label3.TabIndex = 16;
			this.label3.Text = "Note: you should get into the code if you don\'t like the output.";
			// 
			// FStickerExporter
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlDark;
			this.ClientSize = new System.Drawing.Size(626, 268);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.UsernameTB);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.StatusL);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.MenuP);
			this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ForeColor = System.Drawing.Color.White;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "FStickerExporter";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Sticker exporter";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.FStickerExporter_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel MenuP;
		private System.Windows.Forms.Label label2;
		private System.ComponentModel.BackgroundWorker ExporterThread;
		private System.Windows.Forms.Label label4;
		public System.Windows.Forms.Label StatusL;
		private System.Windows.Forms.TextBox UsernameTB;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label3;
	}
}