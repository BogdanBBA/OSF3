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
    public partial class FStickerExporter : Form
    {
        private static readonly string[] MenuButtonCaptions = { "Export!", "Close" };

        private FMain MainForm;

        private List<MyButton> MenuButtons;
        private BorderPictureBox BorderPB;

        private int nFails, nTotal;

        public FStickerExporter(FMain mainForm)
        {
            InitializeComponent();
            this.MainForm = mainForm;
            MyGuiComponents.InitializeAndFormatFormComponents(this);
        }

        private void FStickerExporter_Load(object sender, EventArgs e)
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
                case 0: // export
                    if (ExporterThread.IsBusy)
                    {
                        MessageBox.Show("The thread is already running! Please wait for it to finish.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                    ExporterThread.RunWorkerAsync();
                    //
                    break;
                case 1: // close
                    if (ExporterThread.IsBusy)
                    {
                        MessageBox.Show("The thread is running! Please wait for it to finish.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                    this.Hide();
                    break;
                default:
                    MessageBox.Show("Invalid menu button caption :\\", "Weird", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
        }

        private void StatusL_Click(object sender, EventArgs e)
        {
            MessageBox.Show(StatusL.Text, "Full status label text", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //
        //
        //

        private void ExporterThread_DoWork(object sender, DoWorkEventArgs e)
        {
            //
            ExporterThread.ReportProgress(0, "initializing...");
            nFails = 0;
            nTotal = 0;
            try
            {
                for (int iDisc = 0; iDisc < MainForm.CurrSemester.Disciplines.Count; iDisc++)
                {
                    int p1 = 100 / MainForm.CurrSemester.Disciplines.Count * iDisc;
                    ExporterThread.ReportProgress(p1, "gathering info for '" + MainForm.CurrSemester.Disciplines[iDisc].Name + "'...");
                    List<ClassType> classTypes = new List<ClassType>(new ClassType[] { null });
                    foreach (Class cls in MainForm.CurrSemester.Disciplines[iDisc].Classes)
                        if (!classTypes.Contains(cls.ClassType))
                            classTypes.Add(cls.ClassType);
                    for (int iClsType = 0; iClsType < classTypes.Count; iClsType++)
                    {
                        int p2 = p1 + ((100 / MainForm.CurrSemester.Disciplines.Count) / classTypes.Count * iClsType);
                        ExporterThread.ReportProgress(p2, "exporting '" + MainForm.CurrSemester.Disciplines[iDisc].Name + "'/'" + (classTypes[iClsType] != null ? classTypes[iClsType].Name : "plain") + "'...");
                        bool expResult = MainForm.CurrSemester.Disciplines[iDisc].ExportStickerImage(classTypes[iClsType], UsernameTB.Text, MainForm.CurrSemester.ID);
                        if (!expResult)
                            nFails++;
                        nTotal++;
                    }
                }
            }
            catch (Exception E)
            { ExporterThread.ReportProgress(0, "ERROR: " + E.ToString()); return; }
        }

        private void ExporterThread_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            StatusL.Text = string.Format("{0}% : {1}", e.ProgressPercentage, e.UserState as string);
        }

        private void ExporterThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!StatusL.Text.ToUpper().Contains("ERROR"))
                if (nFails == 0)
                    StatusL.Text = "100% : export finished successfully!";
                else
                    StatusL.Text = "100% : export finished with " + nFails + " / " + nTotal + " fail(s)";
        }
    }
}
