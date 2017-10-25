//** Chrome Drop Down Menu- Author: Dynamic Drive (http://www.dynamicdrive.com)
var cssdropdown={disappeardelay:250,dropdownindicator:'',enableswipe:1,enableiframeshim:1,dropmenuobj:null,asscmenuitem:null,domsupport:document.all||document.getElementById,standardbody:null,iframeshimadded:false,swipetimer:undefined,bottomclip:0,getposOffset:function(what,offsettype){var totaloffset=(offsettype=="left")?what.offsetLeft:what.offsetTop;var parentEl=what.offsetParent;while(parentEl!=null){totaloffset=(offsettype=="left")?totaloffset+parentEl.offsetLeft:totaloffset+parentEl.offsetTop;parentEl=parentEl.offsetParent;}
return totaloffset;},swipeeffect:function(){if(this.bottomclip<parseInt(this.dropmenuobj.offsetHeight)){this.bottomclip+=10+(this.bottomclip/10)
this.dropmenuobj.style.clip="rect(0 auto "+this.bottomclip+"px 0)"}
else
return
this.swipetimer=setTimeout("cssdropdown.swipeeffect()",10)},css:function(el,targetclass,action){var needle=new RegExp("(^|\\s+)"+targetclass+"($|\\s+)","ig")
if(action=="check")
return needle.test(el.className)
else if(action=="remove")
el.className=el.className.replace(needle,"")
else if(action=="add"&&!needle.test(el.className))
el.className+=" "+targetclass},showhide:function(obj,e){this.dropmenuobj.style.left=this.dropmenuobj.style.top="-500px"
if(this.enableswipe==1){if(typeof this.swipetimer!="undefined")
clearTimeout(this.swipetimer)
obj.clip="rect(0 auto 0 0)"
this.bottomclip=0
this.swipeeffect()}
obj.visibility="visible"
this.css(this.asscmenuitem,"selected","add")},clearbrowseredge:function(obj,whichedge){var edgeoffset=0
if(whichedge=="rightedge"){var windowedge=document.all&&!window.opera?this.standardbody.scrollLeft+this.standardbody.clientWidth-15:window.pageXOffset+window.innerWidth-15
this.dropmenuobj.contentmeasure=this.dropmenuobj.offsetWidth
if(windowedge-this.dropmenuobj.x<this.dropmenuobj.contentmeasure)
edgeoffset=this.dropmenuobj.contentmeasure-obj.offsetWidth}
else{var topedge=document.all&&!window.opera?this.standardbody.scrollTop:window.pageYOffset
var windowedge=document.all&&!window.opera?this.standardbody.scrollTop+this.standardbody.clientHeight-15:window.pageYOffset+window.innerHeight-18
this.dropmenuobj.contentmeasure=this.dropmenuobj.offsetHeight
if(windowedge-this.dropmenuobj.y<this.dropmenuobj.contentmeasure){edgeoffset=this.dropmenuobj.contentmeasure+obj.offsetHeight
if((this.dropmenuobj.y-topedge)<this.dropmenuobj.contentmeasure)
edgeoffset=this.dropmenuobj.y+obj.offsetHeight-topedge}}
return edgeoffset},dropit:function(obj,e,dropmenuID){if(this.dropmenuobj!=null)
this.hidemenu()
this.clearhidemenu()
this.dropmenuobj=document.getElementById(dropmenuID)
this.asscmenuitem=obj
this.showhide(this.dropmenuobj.style,e)
this.dropmenuobj.x=this.getposOffset(obj,"left")
this.dropmenuobj.y=this.getposOffset(obj,"top")
this.dropmenuobj.style.left=this.dropmenuobj.x-this.clearbrowseredge(obj,"rightedge")+"px"
this.dropmenuobj.style.top=this.dropmenuobj.y-this.clearbrowseredge(obj,"bottomedge")+obj.offsetHeight+1+"px"
this.positionshim()},positionshim:function(){if(this.enableiframeshim&&typeof this.shimobject!="undefined"){if(this.dropmenuobj.style.visibility=="visible"){this.shimobject.style.width=this.dropmenuobj.offsetWidth+"px"
this.shimobject.style.height=this.dropmenuobj.offsetHeight+"px"
this.shimobject.style.left=this.dropmenuobj.style.left
this.shimobject.style.top=this.dropmenuobj.style.top}
this.shimobject.style.display=(this.dropmenuobj.style.visibility=="visible")?"block":"none"}},hideshim:function(){if(this.enableiframeshim&&typeof this.shimobject!="undefined")
this.shimobject.style.display='none'},isContained:function(m,e){var e=window.event||e
var c=e.relatedTarget||((e.type=="mouseover")?e.fromElement:e.toElement)
while(c&&c!=m)try{c=c.parentNode}catch(e){c=m}
if(c==m)
return true
else
return false},dynamichide:function(m,e){if(!this.isContained(m,e)){this.delayhidemenu()}},delayhidemenu:function(){this.delayhide=setTimeout("cssdropdown.hidemenu()",this.disappeardelay)},hidemenu:function(){this.css(this.asscmenuitem,"selected","remove")
this.dropmenuobj.style.visibility='hidden'
this.dropmenuobj.style.left=this.dropmenuobj.style.top=0
this.hideshim()},clearhidemenu:function(){if(this.delayhide!="undefined")
clearTimeout(this.delayhide)},addEvent:function(target,functionref,tasktype){if(target.addEventListener)
target.addEventListener(tasktype,functionref,false);else if(target.attachEvent)
target.attachEvent('on'+tasktype,function(){return functionref.call(target,window.event)});},startchrome:function(){if(!this.domsupport)
return
this.standardbody=(document.compatMode=="CSS1Compat")?document.documentElement:document.body
for(var ids=0;ids<arguments.length;ids++){var menuitems=document.getElementById(arguments[ids]).getElementsByTagName("a")
for(var i=0;i<menuitems.length;i++){if(menuitems[i].getAttribute("rel")){var relvalue=menuitems[i].getAttribute("rel")
var asscdropdownmenu=document.getElementById(relvalue)
this.addEvent(asscdropdownmenu,function(){cssdropdown.clearhidemenu()},"mouseover")
this.addEvent(asscdropdownmenu,function(e){cssdropdown.dynamichide(this,e)},"mouseout")
this.addEvent(asscdropdownmenu,function(){cssdropdown.delayhidemenu()},"click")
try{menuitems[i].innerHTML=menuitems[i].innerHTML+" "+this.dropdownindicator}catch(e){}
this.addEvent(menuitems[i],function(e){if(!cssdropdown.isContained(this,e)){var evtobj=window.event||e
cssdropdown.dropit(this,evtobj,this.getAttribute("rel"))}},"mouseover")
this.addEvent(menuitems[i],function(e){cssdropdown.dynamichide(this,e)},"mouseout")
this.addEvent(menuitems[i],function(){cssdropdown.delayhidemenu()},"click")}}}
if(window.createPopup&&!window.XmlHttpRequest&&!this.iframeshimadded){document.write('<IFRAME id="iframeshim"  src="" style="display: none; left: 0; top: 0; z-index: 90; position: absolute; filter: progid:DXImageTransform.Microsoft.Alpha(style=0,opacity=0)" frameBorder="0" scrolling="no"></IFRAME>')
this.shimobject=document.getElementById("iframeshim")
this.iframeshimadded=true}}}