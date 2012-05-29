<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="Component">
    <td class="HimeData" style="width: 75;">
      <xsl:value-of select="."/>
    </td>
  </xsl:template>

  <xsl:template match="Message">
    <td class="HimeData">
      <xsl:value-of select="."/>
    </td>
  </xsl:template>

  <xsl:template match="Exception">
    <td class="HimeData">
      <img src="hime_data/button_plus.gif">
        <xsl:attribute name="id">
          <xsl:text>button_ex_</xsl:text>
          <xsl:value-of select="@EID"/>
        </xsl:attribute>
        <xsl:attribute name="onclick">
          <xsl:text>toggle(</xsl:text>
          <xsl:text>button_ex_</xsl:text>
          <xsl:value-of select="@EID"/>
          <xsl:text>,</xsl:text>
          <xsl:text>content_ex_</xsl:text>
          <xsl:value-of select="@EID"/>
          <xsl:text>)</xsl:text>
        </xsl:attribute>
      </img>
      <xsl:value-of select="Message"/>
      <div style="display: none;">
        <xsl:attribute name="id">
          <xsl:text>content_ex_</xsl:text>
          <xsl:value-of select="@EID"/>
        </xsl:attribute>
        <br/>
        <table cellpadding="0" cellspacing="0" border="1" rules="all" frame="box">
          <xsl:apply-templates select="Stack"/>
        </table>
      </div>
    </td>
  </xsl:template>

  <xsl:template match="Line">
    <tr>
      <td class="HimeDataLine">
        <xsl:attribute name="style">
          <xsl:text>background-color: </xsl:text>
          <xsl:choose>
            <xsl:when test="(position() mod 4)=0">
              <xsl:text>#FFFFFF</xsl:text>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>#DDDDDD</xsl:text>
            </xsl:otherwise>
          </xsl:choose>
          <xsl:text>;</xsl:text>
        </xsl:attribute>
        <div style="margin: 3px;">
          <xsl:value-of select="."/>
        </div>
      </td>
    </tr>
  </xsl:template>
  
  <xsl:template match="Conflict">
    <td class="HimeData">
      <img src="hime_data/button_plus.gif">
        <xsl:attribute name="id">
          <xsl:text>button_</xsl:text>
          <xsl:value-of select="./Header/@set"/>
          <xsl:text>_</xsl:text>
          <xsl:value-of select="./Header/Symbol/@sid"/>
        </xsl:attribute>
        <xsl:attribute name="onclick">
          <xsl:text>toggle(</xsl:text>
          <xsl:text>button_</xsl:text>
          <xsl:value-of select="./Header/@set"/>
          <xsl:text>_</xsl:text>
          <xsl:value-of select="./Header/Symbol/@sid"/>
          <xsl:text>,</xsl:text>
          <xsl:text>conflict_</xsl:text>
          <xsl:value-of select="./Header/@set"/>
          <xsl:text>_</xsl:text>
          <xsl:value-of select="./Header/Symbol/@sid"/>
          <xsl:text>)</xsl:text>
        </xsl:attribute>
      </img>
      Conflict
      <xsl:value-of select="./Header/@type"/>
      in state
      <span class="HimeLRState">
        <xsl:value-of select="./Header/@set"/>
      </span>
      on
      <xsl:apply-templates select="Header"/>
      :
      <div style="display: none;">
        <xsl:attribute name="id">
          <xsl:text>conflict_</xsl:text>
          <xsl:value-of select="./Header/@set"/>
          <xsl:text>_</xsl:text>
          <xsl:value-of select="./Header/Symbol/@sid"/>
        </xsl:attribute>
        <br/>
        <table border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td class="HimeDataLine">Items:</td>
            <td>
              <table cellpadding="0" cellspacing="0" border="1" rules="all" frame="box">
                <xsl:apply-templates select="Items"/>
              </table>
            </td>
          </tr>
        </table>
        <br/>
        <table boder="0" cellpadding="0" cellspacing="0">
          <tr>
            <td class="HimeDataLine">Input examples:</td>
            <td>
              <table cellpadding="0" cellspacing="0" border="1" rules="all" frame="box">
                <xsl:apply-templates select="Examples"/>
              </table>
            </td>
          </tr>
        </table>
      </div>
    </td>
  </xsl:template>

  <xsl:template match="Symbol">
    <span>
	  <xsl:attribute name="class">
		<xsl:text>HimeSymbol</xsl:text>
		<xsl:value-of select="@type"/>
	  </xsl:attribute>
	  <xsl:value-of select="@name"/>
	</span>
  </xsl:template>
  
  <xsl:template match="Dot">
    <span>●</span>
  </xsl:template>

  <xsl:template match="Example">
    <tr>
      <td class="HimeDataLine">
        <xsl:attribute name="style">
          <xsl:text>background-color: </xsl:text>
          <xsl:choose>
            <xsl:when test="(position() mod 4)=0">
              <xsl:text>#FFFFFF</xsl:text>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>#DDDDDD</xsl:text>
            </xsl:otherwise>
          </xsl:choose>
          <xsl:text>;</xsl:text>
        </xsl:attribute>
        <div style="margin: 3px;">
          <xsl:apply-templates/>
        </div>
      </td>
    </tr>
  </xsl:template>
  
  <xsl:template match="Item">
    <tr class="HimeLRItem">
      <td class="HimeDataLine">
        <xsl:attribute name="style">
          <xsl:text>background-color: </xsl:text>
          <xsl:choose>
            <xsl:when test="(position() mod 4)=0">
              <xsl:text>#FFFFFF</xsl:text>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>#DDDDDD</xsl:text>
            </xsl:otherwise>
          </xsl:choose>
          <xsl:text>;</xsl:text>
        </xsl:attribute>
        <div style="margin: 3px;">
          <span class="HimeSymbolVariable">
            <xsl:value-of select="@HeadName"/>
          </span>
          →
          <xsl:apply-templates select="Symbols"/>
        </div>
      </td>
    </tr>
  </xsl:template>

  <xsl:template match="Entry">
    <tr>
      <td class="HimeData" style="width: 20;">
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
        <table cellspacing="0" cellpadding="0" style="width: 100%;" border="1px" rules="all" frame="void" bordercolor="gray">
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
        <link rel="stylesheet" type="text/css" href="hime_data/Hime.css" />
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
