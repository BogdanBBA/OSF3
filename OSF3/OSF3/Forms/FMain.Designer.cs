namespace OSF3
{
	partial class FMain
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMain));
			this.MenuP = new System.Windows.Forms.Panel();
			this.ContentsP = new System.Windows.Forms.Panel();
			this.ClassesP = new System.Windows.Forms.Panel();
			this.DateP = new System.Windows.Forms.Panel();
			this.TimeP = new System.Windows.Forms.Panel();
			this.ContentsP.SuspendLayout();
			this.SuspendLayout();
			// 
			// MenuP
			// 
			this.MenuP.Location = new System.Drawing.Point(45, 36);
			this.MenuP.Name = "MenuP";
			this.MenuP.Size = new System.Drawing.Size(200, 100);
			this.MenuP.TabIndex = 0;
			// 
			// ContentsP
			// 
			this.ContentsP.Controls.Add(this.ClassesP);
			this.ContentsP.Controls.Add(this.DateP);
			this.ContentsP.Controls.Add(this.TimeP);
			this.ContentsP.Location = new System.Drawing.Point(45, 154);
			this.ContentsP.Name = "ContentsP";
			this.ContentsP.Size = new System.Drawing.Size(582, 228);
			this.ContentsP.TabIndex = 1;
			// 
			// ClassesP
			// 
			this.ClassesP.Location = new System.Drawing.Point(245, 119);
			this.ClassesP.Name = "ClassesP";
			this.ClassesP.Size = new System.Drawing.Size(200, 100);
			this.ClassesP.TabIndex = 3;
			// 
			// DateP
			// 
			this.DateP.Location = new System.Drawing.Point(245, 13);
			this.DateP.Name = "DateP";
			this.DateP.Size = new System.Drawing.Size(200, 100);
			this.DateP.TabIndex = 2;
			// 
			// TimeP
			// 
			this.TimeP.Location = new System.Drawing.Point(12, 60);
			this.TimeP.Name = "TimeP";
			this.TimeP.Size = new System.Drawing.Size(200, 100);
			this.TimeP.TabIndex = 1;
			// 
			// FMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlDark;
			this.ClientSize = new System.Drawing.Size(721, 394);
			this.ControlBox = false;
			this.Controls.Add(this.ContentsP);
			this.Controls.Add(this.MenuP);
			this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ForeColor = System.Drawing.Color.White;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "FMain";
			this.Text = "OSF3";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.FMain_Load);
			this.ContentsP.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel MenuP;
		private System.Windows.Forms.Panel ContentsP;
		private System.Windows.Forms.Panel DateP;
		private System.Windows.Forms.Panel TimeP;
		public System.Windows.Forms.Panel ClassesP;
	}
}

