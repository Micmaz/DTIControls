<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="DTIMapControlTester._Default" %>
<%@ Register Assembly="DTIMapControl" Namespace="DTIMapControl" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script type="text/javascript">
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div>
    <asp:CheckBox runat="server" ID="cbEdit" AutoPostBack="true" Text="Edit" /><br /><br />
    <cc2:DTIMapServerControl runat="server" ID="mapcontrol1" AddressTitle="Digital Tadpole" width="300px">112-B Pheasant Wood Ct., Morrisville, NC 27560</cc2:DTIMapServerControl>
    
    <cc2:DTIMapServerControl runat="server" ID="mapcontrol2" AddressTitle="Da Crib">1233 Creekwatch Ln., Cary, NC 27513</cc2:DTIMapServerControl>
    
    </div>
    </form>
</body>
</html>
