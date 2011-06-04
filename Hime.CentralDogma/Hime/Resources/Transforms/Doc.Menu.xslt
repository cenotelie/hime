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
              <img src="hime_data/Hime.GoTo.png" alt="Go to" style="width: 20px; height: 20px; cursor: pointer;">
                <xsl:attribute name="onclick">
                  <xsl:text>display('Set_</xsl:text>
                  <xsl:value-of select="@SetID"/>
                  <xsl:text>.html')</xsl:text>
                </xsl:attribute>
              </img>
            </td>
            <td style="width: 20px; height: 20px">
              <img src="hime_data/Hime.GoTo.png" alt="Go to" style="width: 20px; height: 20px; cursor: pointer;">
                <xsl:attribute name="onclick">
                  <xsl:text>display('Set_</xsl:text>
                  <xsl:value-of select="@SetID"/>
                  <xsl:text>.dot')</xsl:text>
                </xsl:attribute>
              </img>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </xsl:template>
  
  <xsl:template match="LRGraph">
    <html xmlns="http://www.w3.org/1999/xhtml" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:schemalocation="http://www.w3.org/MarkUp/SCHEMA/xhtml11.xsd" xml:lang="en">
      <head>
        <title>Menu</title>
        <link rel="stylesheet" type="text/css" href="hime_data/Hime.css" />
        <script src="hime_data/Hime.js" type="text/javascript">aaa</script>
      </head>
      <body class="HimeBody">
        <table width="100%">
          <tr>
            <td>
              <div style="width:150px; height:500px; overflow:auto;">
                <table border="0" cellspacing="0" cellpadding="0">
                  <tr class="HimeEntry">
                    <td class="HimeData" style="border-top: none; border-left: none;">
                      <table>
                        <tr>
                          <td>
                            Grammar
                          </td>
                          <td style="width: 20px; height: 20px">
                            <img src="hime_data/Hime.GoTo.png" alt="Go to" style="width: 20px; height: 20px; cursor: pointer;" onclick="display('grammar.html')"/>
                          </td>
                        </tr>
                      </table>
                    </td>
                  </tr>
                  <xsl:apply-templates/>
                </table>
              </div>
            </td>
            <td style="width: 100%; height: 100%;">
              <div id="myContent">
              </div>
            </td>
          </tr>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
