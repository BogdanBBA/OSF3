using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace OSF3.DataTypes
{
    public class Database
    {
        public readonly List<TimeInterval> TimeIntervals;
        public readonly List<ClassType> ClassTypes;
        public readonly List<Professor> Professors;
        public readonly List<Room> Rooms;
        public readonly List<Semester> Semesters;

        public Database()
        {
            this.TimeIntervals = new List<TimeInterval>();
            this.ClassTypes = new List<ClassType>();
            this.Professors = new List<Professor>();
            this.Rooms = new List<Room>();
            this.Semesters = new List<Semester>();
        }

        public TimeInterval GetTimeIntervalByID(string id)
        {
            foreach (TimeInterval timeInterval in TimeIntervals)
                if (timeInterval.ID.Equals(id))
                    return timeInterval;
            return null;
        }

        public ClassType GetClassTypeByID(string id)
        {
            foreach (ClassType classType in ClassTypes)
                if (classType.ID.Equals(id))
                    return classType;
            return null;
        }

        public Professor GetProfessorByID(string id)
        {
            foreach (Professor professor in Professors)
                if (professor.ID.Equals(id))
                    return professor;
            return null;
        }

        public Room GetRoomByID(string id)
        {
            foreach (Room room in Rooms)
                if (room.ID.Equals(id))
                    return room;
            return null;
        }

        public Semester GetSemesterByID(string id)
        {
            foreach (Semester semester in Semesters)
                if (semester.ID.Equals(id))
                    return semester;
            return null;
        }

        public string OpenDatabase()
        {
            string phase = "initializing";
            try
            {
                phase = "checking files/folders";
                if (!File.Exists(Paths.DataFile))
                    throw new ApplicationException("The file \"" + Paths.DataFile + "\" doesn't exist!");
                /*if (!File.Exists(Paths.SettingsFile))
                    throw new ApplicationException("The file \"" + Paths.SettingsFile + "\" doesn't exist!");*/
                if (!Directory.Exists(Paths.ExportsFolder))
                    Directory.CreateDirectory(Paths.ExportsFolder);
                if (!Directory.Exists(Paths.ExportsFolder))
                    throw new ApplicationException("The folder \"" + Paths.ExportsFolder + "\" doesn't exist!");
                if (!Directory.Exists(Paths.ImportsFolder))
                    Directory.CreateDirectory(Paths.ImportsFolder);
                if (!Directory.Exists(Paths.ImportsFolder))
                    throw new ApplicationException("The folder \"" + Paths.ImportsFolder + "\" doesn't exist!");

                phase = "opening xml";
                XmlDocument doc = new XmlDocument();
                XmlNodeList nodes, sems;
                doc.Load(Paths.DataFile);

                phase = "decoding /DateGlobale/IntervaleOrare/";
                nodes = doc.SelectNodes("OSF3_Database/DateGlobale/IntervaleOrare/intervalOrar");
                foreach (XmlNode node in nodes)
                {
                    string id = node.Attributes["ID"].Value;
                    TimeSpan start = Utils.DecodeTime(node.Attributes["deLa"].Value);
                    TimeSpan end = Utils.DecodeTime(node.Attributes["la"].Value);
                    TimeIntervals.Add(new TimeInterval(id, start, end));
                }

                phase = "decoding /DateGlobale/TipuriOre/";
                nodes = doc.SelectNodes("OSF3_Database/DateGlobale/TipuriOre/tipOre");
                foreach (XmlNode node in nodes)
                {
                    string id = node.Attributes["ID"].Value;
                    string name = node.Attributes["nume"].Value;
                    Color tC = ColorTranslator.FromHtml(node.Attributes["col"].Value);
                    ClassTypes.Add(new ClassType(id, name, tC));
                }

                phase = "decoding /DateGlobale/Profesori/";
                nodes = doc.SelectNodes("OSF3_Database/DateGlobale/Profesori/profesor");
                foreach (XmlNode node in nodes)
                {
                    string id = node.Attributes["ID"].Value;
                    string name = node.Attributes["nume"].Value;
                    Professors.Add(new Professor(id, name));
                }

                phase = "decoding /DateGlobale/Sali/";
                nodes = doc.SelectNodes("OSF3_Database/DateGlobale/Sali/sala");
                foreach (XmlNode node in nodes)
                {
                    string id = node.Attributes["ID"].Value;
                    string name = node.Attributes["nume"].Value;
                    Rooms.Add(new Room(id, name));
                }

                phase = "decoding /Semestre/";
                sems = doc.SelectNodes("OSF3_Database/Semestre/Semestru");
                foreach (XmlNode sem in sems)
                {
                    string id = sem.Attributes["ID"].Value;
                    string university = sem.Attributes["universitate"].Value;
                    string faculty = sem.Attributes["facultate"].Value;
                    string year = sem.Attributes["grupa"].Value;
                    string group = sem.Attributes["anUniversitar"].Value;
                    string semesterNumber = sem.Attributes["semestru"].Value;
                    List<WeekCategory> weekCategories = new List<WeekCategory>();
                    List<Discipline> disciplines = new List<Discipline>();

                    phase = "decoding /Semestre/ ... CategoriiSaptamani/";
                    XmlNodeList wCategs = sem.SelectNodes("CategoriiSaptamani/categorie");
                    foreach (XmlNode wCateg in wCategs)
                    {
                        string wcid = wCateg.Attributes["ID"].Value;
                        string name = wCateg.Attributes["nume"].Value;
                        List<Week> weeks = Utils.DecodeWeekList(wCateg.Attributes["incepand"].Value);
                        weekCategories.Add(new WeekCategory(wcid, name, weeks));
                    }

                    phase = "decoding /Semestre/ ... Discipline/";
                    XmlNodeList discs = sem.SelectNodes("Discipline/disciplina");
                    foreach (XmlNode disc in discs)
                    {
                        string did = disc.Attributes["ID"].Value;
                        string dname = disc.Attributes["nume"].Value;
                        Color col1 = ColorTranslator.FromHtml(disc.Attributes["col1"].Value);
                        Color col2 = ColorTranslator.FromHtml(disc.Attributes["col2"].Value);
                        List<Class> classes = new List<Class>();

                        disciplines.Add(new Discipline(did, dname, col1, col2, classes));

                        XmlNodeList clss = disc.SelectNodes("ore");
                        foreach (XmlNode cls in clss)
                        {
                            ClassType classType = GetClassTypeByID(cls.Attributes["tip"].Value);
                            Professor whoWith = GetProfessorByID(cls.Attributes["cuCine"].Value);
                            Room where = GetRoomByID(cls.Attributes["unde"].Value);
                            string when = cls.Attributes["cand"].Value;
                            classes.AddRange(Utils.GenerateClassesForValues(disciplines.Last(), when, classType, whoWith, where, weekCategories, this));
                        }
                        Utils.SortClassesChronologically(classes);
                    }

                    Semesters.Add(new Semester(id, university, faculty, year, group, semesterNumber, weekCategories, disciplines));
                }
            }
            catch (ApplicationException E)
            {
                return "An understandable ERROR occured while initializing the application.\n\nPhase: " + phase + "\n\n" + E.ToString();
            }
            catch (Exception E)
            {
                return "An unchecked/unexpected ERROR occured while initializing the application.\n\nPhase: " + phase + "\n\n" + E.ToString();
            }
            WriteSummaryData();
            return "";
        }

        public void WriteSummaryData()
        {
            List<string> l = new List<string>();

            l.Add(string.Format("Database last successfully read ({0}), with the folowing data:\n", DateTime.Now.ToString("yyyy MMMM dd, HH:mm:ss")));

            l.Add(string.Format("======== TIME INTERVALS ({0}) ========", TimeIntervals.Count));
            foreach (TimeInterval interval in TimeIntervals)
                l.Add(string.Format("{0} ({1})", interval.ID, interval.FormatInterval(true)));

            l.Add(string.Format("\n======== CLASS TYPES ({0}) ========", ClassTypes.Count));
            foreach (ClassType type in ClassTypes)
                l.Add(string.Format("[{0}]. {1} ({2})", type.ID, type.Name, ColorTranslator.ToHtml((type.TextBr as SolidBrush).Color)));

            l.Add(string.Format("\n======== PROFESSORS ({0}) ========", Professors.Count));
            foreach (Professor prof in Professors)
                l.Add(string.Format("[{0}]. {1}", prof.ID, prof.Name));

            l.Add(string.Format("\n======== ROOMS ({0}) ========", Rooms.Count));
            foreach (Room room in Rooms)
                l.Add(string.Format("[{0}]. {1}", room.ID, room.Name));

            l.Add(string.Format("\n======== SEMESTERS ({0}) ========", Semesters.Count));
            foreach (Semester sem in Semesters)
            {
                l.Add(string.Format("\n====== Semester {0} ======", sem.ID));
                l.Add(string.Format(" : {0} / {1} / {2} / {3} / {4}\n", sem.University, sem.Faculty, sem.Group, sem.Year, sem.SemesterNumber));
                l.Add(string.Format("=== Week categories ({0}) ===", sem.WeekCategories.Count));
                foreach (WeekCategory wC in sem.WeekCategories)
                {
                    l.Add(string.Format("  # [{0}]. {1}", wC.ID, wC.Name));
                    foreach (Week w in wC.Weeks)
                        l.Add(string.Format("     * {0}", w.FormatWeek(true)));
                }
                l.Add(string.Format("=== Weeks ({0}) ===", sem.Weeks.Count));
                foreach (Week w in sem.Weeks)
                    l.Add(string.Format("     * {0}", w.FormatWeek(true)));
                l.Add(string.Format("=== Disciplines ({0}) ===", sem.Disciplines.Count));
                foreach (Discipline d in sem.Disciplines)
                {
                    l.Add(string.Format("  # [{0}]. {1} ({2} classes; {3}, {4})",
                        d.ID, d.Name, d.Classes.Count, ColorTranslator.ToHtml(d.Color1), ColorTranslator.ToHtml(d.Color2)));
                    foreach (Class c in d.Classes)
                    {
                        if (l.Count > 255)
                        {
                            int x = 0;
                        }
                        l.Add(string.Format("     * {0}, in {1} with {2}, at {3} ({4})",
                            c.ClassType.ID, c.Where.ID, c.WhoWith.ID, Utils.EncodeDate(c.When.Date), c.When.TimeInterval.FormatInterval(true)));
                    }
                }
            }

            File.WriteAllLines(Paths.SummaryDataFile, l);
        }

        public string SaveDatabase()
        {
            string phase = "initializing";
            try
            {
                phase = "checking folders/files";
            }
            catch (ApplicationException E)
            {
                return "An understandable ERROR occured while initializing the application.\n\nPhase: " + phase + "\n\n" + E.ToString();
            }
            catch (Exception E)
            {
                return "An unchecked/unexpected ERROR occured while initializing the application.\n\nPhase: " + phase + "\n\n" + E.ToString();
            }
            return "";
        }
    }
}
