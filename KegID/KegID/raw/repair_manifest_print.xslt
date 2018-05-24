<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:k="http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects">
    <xsl:output
        method="html"
        indent="yes"
        doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN"
        doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"/>

    <xsl:template match="/">
        <html xmlns="http://www.w3.org/1999/xhtml">
            <head>
                <title>Maintenance Manifest</title>
                <style type="text/css"
                       xmlns="http://www.w3.org/1999/xhtml">
                    html,
                    body
                    {
                    margin:0;
                    padding:0;
                    height:100%;
                    font-size: 10pt;
                    font-family: Trebuchet MS,Verdana,
                    Arial, Helvetica,sans-serif;
                    }
                    #container
                    {
                    min-height:100%;
                    position:relative;
                    }
                    #header
                    {
                    background-color: black;
                    }
                    #body
                    {
                    padding:10px;
                    padding-bottom:48px; /* Height of the footer */
                    }
                    #footer
                    {
                    position:absolute;
                    bottom:0;
                    width:100%;
                    height:48px; /*
                    Height of the footer */
                    background-color: #f5f5f5;
                    color: #222B3A;
                    }

                    .border
                    {
                    border: 1px black solid;
                    }

                    TD.center
                    {
                    height: 100%
                    text-align:
                    center;
                    vertical-align: middle;
                    padding: 4px;
                    }
                    TD.track
                    {
                    text-align:
                    center;
                    vertical-align: middle;
                    padding: 4px;
                    font-size: 10pt;
                    border:
                    1px black solid;
                    }
                    .manifest_line_header
                    {
                    padding: 4px;
                    border: 1px
                    solid black;
                    text-align: center;
                    }
                    .manifest_line_item_cell
                    {
                    border-left: 1px solid black;
                    border-right: 1px solid black;
                    text-align: left;
                    padding: 9px;
                    }
                    .manifest_total
                    {
                    border: 1px solid
                    black;
                    font-size: 12pt;
                    border-left: 1px solid black;
                    border-right:
                    1px solid black;
                    text-align: right;
                    padding: 9px;
                    }
                </style>
            </head>
            <body>
                <div style="width: 7.5in;">
                    <div id="container">
                        <div>
                            <div id="header">
                                <xsl:call-template name="kegidheader"></xsl:call-template>
                            </div>
                            <div id="body">
                                <xsl:apply-templates select="k:Manifest"/>
                            </div>
                            <div id="footer">
                            </div>
                        </div>
                    </div>
                </div>
            </body>
        </html>
    </xsl:template>
    <xsl:template match="k:Manifest">
        <div id="trackingdiv">
            <table xmlns="http://www.w3.org/1999/xhtml"
                   width="100%"
                   cellspacing="0"
                   cellpadding="0">
                <tr>
                    <td></td>
                    <td align="right">
                        <table
                            cellspacing="0"
                            cellpadding="6">
                            <tr>
                                <td
                                    class="track"
                                    style="width: 1in;">
                                    Ship Date
                                </td>
                                <td
                                    class="track"
                                    style="width: 1in;">
                                    Tracking #
                                </td>
                            </tr>
                            <tr>
                                <td class="track">
                                    <xsl:call-template name="formatintldate">
                                        <xsl:with-param
                                            name="datestr"
                                            select="k:MTypeName"/>
                                    </xsl:call-template>
                                    <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
                                </td>
                                <td class="track">
                                    <xsl:value-of select="k:TrackingNumber"/>
                                    <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <!-- maint items -->
                <tr>
                    <td></td>
                    <td align="right">
                        <table
                            width="100%"
                            cellspacing="0"
                            cellpadding="6"  style="width: 100%; 1px black solid">
                            <thead style="background-color: #E0E0E0">
                            <tr>
                                <th
                                    class="track"
                                    align="center"
                                    style="width: 2in;">
                                    Maintenance performed
                                </th>
                            </tr>
                            </thead>
                            <xsl:for-each select="k:MaintTypes">
                            <tr>
                                <td class="track" align="center">
                                    <xsl:value-of select="k:."/>
                                    <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
                                </td>
                            </tr>
                            </xsl:for-each>
                        </table>
                    </td>
                </tr>
                <!-- maint items end -->
            </table>
        </div>
        <div style="height: .13in; width: 100%">
            <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
        </div>
        <br/>
        <div id="itemsdiv">
            <table
                style="width: 100%; 1px black solid"
                cellpadding="0"
                cellspacing="0"
                border="0">
                <thead style="background-color: #E0E0E0">
                    <td class="manifest_line_header">
                        Size
                    </td>
                    <td class="manifest_line_header">
                        Quantity
                    </td>
                </thead>
                <tbody>
                    <xsl:apply-templates select="k:ManifestItems"/>
                </tbody>
                <tfoot>
                    <tr>
                        <td
                            class="manifest_total"
                            style="font-weight: bold;">
                            Total Items
                        </td>
                        <td class="manifest_total">
                            <xsl:value-of select="count(k:ManifestItems/k:ManifestItem)"/>
                            <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
        <br/>
        <br/>
    </xsl:template>
    <xsl:template match="k:ManifestItems">
        <xsl:for-each select="k:ManifestItem">
            <xsl:sort select="concat(k:Pallet/k:Barcode, k:Keg/k:Barcode)"/>
            <tr>
                <td class="manifest_line_item_cell">
                   <xsl:value-of select="k:Size"/>
                    <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
                </td>
                <td class="manifest_line_item_cell">
                    <xsl:value-of select="k:Quantity"/>
                    <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
                </td>
            </tr>
        </xsl:for-each>
    </xsl:template>
    <xsl:template name="ack_indicator">
        <xsl:param name="submitteddate"></xsl:param>
        <xsl:param name="boolvalue"></xsl:param>
        <xsl:choose>
            <xsl:when test="$boolvalue='true'">
                <span xmlns="http://www.w3.org/1999/xhtml"
                      class="check_true">&#x2714;
                </span>
            </xsl:when>
            <xsl:otherwise>
                <span xmlns="http://www.w3.org/1999/xhtml"
                      class="check_true">
                </span>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

    <xsl:template name="kegidheader">
        <div xmlns="http://www.w3.org/1999/xhtml"
             id="kegidheader"
             style=" background-color: #4b6c9e; padding: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td align="center">
                        <h2>Maintenance Manifest</h2>
                    </td>
                </tr>
            </table>
        </div>
    </xsl:template>
</xsl:stylesheet>
