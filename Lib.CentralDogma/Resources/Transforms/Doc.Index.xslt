<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="ItemSet">
    <tr class="HimeEntry">
      <td class="HimeData" style="border-left: none;">
        <table>
          <tr>
            <td>
              Set <xsl:value-of select="@SetID"/>
            </td>
            <td style="width: 20px; height: 20px">
              <img src="resources/Hime.GoTo.png" alt="Go to" style="width: 20px; height: 20px; cursor: pointer;">
                <xsl:attribute name="onclick">
                  <xsl:text>display('Set_</xsl:text>
                  <xsl:value-of select="@SetID"/>
                  <xsl:text>.html')</xsl:text>
                </xsl:attribute>
              </img>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </xsl:template>
  
  <xsl:template match="LRGraph">
    <html>
      <head>
        <meta charset="utf-8"/>
        <title>Grammar documentation</title>
        <link rel="stylesheet" type="text/css" href="resources/Hime.css" />
        <script src="resources/Hime.js" type="text/javascript">aaa</script>
      </head>
      <body class="HimeBody">
        <div style="width:15%; height:100%; overflow:auto; float:left;">
          <table border="0" cellspacing="0" cellpadding="0">
            <tr class="HimeEntry">
              <td class="HimeData" style="border-top: none; border-left: none;">
                <table>
                  <tr>
                    <td>
                      Grammar
                    </td>
                    <td style="width: 20px; height: 20px">
                      <img src="resources/Hime.GoTo.png" alt="Go to" style="width: 20px; height: 20px; cursor: pointer;" onclick="display('grammar.html')"/>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
            <xsl:apply-templates/>
          </table>
        </div>
        <div style="width:80%; height:100%; overflow:auto; float:right;">
          <div id="myContent" class="maximize" />
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
