<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="Component">
    <td class="HimeData">
      <xsl:attribute name="style">
        <xsl:if test="count(parent::*/preceding-sibling::*)=0">
          <xsl:text>border-top: none;</xsl:text>
        </xsl:if>
      </xsl:attribute>
      <xsl:value-of select="."/>
    </td>
  </xsl:template>

  <xsl:template match="Message">
    <td class="HimeData">
      <xsl:attribute name="style">
        <xsl:if test="count(parent::*/preceding-sibling::*)=0">
          <xsl:text>border-top: none;</xsl:text>
        </xsl:if>
      </xsl:attribute>
      <xsl:value-of select="."/>
    </td>
  </xsl:template>
  
  
  <xsl:template match="Conflict">
    <td class="HimeData">
      <xsl:attribute name="style">
        <xsl:if test="count(parent::*/preceding-sibling::*)=0">
          <xsl:text>border-top: none;</xsl:text>
        </xsl:if>
      </xsl:attribute>
      <table border="0" cellpadding="0" cellspacing="0">
        <xsl:apply-templates/>
      </table>
    </td>
  </xsl:template>

  <xsl:template match="Header">
    <tr>
      <td class="HimeDataLine">
        Conflict <xsl:value-of select="@type"/> in state <xsl:value-of select="@set"/> on 
        <xsl:apply-templates/>
        for items:
      </td>
    </tr>
  </xsl:template>

  <xsl:template match="Symbol">
    <xsl:if test="@SymbolType='Variable'">
      <span class="HimeSymbolVariable">
        <xsl:value-of select="@SymbolValue"/>
      </span>
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
  <xsl:template match="SymbolTerminalText">
    <span class="HimeSymbolTerminal">
      <xsl:value-of select="@Value"/>
    </span>
  </xsl:template>
  <xsl:template match="SymbolTerminal">
    <span class="HimeSymbolTerminal">
      <xsl:value-of select="@Value"/>
    </span>
  </xsl:template>
  <xsl:template match="Dot">
    <span>•</span>
  </xsl:template>

  <xsl:template match="Item">
    <tr class="HimeLRItem">
      <td class="HimeDataLine">
        <span class="HimeSymbolVariable">
          <xsl:value-of select="@HeadName"/>
        </span>
        →
        <xsl:apply-templates select="Symbols"/>
      </td>
    </tr>
  </xsl:template>

  <xsl:template match="Example">
    <tr>
      <td class="HimeDataLine">
        Input sample: 
        <xsl:apply-templates/>
      </td>
    </tr>
  </xsl:template>

  <xsl:template match="Entry">
    <tr class="HimeEntry">
      <td class="HimeData">
        <xsl:attribute name="style">
          <xsl:if test="position()=1">
            <xsl:text>border-top: none; border-left: none; width: 15;</xsl:text>
          </xsl:if>
          <xsl:if test="position()>=2">
            <xsl:text>border-left: none; width: 15;</xsl:text>
          </xsl:if>
        </xsl:attribute>
        <xsl:if test="@mark='Info'">
          <img src="hime_data/Hime.Info.png" alt="Information" style="width: 15pt; height: 15pt;" />
        </xsl:if>
        <xsl:if test="@mark='Warning'">
          <img src="hime_data/Hime.Warning.png" alt="Warning" style="width: 15pt; height: 15pt;" />
        </xsl:if>
        <xsl:if test="@mark='Error'">
          <img src="hime_data/Hime.Error.png" alt="Error" style="width: 15pt; height: 15pt;" />
        </xsl:if>
      </td>
      <xsl:apply-templates/>
    </tr>
  </xsl:template>

  <xsl:template match="Section">
    <div class="HimeSection">
      <div class="HimeSectionTitle">
        <img src="hime_data/button_minus.gif">
          <xsl:attribute name="id">
            <xsl:value-of select="@id"/>
            <xsl:text>_button</xsl:text>
          </xsl:attribute>
          <xsl:attribute name="onclick">
            <xsl:text>toggle(</xsl:text>
            <xsl:value-of select="@id"/>
            <xsl:text>_button,</xsl:text>
            <xsl:value-of select="@id"/>
            <xsl:text>_content)</xsl:text>
          </xsl:attribute>
        </img>
        <span>
          <xsl:value-of select="@name"/>
        </span>
      </div>
      <div class="HimeSectionContent">
        <xsl:attribute name="id">
          <xsl:value-of select="@id"/>
          <xsl:text>_content</xsl:text>
        </xsl:attribute>
        <table border="0" cellspacing="0" cellpadding="0" style="width: 100%;">
          <xsl:apply-templates select="Entry" />
        </table>
      </div>
    </div>
  </xsl:template>

  <xsl:template match="Log">
    <html xmlns="http://www.w3.org/1999/xhtml" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:schemalocation="http://www.w3.org/MarkUp/SCHEMA/xhtml11.xsd" xml:lang="en">
      <head>
        <title>
          <xsl:value-of select="@title"/>
        </title>
        <link rel="stylesheet" type="text/css" href="hime_data/Logs.css" />
        <script src="hime_data/Hime.js" type="text/javascript">aaa</script>
      </head>
      <body>
        <div id="HimeXHTMLHeader" class="HimeHeader">
          <img src="hime_data/Hime.Logo.png" class="HimeLogo" alt="Hime Systems Logo" />
          <span class="HimeDocumentTitle">
            <xsl:value-of select="@title"/>
          </span>
        </div>
        <div class="HimeBody">
          <xsl:apply-templates select="Section"/>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
