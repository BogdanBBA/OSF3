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
	public partial class FSelectWeek : Form
	{
		private static readonly string[] MenuButtonCaptions = { "Select week", "Close" };

		private FMain MainForm;

		private List<MyButton> MenuButtons;
		private BorderPictureBox BorderPB;

		public FSelectWeek(FMain mainForm)
		{
			InitializeComponent();
			this.MainForm = mainForm;
			MyGuiComponents.InitializeAndFormatFormComponents(this);
		}

		private void FSelectSem_Load(object sender, EventArgs e)
		{
			BorderPB = new BorderPictureBox(BorderPictureBox.DefaultBorderWidth);
			BorderPB.Parent = this;
			BorderPB.SetBounds(0, 0, this.Width, this.Height);
			BorderPB.RedrawBorder();
			MenuButtons = MyGuiComponents.CreateMenuButtons(MenuP, MenuButtonCaptions, true, MenuButton_Click);
		}

		public void RefreshWeekList()
		{
			WeekListCB.Items.Clear();
			for (int i = 0; i < MainForm.CurrSemester.Weeks.Count; i++)
				WeekListCB.Items.Add(string.Format("[{0}]. {1}", i + 1, MainForm.CurrSemester.Weeks[i].FormatWeekPrettily()));
			WeekListCB.SelectedIndex = Utils.IndexOfWeekInWeekList(MainForm.CurrWeek, MainForm.CurrSemester.Weeks);
		}

		private void WeekListCB_SelectedIndexChanged(object sender, EventArgs e)
		{
			RB2.Checked = true;
		}

		private void MenuButton_Click(object sender, EventArgs e)
		{
			int r = Utils.IndexOfStringInArray(MenuButtonCaptions, (sender as MyButton).Caption);
			switch (r)
			{
				case 0: // select week
					if (RB1.Checked) // closest to today
					{
						MainForm.CurrWeek = Utils.GetSemesterWeekMostRelevantToToday(MainForm.CurrSemester);
					}
					else // specific
					{
						string s = (WeekListCB.Items[WeekListCB.SelectedIndex] as string).Substring(1);
						int p = Int32.Parse(s.Substring(0, s.IndexOf(']')));
						MainForm.CurrWeek = MainForm.CurrSemester.Weeks[p - 1];
					}
					MainForm.MenuForm.RefreshMainFormData();
					this.Hide();
					break;
				case 1: // close
					this.Hide();
					break;
				default:
					MessageBox.Show("Invalid menu button caption :\\", "Weird", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					break;
			}
		}
	}
}
