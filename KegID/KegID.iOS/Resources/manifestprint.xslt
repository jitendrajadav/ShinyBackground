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
                                <td
                                   class="track"
                                   style="width: 1in;">
                                Order #
                              </td>
                            </tr>
                            <tr>
                                <td class="track">
                                    <!--<xsl:call-template name="formatintldate">
                                        <xsl:with-param
                                            name="datestr"
                                            select="k:ShipDate"/>
                                    </xsl:call-template>
                                    <xsl:text disable-output-escaping="yes"></xsl:text>-->
                                    <xsl:value-of select="k:ShipDate"/>
                                    <xsl:text disable-output-escaping="yes"></xsl:text>
                                </td>
                                <td class="track">
                                    <xsl:value-of select="k:TrackingNumber"/>
                                    <xsl:text disable-output-escaping="yes"></xsl:text>
                                </td>
                              <td class="track">
                                <xsl:value-of select="k:Order"/>
                                <xsl:text disable-output-escaping="yes"></xsl:text>
                              </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div style="height: .13in; width: 100%">
            <xsl:text disable-output-escaping="yes"></xsl:text>
        </div>
        <div
            id="divPartners"
            style="margin-left: .5in;">
            <table
                style="width: 100%"
                cellpadding="0"
                cellspacing="0">
                <tr>
                    <td
                        style="border: 1px solid black; height: .25in; width: 3in; vertical-align: middle; padding-left: .13in;">
                        Origin
                    </td>
                    <td>
                        <xsl:text disable-output-escaping="yes"></xsl:text>
                    </td>
                    <td
                        style="border: 1px solid black; height: .25in; width: 3in; vertical-align: middle; padding-left: .13in;">
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
                        <xsl:text disable-output-escaping="yes"></xsl:text>
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
                            <xsl:text disable-output-escaping="yes"></xsl:text>
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
            <!--<tr xmlns="http://www.w3.org/1999/xhtml"> <td colspan="3" class="manifest_line_item_cell"
                style="border: 1px black solid"> <small> <xsl:value-of select="k:Pallet/k:Barcode"
                /> </small> </td> </tr> -->
            <tr>
                <td class="manifest_line_item_cell">
                    <b>
                        <xsl:value-of select="k:Keg/k:Barcode"/>
                    </b>
                    <xsl:text disable-output-escaping="yes"></xsl:text>
                    <br xmlns="http://www.w3.org/1999/xhtml"/>
                    <small>
                        <xsl:value-of select="k:Keg/k:OwnerName"/>
                    </small>
                    <xsl:if test="string-length(k:Pallet/k:Barcode) &gt; 0">
                        <br xmlns="http://www.w3.org/1999/xhtml"/>
                        <small>
                            On Pallet
                            <xsl:text disable-output-escaping="yes"></xsl:text>
                            <xsl:value-of select="k:Pallet/k:Barcode"/>
                        </small>
                    </xsl:if>
                </td>
                <td class="manifest_line_item_cell">
                    <xsl:value-of select="k:Keg/k:TypeName"/>
                    <xsl:text disable-output-escaping="yes">
                    </xsl:text>
                    <xsl:value-of select="k:Keg/k:SizeName"/>
                </td>
                <td class="manifest_line_item_cell">
                    <xsl:value-of select="k:Contents"/>
                    <xsl:text disable-output-escaping="yes"></xsl:text>
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
                <xsl:choose>
                    <xsl:when test="string-length($submitteddate) > 0">
                        <span xmlns="http://www.w3.org/1999/xhtml"
                              class="check_false">&#x2717;
                        </span>
                    </xsl:when>
                    <xsl:otherwise>
                        <span xmlns="http://www.w3.org/1999/xhtml"
                              class="check_neutral">&#x2610;
                        </span>
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
                <xsl:text disable-output-escaping="yes"></xsl:text>
                <xsl:value-of select="$state"/>
                <xsl:text disable-output-escaping="yes"></xsl:text>
                <xsl:value-of select="$postal"/>
                <br xmlns="http://www.w3.org/1999/xhtml"/>
                <xsl:value-of select="$country"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

    <!--<xsl:template name="formatpopdate"> <xsl:param name="datestr"></xsl:param>
        <xsl:variable name="dt" as="xs:dateTime" select="xs:dateTime('2012-10-21T22:10:15')"/>
        <xsl:value-of select="format-dateTime($dt, '[Y0001]/[M01]/[D01]')"/> </xsl:template> -->

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
        <xsl:text disable-output-escaping="yes"></xsl:text>
        <xsl:choose>
            <xsl:when test="$MM=1">
                Jan
            </xsl:when>
            <xsl:when test="$MM=2">
                Feb
            </xsl:when>
            <xsl:when test="$MM=3">
                Mar
            </xsl:when>
            <xsl:when test="$MM=4">
                Apr
            </xsl:when>
            <xsl:when test="$MM=5">
                May
            </xsl:when>
            <xsl:when test="$MM=6">
                Jun
            </xsl:when>
            <xsl:when test="$MM=7">
                Jul
            </xsl:when>
            <xsl:when test="$MM=8">
                Aug
            </xsl:when>
            <xsl:when test="$MM=9">
                Sep
            </xsl:when>
            <xsl:when test="$MM=10">
                Oct
            </xsl:when>
            <xsl:when test="$MM=11">
                Nov
            </xsl:when>
            <xsl:when test="$MM=12">
                Dec
            </xsl:when>
        </xsl:choose>
        <xsl:text disable-output-escaping="yes"></xsl:text>
        <xsl:value-of select="$yyyy"/>
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
