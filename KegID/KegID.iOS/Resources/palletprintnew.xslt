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
                <title>Pallet</title>
                <style type="text/css"
                       media="print"
                       xmlns="http://www.w3.org/1999/xhtml">
                    @page
                    {
                    size: landscape;
                    margin: .25in;
                    }
                </style>
                <style type="text/css"
                       xmlns="http://www.w3.org/1999/xhtml">
                    html,
                    body
                    {
                    font-family: Calibri;
                    font-size: 12pt;
                    margin:0;
                    padding:0;
                    height:100%;
                    width: 10in;
                    height: 7.5in
                    }
                    #container {
                    min-height:100%;
                    height:100%;
                    position:relative;
                    }
                    #header {
                    padding:10px;
                    }
                    #body {
                    padding:10px;
                    padding-bottom:60px; /* Height of the footer */
                    }
                    #footer {
                    position:absolute;
                    bottom:0;
                    width:100%;
                    height:60px; /* Height of the footer */
                    }
                    table
                    {
                    border-spacing: 0px;
                    border-collapse: collapse;
                    }

                    td
                    {
                    vertical-align: top;
                    padding: 2px;
                    }

                    .companycontainer
                    {
                    vertical-align: middle;
                    width: 100%;
                    }

                    #companyname
                    {
                    width: 100%;
                    font-size: 28pt;
                    vertical-align: top;
                    font-weight: bold;
                    border-bottom: solid silver 2px;
                    }

                    #companyaddress
                    {
                    font-size: 14pt;
                    }

                    .stockcontainer
                    {
                    padding-left: 48px;
                    padding-right: 48px;
                    }

                    .stockblock
                    {
                    border-collapse: collapse;
                    border: 3px solid silver;
                    width: 100%;
                    }
                    .stockblock TD
                    {
                    border-top: 3px solid silver;
                    }

                    .biglabel
                    {
                    font-size: 14pt;
                    }
                    .stockname
                    {
                    font-size: 24pt;
                    }
                    .partnertypename
                    {
                    font-size: 18pt;
                    }


                    .totallabel
                    {
                    font-weight: bold;
                    font-size: 22pt;
                    vertical-align: middle;
                    }

                    .totalcell
                    {
                    font-weight: bold;
                    text-align: right;
                    font-size: 60pt;
                    vertical-align: middle;
                    }


                    .summaryheader
                    {
                    font-size: 12pt;
                    font-weight: bold;
                    text-align: left;
                    }
                    .summarycontainer
                    {
                    padding-left: 48px;
                    padding-right: 48px;
                    }
                    .summarygrid
                    {
                    width: 100%;
                    }
                    .summarygrid TD
                    {
                    border: 1px gray solid;
                    border-color: lightgray;
                    }
                    .summarygrid TH
                    {
                    background-color: lightgray;
                    border: 1px gray solid;
                    border-color: lightgray;
                    text-align: left;
                    }
                    .sizecell
                    {
                    text-align: left;
                    }
                    .contentscell
                    {
                    text-align: left;
                    }
                    .quantitycell
                    {
                    text-align: right;
                    }


                    .kegscontainer
                    {
                    padding-left: 32px;
                    padding-right: 32px;
                    }
                    .kegsgrid
                    {
                    width: 100%;
                    }

                    .kegcell
                    {
                    padding: 12px;
                    font-size: 12pt;
                    border: 1px gray solid;
                    border-color: gray;
                    }

                    #palletidtext
                    {
                    font-size: 32pt;
                    font-weight: bold;
                    }

                    #builddatetext
                    {
                    font-size: 16pt;
                    }


                </style>
            </head>
            <body>
                <div id="container">
                    <div id="header">
                    </div>
                    <div id="body">
                        <xsl:apply-templates select="k:Pallet"></xsl:apply-templates>
                    </div>
                    <div id="footer">
                        <xsl:call-template name="footertemplate"></xsl:call-template>
                    </div>
                </div>
            </body>
        </html>
    </xsl:template>

    <xsl:template match="k:Pallet">
        <div id="main"
             xmlns="http://www.w3.org/1999/xhtml">
            <table style="width: 100%">
                <tr>
                    <td style="vertical-align: top">
                        <div class="companycontainer">
                            <div id="companyname">
                                <xsl:value-of select="k:Owner/k:FullName"/>
                                <xsl:call-template name="space"/>
                            </div>
                            <div id="companyaddress">
                                <xsl:value-of select="k:Owner/k:Address"/>
                            </div>
                        </div>
                        <br/>
                        <div class="stockcontainer">
                            <table class="stockblock">
                                <tr id="stockrow">
                                    <td colspan="2">
                                        <span class="biglabel">Stock Location</span>
                                        <br/>
                                        <span class="stockname">
                                            <xsl:value-of select="k:StockLocation/k:FullName"/>
                                        </span>
                                        <br/>
                                        <span class="partnertypename">
                                            <xsl:value-of select="k:StockLocation/k:PartnerTypeName"/>
                                        </span>
                                    </td>
                                </tr>
                                <tr id='totalrow'>
                                    <td class="totallabel">Kegs:</td>
                                    <td class="totalcell">
                                        <xsl:value-of select='count(k:PalletItems/k:PalletItem)'/>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan='2'>
                                        <div id="dateinfo">
                                            <span id="builddatetext">

                                                <xsl:text disable-output-escaping="yes">
                                                </xsl:text>
                                                <!--<xsl:call-template name="formatdatetime">
                                                    <xsl:with-param
                                                        name="datestr"
                                                        select="k:CreatedDate[1]/node()[2]/node()[1]"></xsl:with-param>
                                                </xsl:call-template>-->
                                              <xsl:value-of select="k:CreatedDate"/>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td>
                        <div style="text-align: center">
                            <span id="palletidtext">
                                <xsl:value-of select="substring(k:Barcode, 9, 12) "/>
                            </span>
                            <div id="palletbarcode">
                                <br/>
                                <img>
                                    <xsl:attribute name="src">
                                        http://barcodes4.me/barcode/qr/
                                        <xsl:value-of select="k:Barcode"/>.png?value=
                                        <xsl:value-of select="k:Barcode"/>&amp;size=6&amp;eccLevel=3
                                    </xsl:attribute>
                                </img>
                            </div>
                            <br/>
                            <div id="palletcode128">
                                <img>
                                    <xsl:attribute name="src">
                                        http://barcodes4.me/barcode/c128a/
                                        <xsl:value-of select="k:Barcode"/>.png?width=350&amp;height=100&amp;IsTextDrawn=1
                                    </xsl:attribute>
                                </img>
                            </div>
                            <br/>
                        </div>
                    </td>
                </tr>
            </table>
            <hr/>
            <table width="100%">
                <tr>
                    <td style="width:40%">
                        <div style="text-align: center">
                            <span class="summaryheader">Summary</span>
                        </div>
                        <div class="summarycontainer">
                            <table class="summarygrid">
                                <thead>
                                    <tr>
                                        <th>Size</th>
                                        <th>Brand</th>
                                        <th>Qty</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <xsl:apply-templates
                                        select="k:PalletItems"
                                        mode="summary">
                                    </xsl:apply-templates>
                                </tbody>
                                <tfoot>
                                    <tr>
                                    </tr>
                                </tfoot>
                            </table>
                        </div>
                    </td>
                    <td style="width:60%">
                        <div style="text-align: center">
                            <span class="summaryheader">Packing list</span>
                        </div>
                        <div class="kegscontainer">
                            <table class="kegsgrid">
                                <thead></thead>
                                <tbody>
                                    <xsl:apply-templates
                                        select="k:PalletItems"
                                        mode="list"></xsl:apply-templates>
                                </tbody>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </xsl:template>

    <xsl:key
        name="summarykey"
        match="k:PalletItem"
        use="concat(k:Keg/k:SizeName, '|', k:Contents)"
        />

    <xsl:template
        match="k:PalletItems"
        mode="summary">
        <xsl:for-each select="k:PalletItem[count(. | key('summarykey', concat(k:Keg/k:SizeName, '|', k:Contents))[1]) = 1]">
            <xsl:sort select="concat(k:Keg/k:SizeName, '|', k:Contents)"/>
            <xsl:variable
                name="sizename"
                select="k:Keg/k:SizeName"/>
            <xsl:variable
                name="contents"
                select="k:Contents"/>
            <tr xmlns="http://www.w3.org/1999/xhtml"
                class="summaryrow">
                <td class="sizecell">
                    <xsl:value-of select="$sizename"/>
                    <xsl:call-template name="space"/>
                </td>
                <td class="contentscell">
                    <xsl:value-of select="$contents"/>
                    <xsl:call-template name="space"/>
                </td>
                <td class="quantitycell">
                    <xsl:call-template name="space"/>
                    <xsl:value-of select="count(//k:PalletItem[(k:Contents = $contents) and (k:Keg/k:SizeName = $sizename)])"/>
                </td>
            </tr>
        </xsl:for-each>
    </xsl:template>

    <xsl:variable
        name="group"
        select="4"/>

    <xsl:template
        match="k:PalletItems"
        mode="list">
        <xsl:apply-templates
            select="k:PalletItem[position() mod $group = 1]"
            mode="row"/>
    </xsl:template>

    <xsl:template
        match="k:PalletItem"
        mode="row">
        <tr xmlns="http://www.w3.org/1999/xhtml"
            class="kegrow">
            <xsl:apply-templates
                select=".|following-sibling::k:PalletItem[position() &lt; $group]"
                mode="cell"/>
        </tr>
    </xsl:template>

    <xsl:template
        match="k:PalletItem"
        mode="cell">
        <td xmlns="http://www.w3.org/1999/xhtml"
            class="kegcell">
            <xsl:value-of select="k:Keg/k:Barcode"/>
        </td>
    </xsl:template>

    <xsl:template name="formatdatetime">
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
        <xsl:variable name="hh">
            <xsl:value-of select="substring($datestr,12,2)"/>
        </xsl:variable>
        <xsl:variable name="mm">
            <xsl:value-of select="substring($datestr,15,2)"/>
        </xsl:variable>
        <xsl:variable name="ss">
            <xsl:value-of select="substring($datestr,18,2)"/>
        </xsl:variable>

        <xsl:value-of select="$dd"/>
        <xsl:text disable-output-escaping="yes">&amp;</xsl:text>
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
        <xsl:text disable-output-escaping="yes"></xsl:text>
        <xsl:value-of select="$yyyy"/>
        <xsl:text disable-output-escaping="yes"></xsl:text>
        <xsl:value-of select="$hh"/>:
        <xsl:value-of select="$mm"/>
        <xsl:text disable-output-escaping="yes"></xsl:text>
    </xsl:template>

    <xsl:template name="space">
        <xsl:text disable-output-escaping="yes"></xsl:text>
    </xsl:template>

    <xsl:template name="footertemplate">
        <div
            id="footer"
            style="margin-bottom: 0px; height: auto; padding: 10px; text-align: right; vertical-align: bottom;">
            <span
                style="vertical-align: middle; font-family: Consolas; font-size: 12pt; font-variant: small-caps;">
                Powered By
            </span>
            <a
                href="kegid.com"
                style="">
                <img xmlns="http://www.w3.org/1999/xhtml"
                     src="http://s3.amazonaws.com/arapps_images/kegid_logo_noshadow_82x36.png"
                     style="border: none; vertical-align: middle"
                     alt="kegid logo"
                     height="36px"/>
            </a>
        </div>
    </xsl:template>

</xsl:stylesheet>
