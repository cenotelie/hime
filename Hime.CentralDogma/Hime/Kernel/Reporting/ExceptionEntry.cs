namespace Hime.Kernel.Reporting
{
    public class ExceptionEntry : IEntry
    {
        protected System.Exception exception;

        public ELevel Level { get { return ELevel.Error; } }
        public string Component { get { return "Compiler"; } }
        public string Message { get { return "Exception: " + exception.Message; } }

        public ExceptionEntry(System.Exception ex) { exception = ex; }

        public System.Xml.XmlNode GetMessageNode(System.Xml.XmlDocument doc)
        {
            System.Xml.XmlNode element = doc.CreateElement("Exception");
            System.Xml.XmlNode message = doc.CreateElement("Message");
            message.InnerText = Message;
            element.AppendChild(message);
            System.Xml.XmlNode method = doc.CreateElement("Method");
            method.InnerText = exception.TargetSite.ToString();
            element.AppendChild(method);
            System.Xml.XmlNode stack = doc.CreateElement("Stack");
            string data = exception.StackTrace;
            string[] lines = data.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                System.Xml.XmlNode nl = doc.CreateElement("Line");
                nl.InnerText = line;
                stack.AppendChild(nl);
            }
            element.AppendChild(stack);
            return element;
        }
    }
}