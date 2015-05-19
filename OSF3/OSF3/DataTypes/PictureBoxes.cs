using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSF3.DataTypes
{
	public class DateBox : PictureBox
	{
		private static readonly Brush BgBr = new SolidBrush(MyGuiComponents.ButtonC);
		private static readonly Brush BgMOBr = new SolidBrush(MyGuiComponents.ButtonMOC);
		private static readonly Brush BarBr = new SolidBrush(ColorTranslator.FromHtml("#519BDB"));
		private static readonly Brush BarMOBr = new SolidBrush(ColorTranslator.FromHtml("#EDBE02"));
		private static readonly Brush TodayBarBr = new SolidBrush(ColorTranslator.FromHtml("#80D618"));
		private static readonly ColorFont DowF = new ColorFont(Utils.GetFont("Segoe UI", 23, true), MyGuiComponents.TextC);
		private static readonly ColorFont DowMOF = new ColorFont(Utils.GetFont("Segoe UI", 23, true), ColorTranslator.FromHtml("#EDBE02"));
		private static readonly ColorFont DateF = new ColorFont(Utils.GetFont("Segoe UI", 12, false), MyGuiComponents.TextC);
		private static readonly ColorFont DateMOF = new ColorFont(Utils.GetFont("Segoe UI", 12, false), MyGuiComponents.TextMOC);

		public DateTime Date;

		public DateBox(DateTime date, EventHandler click)
		{
			this.Date = date;
			this.Click += click;
			this.MouseEnter += new EventHandler(OnMouseEnter);
			this.MouseLeave += new EventHandler(OnMouseLeave);
			this.RedrawBox(false);
		}

		private void OnMouseEnter(object sender, EventArgs e)
		{ RedrawBox(true); }

		private void OnMouseLeave(object sender, EventArgs e)
		{ RedrawBox(false); }

		public void RedrawBox(bool mouseOver)
		{
			if (this.Image != null)
				this.Image.Dispose();
			Bitmap bmp = new Bitmap(this.Width, this.Height);
			Graphics g = Graphics.FromImage(bmp);
			g.FillRectangle(mouseOver ? BgMOBr : BgBr, 0, 0, this.Width, this.Height);

			Brush barBr = this.Date.Date.Equals(DateTime.Now.Date) ? TodayBarBr : (mouseOver ? BarMOBr : BarBr);
			g.FillRectangle(barBr, 0, this.Height - 8, this.Width, 8);

			string text = this.Date.ToString("dddd");
			Size size = g.MeasureString(text, DowF.Font).ToSize();
			g.DrawString(text, DowF.Font, mouseOver ? DowMOF.Brush : DowF.Brush, new Point(this.Width / 2 - size.Width / 2, 0));

			text = DateTime.Now.Date.Equals(this.Date.Date) ? "Today" : this.Date.ToString("d MMMM yyyy");
			size = g.MeasureString(text, DateF.Font).ToSize();
			g.DrawString(text, DateF.Font, mouseOver ? DateMOF.Brush : DateF.Brush, new Point(this.Width / 2 - size.Width / 2, this.Height - size.Height - 12));

			this.Image = bmp;
		}
	}

	//

	public class TimeBox : PictureBox
	{
		private static readonly Brush ButtonBr = new SolidBrush(MyGuiComponents.ButtonC);
		private static readonly Brush ButtonMOBr = new SolidBrush(MyGuiComponents.ButtonMOC);
		private static readonly Pen TimeLinePen = new Pen(ColorTranslator.FromHtml("#519BDB"));
		private static readonly ColorFont TimeF = new ColorFont(Utils.GetFont("Segoe UI Light", 12, false), MyGuiComponents.TextC);
		private static readonly Pen TimeLineCWPen = new Pen(Color.Orange, 2);
		private static readonly ColorFont TimeCWF = new ColorFont(Utils.GetFont("Segoe UI Light", 12, true), Color.White);

		public TimeInterval TimeInterval;

		public TimeBox(TimeInterval timeInterval, EventHandler click)
		{
			this.TimeInterval = timeInterval;
			this.Click += click;
		}

		public void RedrawBox(bool thisIsActuallyTheCurrentWeek)
		{
			if (this.Image != null)
				this.Image.Dispose();
			Bitmap bmp = new Bitmap(this.Width, this.Height);
			Graphics g = Graphics.FromImage(bmp);
			g.FillRectangle(ButtonBr, 0, 0, this.Width, this.Height);

			int maxHourTextWidth = 0;

			// Calculate time tick frequency in minutes
			int[] tickFreqs = { 120, 60, 30, 20, 15, 10, 5, 1 };
			int tickFreqPos = 0, tickMins = tickFreqs[tickFreqPos];
			int tY1 = (int) (Utils.GetTimeSpanRatio(new TimeSpan(0, 0, 0), TimeInterval) * this.Height), tY2 = 1000;
			while (tickFreqPos < tickFreqs.Length - 1 && Math.Abs(tY2 - tY1) > 60)
			{
				tickMins = tickFreqs[++tickFreqPos];
				tY2 = (int) (Utils.GetTimeSpanRatio(new TimeSpan(0, tickMins, 0), TimeInterval) * this.Height);
				//MessageBox.Show(string.Format("tickMins={0}, tY2={1}, tY1={2}, diff={3}", tickMins, tY2, tY1, Math.Abs(tY2 - tY1)));
			}

			// Draw lines and time text
			TimeSpan time = TimeInterval.Start;
			bool drawnDate = false;
			while (time.Minutes % tickMins != 0)
				time = time.Add(new TimeSpan(0, 1, 0));
			while (time.Ticks < TimeInterval.End.Ticks)
			{
				int y = (int) (Utils.GetTimeSpanRatio(time, TimeInterval) * this.Height);
				g.DrawLine(TimeLinePen, new Point(0, y), new Point(this.Width - MyGuiComponents.ClassBoxPadding, y));
				string text = Utils.EncodeTime(time) + (drawnDate ? "" : " (" + DateTime.Now.ToString("d MMM yyyy") + ")");
				drawnDate = true;
				g.DrawString(text, TimeF.Font, TimeF.Brush, new Point(0, y));
				//
				int hourTextWidth = (int) g.MeasureString(Utils.EncodeTime(time), TimeF.Font).Width;
				if (maxHourTextWidth < hourTextWidth)
					maxHourTextWidth = hourTextWidth;
				time = time.Add(new TimeSpan(0, tickMins, 0));
			}

			// Draw current time 
			int cY = (int) (Utils.GetTimeSpanRatio(DateTime.Now.TimeOfDay, TimeInterval) * this.Height);
			if (true && (cY >= 0 && cY <= this.Height))
			{
				g.DrawLine(TimeLineCWPen, new Point(maxHourTextWidth, cY),
					new Point(this.Width - MyGuiComponents.ClassBoxPadding, cY));
				//
				string text = Utils.EncodeTime(DateTime.Now.TimeOfDay);
				Size size = g.MeasureString(text, TimeCWF.Font).ToSize();
				Point location = new Point(this.Width - MyGuiComponents.ClassBoxPadding - size.Width + 1, cY + 1);
				g.FillRectangle(ButtonBr, new Rectangle(location, size));
				g.DrawString(text, TimeCWF.Font, TimeCWF.Brush, location);
			}

			this.Image = bmp;
		}
	}

	//

	public class ClassBox : PictureBox
	{
		private static readonly Brush BgBr = new SolidBrush(MyGuiComponents.ButtonC);
		private static readonly Brush BgMOBr = new SolidBrush(MyGuiComponents.ButtonMOC);
		private static readonly ColorFont NameF = new ColorFont(Utils.GetFont("Segoe UI", 13, true), MyGuiComponents.TextC);
		private static readonly ColorFont NameMOF = new ColorFont(Utils.GetFont("Segoe UI", 13, true), MyGuiComponents.TextMOC);
		private static readonly Font ClassTypeF = Utils.GetFont("Segoe UI", 9, true);
		private static readonly ColorFont TimeIntervalF = new ColorFont(Utils.GetFont("Segoe UI Light", 9, false), Color.White);
		private static readonly ColorFont ProfessorF = new ColorFont(Utils.GetFont("Segoe UI", 9, false), ColorTranslator.FromHtml("#DECA59"));
		private static readonly ColorFont RoomF = new ColorFont(Utils.GetFont("Segoe UI", 9, false), ColorTranslator.FromHtml("#C7CDD1"));
		private static readonly Pen SelectedLinePen = new Pen(Color.Orange, 2);

		public Class Class;

		public ClassBox(Class aClass, EventHandler click)
		{
			this.Class = aClass;
			//this.Cursor = Cursors.Hand;
			this.Click += click;
			this.MouseEnter += new EventHandler(OnMouseEnter);
			this.MouseLeave += new EventHandler(OnMouseLeave);
			this.RedrawBox(false);
		}

		private void OnMouseEnter(object sender, EventArgs e)
		{ RedrawBox(true); }

		private void OnMouseLeave(object sender, EventArgs e)
		{ RedrawBox(false); }

		public void RedrawBox(bool mouseOver)
		{
			if (this.Image != null)
				this.Image.Dispose();
			Bitmap bmp = new Bitmap(this.Width, this.Height);
			Graphics g = Graphics.FromImage(bmp);
			g.FillRectangle(mouseOver ? BgMOBr : BgBr, 0, 0, this.Width, this.Height);

			// Color bar
			Pen pen1 = new Pen(this.Class.Discipline.Color1, MyGuiComponents.ClassBoxBarWidth);
			Pen pen2 = new Pen(this.Class.Discipline.Color2, MyGuiComponents.ClassBoxBarWidth);
			Point p1 = new Point(-MyGuiComponents.ClassBoxBarWidth, -2 * MyGuiComponents.ClassBoxBarWidth);
			Point p2 = new Point(2 * MyGuiComponents.ClassBoxBarWidth, MyGuiComponents.ClassBoxBarWidth);
			while (p1.Y < this.Height)
			{
				g.DrawLine(pen1, p1, p2);
				p1.Offset(0, MyGuiComponents.ClassBoxBarYOffset);
				p2.Offset(0, MyGuiComponents.ClassBoxBarYOffset);
				g.DrawLine(pen2, p1, p2);
				p1.Offset(0, MyGuiComponents.ClassBoxBarYOffset);
				p2.Offset(0, MyGuiComponents.ClassBoxBarYOffset);
			}
			g.FillRectangle(mouseOver ? BgMOBr : BgBr, MyGuiComponents.ClassBoxBarWidth, 0, this.Width, this.Height);

			// Class type
			int left = MyGuiComponents.ClassBoxBarWidth + 4, lastTop = 0;
			string text = this.Class.ClassType.Name;
			Font font = ClassTypeF;
			Size size = g.MeasureString(text, font).ToSize();
			g.DrawString(text, font, this.Class.ClassType.TextBr, new Point(left + 2, lastTop));

			// Time interval
			text = this.Class.When.TimeInterval.FormatInterval(false);
			font = TimeIntervalF.Font;
			size = g.MeasureString(text, font).ToSize();
			g.DrawString(text, font, TimeIntervalF.Brush, new Point(this.Width - size.Width - 2, lastTop));

			// Class name
			lastTop += size.Height;
			text = this.Class.Discipline.Name;
			size = g.MeasureString(text, mouseOver ? NameMOF.Font : NameF.Font).ToSize();
			g.DrawString(text, mouseOver ? NameMOF.Font : NameF.Font, mouseOver ? NameMOF.Brush : NameF.Brush, new Point(left, lastTop));

			// Where
			text = "În " + this.Class.Where.Name;
			size = g.MeasureString(text, RoomF.Font).ToSize();
			lastTop = this.Height - size.Height - (int) SelectedLinePen.Width - 1;
			g.DrawString(text, RoomF.Font, RoomF.Brush, new Point(left, lastTop));

			// Who with
			text = "Cu " + this.Class.WhoWith.Name;
			size = g.MeasureString(text, ProfessorF.Font).ToSize();
			lastTop -= size.Height;
			g.DrawString(text, ProfessorF.Font, ProfessorF.Brush, new Point(left, lastTop));

			if (mouseOver)
				g.DrawLine(SelectedLinePen, 0, this.Height - SelectedLinePen.Width + 1, this.Width, this.Height - SelectedLinePen.Width + 1);

			this.Image = bmp;
		}
	}
}
