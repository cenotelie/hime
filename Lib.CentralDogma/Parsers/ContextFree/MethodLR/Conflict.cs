/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;
using Hime.Kernel.Reporting;

namespace Hime.Parsers.ContextFree.LR
{
    public class Conflict : IEntry
    {
        private string component;
        private State state;
        private ConflictType type;
        private Terminal lookahead;
        private List<Item> items;
        private List<ConflictExample> examples;
        private bool isError;
        private bool isResolved;

        public Hime.Kernel.Reporting.ELevel Level {
            get
            {
                if (isError) return Kernel.Reporting.ELevel.Error;
                return Kernel.Reporting.ELevel.Warning;
            }
        }
        public string Component
        {
            get { return component; }
            set { component = value; }
        }
        public State State { get { return state; } }
        public string Message { get { return ToString(); } }
        public ConflictType ConflictType { get { return type; } }
        public Terminal ConflictSymbol { get { return lookahead; } }
        public ICollection<Item> Items { get { return items; } }
        public List<ConflictExample> Examples { get { return examples; } }
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
            examples = new List<ConflictExample>();
        }
        public Conflict(string component, State state, ConflictType type)
        {
            this.component = component;
            this.state = state;
            this.type = type;
            this.isError = true;
            this.isResolved = false;
            items = new List<Item>();
            examples = new List<ConflictExample>();
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


            System.Xml.XmlNode nexs = doc.CreateElement("Examples");
            foreach (ConflictExample example in examples)
                nexs.AppendChild(example.GetXMLNode(doc));
            element.AppendChild(nexs);
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
            return Builder.ToString();
        }
    }
}
