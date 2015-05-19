using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSF3.DataTypes
{
    public static class Paths
    {
        public const string DataFile = "data.xml";
        public const string SettingsFile = "settings.xml";
        public const string SummaryDataFile = ExportsFolder + "\\summary.txt";
        public const string ExportsFolder = "exports";
        public const string ImportsFolder = "imports";
    }

    public static class Utils
    {
        public static int IndexOfStringInArray(string[] array, string element)
        {
            for (int i = 0; i < array.Length; i++)
                if (array[i].Equals(element))
                    return i;
            return -1;
        }

        public static Font GetFont(string name, int size, bool bold)
        {
            return new Font(name, size, bold ? FontStyle.Bold : FontStyle.Regular);
        }

        public static string EncodeTime(TimeSpan time)
        {
            return string.Format("{0}:{1:D2}", time.Hours, time.Minutes);
        }

        public static TimeSpan DecodeTime(string time)
        {
            int h = 0, m = 0;
            try
            {
                h = Int32.Parse(time.Substring(0, time.IndexOf(':')));
                m = Int32.Parse(time.Substring(time.IndexOf(':') + 1, time.Length - time.IndexOf(':') - 1));
            }
            catch (Exception E)
            {
                return new TimeSpan(0);
            }
            return new TimeSpan(h, m, 0);
        }

        //

        public static string EncodeDate(DateTime date)
        {
            return date.ToString("yyyy.MM.dd");
        }

        public static DateTime DecodeDate(string date)
        {
            int y = 2012, M = 10, d = 1;
            try
            {
                y = Int32.Parse(date.Substring(0, 4));
                M = Int32.Parse(date.Substring(5, 2));
                d = Int32.Parse(date.Substring(8, 2));
            }
            catch (Exception E)
            {
                return new DateTime(2012, 10, 1, 23, 59, 59);
            }
            return new DateTime(y, M, d, 0, 0, 0);
        }

        //

        public static string EncodeWeekList(List<Week> weekList)
        {
            return "not implemented";
        }

        public static List<Week> DecodeWeekList(string weekList)
        {
            List<Week> result = new List<Week>();

            weekList = weekList.Replace(" ", "");
            string[] mondays = weekList.Split(';');
            foreach (string monday in mondays)
                result.Add(new Week(Utils.DecodeDate(monday)));

            return result;
        }

        //
        //
        //

        public static List<Class> GenerateClassesForValues(Discipline discipline, string when, ClassType classType, Professor professor, Room room, List<WeekCategory> weekCategories, Database database)
        {
            List<Class> result = new List<Class>();

            when = when.Replace(" ", "");
            string[] combos = when.Split('|');
            foreach (string combo in combos) //impare+pare:1@C
            {
                // get all involved weeks
                string[] weekCategs = combo.Substring(0, combo.IndexOf(':')).Split('+');
                List<Week> weeks = new List<Week>();
                foreach (string weekCateg in weekCategs)
                    foreach (WeekCategory weekCategory in weekCategories)
                        if (weekCateg.Equals(weekCategory.ID))
                            foreach (Week week in weekCategory.Weeks)
                                if (!weeks.Contains(week))
                                    weeks.Add(week);
                // get day of w for those involved weeks
                int dayOfWeek = Int32.Parse(combo.Substring(combo.IndexOf(':') + 1, 1));
                // get time interval
                string timeIntervalID = combo.Substring(combo.IndexOf('@') + 1, combo.Length - combo.IndexOf('@') - 1);
                TimeInterval timeInterval = database.GetTimeIntervalByID(timeIntervalID);

                // for all weeks involved, add a new class to the resultWeek 
                foreach (Week week in weeks)
                {
                    DateWithTimeInterval DwTI = new DateWithTimeInterval(week.Monday.AddDays(dayOfWeek - 1), timeInterval);
                    result.Add(new Class(discipline, classType, DwTI, room, professor));
                }
            }

            SortClassesChronologically(result);
            return result;
        }

        public static void SortClassesChronologically(List<Class> classes)
        {
            if (classes.Count < 2)
                return;
            for (int i = 0; i < classes.Count - 1; i++)
                for (int j = i + 1; j < classes.Count; j++)
                {
                    DateWithTimeInterval c1 = classes[i].When, c2 = classes[j].When;
                    if ((c1.Date.Ticks > c2.Date.Ticks) || (c1.Date.Ticks == c2.Date.Ticks && c1.TimeInterval.Start.Ticks > c2.TimeInterval.Start.Ticks))
                    {
                        Class aux = classes[i];
                        classes[i] = classes[j];
                        classes[j] = aux;
                    }
                }
        }

        public static TimeInterval GetTimeIntervalForClasses(List<Class> classes)
        {
            if (classes.Count == 0)
                return new TimeInterval("", new TimeSpan(0), new TimeSpan(0));
            TimeInterval result = new TimeInterval("", new TimeSpan(23, 59, 59), new TimeSpan(0));
            foreach (Class cls in classes)
            {
                if (result.Start.TotalMinutes > cls.When.TimeInterval.Start.TotalMinutes)
                    result.Start = cls.When.TimeInterval.Start;
                if (result.End.TotalMinutes < cls.When.TimeInterval.End.TotalMinutes)
                    result.End = cls.When.TimeInterval.End;
            }
            return result;
        }

        public static Week GetSemesterWeekMostRelevantToToday(Semester semester)
        {
            if (semester.Weeks.Count == 0 || semester.AllClasses.Count == 0)
            {
                MessageBox.Show("Cannot find appropriate Week for Semester with no weeks or classes! Returning GetSemesterWeekMostRelevantToToday==null", "Week detection ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            if (DateTime.Now.Ticks < semester.Weeks[0].Monday.Ticks)
                return semester.Weeks[0];
            foreach (Week week in semester.Weeks)
                if (DateTime.Now.Ticks < week.Monday.AddDays(5).Ticks)
                    return week;
            return semester.Weeks.Last();
        }

        public static int IndexOfWeekInWeekList(Week targetWeek, List<Week> weekList)
        {
            for (int i = 0; i < weekList.Count; i++)
                if (weekList[i].Monday.Date.Ticks == targetWeek.Monday.Date.Ticks)
                    return i;
            return -1;
        }

        //
        //
        //

        public static DateTime GetMondayForDay(DateTime date)
        {
            while (!date.DayOfWeek.Equals(DayOfWeek.Monday))
                date = date.AddDays(-1);
            return date;
        }

        public static int DayOfWeekAsInt(DayOfWeek dow)
        {
            switch (dow)
            {
                case DayOfWeek.Monday:
                    return 1;
                case DayOfWeek.Tuesday:
                    return 2;
                case DayOfWeek.Wednesday:
                    return 3;
                case DayOfWeek.Thursday:
                    return 4;
                case DayOfWeek.Friday:
                    return 5;
                case DayOfWeek.Saturday:
                    return 6;
                case DayOfWeek.Sunday:
                    return 7;
                default:
                    return 0;
            }
        }

        public static double GetTimeSpanRatio(TimeSpan time, TimeInterval interval)
        {
            if ((time.Ticks < interval.Start.Ticks && time.Ticks > interval.End.Ticks) || (interval.Start.Equals(interval.End)))
                return 0;
            return (double) (time.TotalMinutes - interval.Start.TotalMinutes) / (interval.End.TotalMinutes - interval.Start.TotalMinutes);
        }

        //
        //
        //

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            double ratioX = (double) maxWidth / image.Width;
            double ratioY = (double) maxHeight / image.Height;
            double ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int) (image.Width * ratio);
            int newHeight = (int) (image.Height * ratio);

            Image newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }
    }
}
