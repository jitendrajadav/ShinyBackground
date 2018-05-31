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
                                 src="http://s3.amazonaws.com/arapps_images/kegid_logo_white_150.png"
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
