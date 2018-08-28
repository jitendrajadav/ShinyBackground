<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="2.0"
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
                <title>KegID Manifest</title>
                <style type="text/css"
                       xmlns="http://www.w3.org/1999/xhtml">
                    html,
                    body
                    {
                    margin:0;
                    padding:0;
                    height:100%;
                    font-size: 10pt;
                    font-family: Trebuchet MS,Verdana, Arial, Helvetica,sans-serif;
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
                    height:48px; /* Height of the footer */
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
                    text-align: center;
                    vertical-align: middle;
                    padding: 4px;
                    }
                    TD.track
                    {
                    text-align: center;
                    vertical-align: middle;
                    padding: 4px;
                    font-size: 10pt;
                    border: 1px black solid;

                    }
                    .manifest_line_header
                    {
                    padding: 4px;
                    border: 1px solid black;
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
                    border: 1px solid black;
                    font-size: 12pt;
                    border-left: 1px solid black;
                    border-right: 1px solid black;
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
                                <xsl:call-template name="kegstarheader"></xsl:call-template>
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
                                            select="k:ShipDate"/>
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
            </table>
        </div>
        <div style="height: .13in; width: 100%">
            <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
        </div>
        <div
            id="divPartners"
            style="margin-left: .5in;">
            <table
                style="width: 100%"
                cellpadding="0"
                cellspacing="0">
                <tr>
                    <td style="border: 1px solid black; height: .25in; width: 3in; vertical-align: middle; padding-left: .13in;">
                        Origin
                    </td>
                    <td>
                        <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
                    </td>
                    <td style="border: 1px solid black; height: .25in; width: 3in; vertical-align: middle; padding-left: .13in;">
                        Destination
                    </td>
                </tr>
                <tr>
                    <td
                        style="border: 1px solid black;padding: 10px"
                        class="address_block">
                        <xsl:value-of select="k:SenderPartner/k:FullName"/>
                        <br/>
                        <xsl:call-template name="addressblock">
                            <xsl:with-param
                                name="addr1"
                                select="k:SenderShipAddress/k:Line1"></xsl:with-param>
                            <xsl:with-param
                                name="addr2"
                                select="k:SenderShipAddress/k:Line2"></xsl:with-param>
                            <xsl:with-param
                                name="addr3"
                                select="k:SenderShipAddress/k:Line3"></xsl:with-param>
                            <xsl:with-param
                                name="addr4"
                                select="k:SenderShipAddress/k:Line4"></xsl:with-param>
                            <xsl:with-param
                                name="addr5"
                                select="k:SenderShipAddress/k:Line5"></xsl:with-param>
                            <xsl:with-param
                                name="city"
                                select="k:SenderShipAddress/k:City"></xsl:with-param>
                            <xsl:with-param
                                name="state"
                                select="k:SenderShipAddress/k:State"></xsl:with-param>
                            <xsl:with-param
                                name="postal"
                                select="k:SenderShipAddress/k:PostalCode"></xsl:with-param>
                            <xsl:with-param
                                name="country"
                                select="k:SenderShipAddress/k:Country"></xsl:with-param>
                        </xsl:call-template>
                    </td>
                    <td>
                        <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
                    </td>
                    <td
                        style="border: 1px solid black;padding: 10px"
                        class="address_block">
                        <xsl:value-of select="k:ReceiverPartner/k:FullName"/>
                        <br/>
                        <xsl:call-template name="addressblock">
                            <xsl:with-param
                                name="addr1"
                                select="k:ReceiverShipAddress/k:Line1"></xsl:with-param>
                            <xsl:with-param
                                name="addr2"
                                select="k:ReceiverShipAddress/k:Line2"></xsl:with-param>
                            <xsl:with-param
                                name="addr3"
                                select="k:ReceiverShipAddress/k:Line3"></xsl:with-param>
                            <xsl:with-param
                                name="addr4"
                                select="k:ReceiverShipAddress/k:Line4"></xsl:with-param>
                            <xsl:with-param
                                name="addr5"
                                select="k:ReceiverShipAddress/k:Line5"></xsl:with-param>
                            <xsl:with-param
                                name="city"
                                select="k:ReceiverShipAddress/k:City"></xsl:with-param>
                            <xsl:with-param
                                name="state"
                                select="k:ReceiverShipAddress/k:State"></xsl:with-param>
                            <xsl:with-param
                                name="postal"
                                select="k:ReceiverShipAddress/k:PostalCode"></xsl:with-param>
                            <xsl:with-param
                                name="country"
                                select="k:ReceiverShipAddress/k:Country"></xsl:with-param>
                        </xsl:call-template>
                    </td>
                </tr>
            </table>
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
                        Barcode
                    </td>
                    <td class="manifest_line_header">
                        Item
                    </td>
                    <td class="manifest_line_header">
                        Brand
                    </td>
                </thead>
                <tbody>
                    <xsl:apply-templates select="k:ManifestItems"/>
                </tbody>
                <tfoot>
                    <tr>
                        <td
                            colspan="2"
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
            <xsl:sort select="k:Keg/k:Barcode"/>
            <tr>
                <td class="manifest_line_item_cell">
                    <xsl:value-of select="k:Keg/k:Barcode"/>
                    <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
                    <br xmlns="http://www.w3.org/1999/xhtml"/>
                    <small>
                        <xsl:value-of select="k:Keg/k:OwnerName"/>
                    </small>
                </td>
                <td class="manifest_line_item_cell">
                    <xsl:value-of select="k:Keg/k:TypeName"/>
                    <xsl:text disable-output-escaping="yes">&amp;nbsp;&amp;mdash;&amp;nbsp;
                    </xsl:text>
                    <xsl:value-of select="k:Keg/k:SizeName"/>
                </td>
                <td class="manifest_line_item_cell">
                    <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
                    <xsl:value-of select="k:Contents"/>
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
                      class="check_true">&#x2714;</span>
            </xsl:when>
            <xsl:otherwise>
                <xsl:choose>
                    <xsl:when test="string-length($submitteddate) > 0">
                        <span xmlns="http://www.w3.org/1999/xhtml"
                              class="check_false">&#x2717;</span>
                    </xsl:when>
                    <xsl:otherwise>
                        <span xmlns="http://www.w3.org/1999/xhtml"
                              class="check_neutral">&#x2610;</span>
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

    <xsl:template name="addressblock">
        <xsl:param name="addr1"></xsl:param>
        <xsl:param name="addr2"></xsl:param>
        <xsl:param name="addr3"></xsl:param>
        <xsl:param name="addr4"></xsl:param>
        <xsl:param name="addr5"></xsl:param>
        <xsl:param name="city"></xsl:param>
        <xsl:param name="state"></xsl:param>
        <xsl:param name="postal"></xsl:param>
        <xsl:param name="country"></xsl:param>
        <xsl:if test="string-length($addr1) > 0">
            <xsl:value-of select="$addr1"/>
            <br xmlns="http://www.w3.org/1999/xhtml"/>
        </xsl:if>
        <xsl:if test="string-length($addr2) > 0">
            <xsl:value-of select="$addr2"/>
            <br xmlns="http://www.w3.org/1999/xhtml"/>
        </xsl:if>
        <xsl:if test="string-length($addr3) > 0">
            <xsl:value-of select="$addr3"/>
            <br xmlns="http://www.w3.org/1999/xhtml"/>
        </xsl:if>
        <xsl:if test="string-length($addr4) > 0">
            <xsl:value-of select="$addr4"/>
            <br xmlns="http://www.w3.org/1999/xhtml"/>
            <xsl:if test="string-length($addr5) > 0">
                <xsl:value-of select="$addr5"/>
                <br xmlns="http://www.w3.org/1999/xhtml"/>
            </xsl:if>
        </xsl:if>
        <xsl:if test="string-length($addr5) > 0">
            <xsl:value-of select="$addr5"/>
            <br xmlns="http://www.w3.org/1999/xhtml"/>
        </xsl:if>
        <xsl:choose>
            <xsl:when test="string-length($addr4) > 0">
                <xsl:value-of select="$addr4"/>
                <br xmlns="http://www.w3.org/1999/xhtml"/>
                <xsl:if test="string-length($addr5) > 0">
                    <xsl:value-of select="$addr5"/>
                    <br xmlns="http://www.w3.org/1999/xhtml"/>
                </xsl:if>
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="$city"/>
                <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
                <xsl:value-of select="$state"/>
                <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
                <xsl:value-of select="$postal"/>
                <br xmlns="http://www.w3.org/1999/xhtml"/>
                <xsl:value-of select="$country"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

    <xsl:template name="formatpopdate">
        <xsl:param name="datestr"></xsl:param>
        <xsl:variable
            name="dt"
            as="xs:dateTime"
            select="xs:dateTime('2012-10-21T22:10:15')"/>
        <xsl:value-of select="format-dateTime($dt, '[Y0001]/[M01]/[D01]')"/>
    </xsl:template>

    <xsl:template name="formatdate">
        <xsl:param name="datestr"/>
        <!-- input format 'yyyy-MM-ddThh:mm:ss' -->
        <!-- output format mm/dd/yyyy -->
        <xsl:variable name="dd">
            <xsl:value-of select="substring($datestr,9,2)"/>
        </xsl:variable>
        <xsl:variable name="MM">
            <xsl:value-of select="substring($datestr,6,2)"/>
        </xsl:variable>
        <xsl:variable name="yyyy">
            <xsl:value-of select="substring($datestr,1,4)"/>
        </xsl:variable>
        <xsl:value-of select="$MM"/>
        <xsl:value-of select="'/'"/>
        <xsl:value-of select="$dd"/>
        <xsl:value-of select="'/'"/>
        <xsl:value-of select="$yyyy"/>
    </xsl:template>
    <xsl:template name="formatintldate">
        <xsl:param name="datestr"/>
        <!-- input format 'yyyy-MM-ddThh:mm:ss' -->
        <!-- output format dd MMM yyyy -->
        <xsl:variable name="dd">
            <xsl:value-of select="substring($datestr,9,2)"/>
        </xsl:variable>
        <xsl:variable name="MM">
            <xsl:value-of select="substring($datestr,6,2)"/>
        </xsl:variable>
        <xsl:variable name="yyyy">
            <xsl:value-of select="substring($datestr,1,4)"/>
        </xsl:variable>

        <xsl:value-of select="$dd"/>
        <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
        <xsl:choose>
            <xsl:when test="$MM=1">Jan</xsl:when>
            <xsl:when test="$MM=2">Feb</xsl:when>
            <xsl:when test="$MM=3">Mar</xsl:when>
            <xsl:when test="$MM=4">Apr</xsl:when>
            <xsl:when test="$MM=5">May</xsl:when>
            <xsl:when test="$MM=6">Jun</xsl:when>
            <xsl:when test="$MM=7">Jul</xsl:when>
            <xsl:when test="$MM=8">Aug</xsl:when>
            <xsl:when test="$MM=9">Sep</xsl:when>
            <xsl:when test="$MM=10">Oct</xsl:when>
            <xsl:when test="$MM=11">Nov</xsl:when>
            <xsl:when test="$MM=12">Dec</xsl:when>
        </xsl:choose>
        <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
        <xsl:value-of select="$yyyy"/>
    </xsl:template>
    <xsl:template name="kegstarheader">
        <div xmlns="http://www.w3.org/1999/xhtml"
             id="kegstarheader"
             style="background-color: black; background: black; padding: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td>
                        <a href="http://www.kegstar.com">
                            <img xmlns="http://www.w3.org/1999/xhtml"
                                 src="http://s3.amazonaws.com/arapps_images/kegstar_logo.png"
                                 style="border:none; vertical-align: middle; height: 64px"

                                 alt="kegstar logo"/>
                        </a>
                    </td>
                    <td style="text-align: right; color: white; ">
                        <h2>Manifest</h2>
                        <span style="font-size: 9pt; font-variant: small-caps">Powered By</span>
                        <a href="http://www.kegstar.com">
                            <img xmlns="http://www.w3.org/1999/xhtml"
                                 src="http://s3.amazonaws.com/arapps_images/kegid_logo_white_150.png"
                                 style="border:none; vertical-align: middle; height:36px"

                                 alt="kegid logo"/>
                        </a>
                    </td>
                </tr>
            </table>
        </div>
    </xsl:template>
    <xsl:template name="kegidheader">
        <div xmlns="http://www.w3.org/1999/xhtml"
             id="kegidheader"
             style=" background-color: #4b6c9e; padding: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td>
                        <a href="kegid.com">
                            <img xmlns="http://www.w3.org/1999/xhtml"
                                 src="data:image/png;base64,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"
                                 style="border:none; vertical-align: middle; height: 64px"

                                 alt="kegid logo"/>
                        </a>
                    </td>
                    <td style="text-align: right; color: white; ">
                        <h2>Manifest</h2>
                    </td>
                </tr>
            </table>
        </div>
    </xsl:template>
</xsl:stylesheet>
