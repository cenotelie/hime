<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="Data">
    <td class="HimeLogData">
      <xsl:attribute name="style">
        <xsl:if test="count(parent::*/preceding-sibling::*)=0">
          <xsl:text>border-top: none;</xsl:text>
        </xsl:if>
      </xsl:attribute>
      <xsl:value-of select="."/>
    </td>
  </xsl:template>

  <xsl:template match="Entry">
    <tr class="HimeLogEntry">
      <td class="HimeLogData">
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
      <xsl:apply-templates select="Data" />
    </tr>
  </xsl:template>

  <xsl:template match="Section">
    <div class="HimeLogSection">
      <div class="HimeLogSectionTitle">
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
      <div class="HimeLogSectionContent">
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

  <xsl:template match="Meta">

  </xsl:template>

  <xsl:template match="Log">
    <html xmlns="http://www.w3.org/1999/xhtml" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:schemalocation="http://www.w3.org/MarkUp/SCHEMA/xhtml11.xsd" xml:lang="en">
      <head>
        <title>
          <xsl:value-of select="@title"/>
        </title>
        <style type="text/css">
          .HimeHeader
          {
              width: 90%;
              height: 50px;
              padding: 10px;
              border: solid 2px #840000;
              background-color: #FFEEEE;
          }
          .HimeLogo
          {
              width: 50px;
              height: 50px;
          }
          .HimeSystemsName
          {
              font-family: Sans-Serif;
              font-size: 20pt;
              color: #840000;
              height: 100px;
          }
          .HimeDocumentTitle
          {
              position: absolute;
              left: 100px;
              height: 100px;
              width: 50%;
              font-family: Sans-Serif;
              font-size: 20pt;
              text-align: center;
              text-transform: capitalize;
              vertical-align: middle;
          }
          .HimeLogBody
          {
              padding: 10px;
              width: 90%;
              font-family: Sans-Serif;
              font-size: 10pt;
              color: Black;
              background-color: White;
          }
          .HimeLogSection
          {
              padding: 10px;
              width: 100%;
          }
          .HimeLogSectionTitle
          {
              width: 100%;
              background-color: #C8C8FF;
              font-family: Serif;
              font-size: 15pt;
              text-align: left;
              padding-left: 5px;
              padding-right: 5px;
          }
          .HimeLogSectionContent
          {
              position: relative;
              left: 10%;
              width: 90%;
              border: Solid 3px #C8C8FF;
              border-top-style: none;
          }
          .HimeLogEntry
          { }
          .HimeLogData
          {
        	    border-top: Solid 1px Black;
        	    border-left: Solid 1px Black;
              padding-left: 5px;
              padding-right: 5px;
              padding-top: 3px;
              padding-bottom: 3px;
              font-family: Sans-Serif;
              font-size: 10pt;
          }
        </style>
        <script type="text/javascript">
          function toggle(btn, content)
          {
            if (content.style.display == "none")
            {
              content.style.display = "";
              btn.src = "hime_data/button_minus.gif";
            }
            else
            {
              content.style.display = "none";
              btn.src = "hime_data/button_plus.gif";
            }
          }
        </script>
      </head>
      <body>
        <div id="HimeXHTMLHeader" class="HimeHeader">
          <img src="hime_data/Hime.Logo.png" class="HimeLogo" alt="Hime Systems Logo" />
          <span class="HimeDocumentTitle">
            <xsl:value-of select="@title"/>
          </span>
        </div>
        <div class="HimeLogBody">
          <xsl:apply-templates select="Section"/>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
