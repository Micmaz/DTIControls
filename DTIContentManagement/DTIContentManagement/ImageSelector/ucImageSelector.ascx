<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucImageSelector.ascx.vb" Inherits="DTIContentManagement.ucImageSelector" %>
<%@ Register Assembly="DTIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>
<script type="text/javascript" src="~/res/BaseClasses/Scripts.aspx?f=HighslideControls/hsinnerframe.js" language="javascript"></script>
<link rel="stylesheet" type="text/css" href="~/res/BaseClasses/Scripts.aspx?f=HighslideControls/highslide.css" />
<script type="text/javascript" language="javascript">//<![CDATA[
    try {


        //a13fe4e6f3a0bf746da103090dd47568
        parent.window.hs.outlineType = 'rounded-white';
        parent.window.hs.graphicsDir = '~/res/BaseClasses/Scripts.aspx?f=HighslideControls/';
        parent.window.hs.showCredits = false;
        parent.window.hs.wrapperClassName = 'draggable-header';
        var found = false;
        for (var i = 0; i < parent.window.hs.slideshows.length; i++) {
            if (parent.window.hs.slideshows[i].slideshowGroup == 'mediaGall') { found = true; break; }
        } if (!found) {
            parent.window.hs.addSlideshow({
                slideshowGroup: 'mediaGall',
                interval: 5000,
                repeat: false,
                useControls: true,
                fixedControls: false,
                overlayOptions: {
                    opacity: .6,
                    position: 'bottom center',
                    hideOnMouseOut: true
                }
            });
        }

        parent.window.hs.onActivate = function () {
            var theForm = parent.window.document.forms[0];
            if (theForm) theForm.appendChild(parent.window.hs.container);
        }
        addLoadEvent(function () { parent.window.iframe = getIframe(); parent.window.hs.updateAnchors(); });
    } catch (err) { }
//]]></script>
<script type="text/javascript" src="~/res/BaseClasses/Scripts.aspx?f=HighslideControls/hsFixedInner.js" language="javascript"></script>


    <script type="text/javascript">
        $(function () {
            setTimeout(function () { $('input[type=radio]').uncheckbox(); $('input[type=checkbox]').uncheckbox(); }, 0);
            //endwaiter(parent);
            $(".roll,.roll2").css("opacity", "0");
            $(".img").hover(
                function () { $(this).find(".roll,.roll2").stop().animate({ opacity: .7 }, "slow"); },
                function () {
                    $(this).find(".roll,.roll2").stop().animate({
                        opacity: 0
                    }, "slow");
                });
            $(".lazyImgLoad").click(function () {
                var id = $(this).parent().find(".roll,.roll2").prop("id");
                id = id.substring(id.indexOf('_') + 1);
                //$('#rb_' + id + ', #cb_' + id).click();
                var inp = $(this).parent().parent().find('.imgselector input');
                $(inp).attr('checked', !$(inp).attr('checked'));                
                cbrbselect('b_' + id);
            });
            $.each($('.video'), function (i, v){
                var id = $(this).find('.roll').prop('id');
                id = id.substring(id.indexOf('_') + 1);
                if (id.indexOf("V") == 0)
                    loadVimeoThumb(id.substr(1));
                else
                    loadYoutubeText(id.substr(1));
            });
            $('#switchLink').click(function (e) {
                e.preventDefault();
                switchLink();
            });
        });

        function cbrbselect(id){
            var imageid = '';
            var name = '';
            var re = /,$/;
            $('.btnImageSelect').each(function (i, d) {
                if ($(this).is(':checked')){
                    var id = $(d).attr('id');
                    var namestr = $('label[for=' + id + ']').text();
                    id = id.substring(id.indexOf('_') + 1);
                    imageid += id + ',';
                    name += namestr + ',';
                    $(this).parent().prev().addClass('selected');
                } else {
                    $(this).parent().prev().removeClass('selected');
                }
                
            });
            imageid = imageid.replace(re,'');
            <%=onSelectCallback %>
            $('#<%=HiddenField1.ClientID %>').val(imageid);
        }

         function loadVimeoThumb(id){   
            getVimeoData(id, function(data) {
                id = "V" + id;
                $("#sp_" + id).parent().attr('title', data.title);
                $(".LazyLoad_" + id).attr('src',data.thumbnail);
                $("#sp_" + id).attr('title', data.title);
                $("#lbl_" + id).text(data.title);
                $("#lbl_" + id).attr('title', data.title);
            });
        } 
        
        function loadYoutubeText(id){ 
            getYoutubeData(id, function(data) {                
                id = "Y" + id;
                $("#sp_" + id).parent().attr('title', data.title);
                $(".LazyLoad_" + id).attr('src',data.thumbnail);
                $("#b_" + id).attr('title', data.title);
                $("#lbl_" + id).text(data.title);
                $("#lbl_" + id).attr('title', data.title);    
            });    
        }

        function deleteVid(elm,id) {
            var t = $(elm).parent().parent().find("#lbl_" + id).text();
            return parent.jqconfirm(elm,'Are you sure you want to delete ' + t + '?','Delete Image')
        }

        function switchLink() {
            if ($('#picUploadDiv').is(':visible')) {
                $('#vidUploadDiv').show();
                $('#picUploadDiv').hide();
                $('#switchLink').text('Upload a Picture');
            } else {
                $('#picUploadDiv').show();
                $('#vidUploadDiv').hide();
                $('#switchLink').text('Upload Youtube or Vimeo Video');
            }
        }

        function precheckLink(){
            var enc = $('<div>').text($('#<%=tbLink.ClientID()%>').val()).html()
            $('#<%=tbLink.ClientID()%>').val(enc);
        }
</script>
<asp:Literal ID="litVimeoThumbScript" runat="server"></asp:Literal>
<style type="text/css">
span.roll,span.roll2 {
    background:url(~/res/BaseClasses/Scripts.aspx?d=&f=DTIContentManagement/magsm.png) center center no-repeat;
    position: absolute;
    z-index: 10;	
	height:30px;
	width:30px;
	bottom: 55px;
}
span.roll{
    background:url(~/res/BaseClasses/Scripts.aspx?d=&f=DTIContentManagement/magsm.png) center center no-repeat;
    right: 3px;
}
span.roll2{
    background:url(~/res/BaseClasses/Scripts.aspx?d=&f=DTIContentManagement/editsm.png) center center no-repeat;
    left: 3px;
}  
</style> 

<div class="uploaddiv" style="width:489px;float:left;">
    <div id="picUploadDiv">
        Browse for Picture: <asp:FileUpload ID="FileUpload1" runat="server" Width />
        <asp:Button ID="btnUpload" OnClientClick="waiter('Uploading Image...Please wait', parent);" runat="server" Text="Upload" />
    </div>
    <div id="vidUploadDiv" style="display:none">
        Link to Video: <asp:TextBox ID="tbLink" runat="server" Width="250px"></asp:TextBox>
        <asp:Button ID="btnEmbed" OnClientClick="waiter('Getting Video...Please wait', parent);precheckLink();" runat="server" Text="Embed" />
    </div>
</div>
<div id="divUploadLink" style="width:210px;float:right;text-align:right;" runat="server">
    <a id="switchLink" href="#">Embed Youtube or Vimeo Video</a>
</div>
<div style="clear:both"></div>
<cc1:Tabs ID="Tabs1" runat="server">
</cc1:Tabs>
<asp:Button ID="btnSelect" runat="server" Text="Select Image" />
<asp:HiddenField ID="HiddenField1" runat="server" />

