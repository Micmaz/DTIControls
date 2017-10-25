<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="testPage.aspx.vb" Inherits="JqueryUIControlsTest.testPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script type='text/javascript'>
        $(document).ready(function() {
            makePageAjaxy(".update1 #ddl");
            //makePageAjaxy(); //No effects, replaces entire body each postback. 
    });

    function refreshPage(selectorList, updateViewstate, data) {
        data = data.substring(getTagIndex(data, data.indexOf("<" + "body"))); //Concatinate the search so that the string is not found in this script block
        data = data.substring(data.indexOf('>') + 1, data.indexOf("</" + "body" + ">")); //Concatinate the search so that the string is not found in this script block
        if (updateViewstate) {    //Fix the viewstate and event validation.
            $("#__VIEWSTATE").val($("#__VIEWSTATE", data).val());
            $("#__EVENTVALIDATION").val($("#__EVENTVALIDATION", data).val());
        }
        if (!selectorList) {
            $('body').html(data);
            $('input:checkbox').checkbox();
            $('input:radio').radio();
            $('button, input:submit, input:button, input:reset').button();
            $('input[type=text],input[type=password],textarea,select').textbox();
            makePageAjaxy();
            return;
        }
        var selectors = selectorList.split(" ");
        for (i = 0; i < selectors.length; i++) {
            var selector = selectors[i];
            var endIndex = 0;
            $(selector).each(function() {  //for each item in this selector (Slided in new objects and slides out old ones)
                var old = $(this).clone();
                var newElem = $(selector, data);
                $(this).html(newElem.html());
                newElem = $(this);
                $(this).find('input:checkbox').checkbox();
                $(this).find('input:radio').radio();
                $(this).find('button, input:submit, input:button, input:reset').button();
                $(this).find('input[type=text],input[type=password],textarea,select').textbox();

                if (showAnimation && showAnimation.name == 'none') { } else {
                    $(this).children().each(function() {  //Slide in new objects
                        if ($(this).attr("id") && $(old).find("#" + $(this).attr("id")).length == 0) {
                            //$(this).hide().show("explode", {}, 300);
                            if (!showAnimation) {
                                $(this).hide().slideDown("slow");
                            } else {
                                $(this).hide().show(showAnimation.name, showAnimation.parms, showAnimation.speed);
                                //$(this).hide().show(showAnimation.name, showAnimation.parms, showAnimation.speed);
                            }
                        }
                    });
                    var lastmatch = null;
                    $(old).children().each(function() {  //Slide out old missing objects
                        if ($(this).attr("id"))
                            if ($(newElem).find("#" + $(this).attr("id")).length == 0) {
                            if (lastmatch == null) {
                                $(newElem).prepend($(this));
                            } else { lastmatch.after($(this)) }
                            lastmatch = $(this);
                            //$(this).hide("explode", {}, 300, function() { $(this).remove(); });
                            if (!hideAnimation) {
                                $(this).slideUp("slow", function() { $(this).remove(); });
                            } else {
                                $(this).hide(hideAnimation.name, hideAnimation.parms, hideAnimation.speed, function() { $(this).remove(); });
                                //$(this).hide(hideAnimation.name, hideAnimation.parms, hideAnimation.speed, function() { $(this).remove(); });
                            }
                        } else {
                            lastmatch = $(newElem).find("#" + $(this).attr("id"));
                        }
                    });
                }
            });
            var skipfirst = 0;
            $(data).filter("script").each(function() {
                if (skipfirst)
                    $.globalEval(this.text || this.textContent || this.innerHTML || '');
                skipfirst = 1;
            });
        }
        dataobj = null;
    }        


    </script>    
</head>
<body>
    <form id="form1" runat="server">
<div id="updt">
   <select id="effect" onchange="setAnimation($(this).val());">
<option></option><option>blind</option><option>clip</option><option>drop</option><option>explode</option><option>fade</option><option>fold</option><option>puff</option><option>slide</option>
<option>scale</option>
</select><br />

    
    <asp:Panel CssClass="update1" ID="Panel1" runat="server">
    <br /> Test content <div>here</div>
    <asp:Button ID="Button1" runat="server" Text="Add" />
    
    </asp:Panel>
    <div id="ddl">        
    <asp:DropDownList  AutoPostBack="true" ID="DropDownList1" runat="server"><asp:ListItem Value=""/><asp:ListItem Value="dd1"/><asp:ListItem Value="dd2"/></asp:DropDownList>
       <asp:DropDownList AutoPostBack="true"  Visible="false" ID="DropDownList2" runat="server"></asp:DropDownList>     
</div>
</div>
    </form>
</body>
</html>
