<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="Symbol">
    <xsl:if test="@SymbolType='Variable'">
      <a class="HimeSymbolVariable">
        <xsl:attribute name="href">
          <xsl:text>#</xsl:text>
          <xsl:value-of select="@SymbolID"/>
          <xsl:text>_0</xsl:text>
        </xsl:attribute>
        <xsl:value-of select="@SymbolValue"/>
      </a>
    </xsl:if>
    <xsl:if test="@SymbolType!='Variable'">
      <span>
        <xsl:attribute name="class">
          <xsl:text>HimeSymbol</xsl:text>
          <xsl:value-of select="@SymbolType"/>
        </xsl:attribute>
        <xsl:value-of select="@SymbolValue"/>
      </span>
    </xsl:if>
  </xsl:template>
  
  <xsl:template match="Rule">
    <tr class="HimeEntry">
      <xsl:attribute name="id">
        <xsl:value-of select="@HeadSID"/>_<xsl:value-of select="@RuleID"/>
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
    <html xmlns="http://www.w3.org/1999/xhtml" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:schemalocation="http://www.w3.org/MarkUp/SCHEMA/xhtml11.xsd" xml:lang="en">
      <head>
        <title>
          Grammar <xsl:value-of select="@Name"/>
        </title>
        <link rel="stylesheet" type="text/css" href="hime_data/Hime.css" />
        <script src="hime_data/Hime.js" type="text/javascript">aaa</script>
      </head>
      <body>
        <div id="HimeXHTMLHeader" class="HimeHeader">
          <img src="hime_data/Hime.Logo.png" class="HimeLogo" alt="Hime Systems Logo" />
          <span class="HimeDocumentTitle">
            Grammar <xsl:value-of select="@Name"/>
          </span>
        </div>
        <div class="HimeBody">
          <table border="0" cellspacing="0" cellpadding="0" style="width: 100%;">
            <xsl:apply-templates/>
          </table>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
