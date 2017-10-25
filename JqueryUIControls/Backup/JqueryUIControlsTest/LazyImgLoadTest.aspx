<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LazyImgLoadTest.aspx.vb" Inherits="JqueryUIControlsTest.LazyImgLoadTest" %>

<%@ Register assembly="JqueryUIControls" namespace="JqueryUIControls" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">
      
    </script>
    <div>
        <cc1:LazyImgLoad ID="LazyImgLoad1" Width="450" Height="449" runat="server" ImageUrl="/imgs/cat.jpg" />
        <cc1:LazyImgLoad ID="LazyImgLoad2" Width="800" Height="500" runat="server" ImageUrl="/imgs/crater.jpg" />
        <cc1:LazyImgLoad ID="LazyImgLoad3" Width="800" Height="500" runat="server" ImageUrl="/imgs/rock.jpg"/>
        <cc1:LazyImgLoad ID="LazyImgLoad4" Width="768" Height="512" runat="server" ImageUrl="/imgs/space.jpg" Effect=fadeIn EffectDuration=1000/> 
    </div>
    </form>
</body>
</html>
