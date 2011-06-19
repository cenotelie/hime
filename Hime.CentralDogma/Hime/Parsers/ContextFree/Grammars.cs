using System.Collections.Generic;

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
        protected Dictionary<string, CFVariable> variables;
        protected Dictionary<string, Virtual> virtuals;
        protected Dictionary<string, Action> actions;
        internal List<CFGrammarTemplateRule> templateRules;

        public override string LocalName { get { return name; } }
        public int NextSID { get { return nextSID; } }
        public ICollection<string> Options { get { return options.Keys; } }
        public ICollection<Terminal> Terminals { get { return terminals.Values; } }
        public ICollection<CFVariable> Variables { get { return variables.Values; } }
        public ICollection<Virtual> Virtuals { get { return virtuals.Values; } }
        public ICollection<Action> Actions { get { return actions.Values; } }
        public List<CFRule> Rules
        {
            get
            {
                List<CFRule> rules = new List<CFRule>();
                foreach (CFVariable Variable in variables.Values)
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
            variables = new Dictionary<string, CFVariable>();
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

        public TerminalText AddTerminalText(string Name, Automata.NFA NFA, Grammar SubGrammar)
        {
            if (children.ContainsKey(Name) && terminals.ContainsKey(Name))
            {
                TerminalText Terminal = (TerminalText)terminals[Name];
                Terminal.Priority = nextSID;
                Terminal.NFA = NFA;
                Terminal.SubGrammar = SubGrammar;
                nextSID++;
                return Terminal;
            }
            else
            {
                TerminalText Terminal = new TerminalText(this, nextSID, Name, nextSID, NFA, SubGrammar);
                children.Add(Name, Terminal);
                terminals.Add(Name, Terminal);
                nextSID++;
                return Terminal;
            }
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

        public CFVariable AddVariable(string Name)
        {
            if (variables.ContainsKey(Name))
                return variables[Name];
            CFVariable Var = new CFVariable(this, nextSID, Name);
            children.Add(Name, Var);
            variables.Add(Name, Var);
            nextSID++;
            return Var;
        }
        public CFVariable GetVariable(string Name)
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

        public abstract void Inherit(CFGrammar Parent);
        public abstract CFGrammar Clone();

        protected void Export_Documentation(ParserData data, string fileName, bool doLayout)
        {
            string directory = fileName + "_temp";
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

            System.Xml.XmlDocument Doc = new System.Xml.XmlDocument();
            Doc.AppendChild(Export_GetData(Doc));
            Doc.Save(directory + "\\data.xml");
            accessor.AddCheckoutFile(directory + "\\data.xml");
            
            // generate header
            accessor.CheckOut("Transforms.Doc.Header.xslt", directory + "\\Header.xslt");
            System.Xml.Xsl.XslCompiledTransform Transform = new System.Xml.Xsl.XslCompiledTransform();
            Transform.Load(directory + "\\Header.xslt");
            Transform.Transform(directory + "\\data.xml", directory + "\\header.html");
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileText("text/html", "utf-8", "header.html", directory + "\\header.html"));
            accessor.AddCheckoutFile(directory + "\\header.html");
            // generate grammar
            accessor.CheckOut("Transforms.Doc.Grammar.xslt", directory + "\\Grammar.xslt");
            Transform = new System.Xml.Xsl.XslCompiledTransform();
            Transform.Load(directory + "\\Grammar.xslt");
            Transform.Transform(directory + "\\data.xml", directory + "\\grammar.html");
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileText("text/html", "utf-8", "grammar.html", directory + "\\grammar.html"));
            accessor.AddCheckoutFile(directory + "\\grammar.html");

            Doc = new System.Xml.XmlDocument();
            List<System.Xml.XmlNode> nodes = new List<System.Xml.XmlNode>();
            System.Xml.XmlNode nodeGraph = data.SerializeXML(Doc);
            foreach (System.Xml.XmlNode child in nodeGraph.ChildNodes)
                nodes.Add(child);

            // generate sets
            accessor.CheckOut("Transforms.Doc.LRParserData.xslt", directory + "\\LRParserData.xslt");
            Transform = new System.Xml.Xsl.XslCompiledTransform();
            Transform.Load(directory + "\\LRParserData.xslt");
            foreach (System.Xml.XmlNode child in nodes)
            {
                string temp = directory + "\\Set_" + child.Attributes["SetID"].Value;
                while (Doc.HasChildNodes)
                    Doc.RemoveChild(Doc.FirstChild);
                Doc.AppendChild(child);
                Doc.Save(temp + ".xml");
                accessor.AddCheckoutFile(temp + ".xml");
                Transform.Transform(temp + ".xml", temp + ".html");
                compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileText("text/html", "utf-8", "Set_" + child.Attributes["SetID"].Value + ".html", temp + ".html"));
                accessor.AddCheckoutFile(temp + ".html");
            }

            while (Doc.HasChildNodes)
                Doc.RemoveChild(Doc.FirstChild);
            Doc.AppendChild(Doc.CreateXmlDeclaration("1.0", "utf-8", null));
            Doc.AppendChild(nodeGraph);
            foreach (System.Xml.XmlNode child in nodes)
                nodeGraph.AppendChild(child);
            Doc.Save(directory + "\\data.xml");
            // generate menu
            accessor.CheckOut("Transforms.Doc.Menu.xslt", directory + "\\Menu.xslt");
            Transform = new System.Xml.Xsl.XslCompiledTransform();
            Transform.Load(directory + "\\Menu.xslt");
            Transform.Transform(directory + "\\data.xml", directory + "\\menu.html");
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileText("text/html", "utf-8", "menu.html", directory + "\\menu.html"));
            accessor.AddCheckoutFile(directory + "\\menu.html");

            // export parser data
            List<string> files = data.SerializeVisuals(directory, doLayout);
            foreach (string file in files)
            {
                System.IO.FileInfo info = new System.IO.FileInfo(file);
                if (file.EndsWith(".svg"))
                    compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileImage("image/svg+xml", info.Name, file));
                else
                    compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileText("text/plain", "utf-8", info.Name, file));
                accessor.AddCheckoutFile(file);
            }

            compiler.CompileTo(fileName);
            accessor.Close();
            System.IO.Directory.Delete(directory, true);
        }
        protected System.Xml.XmlNode Export_GetData(System.Xml.XmlDocument Document)
        {
            System.Xml.XmlNode root = Document.CreateElement("CFGrammar");
            root.Attributes.Append(Document.CreateAttribute("Name"));
            root.Attributes["Name"].Value = name;
            foreach (CFVariable var in variables.Values)
                root.AppendChild(var.GetXMLNode(Document));
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
            CFVariable Axiom = AddVariable("_Axiom_");
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
                foreach (CFVariable Var in variables.Values)
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
            foreach (CFVariable Var in variables.Values)
                Var.ComputeFollowers_Step1();
            // Apply step 2 and 3 while some modification has occured
            while (mod)
            {
                mod = false;
                foreach (CFVariable Var in variables.Values)
                    if (Var.ComputeFollowers_Step23())
                        mod = true;
            }

            Log.Info("Grammar", "Done !");
            return true;
        }
    }


    public sealed class CFGrammarText : CFGrammar
    {
        private Automata.DFA finalDFA;

        public CFGrammarText(string Name) : base(Name) { }

        public override void Inherit(CFGrammar Parent)
        {
            foreach (string Option in Parent.Options)
                AddOption(Option, Parent.GetOption(Option));
            foreach (TerminalText Terminal in Parent.Terminals)
            {
                TerminalText Clone = AddTerminalText(Terminal.LocalName, Terminal.NFA.Clone(false), Terminal.SubGrammar);
                Clone.NFA.StateExit.Final = Clone;
            }
            foreach (CFVariable Variable in Parent.Variables)
                AddVariable(Variable.LocalName);
            foreach (Virtual Virtual in Parent.Virtuals)
                AddVirtual(Virtual.LocalName);
            foreach (Action Action in Parent.Actions)
                AddAction(Action.LocalName);
            foreach (CFGrammarTemplateRule TemplateRule in Parent.TemplateRules)
                templateRules.Add(new CFGrammarTemplateRule(TemplateRule, this));
            foreach (CFVariable Variable in Parent.Variables)
            {
                CFVariable Clone = variables[Variable.LocalName];
                foreach (CFRule R in Variable.Rules)
                {
                    List<RuleDefinitionPart> Parts = new List<RuleDefinitionPart>();
                    CFRuleDefinition Def = new CFRuleDefinition();
                    foreach (RuleDefinitionPart Part in R.Definition.Parts)
                    {
                        Symbol S = null;
                        if (Part.Symbol is CFVariable)
                            S = variables[Part.Symbol.LocalName];
                        else if (Part.Symbol is Terminal)
                            S = terminals[Part.Symbol.LocalName];
                        else if (Part.Symbol is Virtual)
                            S = virtuals[Part.Symbol.LocalName];
                        else if (Part.Symbol is Action)
                            S = actions[Part.Symbol.LocalName];
                        Parts.Add(new RuleDefinitionPart(S, Part.Action));
                    }
                    Clone.AddRule(new CFRule(Clone, new CFRuleDefinition(Parts), R.ReplaceOnProduction));
                }
            }
        }
        public override CFGrammar Clone()
        {
            CFGrammar Result = new CFGrammarText(name);
            Result.Inherit(this);
            return Result;
        }

        private bool Prepare_DFA(Hime.Kernel.Reporting.Reporter Log)
        {
            Log.Info("Grammar", "Generating DFA for Terminals ...");

            // Construct a global NFA for all the terminals
            Automata.NFA Final = new Automata.NFA();
            Final.StateEntry = Final.AddNewState();
            foreach (TerminalText Terminal in terminals.Values)
            {
                Automata.NFA Sub = Terminal.NFA.Clone();
                Final.InsertSubNFA(Sub);
                Final.StateEntry.AddTransition(Automata.NFA.Epsilon, Sub.StateEntry);
            }
            // Construct the equivalent DFA and minimize it
            finalDFA = new Automata.DFA(Final);
            finalDFA = finalDFA.Minimize();
            finalDFA.RepackTransitions();

            Log.Info("Grammar", "Done !");
            return true;
        }

        public override bool Build(GrammarBuildOptions options)
        {
            options.Reporter.BeginSection(name + " parser data generation");
            if (!Prepare_AddRealAxiom(options.Reporter)) { options.Reporter.EndSection(); return false; }
            if (!Prepare_ComputeFirsts(options.Reporter)) { options.Reporter.EndSection(); return false; }
            if (!Prepare_ComputeFollowers(options.Reporter)) { options.Reporter.EndSection(); return false; }
            if (!Prepare_DFA(options.Reporter)) { options.Reporter.EndSection(); return false; }
            options.Reporter.Info("Grammar", "Lexer DFA generated");

            Terminal Separator = null;
            if (this.options.ContainsKey("Separator"))
                Separator = terminals[this.options["Separator"]];

            //Generate lexer
            Exporters.TextLexerExporter LexerExporter = new Exporters.TextLexerExporter(options.LexerWriter, options.Namespace, name, finalDFA, Separator);
            LexerExporter.Export();

            //Generate parser
            options.Reporter.Info("Grammar", "Parsing method is " + options.ParserGenerator.Name);
            ParserData Data = options.ParserGenerator.Build(this, options.Reporter);
            if (Data == null) { options.Reporter.EndSection(); return false; }
            bool result = Data.Export(options);
            options.Reporter.EndSection();

            //Output data
            if (options.Documentation != null)
            {
                Export_Documentation(Data, options.Documentation, options.BuildVisuals);
                //Kernel.Graphs.DOTSerializer serializer = new Kernel.Graphs.DOTSerializer("Lexer", Options.DocumentationDir + "\\GraphLexer.dot");
                //finalDFA.SerializeGraph(serializer);
                //serializer.Close();
            }
            return result;
        }
    }

    public sealed class CFGrammarBinary : CFGrammar
    {
        public CFGrammarBinary(string Name) : base(Name) { }

        public override void Inherit(CFGrammar Parent)
        {
            foreach (string Option in Parent.Options)
                AddOption(Option, Parent.GetOption(Option));
            foreach (TerminalBin Terminal in Parent.Terminals)
                AddTerminalBin(Terminal.Type, Terminal.LocalName);
            foreach (CFVariable Variable in Parent.Variables)
                AddVariable(Variable.LocalName);
            foreach (Virtual Virtual in Parent.Virtuals)
                AddVirtual(Virtual.LocalName);
            foreach (Action Action in Parent.Actions)
                AddAction(Action.LocalName);
            foreach (CFGrammarTemplateRule TemplateRule in Parent.TemplateRules)
                templateRules.Add(new CFGrammarTemplateRule(TemplateRule, this));
            foreach (CFVariable Variable in Parent.Variables)
            {
                CFVariable Clone = variables[Variable.LocalName];
                foreach (CFRule R in Variable.Rules)
                {
                    List<RuleDefinitionPart> Parts = new List<RuleDefinitionPart>();
                    CFRuleDefinition Def = new CFRuleDefinition();
                    foreach (RuleDefinitionPart Part in R.Definition.Parts)
                    {
                        Symbol S = null;
                        if (Part.Symbol is CFVariable)
                            S = variables[Part.Symbol.LocalName];
                        else if (Part.Symbol is Terminal)
                            S = terminals[Part.Symbol.LocalName];
                        else if (Part.Symbol is Virtual)
                            S = virtuals[Part.Symbol.LocalName];
                        else if (Part.Symbol is Action)
                            S = actions[Part.Symbol.LocalName];
                        Parts.Add(new RuleDefinitionPart(S, Part.Action));
                    }
                    Clone.AddRule(new CFRule(Clone, new CFRuleDefinition(Parts), R.ReplaceOnProduction));
                }
            }
        }
        public override CFGrammar Clone()
        {
            CFGrammar Result = new CFGrammarBinary(name);
            Result.Inherit(this);
            return Result;
        }

        public override bool Build(GrammarBuildOptions options)
        {
            options.Reporter.BeginSection(name + " parser data generation");
            if (!Prepare_AddRealAxiom(options.Reporter)) { options.Reporter.EndSection(); return false; }
            if (!Prepare_ComputeFirsts(options.Reporter)) { options.Reporter.EndSection(); return false; }
            if (!Prepare_ComputeFollowers(options.Reporter)) { options.Reporter.EndSection(); return false; }
            options.Reporter.Info("Grammar", "Lexer DFA generated");

            //Generate lexer

            //Generate parser
            options.Reporter.Info("Grammar", "Parsing method is " + options.ParserGenerator.Name);
            ParserData Data = options.ParserGenerator.Build(this, options.Reporter);
            if (Data == null) { options.Reporter.EndSection(); return false; }
            bool result = Data.Export(options);
            options.Reporter.EndSection();
            
            //Output data
            if (options.Documentation != null)
                Export_Documentation(Data, options.Documentation, options.BuildVisuals);
            return result;
        }
    }
}
