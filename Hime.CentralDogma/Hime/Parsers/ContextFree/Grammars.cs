using System.Collections.Generic;
using Hime.Kernel.Reporting;
using System.Xml;

namespace Hime.Parsers.CF
{
    public interface CFParserGenerator : ParserGenerator
    {
        ParserData Build(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Reporter);
    }

    public abstract class CFGrammar : Grammar
    {
        protected string name;
        protected ushort nextSID;
        protected Dictionary<string, string> options;
        protected Dictionary<string, Terminal> terminals;
        protected Dictionary<string, Variable> variables;
        protected Dictionary<string, Virtual> virtuals;
        protected Dictionary<string, Action> actions;
        internal List<CFGrammarTemplateRule> templateRules;

        public override string LocalName { get { return name; } }
        public int NextSID { get { return nextSID; } }
        public ICollection<string> Options { get { return options.Keys; } }
        public ICollection<Terminal> Terminals { get { return terminals.Values; } }
        public ICollection<Variable> Variables { get { return variables.Values; } }
        public ICollection<Virtual> Virtuals { get { return virtuals.Values; } }
        public ICollection<Action> Actions { get { return actions.Values; } }
        public List<CFRule> Rules
        {
            get
            {
                List<CFRule> rules = new List<CFRule>();
                foreach (Variable Variable in variables.Values)
                    foreach (CFRule Rule in Variable.Rules)
                        rules.Add(Rule);
                return rules;
            }
        }
        internal ICollection<CFGrammarTemplateRule> TemplateRules { get { return templateRules; } }

        public CFGrammar(string Name) : base()
        {
            options = new Dictionary<string, string>();
            terminals = new Dictionary<string, Terminal>();
            variables = new Dictionary<string, Variable>();
            virtuals = new Dictionary<string, Virtual>();
            actions = new Dictionary<string, Action>();
            templateRules = new List<CFGrammarTemplateRule>();
            name = Name;
            nextSID = 3;
        }

        public void AddOption(string Name, string Value)
        {
            if (options.ContainsKey(Name))
                options[Name] = Value;
            else
                options.Add(Name, Value);
        }
        public string GetOption(string Name)
        {
            if (!options.ContainsKey(Name))
                return null;
            return options[Name];
        }

        public Symbol GetSymbol(string Name)
        {
            if (terminals.ContainsKey(Name)) return terminals[Name];
            if (variables.ContainsKey(Name)) return variables[Name];
            if (virtuals.ContainsKey(Name)) return virtuals[Name];
            if (actions.ContainsKey(Name)) return actions[Name];
            return null;
        }

        public TerminalText AddTerminalText(string name, Automata.NFA nfa, Grammar subGrammar)
        {
        	TerminalText terminal;
            if (children.ContainsKey(name) && terminals.ContainsKey(name))
            {
                terminal = (TerminalText)terminals[name];
                terminal.Priority = nextSID;
                terminal.NFA = nfa;
                terminal.SubGrammar = subGrammar;
            }
            else
            {
                terminal = new TerminalText(this, nextSID, name, nextSID, nfa, subGrammar);
                children.Add(name, terminal);
                terminals.Add(name, terminal);
            }
            nextSID++;
            return terminal;
        }
        
        public TerminalBin AddTerminalBin(TerminalBinType Type, string Value)
        {
            if (children.ContainsKey(Value) && terminals.ContainsKey(Value))
            {
                TerminalBin Terminal = (TerminalBin)terminals[Value];
                Terminal.Type = Type;
                Terminal.Value = Value;
                nextSID++;
                return Terminal;
            }
            else
            {
                TerminalBin Terminal = new TerminalBin(this, nextSID, Value, nextSID, Type, Value.Substring(2, Value.Length - 2));
                children.Add(Value, Terminal);
                terminals.Add(Value, Terminal);
                nextSID++;
                return Terminal;
            }
        }
        public Terminal GetTerminal(string Name)
        {
            if (!terminals.ContainsKey(Name))
                return null;
            return terminals[Name];
        }

        public Variable AddVariable(string Name)
        {
            if (variables.ContainsKey(Name))
                return variables[Name];
            Variable Var = new Variable(this, nextSID, Name);
            children.Add(Name, Var);
            variables.Add(Name, Var);
            nextSID++;
            return Var;
        }
        
        public Variable GetVariable(string Name)
        {
            if (!variables.ContainsKey(Name))
                return null;
            return variables[Name];
        }

        public Virtual AddVirtual(string Name)
        {
            if (virtuals.ContainsKey(Name))
                return virtuals[Name];
            Virtual Virtual = new Virtual(this, Name);
            children.Add(Name, Virtual);
            virtuals.Add(Name, Virtual);
            return Virtual;
        }
        public Virtual GetVirtual(string Name)
        {
            if (!virtuals.ContainsKey(Name))
                return null;
            return virtuals[Name];
        }

        public Action AddAction(string Name)
        {
            Action Action = new Action(this, Name);
            children.Add(Name, Action);
            actions.Add(Name, Action);
            return Action;
        }
        public Action GetAction(string Name)
        {
            if (!actions.ContainsKey(Name))
                return null;
            return actions[Name];
        }

        internal CFGrammarTemplateRule AddTemplateRule(Redist.Parsers.SyntaxTreeNode RuleNode, CFGrammarCompiler Compiler)
        {
            CFGrammarTemplateRule Rule = new CFGrammarTemplateRule(this, Compiler, RuleNode);
            templateRules.Add(Rule);
            return Rule;
        }

        public virtual void Inherit(CFGrammar Parent)
        {
            foreach (string Option in Parent.Options)
                AddOption(Option, Parent.GetOption(Option));
        }
        
        public abstract CFGrammar Clone();

        protected void Export_Documentation(ParserData data, CompilationTask options)
        {
            string directory = options.Documentation + "_temp";
            System.IO.Directory.CreateDirectory(directory);
            
            Kernel.Resources.ResourceAccessor accessor = new Kernel.Resources.ResourceAccessor();
            Kernel.Documentation.MHTMLCompiler compiler = new Kernel.Documentation.MHTMLCompiler();
            compiler.Title = "Documentation " + name;
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamText("text/html", "utf-8", "index.html", accessor.GetStreamFor("Transforms.Doc.Index.html")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamText("text/html", "utf-8", "GraphParser.html", accessor.GetStreamFor("Transforms.Doc.Parser.html")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamText("text/css", "utf-8", "hime_data/Hime.css", accessor.GetStreamFor("Transforms.Hime.css")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamText("text/javascript", "utf-8", "hime_data/Hime.js", accessor.GetStreamFor("Transforms.Hime.js")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/gif", "hime_data/button_plus.gif", accessor.GetStreamFor("Visuals.button_plus.gif")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/gif", "hime_data/button_minus.gif", accessor.GetStreamFor("Visuals.button_minus.gif")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.Logo.png", accessor.GetStreamFor("Visuals.Hime.Logo.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.GoTo.png", accessor.GetStreamFor("Visuals.Hime.GoTo.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.Info.png", accessor.GetStreamFor("Visuals.Hime.Info.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.Warning.png", accessor.GetStreamFor("Visuals.Hime.Warning.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.Error.png", accessor.GetStreamFor("Visuals.Hime.Error.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.Shift.png", accessor.GetStreamFor("Visuals.Hime.Shift.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.Reduce.png", accessor.GetStreamFor("Visuals.Hime.Reduce.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.None.png", accessor.GetStreamFor("Visuals.Hime.None.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.ShiftReduce.png", accessor.GetStreamFor("Visuals.Hime.ShiftReduce.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.ReduceReduce.png", accessor.GetStreamFor("Visuals.Hime.ReduceReduce.png")));

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.AppendChild(Export_GetData(doc));
            doc.Save(directory + "\\data.xml");
            accessor.AddCheckoutFile(directory + "\\data.xml");
            
            // generate header
            accessor.CheckOut("Transforms.Doc.Header.xslt", directory + "\\Header.xslt");
            System.Xml.Xsl.XslCompiledTransform transform = new System.Xml.Xsl.XslCompiledTransform();
            transform.Load(directory + "\\Header.xslt");
            transform.Transform(directory + "\\data.xml", directory + "\\header.html");
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileText("text/html", "utf-8", "header.html", directory + "\\header.html"));
            accessor.AddCheckoutFile(directory + "\\header.html");
            // generate grammar
            accessor.CheckOut("Transforms.Doc.Grammar.xslt", directory + "\\Grammar.xslt");
            transform = new System.Xml.Xsl.XslCompiledTransform();
            transform.Load(directory + "\\Grammar.xslt");
            transform.Transform(directory + "\\data.xml", directory + "\\grammar.html");
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileText("text/html", "utf-8", "grammar.html", directory + "\\grammar.html"));
            accessor.AddCheckoutFile(directory + "\\grammar.html");

            doc = new System.Xml.XmlDocument();
            List<System.Xml.XmlNode> nodes = new List<System.Xml.XmlNode>();
            System.Xml.XmlNode nodeGraph = data.SerializeXML(doc);
            foreach (System.Xml.XmlNode child in nodeGraph.ChildNodes)
                nodes.Add(child);

            // generate sets
            string tfile = "ParserData_LR1";
            if (data is LR.ParserDataLRStar)
            {
                if (options.ExportVisuals) tfile = "ParserData_LRStarSVG";
                else tfile = "ParserData_LRStarDOT";
            }
            accessor.CheckOut("Transforms.Doc." + tfile + ".xslt", directory + "\\" + tfile + ".xslt");
            transform = new System.Xml.Xsl.XslCompiledTransform();
            transform.Load(directory + "\\" + tfile + ".xslt");
            foreach (System.Xml.XmlNode child in nodes)
            {
                string temp = directory + "\\Set_" + child.Attributes["SetID"].Value;
                while (doc.HasChildNodes)
                    doc.RemoveChild(doc.FirstChild);
                doc.AppendChild(child);
                doc.Save(temp + ".xml");
                accessor.AddCheckoutFile(temp + ".xml");
                transform.Transform(temp + ".xml", temp + ".html");
                compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileText("text/html", "utf-8", "Set_" + child.Attributes["SetID"].Value + ".html", temp + ".html"));
                accessor.AddCheckoutFile(temp + ".html");
            }

            while (doc.HasChildNodes)
                doc.RemoveChild(doc.FirstChild);
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));
            doc.AppendChild(nodeGraph);
            foreach (System.Xml.XmlNode child in nodes)
                nodeGraph.AppendChild(child);
            doc.Save(directory + "\\data.xml");
            // generate menu
            accessor.CheckOut("Transforms.Doc.Menu.xslt", directory + "\\Menu.xslt");
            transform = new System.Xml.Xsl.XslCompiledTransform();
            transform.Load(directory + "\\Menu.xslt");
            transform.Transform(directory + "\\data.xml", directory + "\\menu.html");
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileText("text/html", "utf-8", "menu.html", directory + "\\menu.html"));
            accessor.AddCheckoutFile(directory + "\\menu.html");

            // export parser data
            List<string> files = data.SerializeVisuals(directory, options);
            foreach (string file in files)
            {
                System.IO.FileInfo info = new System.IO.FileInfo(file);
                if (file.EndsWith(".svg"))
                    compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileImage("image/svg+xml", info.Name, file));
                else
                    compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileText("text/plain", "utf-8", info.Name, file));
                accessor.AddCheckoutFile(file);
            }

            compiler.CompileTo(options.Documentation);
            accessor.Close();
            System.IO.Directory.Delete(directory, true);
        }
        
        protected XmlNode Export_GetData(XmlDocument document)
        {
            XmlNode root = document.CreateElement("CFGrammar");
            root.Attributes.Append(document.CreateAttribute("Name"));
            root.Attributes["Name"].Value = name;
            foreach (Variable var in variables.Values)
                root.AppendChild(var.GetXMLNode(document));
            return root;
        }

        protected bool Prepare_AddRealAxiom(Hime.Kernel.Reporting.Reporter Log)
        {
            Log.Info("Grammar", "Creating axiom ...");

            // Search for Axiom option
            if (!options.ContainsKey("Axiom"))
            {
                Log.Error("Grammar", "Axiom option is undefined");
                return false;
            }
            // Search for the variable specified as the Axiom
            string name = options["Axiom"];
            if (!variables.ContainsKey(name))
            {
                Log.Error("Grammar", "Cannot find axiom variable " + name);
                return false;
            }

            // Create the real axiom rule variable and rule
            Variable Axiom = AddVariable("_Axiom_");
            List<RuleDefinitionPart> Parts = new List<RuleDefinitionPart>();
            Parts.Add(new RuleDefinitionPart(variables[name], RuleDefinitionPartAction.Promote));
            Parts.Add(new RuleDefinitionPart(TerminalDollar.Instance, RuleDefinitionPartAction.Drop));
            Axiom.AddRule(new CFRule(Axiom, new CFRuleDefinition(Parts), false));

            Log.Info("Grammar", "Done !");
            return true;
        }
        protected bool Prepare_ComputeFirsts(Hime.Kernel.Reporting.Reporter Log)
        {
            Log.Info("Grammar", "Computing Firsts sets ...");

            bool mod = true;
            // While some modification has occured, repeat the process
            while (mod)
            {
                mod = false;
                foreach (Variable Var in variables.Values)
                    if (Var.ComputeFirsts())
                        mod = true;
            }

            Log.Info("Grammar", "Done !");
            return true;
        }
        
        protected bool Prepare_ComputeFollowers(Hime.Kernel.Reporting.Reporter Log)
        {
            Log.Info("Grammar", "Computing Followers sets ...");

            bool mod = true;
            // Apply step 1 to each variable
            foreach (Variable Var in variables.Values)
                Var.ComputeFollowers_Step1();
            // Apply step 2 and 3 while some modification has occured
            while (mod)
            {
                mod = false;
                foreach (Variable Var in variables.Values)
                    if (Var.ComputeFollowers_Step23())
                        mod = true;
            }

            Log.Info("Grammar", "Done !");
            return true;
        }
    }
}
