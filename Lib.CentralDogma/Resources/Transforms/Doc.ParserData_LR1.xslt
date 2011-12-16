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
      <td style="width: 60pt;" class="HimeDataCellCenterLeft">
        <xsl:if test="@Conflict!='None'">
          <xsl:attribute name="style">
            background-color: #FFEEEE;
          </xsl:attribute>
        </xsl:if>
        <div class="ColumnAction">
          <img style="width: 20pt; height: 20pt;">
            <xsl:attribute name="src">
              <xsl:text>hime_data/Hime.</xsl:text>
              <xsl:value-of select="Action/@Type"/>
              <xsl:text>.png</xsl:text>
            </xsl:attribute>
            <xsl:attribute name="alt">
              <xsl:value-of select="Action/@Type"/>
            </xsl:attribute>
          </img>
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
        </div>
      </td>
      <td class="HimeDataCellCenter">
        <xsl:if test="@Conflict!='None'">
          <xsl:attribute name="style">
            background-color: #FFEEEE;
          </xsl:attribute>
        </xsl:if>
        <div class="ColumnHead">
          <a class="HimeSymbolVariable" href="grammar.html">
            <xsl:value-of select="@HeadName"/>
          </a>
          →
        </div>
      </td>
      <td class="HimeDataCellCenter">
        <xsl:if test="@Conflict!='None'">
          <xsl:attribute name="style">
            background-color: #FFEEEE;
          </xsl:attribute>
        </xsl:if>
        <div class="ColumnBody">
          <xsl:apply-templates select="Symbols"/>
        </div>
      </td>
      <td class="HimeDataCellCenterRight">
        <xsl:if test="@Conflict!='None'">
          <xsl:attribute name="style">
            background-color: #FFEEEE;
          </xsl:attribute>
        </xsl:if>
        <div class="ColumnLookaheads">
          <xsl:choose>
            <xsl:when test="count(Lookaheads/SymbolTerminalText)=0">∅</xsl:when>
            <xsl:otherwise>
              <xsl:apply-templates select="Lookaheads"/>
            </xsl:otherwise>
          </xsl:choose>
        </div>
      </td>
      <td class="HimeDataCellCenterRight">
        <xsl:if test="@Conflict!='None'">
          <xsl:attribute name="style">
            background-color: #FFEEEE;
          </xsl:attribute>
        </xsl:if>
        <div class="ColumnConflicts">
          <xsl:choose>
            <xsl:when test="count(ConflictLookaheads/SymbolTerminalText)=0">∅</xsl:when>
            <xsl:otherwise>
              <xsl:apply-templates select="ConflictLookaheads"/>
            </xsl:otherwise>
          </xsl:choose>
        </div>
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
        <div>
          <table border="0" cellspacing="0" cellpadding="0" >
            <tr class="HimeLRItem">
              <td style="width: 60pt;" class="HimeDataCellTopLeft">
                <img src="hime_data/button_plus.gif" onclick="showColumn(0)"/>
                <img src="hime_data/button_minus.gif" onclick="hideColumn(0)"/>
                Actions
              </td>
              <td class="HimeDataCellTop">
                <img src="hime_data/button_plus.gif" onclick="showColumn(1)"/>
                <img src="hime_data/button_minus.gif" onclick="hideColumn(1)"/>
                Heads
              </td>
              <td class="HimeDataCellTop">
                <img src="hime_data/button_plus.gif" onclick="showColumn(2)"/>
                <img src="hime_data/button_minus.gif" onclick="hideColumn(2)"/>
                Bodies
              </td>
              <td class="HimeDataCellTopRight">
                <img src="hime_data/button_plus.gif" onclick="showColumn(3)"/>
                <img src="hime_data/button_minus.gif" onclick="hideColumn(3)"/>
                Lookaheads
              </td>
              <td class="HimeDataCellTopRight">
                <img src="hime_data/button_plus.gif" onclick="showColumn(4)"/>
                <img src="hime_data/button_minus.gif" onclick="hideColumn(4)"/>
                Conflicts
              </td>
            </tr>
            <xsl:apply-templates/>
          </table>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
