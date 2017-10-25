<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TagsTest.aspx.vb" Inherits="MiniTests.TagsTest" %>
<%@ Register Assembly="DTIMiniControls" Namespace="DTIMiniControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
    .DTICurrentTag{
        margin: 5px;
        float: left;
    }
    .DTICancelButton{
        color: red;
        float: left;
        cursor: pointer;
    }
    .DTIPopularTag{
        margin: 5px;
        color: blue;
        float: left;
        cursor: pointer;
        text-decoration: underline;
    }
    .DTITagText{
        float: left;
    }
    .DTIlblCurrentTags {
        clear: both;
    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table><tr>
        <td><cc1:Tagger ID="Tagger1" runat="server" Height="178px" Width="248px" /></td>
        <td><cc1:Tagger ID="Tagger2" runat="server" Height="178px" Width="248px" /></td>
        </tr></table>
    </div>
    </form>
</body>
</html>
