<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="Symbol">
    <xsl:choose>
      <xsl:when test="@type='TerminalText'">
        <span class="HimeSymbolTerminalText">
          <xsl:value-of select="@value"/>
        </span>
      </xsl:when>
      <xsl:when test="@type='CFVariable'">
        <a class="HimeSymbolCFVariable">
          <xsl:attribute name="href">
            <xsl:text>#rule_</xsl:text>
            <xsl:value-of select="@sid"/>
            <xsl:text>_0</xsl:text>
          </xsl:attribute>
          <xsl:value-of select="@name"/>
        </a>
      </xsl:when>
      <xsl:otherwise>
        <span>
          <xsl:attribute name="class">
            <xsl:text>HimeSymbol</xsl:text>
            <xsl:value-of select="@type"/>
          </xsl:attribute>
          <xsl:value-of select="@name"/>
        </span>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
  <xsl:template match="Rule">
    <tr class="HimeEntry">
      <xsl:attribute name="id">
        <xsl:text>rule_</xsl:text><xsl:value-of select="@HeadSID"/>_<xsl:value-of select="@RuleID"/>
      </xsl:attribute>
      <td class="HimeData">
        <xsl:attribute name="style">
          <xsl:if test="position()=1">
            <xsl:text>border-top: none; border-left: none; width: 40;</xsl:text>
          </xsl:if>
          <xsl:if test="position()>=2">
            <xsl:text>border-left: none; width: 40;</xsl:text>
          </xsl:if>
        </xsl:attribute>
        <xsl:value-of select="@HeadSID"/>, <xsl:value-of select="@RuleID"/>
      </td>
      <td class="HimeData">
        <xsl:attribute name="style">
          <xsl:if test="position()=1">
            <xsl:text>border-top: none; border-left: none;</xsl:text>
          </xsl:if>
          <xsl:if test="position()>=2">
            <xsl:text>border-left: none;</xsl:text>
          </xsl:if>
        </xsl:attribute>
        <xsl:value-of select="@HeadName"/>
        →
      </td>
      <td class="HimeData">
        <xsl:attribute name="style">
          <xsl:if test="position()=1">
            <xsl:text>border-top: none; border-left: none;</xsl:text>
          </xsl:if>
          <xsl:if test="position()>=2">
            <xsl:text>border-left: none;</xsl:text>
          </xsl:if>
        </xsl:attribute>
        <xsl:apply-templates/>
      </td>
    </tr>
  </xsl:template>

  <xsl:template match="CFGrammar">
    <html>
      <head>
        <meta charset="utf-8"/>
        <title>
          Grammar <xsl:value-of select="@Name"/>
        </title>
        <link rel="stylesheet" type="text/css" href="resources/Hime.css" />
        <script src="resources/Hime.js" type="text/javascript">aaa</script>
      </head>
      <body>
        <div>
          <h1>
            Grammar Rules
          </h1>
        </div>
        <div class="HimeBody">
          <table border="0" cellspacing="0" cellpadding="0" style="width: 100%;">
            <xsl:apply-templates select="//Rule"/>
          </table>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
