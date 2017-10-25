<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="JavaFileUploader.ascx.vb" Inherits="DTIUploader.JavaFileUploader" %>
<div> 
            <!-- --------------------------------------------------------------------------------------------------- --> 
            <!-- --------     A DUMMY APPLET, THAT ALLOWS THE NAVIGATOR TO CHECK THAT JAVA IS INSTALLED   ---------- --> 
            <!-- --------               If no Java: Java installation is prompted to the user.            ---------- --> 
            <!-- --------------------------------------------------------------------------------------------------- --> 
            <!--"CONVERTED_APPLET"--> 
            <!-- HTML CONVERTER --> 
            <script language="JavaScript" type="text/javascript"><!--
                var _info = navigator.userAgent;
                var _ns = false;
                var _ns6 = false;
                var _ie = (_info.indexOf("MSIE") > 0 && _info.indexOf("Win") > 0 && _info.indexOf("Windows 3.1") < 0);
            //--></script> 
                <comment> 
                    <script language="JavaScript" type="text/javascript"><!--
                    var _ns = (navigator.appName.indexOf("Netscape") >= 0 && ((_info.indexOf("Win") > 0 && _info.indexOf("Win16") < 0 && java.lang.System.getProperty("os.version").indexOf("3.5") < 0) || (_info.indexOf("Sun") > 0) || (_info.indexOf("Linux") > 0) || (_info.indexOf("AIX") > 0) || (_info.indexOf("OS/2") > 0) || (_info.indexOf("IRIX") > 0)));
                    var _ns6 = ((_ns == true) && (_info.indexOf("Mozilla/5") >= 0));
            //--></script> 
                </comment> 
             
            <script language="JavaScript" type="text/javascript"><!--
                if (_ie == true) document.writeln('<object classid="clsid:8AD9C840-044E-11D1-B3E9-00805F499D93" WIDTH = "0" HEIGHT = "0" NAME = "JUploadApplet"  codebase="http://java.sun.com/update/1.5.0/jinstall-1_5-windows-i586.cab#Version=5,0,0,3"><noembed><xmp>');
                else if (_ns == true && _ns6 == false) document.writeln('<embed ' +
	                'type="application/x-java-applet;version=1.5" \
                        CODE = "wjhk.jupload2.EmptyApplet" \
                        ARCHIVE = "wjhk.jupload.jar" \
                        NAME = "JUploadApplet" \
                        WIDTH = "0" \
                        HEIGHT = "0" \
                        type ="application/x-java-applet;version=1.6" \
                        scriptable ="false" ' +
	                'scriptable=false ' +
	                'pluginspage="http://java.sun.com/products/plugin/index.html#download"><noembed><xmp>');
            //--></script> 
            <applet  CODE = "wjhk.jupload2.EmptyApplet" ARCHIVE = "wjhk.jupload.jar" WIDTH = "0" HEIGHT = "0" NAME = "JUploadApplet"></xmp> 
                <PARAM NAME = CODE VALUE = "wjhk.jupload2.EmptyApplet" /> 
                <PARAM NAME = ARCHIVE VALUE = "wjhk.jupload.jar" /> 
                <PARAM NAME = NAME VALUE = "JUploadApplet" /> 
                <param name="type" value="application/x-java-applet;version=1.5"/> 
                <param name="scriptable" value="false"/> 
                <PARAM NAME = "type" VALUE="application/x-java-applet;version=1.6"/> 
                <PARAM NAME = "scriptable" VALUE="false"/> 
             
           </xmp>
                
            Java 1.5 or higher plugin required.
            </applet> 
         </noembed> 
            </embed> 
            </object> 
             
            <!--
            <APPLET CODE = "wjhk.jupload2.EmptyApplet" ARCHIVE = "wjhk.jupload.jar" WIDTH = "0" HEIGHT = "0" NAME = "JUploadApplet">
	            <PARAM NAME = "type" VALUE="application/x-java-applet;version=1.6">
	            <PARAM NAME = "scriptable" VALUE="false">
	            </xmp>
            Java 1.5 or higher plugin required.
            </APPLET>
            --> 
            <!-- "END_CONVERTED_APPLET" --> 
            <!-- ---------------------------------------------------------------------------------------------------- --> 
            <!-- --------------------------     END OF THE DUMMY APPLET TAG            ------------------------------ --> 
            <!-- ---------------------------------------------------------------------------------------------------- --> 
             
             <applet code="wjhk.jupload2.JUploadApplet" name="JUpload" archive="wjhk.jupload.jar"
                width="640" height="300" mayscript alt="The java pugin must be installed.">
                <param name="postURL" value="JavaUploader.aspx?f=1&sguid=<%=aguid.ToString %>"  />
                <!--Uploaded in 10mb chunks -->
                <param name="maxChunkSize" value="10000000" />
                <param name="uploadPolicy" value="FileByFileUploadPolicy" />  
                <param name="stringUploadSuccess" value="" />
                <param name="showLogWindow" value="false" />
                <param name="formdata" value="form1" />
                 Java 1.5 or higher plugin required. 
            </applet>
        </div>