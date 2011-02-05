namespace Hime.Kernel.Logs
{
    /// <summary>
    /// Represents a log exception
    /// </summary>
    public sealed class LogException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the ResourceException class
        /// </summary>
        public LogException() : base() { }
        /// <summary>
        /// Initializes a new instance of the ResourceException class with an error message
        /// </summary>
        /// <param name="message">Message describing the error</param>
        public LogException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the ResourceExcetion class with an error message and a reference to the inner exception that causes the error
        /// </summary>
        /// <param name="message">Message describing the error</param>
        /// <param name="innerException">Inner exception causing the error</param>
        public LogException(string message, System.Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Base class representing a log
    /// </summary>
    public abstract class Log
    {
        /// <summary>
        /// Represents a log status
        /// </summary>
        protected enum LogStatus
        {
            /// <summary>
            /// Log is at the root level
            /// </summary>
            AtRoot,
            /// <summary>
            /// Log is in a section but no entry has yet begun
            /// </summary>
            InSection,
            /// <summary>
            /// Log is in an entry
            /// </summary>
            InEntry
        }

        /// <summary>
        /// Log status
        /// </summary>
        protected LogStatus p_Status;

        /// <summary>
        /// Initializes a new instance of the Log class
        /// </summary>
        public Log() { p_Status = LogStatus.AtRoot; }

        /// <summary>
        /// Begins a new section for the log
        /// </summary>
        /// <param name="name">Name of the section to begin</param>
        /// <remarks>The log must be at root level</remarks>
        public virtual void SectionBegin(string name)
        {
            if (p_Status != LogStatus.AtRoot)
                throw new LogException("A section can only be opened at root level");
            p_Status = LogStatus.InSection;
        }
        /// <summary>
        /// Ends the current section
        /// </summary>
        /// <remarks>The log must be at a section level</remarks>
        public virtual void SectionEnd()
        {
            if (p_Status != LogStatus.InSection)
                throw new LogException("No section to close");
            p_Status = LogStatus.AtRoot;
        }
        /// <summary>
        /// Begins a new entry in the current section
        /// </summary>
        /// <remarks>The log must be at a section level</remarks>
        public virtual void EntryBegin()
        {
            if (p_Status != LogStatus.InSection)
                throw new LogException("An entry can only be opened in a section");
            p_Status = LogStatus.InEntry;
        }
        /// <summary>
        /// Begins a new entry in the current section
        /// </summary>
        /// <param name="mark">Mark to apply on the entry</param>
        /// <remarks>The log must be at a section level</remarks>
        public virtual void EntryBegin(string mark)
        {
            if (p_Status != LogStatus.InSection)
                throw new LogException("An entry can only be opened in a section");
            p_Status = LogStatus.InEntry;
        }
        /// <summary>
        /// Adds data to the current log entry
        /// </summary>
        /// <param name="data">Data to be added</param>
        /// <remarks>The log must be at an entry level</remarks>
        public virtual void EntryAddData(string data)
        {
            if (p_Status != LogStatus.InEntry)
                throw new LogException("Data can only be added in an entry");
        }
        /// <summary>
        /// Ends the current log entry
        /// </summary>
        /// <remarks>The log must be at an entry level</remarks>
        public virtual void EntryEnd()
        {
            if (p_Status != LogStatus.InEntry)
                throw new LogException("No entry to close");
            p_Status = LogStatus.InSection;
        }

        /// <summary>
        /// Output raw data in the log
        /// </summary>
        /// <param name="data">Data to output</param>
        public abstract void RawOutput(string data);
    }

    /// <summary>
    /// Represents a log proxy for multiple logs
    /// </summary>
    public sealed class LogProxy : Log
    {
        /// <summary>
        /// Target logs
        /// </summary>
        private System.Collections.Generic.List<Log> p_Targets;

        /// <summary>
        /// Initializes a new instance of the LogProxy class
        /// </summary>
        public LogProxy()
        {
            p_Targets = new System.Collections.Generic.List<Log>();
        }

        /// <summary>
        /// Add a new target log
        /// </summary>
        /// <param name="item">New target log</param>
        public void AddTarget(Log item) { p_Targets.Add(item); }

        /// <summary>
        /// Begins a new section for the log
        /// </summary>
        /// <param name="name">Name of the section to begin</param>
        /// <remarks>The log must be at root level</remarks>
        public override void SectionBegin(string Name) { foreach (Log log in p_Targets) log.SectionBegin(Name); }
        /// <summary>
        /// Ends the current section
        /// </summary>
        /// <remarks>The log must be at a section level</remarks>
        public override void SectionEnd() { foreach (Log log in p_Targets) log.SectionEnd(); }
        /// <summary>
        /// Begins a new entry in the current section
        /// </summary>
        /// <remarks>The log must be at a section level</remarks>
        public override void EntryBegin() { foreach (Log log in p_Targets) log.EntryBegin(); }
        /// <summary>
        /// Begins a new entry in the current section
        /// </summary>
        /// <param name="mark">Mark to apply on the entry</param>
        /// <remarks>The log must be at a section level</remarks>
        public override void EntryBegin(string Mark) { foreach (Log log in p_Targets) log.EntryBegin(Mark); }
        /// <summary>
        /// Adds data to the current log entry
        /// </summary>
        /// <param name="data">Data to be added</param>
        /// <remarks>The log must be at an entry level</remarks>
        public override void EntryAddData(string Data) { foreach (Log log in p_Targets) log.EntryAddData(Data); }
        /// <summary>
        /// Ends the current log entry
        /// </summary>
        /// <remarks>The log must be at an entry level</remarks>
        public override void EntryEnd() { foreach (Log log in p_Targets) log.EntryEnd(); }
        /// <summary>
        /// Output raw data in the log
        /// </summary>
        /// <param name="data">Data to output</param>
        public override void RawOutput(string Data) { foreach (Log log in p_Targets) log.RawOutput(Data); }
    }

    /// <summary>
    /// Represents a log using the console as output
    /// </summary>
    public sealed class LogConsole : Log
    {
        /// <summary>
        /// Class instance
        /// </summary>
        private static LogConsole p_Instance;
        /// <summary>
        /// Stack of sections
        /// </summary>
        private System.Collections.Generic.Stack<string> p_Sections;

        /// <summary>
        /// Get an instance of the LogConsole class
        /// </summary>
        /// <value>Instance of the LogConsole class</value>
        public static LogConsole Instance
        {
            get
            {
                if (p_Instance == null)
                    p_Instance = new LogConsole();
                return p_Instance;
            }
        }

        /// <summary>
        /// Initializes a new instance of the LogConsole class
        /// </summary>
        private LogConsole()
        {
            p_Sections = new System.Collections.Generic.Stack<string>();
        }

        /// <summary>
        /// Begins a new section for the log
        /// </summary>
        /// <param name="name">Name of the section to begin</param>
        /// <remarks>The log must be at root level</remarks>
        public override void SectionBegin(string Name)
        {
            base.SectionBegin(Name);
            p_Sections.Push(Name);
            System.Console.WriteLine("------ Section : " + Name + " ------");
        }
        /// <summary>
        /// Ends the current section
        /// </summary>
        /// <remarks>The log must be at a section level</remarks>
        public override void SectionEnd()
        {
            base.SectionEnd();
            string Name = p_Sections.Pop();
            System.Console.WriteLine("------ End of section : " + Name + " ------");
            System.Console.WriteLine();
        }
        /// <summary>
        /// Begins a new entry in the current section
        /// </summary>
        /// <remarks>The log must be at a section level</remarks>
        public override void EntryBegin() { base.EntryBegin(); }
        /// <summary>
        /// Begins a new entry in the current section
        /// </summary>
        /// <param name="mark">Mark to apply on the entry</param>
        /// <remarks>The log must be at a section level</remarks>
        public override void EntryBegin(string Mark)
        {
            base.EntryBegin();
            System.Console.Write("[");
            System.Console.Write(Mark);
            System.Console.Write("]\t");
        }
        /// <summary>
        /// Adds data to the current log entry
        /// </summary>
        /// <param name="data">Data to be added</param>
        /// <remarks>The log must be at an entry level</remarks>
        public override void EntryAddData(string Data)
        {
            base.EntryAddData(Data);
            System.Console.Write(Data);
            System.Console.Write("\t");
        }
        /// <summary>
        /// Ends the current log entry
        /// </summary>
        /// <remarks>The log must be at an entry level</remarks>
        public override void EntryEnd()
        {
            base.EntryEnd();
            System.Console.WriteLine();
        }
        /// <summary>
        /// Output raw data in the log
        /// </summary>
        /// <param name="data">Data to output</param>
        public override void RawOutput(string Data)
        {
            System.Console.Write("->\t");
            System.Console.WriteLine(Data);
        }
    }

    /// <summary>
    /// Represents a log using XML as output
    /// </summary>
    public sealed class LogXML : Log
    {
        /// <summary>
        /// XML document for output
        /// </summary>
        private System.Xml.XmlDocument p_XMLDocument;
        /// <summary>
        /// Current node in the XML document
        /// </summary>
        private System.Xml.XmlNode p_XMLCurrentNode;
        /// <summary>
        /// Next section ID
        /// </summary>
        private int p_NextID;

        /// <summary>
        /// Initializes a new instance of the LogXML class
        /// </summary>
        public LogXML()
        {
            p_XMLDocument = new System.Xml.XmlDocument();
            p_XMLDocument.AppendChild(p_XMLDocument.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            p_XMLDocument.AppendChild(p_XMLDocument.CreateElement("Log"));
            p_XMLCurrentNode = p_XMLDocument.ChildNodes[1];
            p_XMLCurrentNode.Attributes.Append(p_XMLDocument.CreateAttribute("title"));
            p_NextID = 0;
        }

        /// <summary>
        /// Begins a new section for the log
        /// </summary>
        /// <param name="name">Name of the section to begin</param>
        /// <remarks>The log must be at root level</remarks>
        public override void SectionBegin(string Name)
        {
            base.SectionBegin(Name);
            System.Xml.XmlNode Section = p_XMLDocument.CreateElement("Section");
            Section.Attributes.Append(p_XMLDocument.CreateAttribute("id"));
            Section.Attributes.Append(p_XMLDocument.CreateAttribute("name"));
            Section.Attributes["id"].Value = "section" + p_NextID.ToString();
            Section.Attributes["name"].Value = Name;
            p_XMLCurrentNode.AppendChild(Section);
            p_XMLCurrentNode = Section;
            p_NextID++;
        }
        /// <summary>
        /// Ends the current section
        /// </summary>
        /// <remarks>The log must be at a section level</remarks>
        public override void SectionEnd()
        {
            base.SectionEnd();
            p_XMLCurrentNode = p_XMLCurrentNode.ParentNode;
        }
        /// <summary>
        /// Begins a new entry in the current section
        /// </summary>
        /// <remarks>The log must be at a section level</remarks>
        public override void EntryBegin()
        {
            base.EntryBegin();
            System.Xml.XmlNode Entry = p_XMLDocument.CreateElement("Entry");
            p_XMLCurrentNode.AppendChild(Entry);
            p_XMLCurrentNode = Entry;
        }
        /// <summary>
        /// Begins a new entry in the current section
        /// </summary>
        /// <param name="mark">Mark to apply on the entry</param>
        /// <remarks>The log must be at a section level</remarks>
        public override void EntryBegin(string Mark)
        {
            base.EntryBegin();
            System.Xml.XmlNode Entry = p_XMLDocument.CreateElement("Entry");
            Entry.Attributes.Append(p_XMLDocument.CreateAttribute("mark"));
            Entry.Attributes["mark"].Value = Mark;
            p_XMLCurrentNode.AppendChild(Entry);
            p_XMLCurrentNode = Entry;
        }
        /// <summary>
        /// Adds data to the current log entry
        /// </summary>
        /// <param name="data">Data to be added</param>
        /// <remarks>The log must be at an entry level</remarks>
        public override void EntryAddData(string Data)
        {
            base.EntryAddData(Data);
            System.Xml.XmlNode Node = p_XMLDocument.CreateElement("Data");
            Node.InnerText = Data;
            p_XMLCurrentNode.AppendChild(Node);
        }
        /// <summary>
        /// Ends the current log entry
        /// </summary>
        /// <remarks>The log must be at an entry level</remarks>
        public override void EntryEnd()
        {
            base.EntryEnd();
            p_XMLCurrentNode = p_XMLCurrentNode.ParentNode;
        }
        /// <summary>
        /// Output raw data in the log
        /// </summary>
        /// <param name="data">Data to output</param>
        public override void RawOutput(string Data) { }

        /// <summary>
        /// Extracts the log name from the file name
        /// </summary>
        /// <param name="fileName">File path and name</param>
        /// <returns>Returns the log's name</returns>
        private string Save_ExtractName(string fileName)
        {
            for (int i = fileName.Length - 1; i != 0; i--)
            {
                if (fileName[i] == '.')
                {
                    return fileName.Substring(0, i).Replace("\\", "/");
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// Save the log as a XML file
        /// </summary>
        /// <param name="fileName">File path and name</param>
        /// <param name="title">Title for the log</param>
        public void SaveXML(string fileName, string title)
        {
            p_XMLDocument.ChildNodes[1].Attributes["title"].Value = title;
            p_XMLDocument.Save(fileName);
        }
        /// <summary>
        /// Export the log as a HTML file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="title">Title for the log</param>
        public void ExportHTML(string fileName, string title)
        {
            System.IO.FileInfo File = new System.IO.FileInfo(fileName);
            p_XMLDocument.ChildNodes[1].Attributes["title"].Value = title;
            p_XMLDocument.Save(fileName + ".xml");
            System.IO.Directory.CreateDirectory(File.DirectoryName + "\\hime_data");

            Resources.AccessorSession Session = Resources.ResourceAccessor.CreateCheckoutSession();
            Session.AddCheckoutFile(fileName + ".xml");
            Resources.ResourceAccessor.CheckOut(Session, "Transforms.Logs.LogXML.xslt", File.DirectoryName + "LogXML.xslt");
            Resources.ResourceAccessor.Export("Visuals.button_plus.gif", File.DirectoryName + "\\hime_data\\button_plus.gif");
            Resources.ResourceAccessor.Export("Visuals.button_minus.gif", File.DirectoryName + "\\hime_data\\button_minus.gif");
            Resources.ResourceAccessor.Export("Visuals.Hime.Error.png", File.DirectoryName + "\\hime_data\\Hime.Error.png");
            Resources.ResourceAccessor.Export("Visuals.Hime.Warning.png", File.DirectoryName + "\\hime_data\\Hime.Warning.png");
            Resources.ResourceAccessor.Export("Visuals.Hime.Info.png", File.DirectoryName + "\\hime_data\\Hime.Info.png");
            Resources.ResourceAccessor.Export("Visuals.Hime.Logo.png", File.DirectoryName + "\\hime_data\\Hime.Logo.png");

            System.Xml.Xsl.XslCompiledTransform Transform = new System.Xml.Xsl.XslCompiledTransform();
            Transform.Load(File.DirectoryName + "LogXML.xslt");
            Transform.Transform(fileName + ".xml", fileName);

            Session.Close();
        }
    }
}