<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="VideoPreview.ascx.vb" Inherits="DTIVideoManager.VideoPreview" %>
<%@ Register Assembly="DTIControls" Namespace="DTIVideoManager" TagPrefix="cc2" %>
<%@ Register Assembly="DTIControls" Namespace="DTIMiniControls" TagPrefix="cc2" %>
<%@ Register Assembly="DTIControls" Namespace="DTIAjax" TagPrefix="cc1" %>
<cc1:jsonSeverConrol jsCompleteFunc="done" ajaxReturn="html" ID="JsonSeverConrol1" runat="server" workerclass="DTIVideoManager.VideoPreviewHelper" >
</cc1:jsonSeverConrol>
<script type="text/javascript">
    $(function () {
        $("#<%=videoSlider.ClientId %>").slider({max: <%=StepAndMax %>, value: <%=SliderValue %>, change: 
            function(event, ui) {
                ajax<%=JsonSeverConrol1.ClientId %>("resetThumbnail",{vid_id:'<%=myVideoRow.Id %>',sec_mark: ui.value },function() { 
                    var src = $('#<%=videoHolder.ClientId %> img').attr('src');
                    $('#<%=videoHolder.ClientId %> img').attr('src', src.substring(0, src.indexOf('?') + 1) + $.query.load(src).set('r', Math.floor(Math.random()*101))); 
                    $('#<%=changeThumbNailDiv.ClientId %>').hide();
                });
            }
        });
    });
</script>
<asp:Panel id="checkConversionScript" runat="server"><script type="text/javascript">
    $(document).everyTime("10s", "checkConversion<%=myVideoRow.Id %>", function() {
        ajax<%=JsonSeverConrol1.ClientId %>('checkConversion','{"vid_id":"<%=myVideoRow.Id %>"}',function(isConverted) {
            if(isConverted) {
                $('#<%=imgScreenshot.ClientId %>').hide();
                $('#<%=videoHolder.ClientId %>').show();
                $('#<%=lblStatusValue.ClientId %>').html('Processing Complete').css('font-weight', 'bold');
                $('#<%=imgStatus.ClientId %>').hide();
                $('#<%=pnlScreenshot.ClientId %>').mouseover(function() {  $('#<%=pnlScreenShotEditor.ClientId %>').show();}).mouseout(function() {  $('#<%=pnlScreenShotEditor.ClientId %>').hide();});
                $(document).stopTime("checkConversion<%=myVideoRow.Id %>");
            }
        });
    });
</script></asp:Panel>
<table>
    <tr>
        <td>
            <asp:Label ID="lblRawFile" runat="server" Text="Raw File: "></asp:Label></td>
        <td>
            <asp:Label ID="lblRawFileValue" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblTime" runat="server" Text="Time: "></asp:Label></td>
        <td>
            <asp:Label ID="lblTimeValue" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblDimensions" runat="server" Text="Dimensions: "></asp:Label></td>
        <td>
            <asp:Label ID="lblDimensionsValue" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblStatus" runat="server" Text="Status: "></asp:Label></td>
        <td>
            
            <asp:Label ID="lblStatusValue" runat="server"></asp:Label>
            <asp:Image runat="Server" ID="imgStatus" ImageUrl="~/res/BaseClasses/Scripts.aspx?f=DTIVideoManager/videoStatusIndicator.gif" />
                
        </td>
    </tr>
    <tr>
        <td colspan="2">
            
                    <asp:Panel runat="server" ID="pnlScreenshot" style="width:150px; height:120px;">
                        <asp:Image ID="imgScreenshot" runat="server" />
                        
                        <asp:Panel runat="server" ID="videoHolder">
                            <cc2:VideoThumb ID="VideoThumb1" runat="server"></cc2:VideoThumb>
                        </asp:Panel>
                        
                        <asp:Panel runat="server" ID="pnlScreenShotEditor" style="display:none;">
                            <a href="#" onclick="$('#<%=changeThumbNailDiv.clientID %>').show(); return false;">Change Thumbnail</a>
                        </asp:Panel>
                    </asp:Panel>
                 
                    
            
                <div id="changeThumbNailDiv" runat="Server" style="display:none; height:75px;">
                    <asp:Label runat="server" ID="lblZero" style="font-size:x-small; float:left;">00:00:00</asp:Label>
                    <asp:Label runat="server" ID="lblMax" style="font-size:x-small; float:right;"></asp:Label><br style="clear:both;" />
                    <div runat="server" id="videoSlider"></div>
                </div>
            </td>
    </tr>
    
    
       
    
 </table>
