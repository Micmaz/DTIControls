// JScript File
function DTIShowEdit(lbl, tb){
    document.getElementById(tb).style.display = "block";
    document.getElementById(lbl).style.display = "none";
    document.getElementById(tb).focus();
    document.getElementById(tb).value = document.getElementById(lbl).innerHTML;
    return false;
}
function DTIHideEdit(lbl, tb)
{
    document.getElementById(tb).style.display = "none";
    document.getElementById(lbl).style.display = "block";
    document.getElementById(lbl).innerHTML = document.getElementById(tb).value;
    return false;
}
function DTIhandleKeydown(evnt, lbl, tb)
{
    var e = (window.event) ? window.event : evnt;
    var keynum = e.keyCode;
    if ((keynum == 13) && (e.shiftKey));
    if (keynum == 13) { // Enter Key
        if (window.event) 
            e.returnValue = false;
        else
            e.preventDefault();
        return;
    }
    if (keynum == 27) { // Escape Key
        if (window.event) 
            e.returnValue = false;
        else
            e.preventDefault();
        return;
    }
}