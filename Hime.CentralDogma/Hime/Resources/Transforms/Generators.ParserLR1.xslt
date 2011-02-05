<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="text" indent="yes"/>
  
  <xsl:template name="Hime_ClassParserLR1_ProductionRule">
    private static void ReduceRule_<xsl:value-of select="@HeadSID"/>_<xsl:value-of select="@RuleID"/>(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
    {
    <xsl:if test="RuleDefinition/@ParserLength!=0">
      System.Collections.Generic.List&lt;Hime.Kernel.Parsers.SyntaxTreeNode&gt; Definition = nodes.GetRange(nodes.Count - <xsl:value-of select="RuleDefinition/@ParserLength"/>, <xsl:value-of select="RuleDefinition/@ParserLength"/>);
      nodes.RemoveRange(nodes.Count - <xsl:value-of select="RuleDefinition/@ParserLength"/>, <xsl:value-of select="RuleDefinition/@ParserLength"/>);
    </xsl:if>
      Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x<xsl:value-of select="@HeadSID"/>, "<xsl:value-of select="@HeadName"/>")<xsl:if test="@Replace='True'">, Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace</xsl:if>);
    <xsl:for-each select="RuleDefinition/Symbol">
      <xsl:if test="@SymbolType='Action'">
        SubRoot.AppendChild(new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.SymbolAction("<xsl:value-of select="@SymbolName"/>"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.<xsl:value-of select="@Action"/>));
      </xsl:if>
      <xsl:if test="@SymbolType='Virtual'">
        SubRoot.AppendChild(new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVirtual("<xsl:value-of select="@SymbolName"/>"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.<xsl:value-of select="@Action"/>));
      </xsl:if>
      <xsl:if test="@SymbolType='Variable'">
        SubRoot.AppendChild(Definition[<xsl:value-of select="@ParserIndex"/>]<xsl:if test="@Action!='Nothing'">, Hime.Kernel.Parsers.SyntaxTreeNodeAction.<xsl:value-of select="@Action"/></xsl:if>);
      </xsl:if>
      <xsl:if test="@SymbolType='Terminal'">
        SubRoot.AppendChild(Definition[<xsl:value-of select="@ParserIndex"/>]<xsl:if test="@Action!='Nothing'">, Hime.Kernel.Parsers.SyntaxTreeNodeAction.<xsl:value-of select="@Action"/></xsl:if>);
      </xsl:if>
    </xsl:for-each>
      nodes.Add(SubRoot);
    }
  </xsl:template>
  
  <xsl:template name="Hime_ClassParserLR1_Data">
    private delegate void Production(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes);
    <xsl:for-each select="Rules/Rule">
      <xsl:call-template name="Hime_ClassParserLR1_ProductionRule"/>
    </xsl:for-each>
    private static Production[] p_Rules = { <xsl:for-each select="Rules/Rule"><xsl:if test="position()!=1">, </xsl:if>ReduceRule_<xsl:value-of select="@HeadSID"/>_<xsl:value-of select="@RuleID"/></xsl:for-each> };
    private static ushort[]     p_RulesHeadID = { <xsl:for-each select="Rules/Rule"><xsl:if test="position()!=1">, </xsl:if>0x<xsl:value-of select="@HeadSID"/></xsl:for-each> };
    private static string[]     p_RulesHeadName = { <xsl:for-each select="Rules/Rule"><xsl:if test="position()!=1">, </xsl:if>"<xsl:value-of select="@HeadName"/>"</xsl:for-each> };
    private static ushort[]     p_RulesParserLength = { <xsl:for-each select="Rules/Rule"><xsl:if test="position()!=1">, </xsl:if><xsl:value-of select="RuleDefinition/@ParserLength"/></xsl:for-each> };
    <xsl:for-each select="States/State">
    private static ushort[]     p_StateExpectedIDs_<xsl:value-of select="@ID"/> = { <xsl:for-each select="Expected/Symbol"><xsl:if test="position()!=1">, </xsl:if>0x<xsl:value-of select="@SID"/></xsl:for-each> };
    private static string[]     p_StateExpectedNames_<xsl:value-of select="@ID"/> = { <xsl:for-each select="Expected/Symbol"><xsl:if test="position()!=1">, </xsl:if>"<xsl:value-of select="@Name"/>"</xsl:for-each> };
    private static string[]     p_StateItems_<xsl:value-of select="@ID"/> = { <xsl:for-each select="Items/Item"><xsl:if test="position()!=1">, </xsl:if>"<xsl:value-of select="."/>"</xsl:for-each> };
    private static ushort[][]   p_StateShiftsOnTerminal_<xsl:value-of select="@ID"/> = { <xsl:for-each select="Actions/OnTerminal/Shift"><xsl:if test="position()!=1">, </xsl:if>new ushort[2] { 0x<xsl:value-of select="@Symbol"/>, 0x<xsl:value-of select="@Next"/> }</xsl:for-each> };
    private static ushort[][]   p_StateShiftsOnVariable_<xsl:value-of select="@ID"/> = { <xsl:for-each select="Actions/OnVariable/Shift"><xsl:if test="position()!=1">, </xsl:if>new ushort[2] { 0x<xsl:value-of select="@Symbol"/>, 0x<xsl:value-of select="@Next"/> }</xsl:for-each> };
    private static ushort[][]   p_StateReducsOnTerminal_<xsl:value-of select="@ID"/> = { <xsl:for-each select="Actions/OnTerminal/Reduction"><xsl:if test="position()!=1">, </xsl:if>new ushort[2] { 0x<xsl:value-of select="@Symbol"/>, 0x<xsl:value-of select="@Index"/> }</xsl:for-each> };
    </xsl:for-each>
    private static ushort[][]   p_StateExpectedIDs = { <xsl:for-each select="States/State"><xsl:if test="position()!=1">, </xsl:if>p_StateExpectedIDs_<xsl:value-of select="@ID"/></xsl:for-each> };
    private static string[][]   p_StateExpectedNames = { <xsl:for-each select="States/State"><xsl:if test="position()!=1">, </xsl:if>p_StateExpectedNames_<xsl:value-of select="@ID"/></xsl:for-each> };
    private static string[][]   p_StateItems = { <xsl:for-each select="States/State"><xsl:if test="position()!=1">, </xsl:if>p_StateItems_<xsl:value-of select="@ID"/></xsl:for-each> };
    private static ushort[][][] p_StateShiftsOnTerminal = { <xsl:for-each select="States/State"><xsl:if test="position()!=1">, </xsl:if>p_StateShiftsOnTerminal_<xsl:value-of select="@ID"/></xsl:for-each> };
    private static ushort[][][] p_StateShiftsOnVariable = { <xsl:for-each select="States/State"><xsl:if test="position()!=1">, </xsl:if>p_StateShiftsOnVariable_<xsl:value-of select="@ID"/></xsl:for-each> };
    private static ushort[][][] p_StateReducsOnTerminal = { <xsl:for-each select="States/State"><xsl:if test="position()!=1">, </xsl:if>p_StateReducsOnTerminal_<xsl:value-of select="@ID"/></xsl:for-each> };
    private static int          p_ErrorSimulationLength = 3;
    
    private System.Collections.Generic.List&lt;Hime.Kernel.Parsers.ParserError&gt; p_Errors;
    private Lexer_<xsl:value-of select="../@Name"/> p_Lexer;
    private Hime.Kernel.Parsers.SyntaxTreeNodeCollection p_Nodes;
    private System.Collections.Generic.Stack&lt;ushort&gt; p_Stack;
    private Hime.Kernel.Parsers.SymbolToken p_NextToken;
    private ushort p_CurrentState;

    public System.Collections.Generic.List&lt;Hime.Kernel.Parsers.ParserError&gt; Errors { get { return p_Errors; } }
  </xsl:template>

  <xsl:template name="Hime_ClassParserLR1_DataAccess">
    private static ushort Analyse_GetNextByShiftOnTerminal(ushort state, ushort sid)
    {
	    for (int i=0; i!=p_StateShiftsOnTerminal[state].Length; i++)
	    {
		    if (p_StateShiftsOnTerminal[state][i][0] == sid)
			    return p_StateShiftsOnTerminal[state][i][1];
	    }
	    return 0xFFFF;
    }
    private static ushort Analyse_GetNextByShiftOnVariable(ushort state, ushort sid)
    {
	    for (int i=0; i!=p_StateShiftsOnVariable[state].Length; i++)
	    {
		    if (p_StateShiftsOnVariable[state][i][0] == sid)
			    return p_StateShiftsOnVariable[state][i][1];
	    }
	    return 0xFFFF;
    }
    private static ushort Analyse_GetProductionOnTerminal(ushort state, ushort sid)
    {
	    for (int i=0; i!=p_StateReducsOnTerminal[state].Length; i++)
	    {
		    if (p_StateReducsOnTerminal[state][i][0] == sid)
			    return p_StateReducsOnTerminal[state][i][1];
	    }
	    return 0xFFFF;
    }
  </xsl:template>

  <xsl:template name="Hime_ClassParserLR1_ErrorHandling">
    private void Analyse_HandleUnexpectedToken()
    {
	    p_Errors.Add(new Hime.Kernel.Parsers.ParserErrorUnexpectedToken(p_NextToken, p_StateExpectedNames[p_CurrentState]));
    	
	    if (p_Errors.Count >= 100)
		    throw new Hime.Kernel.Parsers.ParserException("Too much errors, parsing stopped.");

      if (Analyse_HandleUnexpectedToken_SimpleRecovery()) return;
      throw new Hime.Kernel.Parsers.ParserException("Unrecoverable error encountered");
    }
    private bool Analyse_HandleUnexpectedToken_SimpleRecovery()
    {
      if (Analyse_HandleUnexpectedToken_SimpleRecovery_RemoveUnexpected()) return true;
      if (Analyse_HandleUnexpectedToken_SimpleRecovery_InsertExpected()) return true;
      if (Analyse_HandleUnexpectedToken_SimpleRecovery_ReplaceUnexpectedByExpected()) return true;
      return false;
    }
    private bool Analyse_HandleUnexpectedToken_SimpleRecovery_RemoveUnexpected()
    {
      Lexer_<xsl:value-of select="../@Name"/> TestLexer = p_Lexer.Clone_<xsl:value-of select="../@Name"/>();
      System.Collections.Generic.List&lt;ushort&gt; TempStack = new System.Collections.Generic.List&lt;ushort&gt;(p_Stack);
      TempStack.Reverse();
      System.Collections.Generic.Stack&lt;ushort&gt; TestStack = new System.Collections.Generic.Stack&lt;ushort&gt;(TempStack);
      if (Analyse_Simulate(TestStack, TestLexer))
      {
        p_NextToken = p_Lexer.GetNextToken();
        return true;
      }
      return false;
    }
    private bool Analyse_HandleUnexpectedToken_SimpleRecovery_InsertExpected()
    {
      for (int i=0; i!=p_StateExpectedIDs[p_CurrentState].Length; i++)
      {
        Lexer_<xsl:value-of select="../@Name"/> TestLexer = p_Lexer.Clone_<xsl:value-of select="../@Name"/>();
        System.Collections.Generic.List&lt;ushort&gt; TempStack = new System.Collections.Generic.List&lt;ushort&gt;(p_Stack);
        TempStack.Reverse();
        System.Collections.Generic.Stack&lt;ushort&gt; TestStack = new System.Collections.Generic.Stack&lt;ushort&gt;(TempStack);
        System.Collections.Generic.List&lt;Hime.Kernel.Parsers.SymbolToken&gt; Inserted = new System.Collections.Generic.List&lt;Hime.Kernel.Parsers.SymbolToken&gt;();
        Inserted.Add(new Hime.Kernel.Parsers.SymbolTokenText(p_StateExpectedNames[p_CurrentState][i], p_StateExpectedIDs[p_CurrentState][i], string.Empty, p_Lexer.CurrentLine));
        Inserted.Add(p_NextToken);
        if (Analyse_Simulate(TestStack, TestLexer, Inserted))
        {
          Analyse_RunForToken(Inserted[0]);
          Analyse_RunForToken(Inserted[1]);
          p_NextToken = p_Lexer.GetNextToken();
          return true;
        }
      }
      return false;
    }
    private bool Analyse_HandleUnexpectedToken_SimpleRecovery_ReplaceUnexpectedByExpected()
    {
      for (int i=0; i!=p_StateExpectedIDs[p_CurrentState].Length; i++)
      {
        Lexer_<xsl:value-of select="../@Name"/> TestLexer = p_Lexer.Clone_<xsl:value-of select="../@Name"/>();
        System.Collections.Generic.List&lt;ushort&gt; TempStack = new System.Collections.Generic.List&lt;ushort&gt;(p_Stack);
        TempStack.Reverse();
        System.Collections.Generic.Stack&lt;ushort&gt; TestStack = new System.Collections.Generic.Stack&lt;ushort&gt;(TempStack);
        System.Collections.Generic.List&lt;Hime.Kernel.Parsers.SymbolToken&gt; Inserted = new System.Collections.Generic.List&lt;Hime.Kernel.Parsers.SymbolToken&gt;();
        Inserted.Add(new Hime.Kernel.Parsers.SymbolTokenText(p_StateExpectedNames[p_CurrentState][i], p_StateExpectedIDs[p_CurrentState][i], string.Empty, p_Lexer.CurrentLine));
        if (Analyse_Simulate(TestStack, TestLexer, Inserted))
        {
          Analyse_RunForToken(Inserted[0]);
          p_NextToken = p_Lexer.GetNextToken();
          return true;
        }
      }
      return false;
    }
  </xsl:template>

  <xsl:template name="Hime_ClassParserLR1_AnalyseSimulateText">
    private bool Analyse_Simulate(System.Collections.Generic.Stack&lt;ushort&gt; stack, Lexer_<xsl:value-of select="../@Name"/> lexer, System.Collections.Generic.List&lt;Hime.Kernel.Parsers.SymbolToken&gt; inserted)
    {
      int InsertedIndex = 0;
	    ushort CurrentState = stack.Peek();
	    Hime.Kernel.Parsers.SymbolToken NextToken = null;
      if (inserted.Count != 0)
      {
        NextToken = inserted[0];
        InsertedIndex++;
      }
      else
        NextToken = lexer.GetNextToken();
    	
	    for (int i=0; i!=p_ErrorSimulationLength+inserted.Count; i++)
	    {
		    ushort NextState = Analyse_GetNextByShiftOnTerminal(CurrentState, NextToken.SymbolID);
		    if (NextState != 0xFFFF)
		    {
			    CurrentState = NextState;
			    stack.Push(CurrentState);
          if (InsertedIndex != inserted.Count)
          {
            NextToken = inserted[InsertedIndex];
            InsertedIndex++;
          }
          else
			      NextToken = lexer.GetNextToken();
			    continue;
		    }
		    ushort ReductionIndex = Analyse_GetProductionOnTerminal(CurrentState, NextToken.SymbolID);
		    if (ReductionIndex != 0xFFFF)
		    {
          Production Reduce = p_Rules[ReductionIndex];
			    ushort HeadID = p_RulesHeadID[ReductionIndex];
          for (ushort j=0; j!=p_RulesParserLength[ReductionIndex]; j++)
            stack.Pop();
          // If next symbol is ε (after $) : return
			    if (NextToken.SymbolID == 0x1)
				    return true;
          // Shift to next state on the reduce variable
			    NextState = Analyse_GetNextByShiftOnVariable(stack.Peek(), HeadID);
			    // Handle error here : no transition for symbol HeadID
			    if (NextState == 0xFFFF)
				    return false;
			    CurrentState = NextState;
			    stack.Push(CurrentState);
			    continue;
		    }
		    // Handle error here : no action for symbol NextToken.SymbolID
		    return false;
	    }
      return true;
    }
    private bool Analyse_Simulate(System.Collections.Generic.Stack&lt;ushort&gt; stack, Lexer_<xsl:value-of select="../@Name"/> lexer)
    {
      return Analyse_Simulate(stack, lexer, new System.Collections.Generic.List&lt;Hime.Kernel.Parsers.SymbolToken&gt;());
    }
  </xsl:template>
  
  <xsl:template name="Hime_ClassParserLR1_AnalyseText">
    private bool Analyse_RunForToken(Hime.Kernel.Parsers.SymbolToken token)
    {
      while (true)
	    {
		    ushort NextState = Analyse_GetNextByShiftOnTerminal(p_CurrentState, token.SymbolID);
		    if (NextState != 0xFFFF)
		    {
			    p_Nodes.Add(new Hime.Kernel.Parsers.SyntaxTreeNode(token));
			    p_CurrentState = NextState;
			    p_Stack.Push(p_CurrentState);
          return true;
		    }
		    ushort ReductionIndex = Analyse_GetProductionOnTerminal(p_CurrentState, token.SymbolID);
		    if (ReductionIndex != 0xFFFF)
		    {
          Production Reduce = p_Rules[ReductionIndex];
			    ushort HeadID = p_RulesHeadID[ReductionIndex];
          Reduce(p_Nodes);
          for (ushort j=0; j!=p_RulesParserLength[ReductionIndex]; j++)
            p_Stack.Pop();
          // Shift to next state on the reduce variable
			    NextState = Analyse_GetNextByShiftOnVariable(p_Stack.Peek(), HeadID);
			    if (NextState == 0xFFFF)
				    return false;
			    p_CurrentState = NextState;
			    p_Stack.Push(p_CurrentState);
			    continue;
		    }
        return false;
	    }
    }
    public Hime.Kernel.Parsers.SyntaxTreeNode Analyse()
    {
	    p_Stack.Push(p_CurrentState);
	    p_NextToken = p_Lexer.GetNextToken();
    	
	    while (true)
	    {
        if (Analyse_RunForToken(p_NextToken))
        {
          p_NextToken = p_Lexer.GetNextToken();
          continue;
        }
        else if (p_NextToken.SymbolID == 0x0001)
          return p_Nodes[0];
        else
		      Analyse_HandleUnexpectedToken();
	    }
    }
  </xsl:template>

  <xsl:template name="Hime_ClassParserLR1_AnalyseBinary">
    private bool Analyse_RunForToken(Hime.Kernel.Parsers.SymbolToken token)
    {
      while (true)
	    {
		    ushort NextState = Analyse_GetNextByShiftOnTerminal(p_CurrentState, token.SymbolID);
		    if (NextState != 0xFFFF)
		    {
			    p_Nodes.Add(new Hime.Kernel.Parsers.SyntaxTreeNode(token));
			    p_CurrentState = NextState;
			    p_Stack.Push(p_CurrentState);
          return true;
		    }
		    ushort ReductionIndex = Analyse_GetProductionOnTerminal(p_CurrentState, token.SymbolID);
		    if (ReductionIndex != 0xFFFF)
		    {
          Production Reduce = p_Rules[ReductionIndex];
			    ushort HeadID = p_RulesHeadID[ReductionIndex];
          Reduce(p_Nodes);
          for (ushort j=0; j!=p_RulesParserLength[ReductionIndex]; j++)
            p_Stack.Pop();
          // Shift to next state on the reduce variable
			    NextState = Analyse_GetNextByShiftOnVariable(p_Stack.Peek(), HeadID);
			    if (NextState == 0xFFFF)
				    return false;
			    p_CurrentState = NextState;
			    p_Stack.Push(p_CurrentState);
			    continue;
		    }
        return false;
	    }
    }
    public Hime.Kernel.Parsers.SyntaxTreeNode Analyse()
    {
	    p_Stack.Push(p_CurrentState);
	    p_NextToken = p_Lexer.GetNextToken(p_ExpectedIDs[p_CurrentState]);
    	
	    while (true)
	    {
        if (Analyse_RunForToken(p_NextToken))
        {
          p_NextToken = p_Lexer.GetNextToken(p_ExpectedIDs[p_CurrentState]);
          continue;
        }
        else if (p_NextToken.SymbolID == 0x0001)
          return p_Nodes[0];
        else
		      Analyse_HandleUnexpectedToken();
	    }
    }
  </xsl:template>

  <xsl:template name="Hime_ClassParserLR1_Text">
    public class Parser_<xsl:value-of select="../@Name"/> : Hime.Kernel.Parsers.IParser
    {
    <xsl:call-template name="Hime_ClassParserLR1_Data"/>
    <xsl:call-template name="Hime_ClassParserLR1_DataAccess"/>
    
	    public Parser_<xsl:value-of select="../@Name"/>(Lexer_<xsl:value-of select="../@Name"/> input)
	    {
		    p_Errors = new System.Collections.Generic.List&lt;Hime.Kernel.Parsers.ParserError&gt;();
		    p_Lexer = input;
		    p_Nodes = new Hime.Kernel.Parsers.SyntaxTreeNodeCollection();
		    p_Stack = new System.Collections.Generic.Stack&lt;ushort&gt;();
		    p_CurrentState = 0x0;
		    p_NextToken = null;
	    }

    <xsl:call-template name="Hime_ClassParserLR1_ErrorHandling"/>
    <xsl:call-template name="Hime_ClassParserLR1_AnalyseSimulateText"/>
    <xsl:call-template name="Hime_ClassParserLR1_AnalyseText"/>
    }
  </xsl:template>
  
  <xsl:template name="Hime_ClassParserLR1_Binary">
    public class Parser_<xsl:value-of select="../@Name"/> : Hime.Kernel.Parsers.IParser
    {
    <xsl:call-template name="Hime_ClassParserLR1_Data"/>
    <xsl:call-template name="Hime_ClassParserLR1_DataAccess"/>
    
	    public Parser_<xsl:value-of select="../@Name"/>(Lexer_<xsl:value-of select="../@Name"/> input)
	    {
		    p_Errors = new System.Collections.Generic.List&lt;Hime.Kernel.Parsers.ParserError&gt;();
		    p_Lexer = input;
		    p_Nodes = new Hime.Kernel.Parsers.SyntaxTreeNodeCollection();
		    p_Stack = new System.Collections.Generic.Stack&lt;ushort&gt;();
		    p_CurrentState = 0x0;
		    p_NextToken = null;
	    }

    <xsl:call-template name="Hime_ClassParserLR1_ErrorHandling"/>
    <xsl:call-template name="Hime_ClassParserLR1_AnalyseBinary"/>
    }
  </xsl:template>
</xsl:stylesheet>
