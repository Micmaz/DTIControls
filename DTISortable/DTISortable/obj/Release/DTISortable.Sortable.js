function ToggleDisplay(id)
{var elem=document.getElementById('divelem'+id);var img=document.getElementById('imgelem'+id);if(elem)
{if(elem.style.display!='none')
{elem.style.display='none';elem.style.visibility='hidden';img.src="~/res/BaseClasses/Scripts.aspx?f=DTISortable/expand.jpg"
if(id!=0)
img.alt="Show"}
else
{elem.style.display='block';elem.style.visibility='visible';img.src="~/res/BaseClasses/Scripts.aspx?f=DTISortable/collapse.jpg"
if(id!=0)
img.alt="Hide"}}}

function AddHeader(ui,hidid,sortid){
    var itm = ui.item[0];

    if (itm.id.indexOf('Menu') > -1)
    {
        if (itm.firstChild.className.indexOf('DTIItemHandle') == -1)
        {
            var itms = $(sortid + '> .ui-draggable');
            itms.each(function(i) {
                if (this.id == ""){
                    var itmid = '#' + itm.id;
                    
                    this.id = itm.id;                    
                    itm.id += "_x";
                    
                    var pclass = itm.parentNode.className;
                    var cclass = pclass.substring(pclass.indexOf('-'));
                    var left = "<span style=\"float:left;\">Click to Drag</span>";
                    var right = "<span style=\"cursor:pointer;float:right\" onclick=\"DeleteMenuItem('" + itmid + "', '" + hidid + "','" + sortid + "')\">X</span>";
                    var clear = "<br clear=\"both\" />";

                    var prep = "<div class=\"DTIItemHandle" + cclass + "\">" + left + right + clear + "</div>";
                    $(this).css("cursor", "auto");
                    $(this).prepend(prep);
                    return true; //break out of each
                }
            });          
        }
    }
}