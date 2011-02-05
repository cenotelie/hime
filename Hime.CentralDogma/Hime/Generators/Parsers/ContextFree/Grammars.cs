namespace Hime.Generators.Parsers.ContextFree
{
    public interface CFGrammarMethod : GrammarMethod
    {
        bool Construct(CFGrammar Grammar, log4net.ILog Log);
    }


    public abstract class CFGrammar : Grammar
    {
        protected string p_Name;
        protected ushort p_NextSID;
        protected System.Collections.Generic.Dictionary<string, string> p_Options;
        protected System.Collections.Generic.Dictionary<string, Terminal> p_Terminals;
        protected System.Collections.Generic.Dictionary<string, CFVariable> p_Variables;
        protected System.Collections.Generic.Dictionary<string, Virtual> p_Virtuals;
        protected System.Collections.Generic.Dictionary<string, Action> p_Actions;
        protected System.Collections.Generic.List<CFGrammarTemplateRule> p_TemplateRules;

        public override string LocalName { get { return p_Name; } }
        public int NextSID { get { return p_NextSID; } }
        public System.Collections.Generic.IEnumerable<string> Options { get { return p_Options.Keys; } }
        public System.Collections.Generic.IEnumerable<Terminal> Terminals { get { return p_Terminals.Values; } }
        public System.Collections.Generic.IEnumerable<CFVariable> Variables { get { return p_Variables.Values; } }
        public System.Collections.Generic.IEnumerable<Virtual> Virtuals { get { return p_Virtuals.Values; } }
        public System.Collections.Generic.IEnumerable<Action> Actions { get { return p_Actions.Values; } }
        public System.Collections.Generic.List<CFRule> Rules
        {
            get
            {
                System.Collections.Generic.List<CFRule> p_Rules = new System.Collections.Generic.List<CFRule>();
                foreach (CFVariable Variable in p_Variables.Values)
                    foreach (CFRule Rule in Variable.Rules)
                        p_Rules.Add(Rule);
                return p_Rules;
            }
        }
        public System.Collections.Generic.IEnumerable<CFGrammarTemplateRule> TemplateRules { get { return p_TemplateRules; } }

        public CFGrammar(string Name) : base()
        {
            p_Options = new System.Collections.Generic.Dictionary<string, string>();
            p_Terminals = new System.Collections.Generic.Dictionary<string, Terminal>();
            p_Variables = new System.Collections.Generic.Dictionary<string, CFVariable>();
            p_Virtuals = new System.Collections.Generic.Dictionary<string, Virtual>();
            p_Actions = new System.Collections.Generic.Dictionary<string, Action>();
            p_TemplateRules = new System.Collections.Generic.List<CFGrammarTemplateRule>();
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

        public Action AddAction(Hime.Kernel.QualifiedName ActionName)
        {
            string Name = "Action" + p_NextSID.ToString();
            p_NextSID++;
            Action Action = new Action(this, Name, ActionName);
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

        public CFGrammarTemplateRule AddTemplateRule(Hime.Kernel.Parsers.SyntaxTreeNode RuleNode, CFGrammarCompiler Compiler)
        {
            CFGrammarTemplateRule Rule = new CFGrammarTemplateRule(this, Compiler, RuleNode);
            p_TemplateRules.Add(Rule);
            return Rule;
        }

        public abstract void Inherit(CFGrammar Parent);
        public abstract CFGrammar Clone();

        protected bool Prepare_AddRealAxiom(log4net.ILog Log)
        {
            Log.Info("Creating axiom ...");

            // Search for Axiom option
            if (!p_Options.ContainsKey("Axiom"))
            {
                Log.Error("Grammar: Axiom option is undefined");
                return false;
            }
            // Search for the variable specified as the Axiom
            string name = p_Options["Axiom"];
            if (!p_Variables.ContainsKey(name))
            {
                Log.Error("Grammar: Cannot find axiom variable " + name);
                return false;
            }

            // Create the real axiom rule variable and rule
            CFVariable Axiom = AddVariable("_Axiom_");
            System.Collections.Generic.List<RuleDefinitionPart> Parts = new System.Collections.Generic.List<RuleDefinitionPart>();
            Parts.Add(new RuleDefinitionPart(p_Variables[name], RuleDefinitionPartAction.Promote));
            Parts.Add(new RuleDefinitionPart(TerminalDollar.Instance, RuleDefinitionPartAction.Drop));
            Axiom.AddRule(new CFRule(Axiom, new CFRuleDefinition(Parts), false));

            Log.Info("Done !");
            return true;
        }

        protected bool Prepare_ComputeFirsts(log4net.ILog Log)
        {
            Log.Info("Computing Firsts sets ...");

            bool mod = true;
            // While some modification has occured, repeat the process
            while (mod)
            {
                mod = false;
                foreach (CFVariable Var in p_Variables.Values)
                    if (Var.ComputeFirsts())
                        mod = true;
            }

            Log.Info("Done !");
            return true;
        }

        protected bool Prepare_ComputeFollowers(log4net.ILog Log)
        {
            Log.Info("Computing Followers sets ...");

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

            Log.Info("Done !");
            return true;
        }

        protected System.Xml.XmlNode GetGrammarInfoXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("Grammar");
            Node.Attributes.Append(Doc.CreateAttribute("name"));
            Node.Attributes["name"].Value = p_CompleteName.ToString();

            Node.AppendChild(Doc.CreateElement("Options"));
            foreach (string Option in p_Options.Keys)
            {
                System.Xml.XmlNode opNode = Doc.CreateElement("Option");
                opNode.Attributes.Append(Doc.CreateAttribute("Name"));
                opNode.Attributes["Name"].Value = p_Options[Option];
                Node.LastChild.AppendChild(opNode);
            }
            Node.AppendChild(Doc.CreateElement("Terminals"));
            foreach (Terminal Terminal in p_Terminals.Values)
                Node.LastChild.AppendChild(Terminal.GetXMLNode(Doc));
            Node.AppendChild(Doc.CreateElement("Variables"));
            foreach (CFVariable Variable in p_Variables.Values)
                Node.LastChild.AppendChild(Variable.GetXMLNode(Doc));
            Node.AppendChild(Doc.CreateElement("Virtuals"));
            foreach (Virtual Virtual in p_Virtuals.Values)
                Node.LastChild.AppendChild(Virtual.GetXMLNode(Doc));

            return Node;
        }

        public override void GenerateGrammarInfo(string File, log4net.ILog Log)
        {
            System.Xml.XmlDocument Document = new System.Xml.XmlDocument();
            Document.AppendChild(Document.CreateXmlDeclaration("1.0", "utf-8", null));
            Document.AppendChild(GetGrammarInfoXMLNode(Document));
            Document.Save(File);
        }
    }


    public class CFGrammarText : CFGrammar
    {
        protected Automata.DFA p_FinalDFA;

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
                AddAction(Action.ActionName);
            foreach (CFGrammarTemplateRule TemplateRule in Parent.TemplateRules)
                p_TemplateRules.Add(new CFGrammarTemplateRule(TemplateRule, this));
            foreach (CFVariable Variable in Parent.Variables)
            {
                CFVariable Clone = p_Variables[Variable.LocalName];
                foreach (CFRule R in Variable.Rules)
                {
                    System.Collections.Generic.List<RuleDefinitionPart> Parts = new System.Collections.Generic.List<RuleDefinitionPart>();
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


        public override System.Xml.XmlNode GenerateXMLNode(System.Xml.XmlDocument Document, GrammarParseMethod MethodName, log4net.ILog Log, bool DrawVisual)
        {
            Log.Info(p_Name + " parser data generation");
            if (!Prepare_AddRealAxiom(Log)) return null;
            if (!Prepare_ComputeFirsts(Log)) return null;
            if (!Prepare_ComputeFollowers(Log)) return null;
            if (!Prepare_DFA(Log)) return null;
            Log.Info("Grammar: Lexer DFA generated");

            GrammarMethod Method = null;
            if (MethodName == GrammarParseMethod.LR0)
            {
                Method = new LR.MethodLR0();
                Log.Info("Grammar: Parsing method is set to LR(0)");
            }
            else if (MethodName == GrammarParseMethod.LR1)
            {
                Method = new LR.MethodLR1();
                Log.Info("Grammar: Parsing method is set to LR(1)");
            }
            else if (MethodName == GrammarParseMethod.LALR1)
            {
                Method = new LR.MethodLALR1();
                Log.Info("Grammar: Parsing method is set to LALR(1)");
            }
            else if (MethodName == GrammarParseMethod.GLR1)
            {
                Method = new LR.MethodGLR1();
                Log.Info("Grammar: Parsing method is set to GLR(1)");
                Log.Warn("Grammar: Code generation is not yet supported for this parsing method");
            }
            else if (MethodName == GrammarParseMethod.GLALR1)
            {
                Method = new LR.MethodGLALR1();
                Log.Info("Grammar: Parsing method is set to GLALR(1)");
                Log.Warn("Grammar: Code generation is not yet supported for this parsing method");
            }
            else if (MethodName == GrammarParseMethod.RNGLR1)
            {
                Method = new LR.MethodRNGLR1();
                Log.Info("Grammar: Parsing method is set to RNGLR(1)");
                Log.Warn("Grammar: Code generation is not yet supported for this parsing method");
            }
            else if (MethodName == GrammarParseMethod.RNGLALR1)
            {
                Method = new LR.MethodRNGLALR1();
                Log.Info("Grammar: Parsing method is set to RNGLALR(1)");
                Log.Warn("Grammar: Code generation is not yet supported for this parsing method");
            }
            else
            {
                Log.Error("Grammar: The provided method is not appropriate for this grammar");
                return null;
            }
            if (!Method.Construct(this, Log))
                return null;
            if (DrawVisual)
            {
                p_FinalDFA.GenerateVisual().Save(p_CompleteName.ToString() + "_DFA.bmp");
                Method.GenerateVisual().Save(p_CompleteName.ToString() + "_Parser.bmp");
               Log.Info("Grammar: Visuals for the DFA and the LR graph have been generated");
            }

            System.Xml.XmlNode Node = Document.CreateElement("ContextFreeGrammar");
            Node.Attributes.Append(Document.CreateAttribute("Name"));
            Node.Attributes.Append(Document.CreateAttribute("Method"));
            Node.Attributes.Append(Document.CreateAttribute("Input"));
            Node.Attributes["Name"].Value = p_CompleteName.ToString('_');
            Node.Attributes["Method"].Value = MethodName.ToString();
            Node.Attributes["Input"].Value = "Text";
            Node.AppendChild(Generate_DataLexer(Document));
            Node.AppendChild(Method.GenerateData(Document));
            Node.AppendChild(Document.CreateElement("SubGrammars"));
            
            System.Collections.Generic.List<Grammar> SubGrammars = new System.Collections.Generic.List<Grammar>();
            foreach (TerminalText Terminal in p_Terminals.Values)
            {
                if (Terminal.SubGrammar == null) continue;
                if (SubGrammars.Contains(Terminal.SubGrammar)) continue;

                Node.ChildNodes[2].AppendChild(Document.CreateElement("GrammarReference"));
                Node.ChildNodes[2].LastChild.Attributes.Append(Document.CreateAttribute("Name"));
                Node.ChildNodes[2].LastChild.Attributes["Name"].Value = Terminal.SubGrammar.CompleteName.ToString('_');
                SubGrammars.Add(Terminal.SubGrammar);
            }
            return Node;
        }


        public override bool GenerateParser(string Namespace, GrammarParseMethod MethodName, string File, log4net.ILog Log)
        {
            return GenerateParser(Namespace, MethodName, File, Log, false);
        }
        public override bool GenerateParser(string Namespace, GrammarParseMethod MethodName, string File, log4net.ILog Log, bool DrawVisual)
        {
            System.Xml.XmlDocument Document = new System.Xml.XmlDocument();
            Document.AppendChild(Document.CreateXmlDeclaration("1.0", "utf-8", null));
            Document.AppendChild(Document.CreateElement("ResourceFile"));
            Document.ChildNodes[1].Attributes.Append(Document.CreateAttribute("Namespace"));
            Document.ChildNodes[1].Attributes["Namespace"].Value = Namespace;
            System.Xml.XmlNode GrammarNode = GenerateXMLNode(Document, MethodName, Log, DrawVisual);
            if (GrammarNode == null) return false;
            Document.ChildNodes[1].AppendChild(GrammarNode);
            Document.Save(File + ".xml");

            Kernel.Resources.AccessorSession Session = Kernel.Resources.ResourceAccessor.CreateCheckoutSession();
            Session.AddCheckoutFile(File + ".xml");
            Kernel.Resources.ResourceAccessor.CheckOut(Session, "Transforms.Generators.Resources.xslt", "Generators.Resources.xslt");
            Kernel.Resources.ResourceAccessor.CheckOut(Session, "Transforms.Generators.CFGrammars.xslt", "Generators.CFGrammars.xslt");
            Kernel.Resources.ResourceAccessor.CheckOut(Session, "Transforms.Generators.Lexers.xslt", "Generators.Lexers.xslt");
            Kernel.Resources.ResourceAccessor.CheckOut(Session, "Transforms.Generators.ParserLR1.xslt", "Generators.ParserLR1.xslt");
            System.Xml.Xsl.XslCompiledTransform Transform = new System.Xml.Xsl.XslCompiledTransform();
            Transform.Load("Generators.Resources.xslt");
            Transform.Transform(File + ".xml", File);
            Session.Close();
            return true;
        }

        protected bool Prepare_DFA(log4net.ILog Log)
        {
            Log.Info("Generating DFA for Terminals ...");

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

            Log.Info("Done !");
            return true;
        }

        protected System.Xml.XmlNode Generate_DataLexer(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("Lexer");

            if (p_Options.ContainsKey("Separator"))
            {
                string Separator = p_Options["Separator"];
                if (p_Terminals.ContainsKey(Separator))
                {
                    Node.Attributes.Append(Doc.CreateAttribute("Separator"));
                    Node.Attributes["Separator"].Value = p_Terminals[Separator].SID.ToString();
                }
            }

            System.Collections.Generic.List<int> Indexes;
            Node.AppendChild(Generate_DataLexer_Symbols(Doc, out Indexes));

            Node.AppendChild(Doc.CreateElement("States"));
            int i = 0;
            foreach (Automata.DFAState State in p_FinalDFA.States)
            {
                Node.LastChild.AppendChild(Generate_DataLexer_DFAState(Doc, State, Indexes[i]));
                i++;
            }
            return Node;
        }

        private System.Xml.XmlNode Generate_DataLexer_Symbols(System.Xml.XmlDocument Doc, out System.Collections.Generic.List<int> Indexes)
        {
            System.Collections.Generic.List<Terminal> Symbols = new System.Collections.Generic.List<Terminal>();
            Indexes = new System.Collections.Generic.List<int>();

            System.Xml.XmlNode Node = Doc.CreateElement("Symbols");
            foreach (Automata.DFAState State in p_FinalDFA.States)
            {
                if (State.Final != null)
                {
                    if (Symbols.Contains(State.Final))
                    {
                        Indexes.Add(Symbols.IndexOf(State.Final));
                    }
                    else
                    {
                        Node.AppendChild(State.Final.GetXMLNode(Doc));
                        Indexes.Add(Symbols.Count);
                        Symbols.Add(State.Final);
                    }
                }
                else
                {
                    Indexes.Add(-1);
                }
            }

            return Node;
        }

        private System.Xml.XmlNode Generate_DataLexer_DFAState(System.Xml.XmlDocument Doc, Automata.DFAState State, int FinalIndex)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("DFAState");
            Node.Attributes.Append(Doc.CreateAttribute("ID"));
            Node.Attributes.Append(Doc.CreateAttribute("Final"));
            Node.Attributes["ID"].Value = State.ID.ToString();
            Node.Attributes["Final"].Value = FinalIndex.ToString();

            foreach (Automata.TerminalNFACharSpan Span in State.Transitions.Keys)
            {
                Node.AppendChild(Doc.CreateElement("Transition"));
                Node.LastChild.Attributes.Append(Doc.CreateAttribute("CharBegin"));
                Node.LastChild.Attributes.Append(Doc.CreateAttribute("CharEnd"));
                Node.LastChild.Attributes.Append(Doc.CreateAttribute("Next"));
                Node.LastChild.Attributes[0].Value = System.Convert.ToUInt16(Span.Begin).ToString("X");
                Node.LastChild.Attributes[1].Value = System.Convert.ToUInt16(Span.End).ToString("X");
                Node.LastChild.Attributes[2].Value = State.Transitions[Span].ID.ToString("X");
            }

            return Node;
        }
    }

    public class CFGrammarBinary : CFGrammar
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
                AddAction(Action.ActionName);
            foreach (CFGrammarTemplateRule TemplateRule in Parent.TemplateRules)
                p_TemplateRules.Add(new CFGrammarTemplateRule(TemplateRule, this));
            foreach (CFVariable Variable in Parent.Variables)
            {
                CFVariable Clone = p_Variables[Variable.LocalName];
                foreach (CFRule R in Variable.Rules)
                {
                    System.Collections.Generic.List<RuleDefinitionPart> Parts = new System.Collections.Generic.List<RuleDefinitionPart>();
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

        public override bool GenerateParser(string Namespace, GrammarParseMethod MethodName, string File, log4net.ILog Log)
        {
            return GenerateParser(Namespace, MethodName, File, Log, false);
        }

        public override System.Xml.XmlNode GenerateXMLNode(System.Xml.XmlDocument Document, GrammarParseMethod MethodName, log4net.ILog Log, bool DrawVisual)
        {
            Log.Info(p_Name + " parser generation");
            if (!Prepare_AddRealAxiom(Log)) return null;
            if (!Prepare_ComputeFirsts(Log)) return null;
            if (!Prepare_ComputeFollowers(Log)) return null;

            GrammarMethod Method = null;
            if (MethodName == GrammarParseMethod.LR1)
            {
                Method = new LR.MethodLR1();
                Log.Info("Grammar: Parsing method is set to LR(1)");
            }
            else if (MethodName == GrammarParseMethod.GLR1)
            {
                Method = new LR.MethodGLR1();
                Log.Info("Grammar: Parsing method is set to GLR(1)");
                Log.Warn("Grammar: Code generation is not yet supported for this parsing method");
            }
            else if (MethodName == GrammarParseMethod.RNGLR1)
            {
                Method = new LR.MethodRNGLR1();
                Log.Info("Grammar: Parsing method is set to RNGLR(1)");
                Log.Warn("Grammar: Code generation is not yet supported for this parsing method");
            }
            else
            {
                Log.Error("Grammar: The provided method is not appropriate for this grammar");
                return null;
            }
            Method.Construct(this, Log);
            if (DrawVisual)
            {
                Method.GenerateVisual().Save(p_CompleteName.ToString() + "_Parser.bmp");
                Log.Info("Grammar: A visual for the LR graph has been generated");
            }

            System.Xml.XmlNode Node = Document.CreateElement("ContextFreeGrammar");
            Node.Attributes.Append(Document.CreateAttribute("Name"));
            Node.Attributes.Append(Document.CreateAttribute("Method"));
            Node.Attributes.Append(Document.CreateAttribute("Input"));
            Node.Attributes["Name"].Value = p_CompleteName.ToString('_');
            Node.Attributes["Method"].Value = MethodName.ToString();
            Node.Attributes["Input"].Value = "Text";
            Node.AppendChild(Generate_DataLexer(Document));
            Node.AppendChild(Method.GenerateData(Document));
            Node.AppendChild(Document.CreateElement("SubGrammars"));
            return Node;
        }

        public override bool GenerateParser(string Namespace, GrammarParseMethod MethodName, string File, log4net.ILog Log, bool DrawVisual)
        {
            System.Xml.XmlDocument Document = new System.Xml.XmlDocument();
            Document.AppendChild(Document.CreateXmlDeclaration("1.0", "utf-8", null));
            Document.AppendChild(Document.CreateElement("ResourceFile"));
            Document.ChildNodes[1].Attributes.Append(Document.CreateAttribute("Namespace"));
            Document.ChildNodes[1].Attributes["Namespace"].Value = Namespace;
            System.Xml.XmlNode GrammarNode = GenerateXMLNode(Document, MethodName, Log, DrawVisual);
            if (GrammarNode == null) return false;
            Document.ChildNodes[1].AppendChild(GrammarNode);
            Document.Save(File + ".xml");

            Kernel.Resources.AccessorSession Session = Kernel.Resources.ResourceAccessor.CreateCheckoutSession();
            Session.AddCheckoutFile(File + ".xml");
            Kernel.Resources.ResourceAccessor.CheckOut(Session, "Transforms.Generators.Resources.xslt", "Generators.Resources.xslt");
            Kernel.Resources.ResourceAccessor.CheckOut(Session, "Transforms.Generators.CFGrammars.xslt", "Generators.CFGrammars.xslt");
            Kernel.Resources.ResourceAccessor.CheckOut(Session, "Transforms.Generators.Lexers.xslt", "Generators.Lexers.xslt");
            Kernel.Resources.ResourceAccessor.CheckOut(Session, "Transforms.Generators.ParserLR1.xslt", "Generators.ParserLR1.xslt");
            System.Xml.Xsl.XslCompiledTransform Transform = new System.Xml.Xsl.XslCompiledTransform();
            Transform.Load("Generators.Resources.xslt");
            Transform.Transform(File + ".xml", File);
            Session.Close();
            return true;
        }

        protected System.Xml.XmlNode Generate_DataLexer(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("Lexer");
            foreach (TerminalBin Terminal in p_Terminals.Values)
                Node.AppendChild(Terminal.GetXMLNode(Doc));
            return Node;
        }
    }
}