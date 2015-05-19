using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSF3.DataTypes
{
	public static class MyGuiComponents
	{
		public const int ControlPadding = 8;
		public const int ButtonBarHeight = 5;
		public const int TimePanelWidth = 160;
		public const int DayPanelHeight = 80;
		public const int ClassBoxPadding = 8;
		public const int ClassBoxBarWidth = 24;
		public const int ClassBoxBarYOffset = 6;

		public static readonly Size DefaultMenuButtonSize = new Size(300, 60);

		public static readonly Color FormC = ColorTranslator.FromHtml("#171717");
		public static readonly Color ButtonC = ColorTranslator.FromHtml("#171717");
		public static readonly Color ButtonMOC = ColorTranslator.FromHtml("#1C1C1C");

		public static readonly Color TextC = ColorTranslator.FromHtml("#E6EBEB");
		public static readonly Color TextMOC = ColorTranslator.FromHtml("#F2F7F7");

		public static void InitializeAndFormatFormComponents(Form form)
		{
			form.BackColor = FormC;
			foreach (Control control in form.Controls)
				InitializeAndFormatControlComponents(control);
		}

		private static void InitializeAndFormatControlComponents(Control control)
		{
			if (control is Label || control is PictureBox || control is Panel)
				control.BackColor = FormC;
			foreach (Control subControl in control.Controls)
				if (subControl is Label)
					(subControl as Label).BackColor = control.BackColor;
				else if (subControl is PictureBox)
					(subControl as PictureBox).BackColor = control.BackColor;
				else if (subControl is Panel)
					InitializeAndFormatControlComponents(subControl);
		}

		public static List<MyButton> CreateMenuButtons(Panel container, string[] captions, bool horizontal, EventHandler click)
		{
			List<MyButton> result = new List<MyButton>();
			for (int i = 0, n = captions.Length, dim = horizontal ? container.Width / n : container.Height / n; i < n; i++)
			{
				MyButton butt = new MyButton(captions[i], click);
				butt.Parent = container;
				butt.Cursor = Cursors.Hand;
				if (horizontal)
					butt.SetBounds(i * dim, 0, dim, container.Height);
				else
					butt.SetBounds(0, i * dim, container.Width, dim);
				butt.RedrawButton(false);
				result.Add(butt);
			}
			return result;
		}

		public static List<DateBox> CreateDateBoxes(Panel container, DateTime monday, bool horizontal, EventHandler click)
		{
			List<DateBox> result = new List<DateBox>();
			for (int i = 0, n = 5, dim = horizontal ? container.Width / n : container.Height / n; i < n; i++)
			{
				DateBox dateBox = new DateBox(monday.AddDays(i), click);
				dateBox.Parent = container;
				dateBox.Cursor = Cursors.Hand;
				if (horizontal)
					dateBox.SetBounds(i * dim, 0, dim, container.Height);
				else
					dateBox.SetBounds(0, i * dim, container.Width, dim);
				dateBox.RedrawBox(false);
				result.Add(dateBox);
			}
			return result;
		}

		public static List<ClassBox> RecreateClassBoxesAndDoStuff(Panel container, Semester semester, Week week, EventHandler click, TimeBox timeBox, List<ClassBox> classBoxes)
		{
			// get all classes for this week
			List<Class> classesThisWeek = new List<Class>();
			foreach (Class cls in semester.AllClasses)
				if (week.DateIsIncluded(cls.When.Date))
					classesThisWeek.Add(cls);
			// get time range
			TimeInterval clsTimeInterval = Utils.GetTimeIntervalForClasses(classesThisWeek);
			timeBox.TimeInterval = clsTimeInterval;
			bool isCurrWeek = classesThisWeek.Count == 0 ? false : week.DateIsIncluded(DateTime.Now);
			timeBox.RedrawBox(isCurrWeek);
			// hide extra boxes
			for (int iBox = classesThisWeek.Count; iBox < classBoxes.Count; iBox++)
				classBoxes[iBox].Hide();
			// iterate through the classes and (create if necessary and) set correct box bounds, then refresh
			for (int iBox = 0, width = container.Width / 5; iBox < classesThisWeek.Count; iBox++)
			{
				Class currCls = classesThisWeek[iBox];
				if (iBox >= classBoxes.Count)
				{
					ClassBox clsBox = new ClassBox(currCls, click);
					clsBox.Parent = container;
					classBoxes.Add(clsBox);
				}
				classBoxes[iBox].Class = currCls;
				Point boxLocation = new Point((Utils.DayOfWeekAsInt(currCls.When.Date.DayOfWeek) - 1) * width,
					(int) (Utils.GetTimeSpanRatio(currCls.When.TimeInterval.Start, clsTimeInterval) * container.Height));
				Size boxSize = new Size(width - MyGuiComponents.ClassBoxPadding, (int) ((Utils.GetTimeSpanRatio(currCls.When.TimeInterval.End, clsTimeInterval)
					- Utils.GetTimeSpanRatio(currCls.When.TimeInterval.Start, clsTimeInterval)) * container.Height));
				classBoxes[iBox].Hide();
				classBoxes[iBox].Bounds = new Rectangle(boxLocation, boxSize);
				classBoxes[iBox].RedrawBox(false);
				classBoxes[iBox].Show();
			}
			return classBoxes;
		}
	}

	public class ColorFont
	{
		public readonly Font Font;
		public readonly Color Color;
		public readonly Brush Brush;

		public ColorFont(Font font, Color color)
		{
			this.Font = font;
			this.Color = color;
			this.Brush = new SolidBrush(color);
		}
	}

	public class MyButton : PictureBox
	{
		private static readonly Brush ButtonBr = new SolidBrush(MyGuiComponents.ButtonC);
		private static readonly Brush ButtonMOBr = new SolidBrush(MyGuiComponents.ButtonMOC);
		private static readonly Brush BarBr = new SolidBrush(ColorTranslator.FromHtml("#424242"));
		private static readonly Brush BarMOBr = new SolidBrush(Color.Orange);
		private static readonly ColorFont CaptionF = new ColorFont(Utils.GetFont("Segoe UI", 17, true), MyGuiComponents.TextC);
		private static readonly ColorFont CaptionMOF = new ColorFont(Utils.GetFont("Segoe UI", 17, true), Color.Orange);

		public string Caption { get; set; }
		private EventHandler ClickEH;

		public MyButton(string caption, EventHandler click)
			: base()
		{
			this.Caption = caption;
			this.SetOnClickEventHandler(click);
			this.MouseEnter += new EventHandler(OnMouseEnter);
			this.MouseLeave += new EventHandler(OnMouseLeave);
			this.RedrawButton(false);
		}

		public void SetOnClickEventHandler(EventHandler click)
		{
			ClearOnClickEventHandler();
			this.ClickEH = click;
			this.Click += click;
		}

		public void ClearOnClickEventHandler()
		{
			this.Click -= ClickEH;
		}

		private void OnMouseEnter(object sender, EventArgs e)
		{ RedrawButton(true); }

		private void OnMouseLeave(object sender, EventArgs e)
		{ RedrawButton(false); }

		public void RedrawButton(bool mouseOver)
		{
			if (this.Image != null)
				this.Image.Dispose();
			Bitmap bmp = new Bitmap(this.Width, this.Height);
			Graphics g = Graphics.FromImage(bmp);
			g.FillRectangle(mouseOver ? ButtonMOBr : ButtonBr, 0, 0, this.Width, this.Height);
			g.FillRectangle(mouseOver ? BarMOBr : BarBr, 0, this.Height - MyGuiComponents.ButtonBarHeight, this.Width - 1, MyGuiComponents.ButtonBarHeight);
			Size size = g.MeasureString(this.Caption, mouseOver ? CaptionMOF.Font : CaptionF.Font).ToSize();
			g.DrawString(this.Caption, mouseOver ? CaptionMOF.Font : CaptionF.Font, mouseOver ? CaptionMOF.Brush : CaptionF.Brush, new Point(this.Width / 2 - size.Width / 2, this.Height / 2 - size.Height / 2));
			this.Image = bmp;
		}
	}

	public class BorderPictureBox : PictureBox
	{
		public const int DefaultBorderWidth = 4;
		private static readonly Brush BgBr = new SolidBrush(MyGuiComponents.FormC);
		private static readonly Brush BorderBr = new SolidBrush(MyGuiComponents.TextMOC);

		public int BorderWidth { get; set; }

		public BorderPictureBox(int borderWidth)
			: base()
		{
			this.BorderWidth = borderWidth;
		}

		public void RedrawBorder()
		{
			if (this.Image != null)
				this.Image.Dispose();
			Bitmap bmp = new Bitmap(this.Width, this.Height);
			Graphics g = Graphics.FromImage(bmp);
			/*Pen pen1 = new Pen(MyGuiComponents.ButtonC, 20);
			Pen pen2 = new Pen(Color.Magenta, 20);
			int maxDim = bmp.Width > bmp.Height ? bmp.Width : bmp.Height, minDim = bmp.Width < bmp.Height ? bmp.Width : bmp.Height;
			int penW = (int) pen1.Width;
			Point p1 = new Point(-maxDim, -2 * maxDim);
			Point p2 = new Point(2 * maxDim, maxDim);
			while (p1.Y < bmp.Height)
			{
				g.DrawLine(pen1, p1, p2);
				p1.Offset(0, penW);
				p2.Offset(0, penW);
				g.DrawLine(pen2, p1, p2);
				p1.Offset(0, penW);
				p2.Offset(0, penW);
			}*/
			g.FillRectangle(BgBr, new Rectangle(0, 0, this.Width, this.Height));
			g.DrawRectangle(new Pen(BorderBr, BorderWidth), new Rectangle(0, 0, this.Width, this.Height));
			this.Image = bmp;
		}
	}
}
