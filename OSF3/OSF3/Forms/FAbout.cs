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
	public partial class FAbout : Form
	{
		private static readonly string[] MenuButtonCaptions = { "Close" };

		private FMain MainForm;

		private List<MyButton> MenuButtons;
		private BorderPictureBox BorderPB;

		public FAbout(FMain mainForm)
		{
			InitializeComponent();
			this.MainForm = mainForm;
			MyGuiComponents.InitializeAndFormatFormComponents(this);
		}

		private void FAbout_Load(object sender, EventArgs e)
		{
			BorderPB = new BorderPictureBox(BorderPictureBox.DefaultBorderWidth);
			BorderPB.Parent = this;
			BorderPB.SetBounds(0, 0, this.Width, this.Height);
			BorderPB.RedrawBorder();
			MenuButtons = MyGuiComponents.CreateMenuButtons(MenuP, MenuButtonCaptions, true, MenuButton_Click);
		}

		private void MenuButton_Click(object sender, EventArgs e)
		{
			int r = Utils.IndexOfStringInArray(MenuButtonCaptions, (sender as MyButton).Caption);
			switch (r)
			{
				case 0: // select
					this.Hide();
					break;
				default:
					MessageBox.Show("Invalid menu button caption :\\", "Weird", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					break;
			}
		}
	}
}
