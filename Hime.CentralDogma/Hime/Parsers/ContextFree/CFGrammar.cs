/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;
using Hime.Kernel.Reporting;
using System.Xml;

namespace Hime.Parsers.ContextFree
{
    public abstract class CFGrammar : Grammar
    {
        protected string name;
        protected ushort nextSID;
        protected Dictionary<string, string> options;
        protected Dictionary<string, Terminal> terminals;
        protected Dictionary<string, Variable> variables;
        protected Dictionary<string, Virtual> virtuals;
        protected Dictionary<string, Action> actions;
        internal List<TemplateRule> templateRules;

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
                foreach (Variable var in variables.Values)
                    foreach (CFRule rule in var.Rules)
                        rules.Add(rule);
                return rules;
            }
        }
        internal ICollection<TemplateRule> TemplateRules { get { return templateRules; } }

        public CFGrammar(string name) : base()
        {
            this.options = new Dictionary<string, string>();
            this.terminals = new Dictionary<string, Terminal>();
            this.variables = new Dictionary<string, Variable>();
            this.virtuals = new Dictionary<string, Virtual>();
            this.actions = new Dictionary<string, Action>();
            this.templateRules = new List<TemplateRule>();
            this.name = name;
            this.nextSID = 3;
        }

        public void AddOption(string name, string value)
        {
            if (options.ContainsKey(name))
                options[name] = value;
            else
                options.Add(name, value);
        }
        public string GetOption(string name)
        {
            if (!options.ContainsKey(name))
                return null;
            return options[name];
        }

        public Symbol GetSymbol(string name)
        {
            if (terminals.ContainsKey(name)) return terminals[name];
            if (variables.ContainsKey(name)) return variables[name];
            if (virtuals.ContainsKey(name)) return virtuals[name];
            if (actions.ContainsKey(name)) return actions[name];
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
        
        public TerminalBin AddTerminalBin(TerminalBinType type, string value)
        {
            if (children.ContainsKey(value) && terminals.ContainsKey(value))
            {
                TerminalBin Terminal = (TerminalBin)terminals[value];
                Terminal.Type = type;
                Terminal.Value = value;
                nextSID++;
                return Terminal;
            }
            else
            {
                TerminalBin Terminal = new TerminalBin(this, nextSID, value, nextSID, type, value.Substring(2, value.Length - 2));
                children.Add(value, Terminal);
                terminals.Add(value, Terminal);
                nextSID++;
                return Terminal;
            }
        }
        public Terminal GetTerminal(string name)
        {
            if (!terminals.ContainsKey(name))
                return null;
            return terminals[name];
        }

        public Variable AddVariable(string name)
        {
            if (variables.ContainsKey(name))
                return variables[name];
            Variable Var = new Variable(this, nextSID, name);
            children.Add(name, Var);
            variables.Add(name, Var);
            nextSID++;
            return Var;
        }
        
        public Variable GetVariable(string name)
        {
            if (!variables.ContainsKey(name))
                return null;
            return variables[name];
        }

        public Virtual AddVirtual(string name)
        {
            if (virtuals.ContainsKey(name))
                return virtuals[name];
            Virtual Virtual = new Virtual(this, name);
            children.Add(name, Virtual);
            virtuals.Add(name, Virtual);
            return Virtual;
        }
        public Virtual GetVirtual(string name)
        {
            if (!virtuals.ContainsKey(name))
                return null;
            return virtuals[name];
        }

        public Action AddAction(string name)
        {
            Action Action = new Action(this, name);
            children.Add(name, Action);
            actions.Add(name, Action);
            return Action;
        }
        public Action GetAction(string name)
        {
            if (!actions.ContainsKey(name))
                return null;
            return actions[name];
        }

        internal TemplateRule AddTemplateRule(Redist.Parsers.SyntaxTreeNode ruleNode, CFGrammarCompiler compiler)
        {
            TemplateRule Rule = new TemplateRule(this, compiler, ruleNode);
            templateRules.Add(Rule);
            return Rule;
        }

        public virtual void Inherit(CFGrammar parent)
        {
            foreach (string option in parent.Options)
                AddOption(option, parent.GetOption(option));
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

        protected bool Prepare_AddRealAxiom(Hime.Kernel.Reporting.Reporter log)
        {
            log.Info("Grammar", "Creating axiom ...");

            // Search for Axiom option
            if (!options.ContainsKey("Axiom"))
            {
                log.Error("Grammar", "Axiom option is undefined");
                return false;
            }
            // Search for the variable specified as the Axiom
            string name = options["Axiom"];
            if (!variables.ContainsKey(name))
            {
                log.Error("Grammar", "Cannot find axiom variable " + name);
                return false;
            }

            // Create the real axiom rule variable and rule
            Variable axiom = AddVariable("_Axiom_");
            List<RuleDefinitionPart> parts = new List<RuleDefinitionPart>();
            parts.Add(new RuleDefinitionPart(variables[name], RuleDefinitionPartAction.Promote));
            parts.Add(new RuleDefinitionPart(TerminalDollar.Instance, RuleDefinitionPartAction.Drop));
            axiom.AddRule(new CFRule(axiom, new CFRuleDefinition(parts), false));

            log.Info("Grammar", "Done !");
            return true;
        }
        protected bool Prepare_ComputeFirsts(Hime.Kernel.Reporting.Reporter log)
        {
            log.Info("Grammar", "Computing Firsts sets ...");

            bool mod = true;
            // While some modification has occured, repeat the process
            while (mod)
            {
                mod = false;
                foreach (Variable Var in variables.Values)
                    if (Var.ComputeFirsts())
                        mod = true;
            }

            log.Info("Grammar", "Done !");
            return true;
        }
        
        protected bool Prepare_ComputeFollowers(Hime.Kernel.Reporting.Reporter log)
        {
            log.Info("Grammar", "Computing Followers sets ...");

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

            log.Info("Grammar", "Done !");
            return true;
        }
    }
}
