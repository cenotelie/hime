<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="text" indent="yes"/>
  <xsl:template name="Hime_ClassLexer_Binary">
    public class Lexer_<xsl:value-of select="../@Name"/> : Hime.Kernel.Parsers.LexerBinary
	  {		
		  public Lexer_<xsl:value-of select="../@Name"/>(Hime.Kernel.Binary.DataInput input) : base(input) { }

		  public override Hime.Kernel.Parsers.SymbolToken GetNextToken(ushort[] IDs)
		  {
			  if (p_DollarEmitted)
				  return new Hime.Kernel.Parsers.SymbolTokenEpsilon();
			  if (p_Input.IsAtEnd &amp;&amp; p_CurrentBitLeft == 8)
			  {
				  p_DollarEmitted = true;
				  return new Hime.Kernel.Parsers.SymbolTokenDollar();
			  }
  			
			  foreach (ushort ID in IDs)
			  {
				  Hime.Kernel.Parsers.SymbolToken Temp = GetNextToken_Apply(ID);
				  if (Temp != null) return Temp;
			  }
			  p_DollarEmitted = true;
			  return new Hime.Kernel.Parsers.SymbolTokenDollar();
		  }
  		
		  private Hime.Kernel.Parsers.SymbolToken GetNextToken_Apply(ushort ID)
		  {
			  switch (ID)
			  {
    <xsl:for-each select="SymbolTerminalBin">
          case 0x<xsl:value-of select="SID"/> : return GetNextToken_Apply_<xsl:value-of select="SID"/>();
    </xsl:for-each>
				  default: return null;
			  }
		  }

	  <xsl:for-each select="SymbolTerminalBin">
		  private Hime.Kernel.Parsers.SymbolToken GetNextToken_Apply_<xsl:value-of select="SID"/>()
		  {
		  <xsl:if test="@Joker='False'">
        <xsl:if test="@LengthByte!=0">
          if (p_CurrentBitLeft != 8) return null;
          if (!p_Input.CanRead(<xsl:value-of select="@LengthByte"/>)) return null;
          <xsl:if test="@LengthByte=1">
            if (p_Input.ReadByte() == <xsl:value-of select="@Value"/>)
            {
              p_Input.ReadAndAdvanceByte();
              return new Hime.Kernel.Parsers.SymbolTokenUInt8("<xsl:value-of select="@Name"/>", 0x<xsl:value-of select="@SID"/>, <xsl:value-of select="@Value"/>);
            }
          </xsl:if>
          <xsl:if test="@LengthByte=2">
            if (p_Input.ReadUShort() == <xsl:value-of select="@Value"/>)
            {
              p_Input.ReadAndAdvanceUInt16();
              return new Hime.Kernel.Parsers.SymbolTokenUInt16("<xsl:value-of select="@Name"/>", 0x<xsl:value-of select="@SID"/>, <xsl:value-of select="@Value"/>);
            }
          </xsl:if>
          <xsl:if test="@LengthByte=4">
            if (p_Input.ReadUInt() == <xsl:value-of select="@Value"/>)
            {
              p_Input.ReadAndAdvanceUInt32();
              return new Hime.Kernel.Parsers.SymbolTokenUInt32("<xsl:value-of select="@Name"/>", 0x<xsl:value-of select="@SID"/>, <xsl:value-of select="@Value"/>);
            }
          </xsl:if>
          <xsl:if test="@LengthByte=4">
            if (p_Input.ReadULong() == <xsl:value-of select="@Value"/>)
            {
              p_Input.ReadAndAdvanceUInt64();
              return new Hime.Kernel.Parsers.SymbolTokenUInt64("<xsl:value-of select="@Name"/>", 0x<xsl:value-of select="@SID"/>, <xsl:value-of select="@Value"/>);
            }
          </xsl:if>
          return null;
        </xsl:if>
        <xsl:if test="@LengthByte=0">
			    if (p_CurrentBitLeft &lt; <xsl:value-of select="@LengthBit"/>) return null;
			    if (((p_Input.ReadByte() >> (p_CurrentBitLeft - <xsl:value-of select="@LengthBit"/>)) &amp; p_Flag<xsl:value-of select="@LengthBit"/>) != <xsl:value-of select="@Value"/>) return null;
			    p_CurrentBitLeft -= <xsl:value-of select="@LengthBit"/>;
          if (p_CurrentBitLeft == 0) { p_CurrentBitLeft = 8; p_Input.ReadAndAdvanceByte(); }
          return new Hime.Kernel.Parsers.SymbolTokenBits("<xsl:value-of select="@Name"/>", 0x<xsl:value-of select="@SID"/>, <xsl:value-of select="@Value"/>);
        </xsl:if>
      </xsl:if>
      <xsl:if test="@Joker='True'">
        <xsl:if test="@LengthByte!=0">
          if (p_CurrentBitLeft != 8) return null;
          if (!p_Input.CanRead(<xsl:value-of select="@LengthByte"/>)) return null;
          <xsl:if test="@LengthByte=1">
            return new Hime.Kernel.Parsers.SymbolTokenUInt8("<xsl:value-of select="@Name"/>", 0x<xsl:value-of select="@SID"/>, p_Input.ReadAndAdvanceByte());
          </xsl:if>
          <xsl:if test="@LengthByte=2">
            return new Hime.Kernel.Parsers.SymbolTokenUInt16("<xsl:value-of select="@Name"/>", 0x<xsl:value-of select="@SID"/>, p_Input.ReadAndAdvanceUInt16());
          </xsl:if>
          <xsl:if test="@LengthByte=4">
            return new Hime.Kernel.Parsers.SymbolTokenUInt32("<xsl:value-of select="@Name"/>", 0x<xsl:value-of select="@SID"/>, p_Input.ReadAndAdvanceUInt32());
          </xsl:if>
          <xsl:if test="@LengthByte=8">
            return new Hime.Kernel.Parsers.SymbolTokenUInt64("<xsl:value-of select="@Name"/>", 0x<xsl:value-of select="@SID"/>, p_Input.ReadAndAdvanceUInt64());
          </xsl:if>
        </xsl:if>
        <xsl:if test="@LengthByte=0">
          if (p_CurrentBitLeft &lt; <xsl:value-of select="@LengthBit"/>) return null;
			    Hime.Kernel.Parsers.SymbolToken Temp = new Hime.Kernel.Parsers.SymbolTokenBits("<xsl:value-of select="@Name"/>", 0x<xsl:value-of select="@SID"/>, (byte)((p_Input.ReadByte() >> (p_CurrentBitLeft - <xsl:value-of select="@LengthBit"/>)) &amp; <xsl:value-of select="@LengthBit"/>));
			    p_CurrentBitLeft -= <xsl:value-of select="@LengthBit"/>;
			    if (p_CurrentBitLeft == 0) { p_CurrentBitLeft = 8; p_Input.ReadAndAdvanceByte(); }
			    return Temp;
        </xsl:if>
		  </xsl:if>
		  }
	  </xsl:for-each>
	  }
  </xsl:template>
  
  
  <xsl:template name="Hime_ClassLexer_Text">
    public class Lexer_<xsl:value-of select="../@Name"/> : Hime.Kernel.Parsers.LexerText
    {
      private static ushort[] p_SymbolsSID = { <xsl:for-each select="Symbols/SymbolTerminalText"><xsl:if test="position()!=1">, </xsl:if>0x<xsl:value-of select="@SID"/></xsl:for-each> };
      private static string[] p_SymbolsName = { <xsl:for-each select="Symbols/SymbolTerminalText"><xsl:if test="position()!=1">, </xsl:if>"<xsl:value-of select="@Name"/>"</xsl:for-each> };
	  <xsl:for-each select="States/DFAState">
      private static ushort[][] p_Transitions<xsl:value-of select="@ID"/> = { <xsl:for-each select="Transition"><xsl:if test="position()!=1">, </xsl:if>  new ushort[3] { 0x<xsl:value-of select="@CharBegin"/>, 0x<xsl:value-of select="@CharEnd"/>, 0x<xsl:value-of select="@Next"/> }</xsl:for-each> };
    </xsl:for-each>
      private static ushort[][][] p_Transitions = { <xsl:for-each select="States/DFAState"><xsl:if test="position()!=1">, </xsl:if>p_Transitions<xsl:value-of select="@ID"/></xsl:for-each> };
      private static int[] p_Finals = { <xsl:for-each select="States/DFAState"><xsl:if test="position()!=1">, </xsl:if><xsl:value-of select="@Final"/></xsl:for-each> };
      
      public Lexer_<xsl:value-of select="../@Name"/>(string input) : base(input) { }
      protected Lexer_<xsl:value-of select="../@Name"/>(string input, int position, int line, System.Collections.Generic.List&lt;Hime.Kernel.Parsers.LexerTextError&gt; errors) : base(input, position, line, errors) { }
      
      public static string GetSymbolName(ushort SID)
      {
        for (int i=0; i!=p_SymbolsSID.Length; i++)
        {
	        if (p_SymbolsSID[i] == SID)
		        return p_SymbolsName[i];
        }
        return null;
      }
      
      <xsl:for-each select="Symbols/SymbolTerminalText">
        <xsl:if test="string-length(@SubGrammar)!=0">
      private Hime.Kernel.Parsers.SyntaxTreeNode MatchSubGrammar_<xsl:value-of select="@SID"/>(string TokenValue)
      {
        Lexer_<xsl:value-of select="@SubGrammar"/> Lexer = new Lexer_<xsl:value-of select="@SubGrammar"/>(TokenValue);
        Parser_<xsl:value-of select="@SubGrammar"/> Parser = new Parser_<xsl:value-of select="@SubGrammar"/>(Lexer);
        return Parser.Analyse();
      }
        </xsl:if>
      </xsl:for-each>
    
      public Lexer_<xsl:value-of select="../@Name"/> Clone_<xsl:value-of select="../@Name"/>() { return new Lexer_<xsl:value-of select="../@Name"/>(p_Input, p_CurrentPosition, p_Line, p_Errors); }
      public override Hime.Kernel.Parsers.SymbolToken GetNextToken(ushort[] IDs) { throw new Hime.Kernel.Parsers.LexerException("Text lexer does not support this method."); }
      public override Hime.Kernel.Parsers.SymbolToken GetNextToken()
      {
	      if (p_CurrentPosition == p_Length)
        {
            p_CurrentPosition++;
            return new Hime.Kernel.Parsers.SymbolTokenDollar();
        }
        if (p_CurrentPosition > p_Length)
            return new Hime.Kernel.Parsers.SymbolTokenEpsilon();
        
	      while (true)
	      {
		      if (p_CurrentPosition == p_Length)
		      {
			      p_CurrentPosition++;
			      return new Hime.Kernel.Parsers.SymbolTokenDollar();
		      }
		      Hime.Kernel.Parsers.SymbolTokenText Token = GetNextToken_DFA();
		      if (Token == null)
		      {
			      p_Errors.Add(new Hime.Kernel.Parsers.LexerTextErrorDiscardedChar(p_Input[p_CurrentPosition], p_Line, CurrentColumn));
			      p_CurrentPosition++;
		      }
	      <xsl:if test="@Separator">
          else if (Token.SymbolID == <xsl:value-of select="@Separator"/>)
		      {
			      p_CurrentPosition += Token.ValueText.Length;
			      foreach (char c in Token.ValueText) { if (c == '\n') p_Line++; }
		      }
        </xsl:if>
          else
          {
        <xsl:for-each select="Symbols/SymbolTerminalText">
          <xsl:if test="string-length(@SubGrammar)!=0">
            if (Token.SymbolID == 0x<xsl:value-of select="@SID"/>)
              Token.SubGrammarRoot = MatchSubGrammar_<xsl:value-of select="@SID"/>(Token.ValueText);
          </xsl:if>
        </xsl:for-each>
	          p_CurrentPosition += Token.ValueText.Length;
	          foreach (char c in Token.ValueText) { if (c == '\n') p_Line++; }
	          return Token;
		      }
	      }
      }

      private Hime.Kernel.Parsers.SymbolTokenText GetNextToken_DFA()
      {
	      System.Collections.Generic.List&lt;Hime.Kernel.Parsers.SymbolTokenText&gt; MatchedTokens = new System.Collections.Generic.List&lt;Hime.Kernel.Parsers.SymbolTokenText&gt;();
	      int End = p_CurrentPosition;
	      ushort State = 0;
      	
	      while (true)
	      {
		      if (p_Finals[State] != -1)
		      {
			      string Value = p_Input.Substring(p_CurrentPosition, End - p_CurrentPosition);
			      MatchedTokens.Add(new Hime.Kernel.Parsers.SymbolTokenText(p_SymbolsName[p_Finals[State]], p_SymbolsSID[p_Finals[State]], Value, p_Line));
		      }
		      if (End == p_Length)
			      break;
		      ushort Char = System.Convert.ToUInt16(p_Input[End]);
		      ushort NextState = 0xFFFF;
		      End++;
		      for (int i=0; i!=p_Transitions[State].Length; i++)
		      {
			      if (Char >= p_Transitions[State][i][0] &amp;&amp; Char &lt;= p_Transitions[State][i][1])
				      NextState = p_Transitions[State][i][2];
		      }
		      if (NextState == 0xFFFF)
			      break;
		      State = NextState;
	      }
	      if (MatchedTokens.Count == 0)
		      return null;
	      return MatchedTokens[MatchedTokens.Count-1];
      }
    }
  </xsl:template>
</xsl:stylesheet>
