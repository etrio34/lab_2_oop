<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="html" encoding="utf-8"/>

  <xsl:template match="newspaper">
    <HTML>
      <HEAD>
        <TITLE>Мережна газета факультету</TITLE>
      </HEAD>
      <BODY>
        <H2>Мережна газета факультету</H2>
        <TABLE BORDER="2">
          <TR>
            <TD><b>Назва</b></TD>
            <TD><b>Автор</b></TD>
            <TD><b>Вид</b></TD>
            <TD><b>Анотація</b></TD>
            <TD><b>Файл</b></TD>
            <TD><b>Дата</b></TD>
            <TD><b>Відгуки читачів</b></TD>
          </TR>
          <xsl:apply-templates select="article"/>
        </TABLE>
      </BODY>
    </HTML>
  </xsl:template>

  <xsl:template match="article">
    <TR>
      <TD><b><xsl:value-of select="@Title"/></b></TD>
      <TD><xsl:value-of select="@Author"/></TD>
      <TD><xsl:value-of select="@Type"/></TD>
      <TD><xsl:value-of select="@Annotation"/></TD>
      <TD><xsl:value-of select="@FilePath"/></TD>
      <TD><xsl:value-of select="@Date"/></TD>
      <TD>
        <xsl:for-each select="reader">
          <p>
            <xsl:value-of select="@Name"/>: <xsl:value-of select="@Review"/>
          </p>
        </xsl:for-each>
      </TD>
    </TR>
  </xsl:template>
</xsl:stylesheet>
