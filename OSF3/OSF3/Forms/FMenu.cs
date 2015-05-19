using OSF3.DataTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSF3.Forms
{
	public partial class FMenu : Form
	{
		private FMain MainForm;

		private List<string> MenuButtonCaptions;
		private List<MyButton> MenuButtons;
		private BorderPictureBox BorderPB;

		public FMenu(FMain mainForm)
		{
			InitializeComponent();
			this.MainForm = mainForm;
			MyGuiComponents.InitializeAndFormatFormComponents(this);
			//
			MenuButtonCaptions = new List<string>();
			MenuButtons = new List<MyButton>();
			BorderPB = new BorderPictureBox(BorderPictureBox.DefaultBorderWidth);
			BorderPB.Parent = this;
		}

		private void FMenu_Load(object sender, EventArgs e)
		{
		}

		public void RefreshMenuForm(List<string> captions, EventHandler menuButton_Click_Event, Point location)
		{
			MenuButtonCaptions.Clear();
			MenuButtonCaptions.AddRange(captions);
			MenuButtonCaptions.Add("CLOSE");
			//
			this.Location = location;
			this.Size = new Size(MyGuiComponents.DefaultMenuButtonSize.Width + 2 * BorderPB.BorderWidth,
				MenuButtonCaptions.Count * MyGuiComponents.DefaultMenuButtonSize.Height + 2 * BorderPB.BorderWidth);
			BorderPB.SetBounds(0, 0, this.Width, this.Height);
			BorderPB.RedrawBorder();
			//
			for (int i = MenuButtonCaptions.Count; i < MenuButtons.Count; i++)
				MenuButtons[i].Hide();
			//
			for (int i = 0; i < MenuButtonCaptions.Count; i++)
			{
				if (i >= MenuButtons.Count)
				{
					MyButton butt = new MyButton("null", null);
					butt.Parent = this;
					butt.SetBounds(BorderPB.BorderWidth, BorderPB.BorderWidth + i * MyGuiComponents.DefaultMenuButtonSize.Height,
						MyGuiComponents.DefaultMenuButtonSize.Width, MyGuiComponents.DefaultMenuButtonSize.Height);
					MenuButtons.Add(butt);
				}
				MenuButtons[i].SetOnClickEventHandler(menuButton_Click_Event);
				MenuButtons[i].Caption = MenuButtonCaptions[i];
				MenuButtons[i].RedrawButton(false);
				MenuButtons[i].Show();
			}
			BorderPB.SendToBack();
			//
			this.Show();
		}

		//
		//
		//

		public void RefreshMainFormData()
		{
			// Dates
			for (int i = 0; i < 5; i++)
			{
				MainForm.DateBoxes[i].Date = MainForm.CurrWeek.Monday.AddDays(i);
				MainForm.DateBoxes[i].RedrawBox(false);
			}
			// Classes
			MainForm.ClassBoxes = MyGuiComponents.RecreateClassBoxesAndDoStuff(MainForm.ClassesP,
				MainForm.CurrSemester, MainForm.CurrWeek, MainForm.ClassBox_Click, MainForm.TimeBox, MainForm.ClassBoxes);
		}

		public void DateBox_Menu_Click(object sender, EventArgs e)
		{
			int r = MenuButtonCaptions.IndexOf((sender as MyButton).Caption);
			this.Hide();
			if (r != MenuButtonCaptions.Count - 1)
			{
				switch (r)
				{
					case 0: // select w
						MainForm.SelectWeekForm.Show();
						MainForm.SelectWeekForm.RefreshWeekList();
						break;
					case 1: // next w						
						if (Utils.IndexOfWeekInWeekList(MainForm.CurrWeek, MainForm.CurrSemester.Weeks) == MainForm.CurrSemester.Weeks.Count - 1)
							MessageBox.Show("This is the last week of the semester that contains classes!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
						else
						{
							int p = Utils.IndexOfWeekInWeekList(MainForm.CurrWeek, MainForm.CurrSemester.Weeks);
							if (p != -1)
								MainForm.CurrWeek = MainForm.CurrSemester.Weeks[p + 1];
							RefreshMainFormData();
						}
						break;
					case 2: // previous w					
						if (Utils.IndexOfWeekInWeekList(MainForm.CurrWeek, MainForm.CurrSemester.Weeks) == 0)
							MessageBox.Show("This is the first week of the semester that contains classes!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
						else
						{
							int p = Utils.IndexOfWeekInWeekList(MainForm.CurrWeek, MainForm.CurrSemester.Weeks);
							if (p != -1)
								MainForm.CurrWeek = MainForm.CurrSemester.Weeks[p - 1];
							RefreshMainFormData();
						}
						break;
					default:
						MessageBox.Show("Invalid menu button caption :\\", "Weird", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						break;
				}
			}
		}

		public void SelectSemester_Menu_Click(object sender, EventArgs e)
		{
			int r = MenuButtonCaptions.IndexOf((sender as MyButton).Caption);
			this.Hide();
			if (r != MenuButtonCaptions.Count - 1)
				if (r >= 0 && r < MenuButtonCaptions.Count - 1)
				{
					// References
					MainForm.CurrSemester = MainForm.DB.Semesters[r];
					MainForm.CurrWeek = Utils.GetSemesterWeekMostRelevantToToday(MainForm.CurrSemester);
                    

					// Refresh
					RefreshMainFormData();
				}
				else
					MessageBox.Show("Invalid menu button caption :\\", "Weird", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		public void View_Menu_Click(object sender, EventArgs e)
		{
			int r = MenuButtonCaptions.IndexOf((sender as MyButton).Caption);
			this.Hide();
			if (r != MenuButtonCaptions.Count - 1)
			{
				switch (r)
				{
					case 0: // 
						MessageBox.Show("View_Menu_Click", "~~~~~", MessageBoxButtons.OK, MessageBoxIcon.Information);
						break;
					default:
						MessageBox.Show("Invalid menu button caption :\\", "Weird", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						break;
				}
			}
		}

		public void Options_Menu_Click(object sender, EventArgs e)
		{
			int r = MenuButtonCaptions.IndexOf((sender as MyButton).Caption);
			this.Hide();
			if (r != MenuButtonCaptions.Count - 1)
				switch (r)
				{
					case 0: // about osf3
						MainForm.AboutForm.Show();
						break;
					case 1: // export stickers
						MainForm.StickerExportForm.Show();
						MainForm.StickerExportForm.StatusL.Text = "waiting";
						break;
					case 2: // open imports folder
						System.Diagnostics.Process.Start(Paths.ImportsFolder);
						break;
					case 3: // open exports folder
						System.Diagnostics.Process.Start(Paths.ExportsFolder);
						break;
					default:
						MessageBox.Show("Invalid menu button caption :\\", "Weird", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						break;
				}
		}
	}
}
