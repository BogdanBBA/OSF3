using OSF3.DataTypes;
using OSF3.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSF3
{
	public partial class FMain : Form
	{
		private static readonly string[] MenuButtonCaptions = { "SELECT SEMESTER", "View", "Options", "EXIT" };

		public FMenu MenuForm;
		public FSelectWeek SelectWeekForm;
		public FStickerExporter StickerExportForm;
		public FAbout AboutForm;

		public List<MyButton> MenuButtons;
		public List<DateBox> DateBoxes;
		public List<ClassBox> ClassBoxes;
		public TimeBox TimeBox;

		public Database DB;
		public Semester CurrSemester;
		public Week CurrWeek;

		public FMain()
		{
			InitializeComponent();
			MyGuiComponents.InitializeAndFormatFormComponents(this);
		}

		private void FMain_Load(object sender, EventArgs e)
		{
			// open database

			DB = new Database();
			string loadResult = DB.OpenDatabase();
			if (!loadResult.Equals(""))
			{
				MessageBox.Show("An ERROR occured while opening the database. Read below for details. The application will now close.\n\n" + loadResult, "Database open ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.Exit();
			}

			// initialize data

			try
			{
				MenuForm = new FMenu(this);
				SelectWeekForm = new FSelectWeek(this);
				StickerExportForm = new FStickerExporter(this);
				AboutForm = new FAbout(this);
				//
				CurrSemester = DB.Semesters[0];
				CurrWeek = Utils.GetSemesterWeekMostRelevantToToday(CurrSemester);

			}
			catch (Exception E)
			{
				MessageBox.Show("An ERROR occured while initializing the application (after successfully opening the database). Read below for details. The application will now close.\n\n" + E.ToString(), "Database open ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.Exit();
			}

			// initialize panels

			MenuP.SetBounds(MyGuiComponents.ControlPadding, MyGuiComponents.ControlPadding, this.Width - 2 * MyGuiComponents.ControlPadding, MyGuiComponents.DefaultMenuButtonSize.Height);
			ContentsP.SetBounds(MyGuiComponents.ControlPadding, MenuP.Bottom + MyGuiComponents.ControlPadding, MenuP.Width, this.Height - MenuP.Height - 3 * MyGuiComponents.ControlPadding);
			TimeP.SetBounds(0, MyGuiComponents.DayPanelHeight, MyGuiComponents.TimePanelWidth, ContentsP.Height - MyGuiComponents.DayPanelHeight);
			DateP.SetBounds(TimeP.Right, 0, ContentsP.Width - TimeP.Right, MyGuiComponents.DayPanelHeight);
			ClassesP.SetBounds(MyGuiComponents.TimePanelWidth, MyGuiComponents.DayPanelHeight, ContentsP.Width - MyGuiComponents.TimePanelWidth, ContentsP.Height - MyGuiComponents.DayPanelHeight);

			// initialize controls

			MenuButtons = MyGuiComponents.CreateMenuButtons(MenuP, MenuButtonCaptions, true, MenuButton_Click);

			DateBoxes = MyGuiComponents.CreateDateBoxes(DateP, CurrWeek.Monday, true, DateBox_Click);

			TimeBox = new TimeBox(new TimeInterval("TimeBox", new TimeSpan(0), new TimeSpan(0)), TimeBox_Click);
			TimeBox.Parent = TimeP;
			TimeBox.SetBounds(0, 0, TimeP.Width, TimeP.Height);

			ClassBoxes = MyGuiComponents.RecreateClassBoxesAndDoStuff(ClassesP, CurrSemester, CurrWeek, ClassBox_Click, TimeBox, new List<ClassBox>());
		}

		private void MenuButton_Click(object sender, EventArgs e)
		{
			int r = Utils.IndexOfStringInArray(MenuButtonCaptions, (sender as MyButton).Caption);
			switch (r)
			{
				case 0: // select semester
					List<string> sems = new List<string>();
					foreach (Semester sem in DB.Semesters)
						sems.Add(sem.ID);
					MenuForm.RefreshMenuForm(sems, MenuForm.SelectSemester_Menu_Click, Cursor.Position);
					break;
				case 1: // view options
					List<string> viewC = new List<string>(new string[1] { "View option" });
					MenuForm.RefreshMenuForm(viewC, MenuForm.View_Menu_Click, Cursor.Position);
					break;
				case 2: // options/settings
					List<string> optC = new List<string>(new string[4] { "About OSF3", "Export stickers", "Open imports folder", "Open exports folder" });
					MenuForm.RefreshMenuForm(optC, MenuForm.Options_Menu_Click, Cursor.Position);
					break;
				case 3: // exit
					MenuForm.Dispose();
					SelectWeekForm.Dispose();
					StickerExportForm.Dispose();
					AboutForm.Dispose();
					Application.Exit();
					break;
				default:
					MessageBox.Show("Invalid menu button caption :\\", "Weird", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					break;
			}
		}

		private void DateBox_Click(object sender, EventArgs e)
		{
			MenuForm.RefreshMenuForm(new List<string>(new string[3] { "Select week", "Next week", "Previous week" }), MenuForm.DateBox_Menu_Click, Cursor.Position);
		}

		private void TimeBox_Click(object sender, EventArgs e)
		{
		}

		public void ClassBox_Click(object sender, EventArgs e)
		{
		}
	}
}
