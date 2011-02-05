<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:include href="Generators.Lexers.xslt"/>
  <xsl:include href="Generators.ParserLR1.xslt"/>
  
  <xsl:template match="ContextFreeGrammar">
    <xsl:if test="@Input='Text'">
      <xsl:for-each select="Lexer">
        <xsl:call-template name="Hime_ClassLexer_Text"/>
      </xsl:for-each>
      <xsl:if test="@Method='LR1'">
        <xsl:for-each select="Parser">
          <xsl:call-template name="Hime_ClassParserLR1_Text"/>
        </xsl:for-each>
      </xsl:if>
      <xsl:if test="@Method='LALR1'">
        <xsl:for-each select="Parser">
          <xsl:call-template name="Hime_ClassParserLR1_Text"/>
        </xsl:for-each>
      </xsl:if>
    </xsl:if>
    <xsl:if test="@Input='Binary'">
      <xsl:for-each select="Lexer">
        <xsl:call-template name="Hime_ClassLexer_Binary"/>
      </xsl:for-each>
      <xsl:if test="@Method='LR1'">
        <xsl:for-each select="Parser">
          <xsl:call-template name="Hime_ClassParserLR1_Binary"/>
        </xsl:for-each>
      </xsl:if>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>
