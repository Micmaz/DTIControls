<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ModernElegance.aspx.vb" Inherits="DTIContentManagementTester.ModernElegance" %>
<%@ Register Assembly="DTISortable" Namespace="DTISortable" TagPrefix="cc2" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Test</title>
    <script src="../js/template.js" type="text/javascript"></script>
	<script src="../js/jquery.js" type="text/javascript" charset="utf-8"></script>
<link href="ModernEleganceColors/css/brown.css" rel="stylesheet" type="text/css" />
<style type="text/css">
.SortablePlaceHolder { 
	        height: 2em; 
	        line-height: 1.5em;
	        background-color:#EEEEEE;
        }
        .OuterRecyclebin{
            border: dotted thin red;
        }
        .DTISortableItemHandle, .DTIRecycledItemHandle, .WidgetMenuItemHandle {
            background-color:#052A3A;
            color:white;
            padding:4px;
        }
        .WidgetMenuOuter, .OuterRecyclebin {
            background-color: #eeeeee;
        }
</style>
 
</head>

<body>
<form id="form1" runat="server">
<div id="navigation">
	<div class="shell">
	<h1 id="logo"><p>
	<asp:CheckBox ID="cbEdit" runat="server" AutoPostBack="true" Text="Edit" />
	<asp:CheckBox ID="cbReorder" runat="server" AutoPostBack="true" Text="Re-Order" /><br />
	<asp:Button ID="Button1" runat="server" Text="Save" />

		</p></h1>
		<div class="cl">&nbsp;</div>

		<div id="chromemenu" class="me">
		<ul>
		    <li><a href="../../">Home</a></li>
		              <li><a href="#" rel="dropmenu0">Clinic_Info</a></li>
	                  <li><a href="#" rel="dropmenu1">Articles </a></li> 
	                  <li><a href="#" rel="dropmenu2">More_Info</a></li>
                        <li><a href="#" target="_top">Pet Portal</a></li>
                        <li><a href="#" target="_top">Shop Online</a></li>
			                     
		</ul>
			                
		</div>
	
		<div class="cl">&nbsp;</div>
	</div>
</div>
	
<div class="shell">
	<div id="content">
		<div class="cl">&nbsp;</div>
		<div class="main" style="background:url(ModernEleganceColors/brown/replaceableImage.gif) no-repeat 0 0;background-position:right top">
			<div class="featured">
				<div class="box">
					<cc2:DTISortable ID="SortableServer1" runat="server" contentType="RotatingArticles" HandleText="Drag Me">
					
					</cc2:DTISortable>
				</div>
			</div>
			<div class="cl">&nbsp;</div>
			<div class="left">
				<cc2:DTISortable ID="DTISortable1" runat="server" contentType="News" HandleText="Drag Me">
				</cc2:DTISortable>
			</div>
			<div class="right">
				
				<div class="news">
					<cc2:DTISortable ID="DTISortable2" runat="server" contentType="aboutUs" HandleText="Drag Me">
					</cc2:DTISortable>
				</div>
			</div>
			<div class="cl">&nbsp;</div>
			<div id="footer">
				<p><a href="#">Privacy Policy</a><span>|</span><a href="#">Legal Terms</a><br>Copyright © 2008 VetStreet</p>
			</div>
		</div>
		<cc2:DTISortable ID="DTISortable3" CssClass="sidebar" runat="server" contentType="Hours" HandleText="Drag Me">
			
		
		<asp:Panel ID="HoursOperation" runat="server" CssClass="hours">
			
			<h1>Hours of Operation</h1>
				hours
			
				</asp:Panel>
			
			
			
		<asp:Panel ID="Panel1" runat="server" CssClass="search">
			<span>Search Our Site:</span>
<table>
    <tbody>
        <tr>
            <td><!--Search--></td>
        </tr>
        <tr>
            <td colspan="2"><select name="search1$ddCategories" id="search1_ddCategories" style="width: 128px;">
            <option selected="selected" value="All Categories">All Categories</option>
            <option value="Article Name">Article Name</option>
            <option value="144">Dog Care</option>
            <option value="146">Cat Care</option>
            <option value="148">Diseases</option>
            <option value="149">Symptoms</option>
            <option value="150">Drug Library</option>
            <option value="151">Tests</option>
            <option value="152">Procedures</option>
            <option value="176">FAQ's</option>
            </select></td>
        </tr>
        <tr>
            <td colspan="2"><select name="search1$ddType" id="search1_ddType" style="width: 128px;">
            <option selected="selected" value="Phrase">By Phrase</option>
            <option value="All">By All Words</option>
            <option value="Any">By Any Word</option>
            </select></td>
        </tr>
        <tr>
            <td style="width: 50px;"><input name="search1$tbQuery" id="search1_tbQuery" style="width: 96px;" type="text" /></td>
            <td align="right"><input name="search1$btnGo" id="search1_btnGo" style="border-right-width: 0px; width: 25px; border-top-width: 0px; border-bottom-width: 0px; height: 25px; border-left-width: 0px;" type="text" text="go" /></td>
        </tr>
        <tr>
            <td colspan="2"></td>
        </tr>
    </tbody>
</table>
			</asp:Panel>
			 
				
			
			<asp:Panel ID="Panel2" runat="server" CssClass="banners">
			
				 
			teasers
</asp:Panel>
			
		
		
		
			</cc2:DTISortable>
		<div class="cl">&nbsp;</div>
		<div style="width:200px">
		
            </div>
	</div>
</div>
<div style="position:fixed; width:180px; left:5px; top:5px; z-index:1000;"><cc2:DTIWidgetMenuServer ID="Menu1" runat="server" HandleText="Drag Me"/>
            <cc2:DTIRecycleBinServer ID="Recycle1" runat="server" NumberOfItems="5" HandleText="Drag Me"/></div>

</form>

</body>

</html>
