<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TabSliderTest.aspx.vb" Inherits="MiniTests.TabSliderTest" %>
<%@ Register Assembly="DTIMiniControls" Namespace="DTIMiniControls" TagPrefix="cc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
     <style type="text/css">
      
      .slide-out-div {
          padding: 20px;
          width: 250px;
          background: #ccc;
          border: 1px solid #29216d;
      }      
      </style>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cc2:TabSlider ID="tabslider1" runat="server" Visible="false"
                CssClass="slide-out-div" ImageUrl="imgs/contact_tab.gif" 
                ImageHeight="122px" ImageWidth="40px" TopPosition="10">
             <h3>Contact me</h3>
                <p>Thanks for checking out my jQuery plugin, I hope you find this useful.
                </p>
                <p>This can be a form to submit feedback, or contact info</p>
        </cc2:TabSlider>
        
         <script type="text/javascript">
            function change(t, d){
                try{
                    num = parseInt(document.getElementById('hf1').value);
                    switch(t){
                        case 'i': 
                            num += d;
                            break;
                        case 'd':
                            num -= d;
                            break;
                        default:
                            var s = document.getElementById('Text1').value;
                            if (s.indexOf('px') != -1){
                                s = s.trim();
                                s = s.substring(0, s.length - 2).trim(); 
                            }                       
                            num = parseInt(s);
                            break;
                    }                
                    $('#div1').animate({ height: num }, 800);    
                    document.getElementById('Text1').value = num;
                    document.getElementById('hf1').value = num;
                }catch(err){
                    document.getElementById('Text1').value = "";
                }
            }
            
            String.prototype.trim = function() {
	            return this.replace(/^\s+|\s+$/g,"");
            }
        </script>
        <asp:HiddenField ID="hf1" runat="server" />
        <div id="div1" style="height:25px; background-color:Red; text-align:center;">
            <input id="Text1" type="text" style="width: 50px;" />
            <input id="Button1" type="button" value="-" onclick="change('d',10)" />
            <input id="Button2" type="button" value="+" onclick="change('i',10)" />
            <input id="Button3" type="button" value="Set" onclick="change()" />
        </div>
        <div>
        test
        </div>
    </div>
    </form>
</body>
</html>
