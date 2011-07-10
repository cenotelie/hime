using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    enum ConflictType
    {
        ShiftReduce,
        ReduceReduce,
        None
    }

    class Conflict : Hime.Kernel.Reporting.Entry
    {
        private string component;
        private State state;
        private ConflictType type;
        private Terminal lookahead;
        private List<Item> items;
        private List<Terminal> inputSample;
        private bool isError;
        private bool isResolved;

        public Hime.Kernel.Reporting.Level Level {
            get
            {
                if (isError) return Kernel.Reporting.Level.Error;
                return Kernel.Reporting.Level.Warning;
            }
        }
        public string Component { get { return component; } }
        public State State { get { return state; } }
        public string Message { get { return ToString(); } }
        public ConflictType ConflictType { get { return type; } }
        public Terminal ConflictSymbol { get { return lookahead; } }
        public ICollection<Item> Items { get { return items; } }
        public List<Terminal> InputSample
        {
            get { return inputSample; }
            set { inputSample = value; }
        }
        public bool IsError
        {
            get { return isError; }
            set { isError = value; }
        }
        public bool IsResolved
        {
            get { return isResolved; }
            set { isResolved = value; }
        }

        public Conflict(string component, State state, ConflictType type, Terminal lookahead)
        {
            this.component = component;
            this.state = state;
            this.type = type;
            this.lookahead = lookahead;
            this.isError = true;
            this.isResolved = false;
            items = new List<Item>();
        }
        public Conflict(string component, State state, ConflictType type)
        {
            this.component = component;
            this.state = state;
            this.type = type;
            this.isError = true;
            this.isResolved = false;
            items = new List<Item>();
        }

        public void AddItem(Item Item) { items.Add(Item); }
        public bool ContainsItem(Item Item) { return items.Contains(Item); }

        public System.Xml.XmlNode GetMessageNode(System.Xml.XmlDocument doc)
        {
            System.Xml.XmlNode element = doc.CreateElement("Conflict");

            System.Xml.XmlNode header = doc.CreateElement("Header");
            header.Attributes.Append(doc.CreateAttribute("type"));
            header.Attributes.Append(doc.CreateAttribute("set"));
            header.Attributes["type"].Value = type.ToString();
            header.Attributes["set"].Value = state.ID.ToString("X");
            header.AppendChild(lookahead.GetXMLNode(doc));
            element.AppendChild(header);

            System.Xml.XmlNode nodeItems = doc.CreateElement("Items");
            foreach (Item item in items)
                nodeItems.AppendChild(item.GetXMLNode(doc, state));
            element.AppendChild(nodeItems);

            if (inputSample != null)
            {
                System.Xml.XmlNode example = doc.CreateElement("Example");
                foreach (Terminal t in inputSample)
                    example.AppendChild(t.GetXMLNode(doc));
                example.AppendChild(doc.CreateElement("Dot"));
                example.AppendChild(lookahead.GetXMLNode(doc));
                element.AppendChild(example);
            }
            return element;
        }

        public override string ToString()
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("Conflict ");
            if (type == ConflictType.ShiftReduce)
                Builder.Append("Shift/Reduce");
            else
                Builder.Append("Reduce/Reduce");
            Builder.Append(" in ");
            Builder.Append(state.ID.ToString("X"));
            if (lookahead != null)
            {
                Builder.Append(" on terminal '");
                Builder.Append(lookahead.ToString());
                Builder.Append("'");
            }
            Builder.Append(" for items {");
            foreach (Item Item in items)
            {
                Builder.Append(" ");
                Builder.Append(Item.ToString());
                Builder.Append(" ");
            }
            Builder.Append("}");
            if (inputSample != null)
            {
                Builder.Append("\nInput example:");
                foreach (Terminal t in inputSample)
                {
                    Builder.Append(" ");
                    Builder.Append(t.ToString());
                }
                Builder.Append(" ");
                Builder.Append(Item.dot);
                Builder.Append(" ");
                Builder.Append(lookahead.ToString());
            }
            return Builder.ToString();
        }
    }
}
