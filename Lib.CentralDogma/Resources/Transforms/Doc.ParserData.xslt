<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:exsl="http://exslt.org/common" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="Symbol">
    <xsl:choose>
      <xsl:when test="@type='Variable'">
        <a class="HimeSymbolVariable">
          <xsl:attribute name="href">
            <xsl:text>grammar.html#rule_</xsl:text>
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

  <xsl:template match="Dot">
    <span>●</span>
  </xsl:template>

  <xsl:template match="Origin">
    <tr>
      <td>
        <span>from</span>
        <a>
          <xsl:attribute name="href">
            <xsl:text>set_</xsl:text>
            <xsl:value-of select="@State"/>
            <xsl:text>.html</xsl:text>
          </xsl:attribute>
          <xsl:value-of select="@State"/>
        </a>
        <span>by</span>
        <xsl:apply-templates select="Symbol"/>
      </td>
    </tr>
  </xsl:template>
  
  <xsl:template match="Item">
    <tr class="HimeLRItem">
      <td style="width: 60pt;" class="HimeDataCellCenterLeft">
        <xsl:if test="@Conflict!='false'">
          <xsl:attribute name="style">
            <xsl:text>background-color: #FFEEEE;</xsl:text>
          </xsl:attribute>
        </xsl:if>
        <div class="ColumnAction">
          <img style="width: 20pt; height: 20pt;">
            <xsl:attribute name="src">
              <xsl:text>resources/Hime.</xsl:text>
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
                  <xsl:text>set_</xsl:text>
                  <xsl:value-of select="Action"/>
                  <xsl:text>.html</xsl:text>
                </xsl:attribute>
                <xsl:value-of select="Action"/>
              </a>
            </span>
          </xsl:if>
        </div>
        <div/>
      </td>
      <td class="HimeDataCellCenter">
        <xsl:if test="@Conflict!='false'">
          <xsl:attribute name="style">
            <xsl:text>background-color: #FFEEEE;</xsl:text>
          </xsl:attribute>
        </xsl:if>
        <div class="ColumnHead">
          <a class="HimeSymbolVariable" href="grammar.html">
            <xsl:value-of select="@HeadName"/>
          </a>
          →
        </div>
        <div/>
      </td>
      <td class="HimeDataCellCenter">
        <xsl:if test="@Conflict!='false'">
          <xsl:attribute name="style">
            <xsl:text>background-color: #FFEEEE;</xsl:text>
          </xsl:attribute>
        </xsl:if>
        <div class="ColumnBody">
          <xsl:apply-templates select="Symbols"/>
        </div>
        <div/>
      </td>
      <td class="HimeDataCellCenterRight">
        <xsl:if test="@Conflict!='false'">
          <xsl:attribute name="style">
            <xsl:text>background-color: #FFEEEE;</xsl:text>
          </xsl:attribute>
        </xsl:if>
        <div class="ColumnLookaheads">
          <xsl:apply-templates select="Lookaheads"/>
        </div>
        <div/>
      </td>
      <td class="HimeDataCellCenterRight">
        <xsl:if test="@Conflict!='false'">
          <xsl:attribute name="style">
            <xsl:text>background-color: #FFEEEE;</xsl:text>
          </xsl:attribute>
        </xsl:if>
        <div class="ColumnConflicts">
          <xsl:apply-templates select="ConflictLookaheads"/>
        </div>
        <div/>
      </td>
      <td class="HimeDataCellCenterRight">
        <xsl:if test="@Conflict!='false'">
          <xsl:attribute name="style">
            <xsl:text>background-color: #FFEEEE;</xsl:text>
          </xsl:attribute>
        </xsl:if>
        <div class="ColumnOrigins">
          <xsl:if test="count(Origins/Origin)!=0">
            <table border="0" cellspacing="0" cellpadding="0" >
              <xsl:apply-templates select="Origins/Origin"/>
            </table>
          </xsl:if>
        </div>
        <div/>
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
        <link rel="stylesheet" type="text/css" href="resources/Hime.css" />
        <script src="resources/Hime.js" type="text/javascript">aaa</script>
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
                <img src="resources/button_plus.gif" onclick="showColumn(0)"/>
                <img src="resources/button_minus.gif" onclick="hideColumn(0)"/>
                Actions
              </td>
              <td class="HimeDataCellTop">
                <img src="resources/button_plus.gif" onclick="showColumn(1)"/>
                <img src="resources/button_minus.gif" onclick="hideColumn(1)"/>
                Heads
              </td>
              <td class="HimeDataCellTop">
                <img src="resources/button_plus.gif" onclick="showColumn(2)"/>
                <img src="resources/button_minus.gif" onclick="hideColumn(2)"/>
                Bodies
              </td>
              <td class="HimeDataCellTopRight">
                <img src="resources/button_plus.gif" onclick="showColumn(3)"/>
                <img src="resources/button_minus.gif" onclick="hideColumn(3)"/>
                Lookaheads
              </td>
              <td class="HimeDataCellTopRight">
                <img src="resources/button_plus.gif" onclick="showColumn(4)"/>
                <img src="resources/button_minus.gif" onclick="hideColumn(4)"/>
                Conflicts
              </td>
              <td class="HimeDataCellTopRight">
                <img src="resources/button_plus.gif" onclick="showColumn(5)"/>
                <img src="resources/button_minus.gif" onclick="hideColumn(5)"/>
                Origins
              </td>
            </tr>
            <xsl:apply-templates select="Item"/>
          </table>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
