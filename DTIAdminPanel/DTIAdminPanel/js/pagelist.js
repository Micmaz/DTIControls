var found = false;
    
function dup(node, tree){
    $('#HiddenField1').val(tree.get_node_data(node)[2]);
    document.getElementById('btnDuplicate').click();
    //$('#btnDuplicate').click();
}

function gotoPage(node, tree){
    var text = tree.get_node_data(node)[3];         
    window.parent.location = '/page/' + text + '.aspx';
}

function editPage(node, tree){           
    var value = tree.get_node_data(node)[2];
    window.location = '~/res/DTIAdminPanel/EditItem.aspx?id=' + value;
}

function pageVisibility(node, tree){
    if (tree.get_type(node) != "folder") { 
        return true;
    }else{
        return false;
    }
}        

function Dropped(node, refnode, type, tree){
    if (tree.get_type(node) != "folder"){       
        var wee = tree.create({
            attributes : {  
                "rel" : "link",
                "val" : "X",
                "key" : tree.get_node_data(node)[2]
            },
            data : {
                title : tree.get_text(node)
            }
        }, refnode, type, "link");
        wee[0].id = wee[0].id.replace("tlMenuItems","tlPages");
    }
}

function doMICreate(node, refnode, type, tree){ 
    if (tree.get_type(node) != "EmptyTreeHolder") {     
        if (tree.get_nodes().length == 2 && 
                ($('#' + tree.get_nodes()[0][0]).attr('rel') == "EmptyTreeHolder" || 
                 $('#' + tree.get_nodes()[1][0]).attr('rel') == "EmptyTreeHolder")){
            tree.remove(refnode,true);
        }
        if (node.getAttribute("val") == null ){  
            tree.set_type("item", node);
            tree.rename(node);
        }else if (node.getAttribute("val") != null){
            var parent = tree.parent(node)
            if(parent != -1){
                if (tree.get_type(parent) != "hidden")
                    tree.set_type("link", node);
                else
                    tree.set_type("hidden", node);
            }else{
                tree.set_type("link", node);
            }
            
        } 
    }           
}

function insertPagesFolder(){
    $.tree.reference("tlPages").create({
        attributes : {
            "rel" : "folder"
        },
        data : {
            title : "New Folder"
        }
    }, -1, "after");
}

//function renameMI(node, tree, rb){
//    if (tree.get_type(node) == "item"){
//        $.tree.reference("tlPages").search(tree.get_text(node));  
//        if (found)                 
//            tree.set_type("link", node);   
//    }                   
//}      

function renamePL(node, tree, rb){
    if (tree.get_type(node) == "page" && tree.get_text(node).match(/[^a-zA-Z0-9\s_\-]/g)){
        var str = tree.get_text(node);
        str = str.replace(/[^a-zA-Z0-9\s_\-]/g, "");
        tree.rename(node, str);
    }                                              
}         

function doPLCreate(node, refnode, type, tree){  
    if (tree.get_type(node) != "folder")
        tree.set_type("folder", node);   
    tree.rename(node);     
}

function search(nodes, tree){
    if (nodes.length > 0)
        found = true;
    else
        found = false;
}

function HideUnhide(node, tree){
    if (tree.get_type(node) == "link"){
        tree.set_type("hidden", node);
        var children = tree.children(node);
        if (children.length > 0){
            var i = 0;
            for (i = 0; i < children.length; i++){
                tree.set_type("hidden",children[i]);
            }
        }
    } else if (tree.get_type(node) == "hidden"){
        tree.set_type("link", node);
    }
}

function ShowHideVisible(node, tree){
    if(tree.get_type(node) == "hidden" || tree.get_type(node) == "link"){
            return 1;
    }else{
        return 0;
    }            
}

function removeSpaces()
{
    var pagetxt = $('#tbPageName').val().replace(/[^a-zA-Z0-9\s\_]/g, "");
    var lbl = $('#lbl');
    
    //pagetxt = pagetext.replace(/[\s/g], '%20');
    $('#tbPageName').val(pagetxt);
    if (pagetxt != "")
        lbl.text('Page URL: http://' + window.location.host + '/page/' + pagetxt + '.aspx' );
    else
        lbl.text('');
}  

function getMenuURL(id){
    var menu = $('#hfMenus').val();
    var start = menu.indexOf('[' + id);
    if (start > -1){
        menu = menu.substring(menu.indexOf(',',start)+1,menu.indexOf(']',start));
        if (menu > -1)
            return getLink(menu);                               
    }
    return '';
}

function getPageId(id){
    var menu = $('#hfMenus').val();
    var start = menu.indexOf('[' + id + ',');
    if (start > -1){
        menu = menu.substring(menu.indexOf(',',start)+1,menu.indexOf(']',start));
        return menu;                               
    }
    return '';
}

function getMenuURL(id){
    var menu = $('#hfMenus').val();
    var start = menu.indexOf('[' + id + ',');
    if (start > -1){
        menu = menu.substring(menu.indexOf(',',start)+1,menu.indexOf(']',start));
        if (menu > -1)
            return getLink(menu);                               
    }
    return '';
}

function getPagename(id){
    var pages = $('#hfPages').val();
    var start = pages.indexOf('[' + id + ',');
    if (start > -1){
        pages = pages.substring(pages.indexOf(',',start)+1,pages.indexOf(']',start));            
        var arr = pages.split(',');
        return arr[2];
    }
    return '';
}

function getLink(id){
    var pages = $('#hfPages').val();
    var start = pages.indexOf('[' + id + ',');
    if (start > -1){
        pages = pages.substring(pages.indexOf(',',start)+1,pages.indexOf(']',start));            
        var arr = pages.split(',');
        if (arr[1] == 'd')
            return '/page/' + arr[0] + '.aspx';                
        else if (arr[0].startsWith('http://'))
            return arr[0];
        else
            return '/' + arr[0];
    }
    return '';
} 

function showMenuLink(node,tree){
    var key = node.getAttribute("key");
    if (node.getAttribute("val") == null)
        key = getPageId(key);
    $('#lblurl').html('   link to page: <a target=_parent href="' + getLink(key) +'">' + getPagename(key) + '</a>')   
}  

parent.window.hs.Expander.prototype.onAfterClose = function(){
    var doit = '0';
    try {
        doit = $('#hfRefresh').val();
    }catch(err){}
    if (doit == '1')
        window.parent.location.reload(true);
}

String.prototype.startsWith = function(str) {return (this.match("^"+str)==str)}  