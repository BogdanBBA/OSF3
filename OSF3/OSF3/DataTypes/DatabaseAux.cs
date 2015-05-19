using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSF3.DataTypes
{
    public class BaseObject
    {
        public readonly string ID;

        public BaseObject(string id)
        {
            this.ID = id;
        }
    }

    public class NamedObject : BaseObject
    {
        public readonly string Name;

        public NamedObject(string id, string name)
            : base(id)
        {
            this.Name = name;
        }
    }

    public class TimeInterval : BaseObject
    {
        public TimeSpan Start;
        public TimeSpan End;

        public TimeInterval(string id, TimeSpan start, TimeSpan end)
            : base(id)
        {
            this.Start = start;
            this.End = end;
        }

        public TimeInterval(string id, string start, string end)
            : this(id, Utils.DecodeTime(start), Utils.DecodeTime(end))
        {
        }

        public int LengthInMinutes { get { return (int) new TimeSpan(End.Ticks - Start.Ticks).TotalMinutes; } }

        public string FormatInterval(bool includeSpaceInSeparator)
        {
            string separator = includeSpaceInSeparator ? " - " : "-";
            return Utils.EncodeTime(Start) + separator + Utils.EncodeTime(End);
        }
    }

    public class DateWithTimeInterval
    {
        public readonly DateTime Date;
        public readonly TimeInterval TimeInterval;

        public DateWithTimeInterval(DateTime date, TimeInterval timeInterval)
        {
            this.Date = date;
            this.TimeInterval = timeInterval;
        }
    }

    public class Week
    {
        public readonly DateTime Monday;
        public readonly DateTime Sunday;

        public Week(DateTime monday)
        {
            this.Monday = monday.Date;
            this.Sunday = monday.Date.AddDays(7).AddTicks(-1);
        }

        public Week(string monday)
            : this(Utils.DecodeDate(monday))
        {
        }

        public bool DateIsIncluded(DateTime date)
        {
            return Monday.Ticks <= date.Ticks && date.Ticks <= Sunday.Ticks;
        }

        public string FormatWeek(bool includeSpaceInSeparator)
        {
            string separator = includeSpaceInSeparator ? " - " : "-";
            return Utils.EncodeDate(Monday) + separator + Utils.EncodeDate(Sunday);
        }

        public string FormatWeekPrettily()
        {
            return string.Format("{0} - {1}", Monday.ToString("d MMM yyyy"), Sunday.ToString("d MMM yyyy"));
        }
    }

    public class WeekCategory : NamedObject
    {
        public readonly List<Week> Weeks;

        public WeekCategory(string id, string name, List<Week> weeks)
            : base(id, name)
        {
            this.Weeks = weeks;
        }
    }

    public class Professor : NamedObject
    {
        public Professor(string id, string name)
            : base(id, name)
        {
        }
    }

    public class ClassType : NamedObject
    {
        public readonly Brush TextBr;

        public ClassType(string id, string name, Color textColor)
            : base(id, name)
        {
            this.TextBr = new SolidBrush(textColor);
        }
    }

    public class Room : NamedObject
    {
        public Room(string id, string name)
            : base(id, name)
        {
        }
    }

    public class Class
    {
        public readonly Discipline Discipline;
        public readonly ClassType ClassType;
        public readonly DateWithTimeInterval When;
        public readonly Room Where;
        public readonly Professor WhoWith;

        public Class(Discipline discipline, ClassType classType, DateWithTimeInterval when, Room where, Professor whoWith)
        {
            this.Discipline = discipline;
            this.ClassType = classType;
            this.When = when;
            this.Where = where;
            this.WhoWith = whoWith;
        }
    }

    public class Discipline : NamedObject
    {
        public readonly Color Color1;
        public readonly Color Color2;
        public readonly List<Class> Classes;

        public Discipline(string id, string name, Color color1, Color color2, List<Class> classes)
            : base(id, name)
        {
            this.Color1 = color1;
            this.Color2 = color2;
            this.Classes = classes;
        }

        public bool ExportStickerImage(ClassType classType, string username, string group)
        {
            try
            {
                string filename = Path.Combine(Paths.ExportsFolder, this.ID + (classType != null ? ("-" + classType.ID) : "")) + ".png";
                Bitmap bmp = new Bitmap((int) (1.6 * 1280), 1280);
                Graphics g = Graphics.FromImage(bmp);

                int imgHeightPenRatio = (int) (bmp.Height * 0.1);
                int titleFontSize = (int) ((double) (bmp.Height * 0.49) / 1.495);
                int subtitleFontSize = (int) ((double) (bmp.Height * 0.18) / 1.775);
                int classTypeFontSize = (int) ((double) (bmp.Height * 0.12) / 1.775);

                // Color bars
                Pen pen1 = new Pen(this.Color1, imgHeightPenRatio);
                Pen pen2 = new Pen(this.Color2, imgHeightPenRatio);
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
                }

                // Border
                Pen borderPen = new Pen(Color.White, penW / 2);
                g.DrawRectangle(borderPen, new Rectangle(penW / 2, penW / 2, bmp.Width - penW, bmp.Height - penW));

                // Title text
                GraphicsPath p = new GraphicsPath();
                string text = this.ID;
                Font textFont = Utils.GetFont("Ubuntu", titleFontSize, true);
                Size textSize = g.MeasureString(text, textFont).ToSize();
                Point location = new Point(bmp.Width / 2 - textSize.Width / 2, (int) (0.13 * bmp.Height));
                p.AddString(text, textFont.FontFamily, (int) textFont.Style, g.DpiY * titleFontSize / 72, location, new StringFormat());
                g.DrawPath(new Pen(this.Color1, penW / 3), p);
                g.FillPath(Brushes.White, p);

                // Subtitle text
                p = new GraphicsPath();
                text = this.Name;
                textFont = Utils.GetFont("Segoe UI Semibold", subtitleFontSize, false);
                textSize = g.MeasureString(text, textFont).ToSize();
                location = new Point(bmp.Width / 2 - textSize.Width / 2, (int) (0.55 * bmp.Height));
                p.AddString(text, textFont.FontFamily, (int) textFont.Style, g.DpiY * subtitleFontSize / 72, location, new StringFormat());
                g.DrawPath(new Pen(this.Color1, penW / 6), p);
                g.FillPath(Brushes.White, p);

                // Class type text
                if (classType != null)
                {
                    p = new GraphicsPath();
                    text = classType.Name;
                    textFont = Utils.GetFont("Premier League with Lion Number", classTypeFontSize, false);
                    textSize = g.MeasureString(text, textFont).ToSize();
                    location = new Point(bmp.Width / 2 - textSize.Width / 2, (int) (0.75 * bmp.Height));
                    p.AddString(text, textFont.FontFamily, (int) textFont.Style, g.DpiY * classTypeFontSize / 72, location, new StringFormat());
                    g.DrawPath(new Pen(this.Color1, penW / 8), p);
                    g.FillPath(Brushes.White, p);
                }

                // Image and group/user
                Size maxLogoSize = new Size(minDim / 10, minDim / 10);
                int detailsPadding = (int) (0.9 * maxLogoSize.Height);
                string groupImagePath = Path.Combine(Paths.ImportsFolder, group.Replace(":", "")) + ".png";
                if (!File.Exists(groupImagePath))
                    throw new ApplicationException("Group image '" + groupImagePath + "' does not exist!");
                Image logoBmp = new Bitmap(groupImagePath);
                Image logo = Utils.ScaleImage(logoBmp, maxLogoSize.Width, maxLogoSize.Height);
                logoBmp.Dispose();
                location = new Point(detailsPadding, detailsPadding);
                g.DrawImage(logo, location);
                //
                p = new GraphicsPath();
                text = group;
                int userFontSize = (int) ((double) (logo.Height * 0.9) / 1.775);
                textFont = Utils.GetFont("Segoe UI Semibold", userFontSize, false);
                textSize = g.MeasureString(text, textFont).ToSize();
                location.Offset(logo.Width, 0);
                p.AddString(text, textFont.FontFamily, (int) textFont.Style, g.DpiY * userFontSize / 72, location, new StringFormat());
                g.DrawPath(new Pen(this.Color1, penW / 10), p);
                g.FillPath(Brushes.White, p);
                //
                p = new GraphicsPath();
                text = username;
                textSize = g.MeasureString(text, textFont).ToSize();
                location = new Point(bmp.Width - detailsPadding - textSize.Width, detailsPadding);
                p.AddString(text, textFont.FontFamily, (int) textFont.Style, g.DpiY * userFontSize / 72, location, new StringFormat());
                g.DrawPath(new Pen(this.Color1, penW / 10), p);
                g.FillPath(Brushes.White, p);

                // Save to file
                bmp.Save(filename);
                return true;
            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
                return false;
            }
        }
    }

    public class Semester : BaseObject
    {
        public readonly string University;
        public readonly string Faculty;
        public readonly string Year;
        public readonly string Group;
        public readonly string SemesterNumber;
        public readonly List<WeekCategory> WeekCategories;
        public readonly List<Week> Weeks;
        public readonly List<Discipline> Disciplines;
        public readonly List<Class> AllClasses;

        public Semester(string id, string university, string faculty, string year, string group, string semesterNumber, List<WeekCategory> weekCategories, List<Discipline> disciplines)
            : base(id)
        {
            this.University = university;
            this.Faculty = faculty;
            this.Year = year;
            this.Group = group;
            this.SemesterNumber = semesterNumber;
            this.WeekCategories = weekCategories;
            this.Disciplines = disciplines;
            //
            this.AllClasses = new List<Class>();
            foreach (Discipline discipline in this.Disciplines)
                foreach (Class cls in discipline.Classes)
                    this.AllClasses.Add(cls);
            //
            this.Weeks = new List<Week>();
            foreach (Class cls in this.AllClasses)
            {
                Week week = new Week(Utils.GetMondayForDay(cls.When.Date));
                if (Utils.IndexOfWeekInWeekList(week, this.Weeks) == -1)
                    this.Weeks.Add(week);
            }
        }
    }
}
