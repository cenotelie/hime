using System.Collections.Generic;

namespace Hime.Parsers.CF
{
    public interface CFParserGenerator : ParserGenerator
    {
        ParserData Build(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Reporter);
    }

    public abstract class CFGrammar : Grammar
    {
        protected string p_Name;
        protected ushort p_NextSID;
        protected Dictionary<string, string> p_Options;
        protected Dictionary<string, Terminal> p_Terminals;
        protected Dictionary<string, CFVariable> p_Variables;
        protected Dictionary<string, Virtual> p_Virtuals;
        protected Dictionary<string, Action> p_Actions;
        internal List<CFGrammarTemplateRule> p_TemplateRules;

        public override string LocalName { get { return p_Name; } }
        public int NextSID { get { return p_NextSID; } }
        public ICollection<string> Options { get { return p_Options.Keys; } }
        public ICollection<Terminal> Terminals { get { return p_Terminals.Values; } }
        public ICollection<CFVariable> Variables { get { return p_Variables.Values; } }
        public ICollection<Virtual> Virtuals { get { return p_Virtuals.Values; } }
        public ICollection<Action> Actions { get { return p_Actions.Values; } }
        public List<CFRule> Rules
        {
            get
            {
                List<CFRule> p_Rules = new List<CFRule>();
                foreach (CFVariable Variable in p_Variables.Values)
                    foreach (CFRule Rule in Variable.Rules)
                        p_Rules.Add(Rule);
                return p_Rules;
            }
        }
        internal ICollection<CFGrammarTemplateRule> TemplateRules { get { return p_TemplateRules; } }

        public CFGrammar(string Name) : base()
        {
            p_Options = new Dictionary<string, string>();
            p_Terminals = new Dictionary<string, Terminal>();
            p_Variables = new Dictionary<string, CFVariable>();
            p_Virtuals = new Dictionary<string, Virtual>();
            p_Actions = new Dictionary<string, Action>();
            p_TemplateRules = new List<CFGrammarTemplateRule>();
            p_Name = Name;
            p_NextSID = 3;
        }

        public void AddOption(string Name, string Value)
        {
            if (p_Options.ContainsKey(Name))
                p_Options[Name] = Value;
            else
                p_Options.Add(Name, Value);
        }
        public string GetOption(string Name)
        {
            if (!p_Options.ContainsKey(Name))
                return null;
            return p_Options[Name];
        }

        public Symbol GetSymbol(string Name)
        {
            if (p_Terminals.ContainsKey(Name)) return p_Terminals[Name];
            if (p_Variables.ContainsKey(Name)) return p_Variables[Name];
            if (p_Virtuals.ContainsKey(Name)) return p_Virtuals[Name];
            if (p_Actions.ContainsKey(Name)) return p_Actions[Name];
            return null;
        }

        public TerminalText AddTerminalText(string Name, Automata.NFA NFA, Grammar SubGrammar)
        {
            if (p_Children.ContainsKey(Name) && p_Terminals.ContainsKey(Name))
            {
                TerminalText Terminal = (TerminalText)p_Terminals[Name];
                Terminal.Priority = p_NextSID;
                Terminal.NFA = NFA;
                Terminal.SubGrammar = SubGrammar;
                p_NextSID++;
                return Terminal;
            }
            else
            {
                TerminalText Terminal = new TerminalText(this, p_NextSID, Name, p_NextSID, NFA, SubGrammar);
                p_Children.Add(Name, Terminal);
                p_Terminals.Add(Name, Terminal);
                p_NextSID++;
                return Terminal;
            }
        }
        public TerminalBin AddTerminalBin(TerminalBinType Type, string Value)
        {
            if (p_Children.ContainsKey(Value) && p_Terminals.ContainsKey(Value))
            {
                TerminalBin Terminal = (TerminalBin)p_Terminals[Value];
                Terminal.Type = Type;
                Terminal.Value = Value;
                p_NextSID++;
                return Terminal;
            }
            else
            {
                TerminalBin Terminal = new TerminalBin(this, p_NextSID, Value, p_NextSID, Type, Value.Substring(2, Value.Length - 2));
                p_Children.Add(Value, Terminal);
                p_Terminals.Add(Value, Terminal);
                p_NextSID++;
                return Terminal;
            }
        }
        public Terminal GetTerminal(string Name)
        {
            if (!p_Terminals.ContainsKey(Name))
                return null;
            return p_Terminals[Name];
        }

        public CFVariable AddVariable(string Name)
        {
            if (p_Variables.ContainsKey(Name))
                return p_Variables[Name];
            CFVariable Var = new CFVariable(this, p_NextSID, Name);
            p_Children.Add(Name, Var);
            p_Variables.Add(Name, Var);
            p_NextSID++;
            return Var;
        }
        public CFVariable GetVariable(string Name)
        {
            if (!p_Variables.ContainsKey(Name))
                return null;
            return p_Variables[Name];
        }

        public Virtual AddVirtual(string Name)
        {
            if (p_Virtuals.ContainsKey(Name))
                return p_Virtuals[Name];
            Virtual Virtual = new Virtual(this, Name);
            p_Children.Add(Name, Virtual);
            p_Virtuals.Add(Name, Virtual);
            return Virtual;
        }
        public Virtual GetVirtual(string Name)
        {
            if (!p_Virtuals.ContainsKey(Name))
                return null;
            return p_Virtuals[Name];
        }

        public Action AddAction(string Name)
        {
            Action Action = new Action(this, Name);
            p_Children.Add(Name, Action);
            p_Actions.Add(Name, Action);
            return Action;
        }
        public Action GetAction(string Name)
        {
            if (!p_Actions.ContainsKey(Name))
                return null;
            return p_Actions[Name];
        }

        internal CFGrammarTemplateRule AddTemplateRule(Redist.Parsers.SyntaxTreeNode RuleNode, CFGrammarCompiler Compiler)
        {
            CFGrammarTemplateRule Rule = new CFGrammarTemplateRule(this, Compiler, RuleNode);
            p_TemplateRules.Add(Rule);
            return Rule;
        }

        public abstract void Inherit(CFGrammar Parent);
        public abstract CFGrammar Clone();

        protected void Export_Documentation(ParserData Data, string Directory)
        {
            if (!Directory.Equals("") && !System.IO.Directory.Exists(Directory))
                System.IO.Directory.CreateDirectory(Directory);
            Export_Resources(Directory);
            Export_GrammarData(Directory);
            Export_ParserData(Directory, Data);
            Export_ParserGraph(Directory, Data);
        }
        protected System.Xml.XmlNode Export_GetData(System.Xml.XmlDocument Document)
        {
            System.Xml.XmlNode root = Document.CreateElement("CFGrammar");
            root.Attributes.Append(Document.CreateAttribute("Name"));
            root.Attributes["Name"].Value = p_Name;
            foreach (CFVariable var in p_Variables.Values)
                root.AppendChild(var.GetXMLNode(Document));
            return root;
        }
        protected void Export_Resources(string directory)
        {
            System.IO.Directory.CreateDirectory(directory + "\\hime_data");
            Kernel.Resources.ResourceAccessor Accessor = new Kernel.Resources.ResourceAccessor();
            Accessor.Export("Transforms.Hime.css", directory + "\\hime_data\\Hime.css");
            Accessor.Export("Transforms.Hime.js", directory + "\\hime_data\\Hime.js");
            Accessor.Export("Visuals.button_plus.gif", directory + "\\hime_data\\button_plus.gif");
            Accessor.Export("Visuals.button_minus.gif", directory + "\\hime_data\\button_minus.gif");

            Accessor.Export("Visuals.Hime.Shift.png", directory + "\\hime_data\\Hime.Shift.png");
            Accessor.Export("Visuals.Hime.Reduce.png", directory + "\\hime_data\\Hime.Reduce.png");
            Accessor.Export("Visuals.Hime.None.png", directory + "\\hime_data\\Hime.None.png");
            Accessor.Export("Visuals.Hime.ShiftReduce.png", directory + "\\hime_data\\Hime.ShiftReduce.png");
            Accessor.Export("Visuals.Hime.ReduceReduce.png", directory + "\\hime_data\\Hime.ReduceReduce.png");

            Accessor.Export("Visuals.Hime.Error.png", directory + "\\hime_data\\Hime.Error.png");
            Accessor.Export("Visuals.Hime.Warning.png", directory + "\\hime_data\\Hime.Warning.png");
            Accessor.Export("Visuals.Hime.Info.png", directory + "\\hime_data\\Hime.Info.png");
            Accessor.Export("Visuals.Hime.Logo.png", directory + "\\hime_data\\Hime.Logo.png");
            Accessor.Close();
        }
        protected void Export_GrammarData(string directory)
        {
            string fileName = directory + "\\Grammar";
            
            System.Xml.XmlDocument Doc = new System.Xml.XmlDocument();
            Doc.AppendChild(Export_GetData(Doc));
            System.IO.FileInfo File = new System.IO.FileInfo(fileName);
            Doc.Save(fileName + ".xml");

            Kernel.Resources.ResourceAccessor Accessor = new Kernel.Resources.ResourceAccessor();
            Accessor.AddCheckoutFile(fileName + ".xml");
            Accessor.CheckOut("Transforms.Doc.CFGrammar.xslt", File.DirectoryName + "CFGrammar.xslt");

            System.Xml.Xsl.XslCompiledTransform Transform = new System.Xml.Xsl.XslCompiledTransform();
            Transform.Load(File.DirectoryName + "CFGrammar.xslt");
            Transform.Transform(fileName + ".xml", fileName + ".html");

            Accessor.Close();
        }
        protected void Export_ParserData(string directory, ParserData data)
        {
            System.Xml.XmlDocument Doc = new System.Xml.XmlDocument();
            Kernel.Resources.ResourceAccessor Accessor = new Kernel.Resources.ResourceAccessor();
            Accessor.CheckOut("Transforms.Doc.LRParserData.xslt", directory + "\\LRParserData.xslt");
            System.Xml.Xsl.XslCompiledTransform Transform = new System.Xml.Xsl.XslCompiledTransform();
            Transform.Load(directory + "\\LRParserData.xslt");
            List<System.Xml.XmlNode> nodes = new List<System.Xml.XmlNode>();
            foreach (System.Xml.XmlNode child in data.SerializeXML(Doc).ChildNodes)
                nodes.Add(child);
            foreach (System.Xml.XmlNode child in nodes)
            {
                string fileName = directory + "\\Set_" + child.Attributes["SetID"].Value;
                while (Doc.HasChildNodes)
                    Doc.RemoveChild(Doc.FirstChild);
                Doc.AppendChild(child);
                System.IO.FileInfo File = new System.IO.FileInfo(fileName);
                Doc.Save(fileName + ".xml");
                Accessor.AddCheckoutFile(fileName + ".xml");
                Transform.Transform(fileName + ".xml", fileName + ".html");
            }
            Accessor.Close();
        }
        protected void Export_ParserGraph(string directory, ParserData data)
        {
            Kernel.Graphs.DOTSerializer serializer = new Kernel.Graphs.DOTSerializer("Parser", directory + "\\GraphParser.dot");
            data.SerializeVisual(serializer);
            serializer.Close();
        }

        protected bool Prepare_AddRealAxiom(Hime.Kernel.Reporting.Reporter Log)
        {
            Log.Info("Grammar", "Creating axiom ...");

            // Search for Axiom option
            if (!p_Options.ContainsKey("Axiom"))
            {
                Log.Error("Grammar", "Axiom option is undefined");
                return false;
            }
            // Search for the variable specified as the Axiom
            string name = p_Options["Axiom"];
            if (!p_Variables.ContainsKey(name))
            {
                Log.Error("Grammar", "Cannot find axiom variable " + name);
                return false;
            }

            // Create the real axiom rule variable and rule
            CFVariable Axiom = AddVariable("_Axiom_");
            List<RuleDefinitionPart> Parts = new List<RuleDefinitionPart>();
            Parts.Add(new RuleDefinitionPart(p_Variables[name], RuleDefinitionPartAction.Promote));
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
                foreach (CFVariable Var in p_Variables.Values)
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
            foreach (CFVariable Var in p_Variables.Values)
                Var.ComputeFollowers_Step1();
            // Apply step 2 and 3 while some modification has occured
            while (mod)
            {
                mod = false;
                foreach (CFVariable Var in p_Variables.Values)
                    if (Var.ComputeFollowers_Step23())
                        mod = true;
            }

            Log.Info("Grammar", "Done !");
            return true;
        }
    }


    public sealed class CFGrammarText : CFGrammar
    {
        private Automata.DFA p_FinalDFA;

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
                p_TemplateRules.Add(new CFGrammarTemplateRule(TemplateRule, this));
            foreach (CFVariable Variable in Parent.Variables)
            {
                CFVariable Clone = p_Variables[Variable.LocalName];
                foreach (CFRule R in Variable.Rules)
                {
                    List<RuleDefinitionPart> Parts = new List<RuleDefinitionPart>();
                    CFRuleDefinition Def = new CFRuleDefinition();
                    foreach (RuleDefinitionPart Part in R.Definition.Parts)
                    {
                        Symbol S = null;
                        if (Part.Symbol is CFVariable)
                            S = p_Variables[Part.Symbol.LocalName];
                        else if (Part.Symbol is Terminal)
                            S = p_Terminals[Part.Symbol.LocalName];
                        else if (Part.Symbol is Virtual)
                            S = p_Virtuals[Part.Symbol.LocalName];
                        else if (Part.Symbol is Action)
                            S = p_Actions[Part.Symbol.LocalName];
                        Parts.Add(new RuleDefinitionPart(S, Part.Action));
                    }
                    Clone.AddRule(new CFRule(Clone, new CFRuleDefinition(Parts), R.ReplaceOnProduction));
                }
            }
        }
        public override CFGrammar Clone()
        {
            CFGrammar Result = new CFGrammarText(p_Name);
            Result.Inherit(this);
            return Result;
        }

        private bool Prepare_DFA(Hime.Kernel.Reporting.Reporter Log)
        {
            Log.Info("Grammar", "Generating DFA for Terminals ...");

            // Construct a global NFA for all the terminals
            Automata.NFA Final = new Automata.NFA();
            Final.StateEntry = Final.AddNewState();
            foreach (TerminalText Terminal in p_Terminals.Values)
            {
                Automata.NFA Sub = Terminal.NFA.Clone();
                Final.InsertSubNFA(Sub);
                Final.StateEntry.AddTransition(Automata.NFA.Epsilon, Sub.StateEntry);
            }
            // Construct the equivalent DFA and minimize it
            p_FinalDFA = new Automata.DFA(Final);
            p_FinalDFA = p_FinalDFA.Minimize();
            p_FinalDFA.RepackTransitions();

            Log.Info("Grammar", "Done !");
            return true;
        }

        public override bool Build(GrammarBuildOptions Options)
        {
            Options.Reporter.BeginSection(p_Name + " parser data generation");
            if (!Prepare_AddRealAxiom(Options.Reporter)) { Options.Reporter.EndSection(); return false; }
            if (!Prepare_ComputeFirsts(Options.Reporter)) { Options.Reporter.EndSection(); return false; }
            if (!Prepare_ComputeFollowers(Options.Reporter)) { Options.Reporter.EndSection(); return false; }
            if (!Prepare_DFA(Options.Reporter)) { Options.Reporter.EndSection(); return false; }
            Options.Reporter.Info("Grammar", "Lexer DFA generated");

            Terminal Separator = null;
            if (p_Options.ContainsKey("Separator"))
                Separator = p_Terminals[p_Options["Separator"]];

            //Generate lexer
            Exporters.TextLexerExporter LexerExporter = new Exporters.TextLexerExporter(Options.LexerWriter, Options.Namespace, p_Name, p_FinalDFA, Separator);
            LexerExporter.Export();

            //Generate parser
            Options.Reporter.Info("Grammar", "Parsing method is " + Options.ParserGenerator.Name);
            ParserData Data = Options.ParserGenerator.Build(this, Options.Reporter);
            if (Data == null) { Options.Reporter.EndSection(); return false; }
            bool result = Data.Export(Options);
            Options.Reporter.EndSection();

            //Output data
            if (Options.DocumentationDir != null)
            {
                Export_Documentation(Data, Options.DocumentationDir);
                Kernel.Graphs.DOTSerializer serializer = new Kernel.Graphs.DOTSerializer("Lexer", Options.DocumentationDir + "\\GraphLexer.dot");
                p_FinalDFA.SerializeGraph(serializer);
                serializer.Close();
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
                p_TemplateRules.Add(new CFGrammarTemplateRule(TemplateRule, this));
            foreach (CFVariable Variable in Parent.Variables)
            {
                CFVariable Clone = p_Variables[Variable.LocalName];
                foreach (CFRule R in Variable.Rules)
                {
                    List<RuleDefinitionPart> Parts = new List<RuleDefinitionPart>();
                    CFRuleDefinition Def = new CFRuleDefinition();
                    foreach (RuleDefinitionPart Part in R.Definition.Parts)
                    {
                        Symbol S = null;
                        if (Part.Symbol is CFVariable)
                            S = p_Variables[Part.Symbol.LocalName];
                        else if (Part.Symbol is Terminal)
                            S = p_Terminals[Part.Symbol.LocalName];
                        else if (Part.Symbol is Virtual)
                            S = p_Virtuals[Part.Symbol.LocalName];
                        else if (Part.Symbol is Action)
                            S = p_Actions[Part.Symbol.LocalName];
                        Parts.Add(new RuleDefinitionPart(S, Part.Action));
                    }
                    Clone.AddRule(new CFRule(Clone, new CFRuleDefinition(Parts), R.ReplaceOnProduction));
                }
            }
        }
        public override CFGrammar Clone()
        {
            CFGrammar Result = new CFGrammarBinary(p_Name);
            Result.Inherit(this);
            return Result;
        }

        public override bool Build(GrammarBuildOptions Options)
        {
            Options.Reporter.BeginSection(p_Name + " parser data generation");
            if (!Prepare_AddRealAxiom(Options.Reporter)) { Options.Reporter.EndSection(); return false; }
            if (!Prepare_ComputeFirsts(Options.Reporter)) { Options.Reporter.EndSection(); return false; }
            if (!Prepare_ComputeFollowers(Options.Reporter)) { Options.Reporter.EndSection(); return false; }
            Options.Reporter.Info("Grammar", "Lexer DFA generated");

            //Generate lexer

            //Generate parser
            Options.Reporter.Info("Grammar", "Parsing method is " + Options.ParserGenerator.Name);
            ParserData Data = Options.ParserGenerator.Build(this, Options.Reporter);
            if (Data == null) { Options.Reporter.EndSection(); return false; }
            bool result = Data.Export(Options);
            Options.Reporter.EndSection();
            
            //Output data
            if (Options.DocumentationDir != null)
                Export_Documentation(Data, Options.DocumentationDir);
            return result;
        }
    }
}
