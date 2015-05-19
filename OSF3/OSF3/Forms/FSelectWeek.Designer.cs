namespace OSF3.Forms
{
	partial class FSelectWeek
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
			this.MenuP = new System.Windows.Forms.Panel();
			this.RB1 = new System.Windows.Forms.RadioButton();
			this.WeekListCB = new System.Windows.Forms.ComboBox();
			this.RB2 = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// MenuP
			// 
			this.MenuP.Location = new System.Drawing.Point(12, 170);
			this.MenuP.Name = "MenuP";
			this.MenuP.Size = new System.Drawing.Size(600, 60);
			this.MenuP.TabIndex = 0;
			// 
			// RB1
			// 
			this.RB1.AutoSize = true;
			this.RB1.Checked = true;
			this.RB1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.RB1.Font = new System.Drawing.Font("Segoe UI Semilight", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RB1.Location = new System.Drawing.Point(39, 51);
			this.RB1.Name = "RB1";
			this.RB1.Size = new System.Drawing.Size(335, 27);
			this.RB1.TabIndex = 1;
			this.RB1.TabStop = true;
			this.RB1.Text = "Select week most relevant to today\'s date";
			this.RB1.UseVisualStyleBackColor = true;
			// 
			// WeekListCB
			// 
			this.WeekListCB.Cursor = System.Windows.Forms.Cursors.Hand;
			this.WeekListCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.WeekListCB.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.WeekListCB.FormattingEnabled = true;
			this.WeekListCB.Location = new System.Drawing.Point(60, 117);
			this.WeekListCB.MaxDropDownItems = 18;
			this.WeekListCB.Name = "WeekListCB";
			this.WeekListCB.Size = new System.Drawing.Size(505, 33);
			this.WeekListCB.TabIndex = 2;
			this.WeekListCB.SelectedIndexChanged += new System.EventHandler(this.WeekListCB_SelectedIndexChanged);
			// 
			// RB2
			// 
			this.RB2.AutoSize = true;
			this.RB2.Cursor = System.Windows.Forms.Cursors.Hand;
			this.RB2.Font = new System.Drawing.Font("Segoe UI Semilight", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RB2.Location = new System.Drawing.Point(39, 84);
			this.RB2.Name = "RB2";
			this.RB2.Size = new System.Drawing.Size(448, 27);
			this.RB2.TabIndex = 3;
			this.RB2.Text = "Select specific semester week in which classes take place";
			this.RB2.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.Orange;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(124, 30);
			this.label1.TabIndex = 4;
			this.label1.Text = "Select week";
			// 
			// FSelectSem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlDark;
			this.ClientSize = new System.Drawing.Size(625, 243);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.RB2);
			this.Controls.Add(this.WeekListCB);
			this.Controls.Add(this.RB1);
			this.Controls.Add(this.MenuP);
			this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ForeColor = System.Drawing.Color.White;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "FSelectSem";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Select semester";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.FSelectSem_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel MenuP;
		private System.Windows.Forms.RadioButton RB1;
		private System.Windows.Forms.ComboBox WeekListCB;
		private System.Windows.Forms.RadioButton RB2;
		private System.Windows.Forms.Label label1;
	}
}