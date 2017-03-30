<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="FacturaElectronica.Default" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <TITLE>Factura electrónica - Ingram Micro</TITLE>		 
    <META http-equiv="Content-Type" content="text/html; charset=ISO-8859-1">		 
    <META http-equiv="language" content="es">
    <meta http-equiv="X-UA-Compatible" content="IE=7,8,9" />
    <!--<LINK href="css/estructura.css" rel="stylesheet" type="text/css">
    <LINK href="css/print.css" rel="stylesheet" type="text/css" media="print">	-->
         <LINK href="css/estilos.css" rel="stylesheet" type="text/css" >		 
         <!--<script type="text/javascript" src="js/principal.js"></script>-->
         
         <META name="GENERATOR" content="MSHTML 10.00.9200.16750"> 

        
</head>
<body>
    <form id="form1" runat="server">
    <DIV id="container"><!-- fi header -->		 
    <div id="header"><img src="img/header.png"  />   </div>
    <div id="contenido"  class="box"> 
         <p>Formulario de alta para Factura Electrónica.</p>
         <p>Rellena todos los campos.</p>
        <div class="evento">
        <FIELDSET style="border: 0px solid rgb(216, 11, 21); width: 400px; margin-right: 0px;">
        <LEGEND> Datos de la empresa</LEGEND>
		<i>Código Cliente:&nbsp; <asp:Label ID="Label2" AutoPostback="false" runat="server" Text="Label"></asp:Label>
            </i>
        <div style="clear:both;"></div>
        <i>Empresa: &nbsp; </i>
        &nbsp;<asp:Label ID="Label1" AutoPostback="false" runat="server" Text="Label"></asp:Label>
        </FIELDSET>
        </div>
        
      <div class="empresa">
            <FIELDSET style="border: 0px solid rgb(0, 0, 255); width: 450px; margin-right: 0px;">
            
             <div style="clear:both; width: 852px; height: 87px;"><asp:Label ID="Label3" 
                     runat="server" Text="Label"></asp:Label>
                </div>
			
                <SPAN style="color: red;">* </SPAN>Email:
           <asp:TextBox ID="T_email" CssClass="obligatorio" runat="server" AutoPostback="false" size="55" 
                    Width="391px"></asp:TextBox>
    </FIELDSET>
    </div>
    <asp:Button ID="B_enviar" runat="server" onclick="B_enviar_Click" 
             OnClientClick="return validar()" Text="Aceptar" AutoPostback="false" CssClass="btnLogin"/>
    <FIELDSET style="border: 0px solid rgb(0, 153, 51); width: 720px; margin-right: 0px;"></FIELDSET>
   
</div>
</DIV>
    </form>
</body>
</html>
