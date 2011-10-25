<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:exsl="http://exslt.org/common" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="Symbol">
    <xsl:if test="@SymbolType='Variable'">
      <a class="HimeSymbolVariable" href="grammar.html">
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
    <span>●</span>
  </xsl:template>

  <xsl:template match="Item">
    <tr class="HimeLRItem">
      <td style="width:60pt">
        <xsl:attribute name="class">
          <xsl:choose>
            <xsl:when test="../Item[1]=.">
              <xsl:text>HimeDataCellTopLeft</xsl:text>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>HimeDataCellCenterLeft</xsl:text>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <img style="width: 20pt; height: 20pt;">
          <xsl:attribute name="src">
            <xsl:text>hime_data/Hime.</xsl:text>
            <xsl:value-of select="@Conflict"/>
            <xsl:text>.png</xsl:text>
          </xsl:attribute>
          <xsl:attribute name="alt">
            <xsl:value-of select="@Conflict"/>
            <xsl:text> conflict</xsl:text>
          </xsl:attribute>
        </img>
      </td>
      <td style="width: 60pt;">
        <xsl:attribute name="class">
          <xsl:choose>
            <xsl:when test="../Item[1]=.">
              <xsl:text>HimeDataCellTop</xsl:text>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>HimeDataCellCenter</xsl:text>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <img style="width: 20pt; height: 20pt;">
          <xsl:attribute name="src">
            <xsl:text>hime_data/Hime.</xsl:text>
            <xsl:value-of select="Action/@Type"/>
            <xsl:text>.png</xsl:text>
          </xsl:attribute>
          <xsl:attribute name="alt">
            <xsl:value-of select="Action/@Type"/>
          </xsl:attribute>
          <xsl:if test="Action/@Type='Shift'">
            <span>
              to
              <a>
                <xsl:attribute name="href">
                  <xsl:text>Set_</xsl:text><xsl:value-of select="Action"/>.html
                </xsl:attribute>
                <xsl:value-of select="Action"/>
              </a>
            </span>
          </xsl:if>
        </img>
      </td>
      <td>
        <xsl:attribute name="class">
          <xsl:choose>
            <xsl:when test="../Item[1]=.">
              <xsl:text>HimeDataCellTop</xsl:text>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>HimeDataCellCenter</xsl:text>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <a class="HimeSymbolVariable" href="grammar.html">
          <xsl:value-of select="@HeadName"/>
        </a>
        →
      </td>
      <td>
        <xsl:attribute name="class">
          <xsl:choose>
            <xsl:when test="../Item[1]=.">
              <xsl:text>HimeDataCellTop</xsl:text>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>HimeDataCellCenter</xsl:text>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:apply-templates select="Symbols"/>
      </td>
      <td>
        <xsl:attribute name="class">
          <xsl:choose>
            <xsl:when test="../Item[1]=.">
              <xsl:text>HimeDataCellTopRight</xsl:text>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>HimeDataCellCenterRight</xsl:text>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:choose>
          <xsl:when test="count(Lookaheads/SymbolTerminalText)=0">∅</xsl:when>
          <xsl:otherwise>
            <xsl:apply-templates select="Lookaheads"/>
          </xsl:otherwise>
        </xsl:choose>
      </td>
    </tr>
  </xsl:template>


  <xsl:template match="ItemSet">
    <html>
      <head>
        <meta charset="utf-8"/>
        <title>
          Set <xsl:value-of select="@SetID"/>
        </title>
        <link rel="stylesheet" type="text/css" href="hime_data/Hime.css" />
        <script src="hime_data/Hime.js" type="text/javascript">aaa</script>
      </head>
      <body>
        <div>
          <h1>
            Set <xsl:value-of select="@SetID"/>
          </h1>
        </div>
        <div class="HimeBody">
          <table border="0" cellspacing="0" cellpadding="0" style="width: 100%;">
            <xsl:apply-templates/>
          </table>
          <br/>
          <br/>
          <iframe frameborder="no" class="maximize">
            <xsl:attribute name="src">
              <xsl:text>Set_</xsl:text>
              <xsl:value-of select="@SetID"/>
              <xsl:text>.dot</xsl:text>
            </xsl:attribute>
          </iframe>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
