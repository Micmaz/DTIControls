using System;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
//using EnvDTE90;
using System.IO;
using System.Windows.Forms;

namespace ToolboxManager
{
    public class Program 
    {
        // Displays progress during installation/uninstallation of toolbox items
        private static Form1 _InstallProgressForm;
        // Indicates whether a form is shown while modifications take place.
        private static bool _IsProgressVisible = true;
        // Represents the toolbox installer
        private static Installer _Installer;

        [STAThread]
        static void Main(string[] args)
        {
            doProcess(args);
        }

        [STAThread]
        public static void doProcess(string[] args)
        {

            #region Show help if no parameters were passed in

            // Was nothing specified?
            if (args.Length == 0)
            {
                MessageBox.Show("Visual Studio Toolbox Manager\r\n"
                                + "Version 1.1  Copyright® 2007-2008  GeoFrameworks, LLC (www.geoframeworks.com)\r\n"
                                + "Written by Jon Person (jperson@geoframeworks.com)\r\n"
                                + "and Bill Bither of Atalasoft (www.atalasoft.com)\r\n"
                                + "\r\n"
                                + "Adds or removes user controls to the Visual Studio 2005 or 2008 toolboxes.  This utility may be "
                                + "redistributed freely as long as it remains unmodified.\r\n"
                                + "\r\n"
                                + "This utility requires parameters to make changes to the toolbox.  Use the following syntax:\r\n"
                                + "\r\n"
                                + "Toolbox.exe [/silent] (/vs2005|/vs2008) /installdesktop assembly toolbox_tab [...]\r\n"
                                + "Toolbox.exe [/silent] (/vs2005|/vs2008) /installdesktop assembly toolbox_tab [...]\r\n"
                                + "Toolbox.exe [/silent] (/vs2005|/vs2008) /installpocketpc assembly toolbox_tab [...]\r\n"
                                + "Toolbox.exe [/silent] (/vs2005|/vs2008) /installcustom template assembly toolbox_tab [...]\r\n"
                                + "Toolbox.exe [/silent] (/vs2005|/vs2008) /uninstall toolbox_tab [...]\r\n"
                                + "\r\n"
                                + "/silent             Suppresses all output during installation or uninstallation.\r\n"
                                + "/installdesktop     Installs controls written for Desktop Framework 2.0 to the toolbox.\r\n"
                                + "/installpocketpc    Installs controls written for Compact Framework 2.0 to the toolbox.\r\n"
                                + "/installcustom      Installs controls written for a specific kind of device project to the toolbox.\r\n"
                                + "/uninstall          Removes an entire Toolbox tab.\r\n"
                                + "/vs2005             Installs Icons into Visual Studio 2005"
                                + "/vs2008             Installs Icons into Visual Studio 2008"
                                + "[...]               Multiple install commands can be appended to install/uninstall multiple controls at one time.\r\n"
                                + "\r\n"
                                + "\"assembly\" is the absolute path to an assembly containing user controls.\r\n"
                                + "\"toolbox_tab\" is the name of the Toolbox Tab where control should be installed.\r\n"
                                + "\"template\" is the name of a ZIP file from the Templates folder indicating a specific kind of project.\r\n"
                                + "\r\n"
                                + "This utility can work with multiple versions of Visual Studio at a time.  If either version is not installed, operations "
                                + "will be skipped without raising an error.\r\n"
                                , "Visual Studio Toolbox Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            #endregion

            #region Perform installation and/or uninstallation tasks

            // Create a new installer
            _Installer = new Installer();

            #region Parse out all parameters 

            try
            {
                // Is one of the arguments "/silent" ?
                for(int index = 0; index < args.Length; index++)
                {
                    // Get the argument name (in lowercase)
                    string argument = args[index].ToLowerInvariant();

                    // What exactly is going on?
                    switch (argument)
                    {
                        case "/installdesktop":

                            // Add a task to install into the .NET Desktop Framework toolbox
                            _Installer.Tasks.Add(new Task(args[index + 1], args[index + 2], "WindowsApplication.zip"));

                            // Advance the index to the next argument
                            index += 2;
                            break;
                        case "/installpocketpc":

                            // Add a task to install into the .NET Compact Framework toolbox for PocketPC apps
                            _Installer.Tasks.Add(new Task(args[index + 1], args[index + 2], "PocketPC2003-WindowsApplication.zip"));

                            // Advance the index to the next argument
                            index += 2;
                            break;
                        case "/installcustom":

                            // Add a task to install using a custom template (typically a Compact Framework platform)
                            _Installer.Tasks.Add(new Task(args[index + 1], args[index + 2], args[index + 3]));

                            // Advance the index to the next argument
                            index += 3;
                            break;
                        case "/uninstall":

                            // Remove an entire toolbox tab
                            _Installer.Tasks.Add(new Task(args[index + 1]));

                            // Advance the index to the next argument
                            index++;
                            break;
                        case "/silent":

                            // Disable the progress form
                            _IsProgressVisible = false;

                            break;
                        case "/vs2005":
                        case "/2005":

                            // Tell the installer to include VS2005 in its tasks
                            _Installer.IsVisualStudio2005 = true;

                            break;
                        case "/vs2008":
                        case "/2008":

                            // Tell the installer to include VS2008 in its tasks
                            _Installer.IsVisualStudio2008 = true;

                            break;
                        default:
                            MessageBox.Show("An argument passed to the Toolbox utility could not be recognized.  Please make sure that any parameters containing spaces are enclosed in quotes, and that each command is followed by the right number of parameters. (Bad command: " + argument + ")", "Visual Studio Toolbox Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Unable to Parse the Command Line", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            #endregion

            #region Perform the toolbox modifications

            // If we're showing progress, hook into installer events
            if (_IsProgressVisible)
            {
                _Installer.OperationStarted += new EventHandler(_Installer_OperationStarted);
                _Installer.OperationCompleted += new EventHandler(_Installer_OperationCompleted);
                _Installer.TaskProgressOccurred += new EventHandler(_Installer_TaskProgressOccurred);
            }

            // Perform all tasks
            _Installer.Execute();

            // Shut down all instances of Visual Studio we've been using
            _Installer.Dispose();

            // Dispose of the progress form (if necessary)
            if (_InstallProgressForm != null)
            {
                if (_IsProgressVisible)
                {
                    _InstallProgressForm.Hide();
                    
                    _InstallProgressForm.Dispose();
                }
            }
            Application.DoEvents();
            #endregion

            #endregion
        }

        #region Installation progress form events

        static void _Installer_TaskProgressOccurred(object sender, EventArgs e)
        {
            if (_InstallProgressForm != null)
            {
                // Bump up the progress bar
                _InstallProgressForm.ToolboxProgressBar.Value++;
            }
            Application.DoEvents();
        }

        static void _Installer_OperationCompleted(object sender, EventArgs e)
        {
            if (_InstallProgressForm != null)
            {
                _InstallProgressForm.ToolboxProgressBar.Value = _InstallProgressForm.ToolboxProgressBar.Maximum;
            }
            Application.DoEvents();
        }

        static void _Installer_OperationStarted(object sender, EventArgs e)
        {
            // Enable visual styles
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
            }
            catch (Exception ex) { 
                //MessageBox.Show(ex.Message + "/n" + ex.StackTrace); 
            }

            // Create and display the progress form
            //_InstallProgressForm = new Form1();
            //_InstallProgressForm.ToolboxProgressBar.Maximum = _Installer.Tasks.Count * 5;
            //_InstallProgressForm.Show();
            Application.DoEvents();
        }

        #endregion
    }
}
