using System.Collections.Generic;

namespace Hime.Kernel.Naming
{
    public class QualifiedName
    {
        private static char separatorChar = '.';
        private List<string> path;

        public static char Separator { get { return separatorChar; } }
        public string PrefixOrder0 { get { return path[0]; } }
        public string NakedName { get { return path[path.Count - 1]; } }
        public QualifiedName Prefix { get { return new QualifiedName(path.GetRange(0, path.Count - 1)); } }
        public QualifiedName SubName { get { return new QualifiedName(path.GetRange(1, path.Count - 1)); } }
        public bool IsEmpty { get { return (path.Count == 0); } }
        public bool HasPrefix { get { return (path.Count > 1); } }
        public bool IsNaked { get { return (path.Count == 1); } }

        public QualifiedName(string name)
        {
            path = new List<string>();
            path.Add(name);
        }
        public QualifiedName(List<string> path) { this.path = path; }

        public static QualifiedName ParseName(string completeName)
        {
            List<string> Path = new List<string>();
            int Begin = 0;
            int End = 0;
            while (End != completeName.Length)
            {
                if (completeName[End] == separatorChar)
                {
                    Path.Add(completeName.Substring(Begin, End - Begin));
                    Begin = End + 1;
                    End = Begin;
                }
                else
                    End++;
            }
            Path.Add(completeName.Substring(Begin, End - Begin));
            return new QualifiedName(Path);
        }

        public override string ToString() { return ToString(separatorChar); }
        public string ToString(char separator)
        {
            if (path.Count == 0)
                return string.Empty;
            System.Text.StringBuilder Builder = new System.Text.StringBuilder(path[0]);
            for (int i = 1; i != path.Count; i++)
            {
                Builder.Append(separator);
                Builder.Append(path[i]);
            }
            return Builder.ToString();
        }

        public static QualifiedName operator +(QualifiedName prefix, string localName)
        {
            List<string> Path = new List<string>();
            Path.AddRange(prefix.path);
            Path.Add(localName);
            return new QualifiedName(Path);
        }
    }
}