<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Puzzled.Master" CodeBehind="Test3.aspx.vb" Inherits="DTIAdminPanelTestProject.Test3" 
    title="Untitled Page" %>
<%@ Register Assembly="DTISortable" Namespace="DTISortable" TagPrefix="cc1" %>
<%@ Register Assembly="DTIContentManagement" Namespace="DTIContentManagement" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<!-- start latest-post --> 
<div class="latest-post">
    <cc1:DTISortable ID="Sortable" CssClass="post" runat="server" contentType="MainContent" ItemsClassName="entry">
        <cc2:EditPanel ID="EditPanel2" runat="server" contentType="epWelcomeTxt">
			<h1 class="title">Welcome to Our Website!</h1> 
			<p class="meta"><small>Posted by Admin on August 3, 2007<br /> 
				Filed under <a href="#">Uncategorized</a> | <a href="#">Comments (12)</a></small></p>
		</cc2:EditPanel>
	    <asp:Panel ID="panTemplate" runat="server">
		    <h2>Change Template</h2>
			<p>
                <asp:DropDownList ID="ddlTemplate" runat="server" AutoPostBack="true">
                </asp:DropDownList></p>
		</asp:Panel> 
        <cc2:EditPanel ID="EditPanel1"  runat="server" contentType="epFirstPost">
                <p>This is <strong>Puzzled</strong>, a free, fully standards-compliant CSS template designed by <a href="http://www.freecsstemplates.org/">Free CSS Templates</a>. This free template is released under a <a href="http://creativecommons.org/licenses/by/2.5/">Creative Commons Attributions 2.5</a> license, so you're pretty much free to do whatever you want with it (even use it commercially) provided you keep the links in the footer intact. The photo used is from <a href="http://www.pdphoto.org/">PDPhoto.org</a>. Aside from that, have fun with it :)</p> 
				<p>This template is also available as a <a href="http://www.freewpthemes.net/preview/Puzzled/">WordPress theme</a> at <a href="http://www.freewpthemes.net/">Free WordPress Themes</a>.</p> 
				<h2>Sample Tags</h2> 
				<blockquote> 
					<p>A blockquoted paragraph. In posuere eleifend odio quisque semper augue mattis wisi maecenas ligula.&#8221;</p> 
				</blockquote> 
				<p>An example of an ordered list:</p> 
				<ol> 
					<li>Lorem ipsum dolor sit amet, consectetuer adipiscing elit.</li> 
					<li>Phasellus nec erat sit amet nibh pellentesque congue.</li> 
					<li>Cras vitae metus aliquam risus pellentesque pharetra.</li> 
				</ol> 
				<h3>Heading Level 3: Followed by an Unordered List</h3> 
				<ul> 
					<li>Cras vitae metus aliquam risus pellentesque pharetra.</li> 
					<li>Maecenas vitae orci vitae tellus feugiat eleifend.</li> 
					<li>Etiam ac tortor eu metus euismod lobortis</li> 
				</ul> 
				<p>Pellentesque tristique ante ut risus. Quisque dictum. Integer nisl risus, sagittis convallis, rutrum id, elementum congue, nibh. Suspendisse dictum porta lectus. Donec placerat odio vel elit. Nullam ante orci, pellentesque eget, tempus quis, ultrices in, est. Curabitur sit amet nulla. Nam in massaed vel tellus. </p> 
		</cc2:EditPanel>
	</cc1:DTISortable>
</div>
<!-- end latest-post --> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
<!-- start recent-posts --> 
    <cc1:DTISortable ID="DTISortable1" runat="server" contentType="RecentPosts" CssClass="recent-posts" ItemsClassName="posts">
        <cc2:EditPanel ID="EditPanel3" runat="server" contentType="epRecent1">
			<h2 class="title">Lorem Ipsum Dolor </h2> 
			<p class="meta"><small>Posted by Admin on August 3, 2007<br /> 
				Filed under <a href="#">Uncategorized</a> | <a href="#">Comments (12)</a></small></p> 
			<div class="entry"> 
				<p>Quisque dictum. Integer nisl risus, sagittis convallis, rutrum id, elementum congue, nibh. Suspendisse dictum porta lectus. Donec placerat odio vel elit [&hellip;]</p> 
				<p><a href="#" class="more">Read more</a></p> 
		    </div> 
		</cc2:EditPanel> 
		<cc2:EditPanel ID="EditPanel4" runat="server" contentType="epRecent2">
			<h2 class="title">Suspendise Dictum </h2> 
			<p class="meta"><small>Posted by Admin on August 3, 2007<br /> 
				Filed under <a href="#">Uncategorized</a> | <a href="#">Comments (12)</a></small></p> 
			<div class="entry"> 
				<p>Quisque dictum. Integer nisl risus, sagittis convallis, rutrum id, elementum congue, nibh. Suspendisse dictum porta lectus. Donec placerat odio vel elit [&hellip;]</p> 
				<p><a href="#" class="more">Read more</a></p> 
			</div> 
		</cc2:EditPanel> 
		<cc2:EditPanel ID="EditPanel5" runat="server" contentType="epRecent3">
			<h2 class="title">Maecenas Vitaeorci Vitae</h2> 
			<p class="meta"><small>Posted by Admin on August 3, 2007<br /> 
				Filed under <a href="#">Uncategorized</a> | <a href="#">Comments (12)</a></small></p> 
			<div class="entry"> 
				<p>Quisque dictum. Integer nisl risus, sagittis convallis, rutrum id, elementum congue, nibh. Suspendisse dictum porta lectus. Donec placerat odio vel elit [&hellip;]</p> 
				<p><a href="#" class="more">Read more</a></p> 
			</div> 
		</cc2:EditPanel> 
	</cc1:DTISortable>
<!-- end recent-posts --> 
</asp:Content>
