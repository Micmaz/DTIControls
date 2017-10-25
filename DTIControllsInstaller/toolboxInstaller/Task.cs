using System;
using System.IO;
using EnvDTE;
using EnvDTE80;

namespace ToolboxManager
{
    /// <summary>
    /// Represents an installation or uninstallation task.
    /// </summary>
    public sealed class Task
    {
        /// <summary>
        /// Controls the path to the assembly which will be installed into the toolbox.
        /// </summary>
        private string _AssemblyPath;
        /// <summary>
        /// Controls the name of the toolbox tab to be installed (or uninstalled).
        /// </summary>
        private string _ToolboxTab;
        /// <summary>
        /// Controls the path to a project template used during installation.
        /// </summary>
        /// <remarks>Project templates help the toolbox installer to target a specific kind of project, such as PocketPC, Smartphone,
        /// Windows Mobile, or other device platform.</remarks>
        private string _ProjectTemplate;
        /// <summary>
        /// Controls whether this task is an installation or uninstallation.
        /// </summary>
        public bool _IsUninstallation;

        /// <summary>
        /// Creates a new install task.
        /// </summary>
        public Task(string assemblyPath, string toolboxTab, string projectTemplate) 
        {
            _AssemblyPath = assemblyPath;
            _ToolboxTab = toolboxTab;
            _ProjectTemplate = projectTemplate;
        }

        /// <summary>
        /// Creates a new uninstall task.
        /// </summary>
        /// <param name="toolboxTab"></param>
        public Task(string toolboxTab)
        {
            _ToolboxTab = toolboxTab;
            _IsUninstallation = true;
        }

        /// <summary>
        /// Occurs when progress has been made during a task.
        /// </summary>
        public event EventHandler ProgressChanged;

        /// <summary>
        /// Occurs when progress has been made during a task.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProgressChanged()
        {
            if (ProgressChanged != null)
                ProgressChanged(null, EventArgs.Empty);
        }

        /// <summary>
        /// Performs an installation of toolbox items.
        /// </summary>
        private void Install(DTE visualStudio)
        {
            // Progress has occurred
            OnProgressChanged();

            // Locate the project template necessary to access the Toolbox
            string tmpFile = Path.GetFileNameWithoutExtension(Path.GetTempFileName());
            string tmpDir = string.Format("{0}{1}", Path.GetTempPath(), tmpFile);
            Solution2 solution = visualStudio.Solution as Solution2;
            string templatePath = solution.GetProjectTemplate(_ProjectTemplate, "CSharp");

            // Progress has occurred
            OnProgressChanged();

            // Create a temporary project to get at the Toolbox for that platform
            Project proj = solution.AddFromTemplate(templatePath, tmpDir, tmpFile, false);

            // Progress has occurred
            OnProgressChanged();

            // Get a handle to the toolbox itself
            EnvDTE.Window window = visualStudio.Windows.Item(EnvDTE.Constants.vsWindowKindToolbox);
            EnvDTE.ToolBox toolbox = (EnvDTE.ToolBox)window.Object;

            // Check to see if the Toolbox tab already exists
            ToolBoxTab CurrentTab = null;
            foreach (ToolBoxTab tab in toolbox.ToolBoxTabs)
            {
                // Is there a match?
                if (tab.Name == _ToolboxTab)
                {
                    // Yes.
                    CurrentTab = tab;
                    break;
                }
            }

            // Is there an existing tab?
            if (CurrentTab == null)
            {
                // No.  Create a new one!
                CurrentTab = toolbox.ToolBoxTabs.Add(_ToolboxTab);
            }

            // Progress has occurred
            OnProgressChanged();

            // Select the Toolbox tab
            CurrentTab.Activate();

            // And add the control to it
            CurrentTab.ToolBoxItems.Add("MyUserControl", _AssemblyPath, vsToolBoxItemFormat.vsToolBoxItemFormatDotNETComponent);

            // Progress has occurred
            OnProgressChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Execute(DTE visualStudio2005, DTE visualStudio2008)
        {
            if (_IsUninstallation)
            {
                // Do we need to perform the task for 2005?
                if (visualStudio2005 != null)
                    Uninstall(visualStudio2005);
                // Do we need to perform the task for 2008?
                if (visualStudio2008 != null)
                    Uninstall(visualStudio2008);
            }
            else
            {
                // Do we need to perform the task for 2005?
                if (visualStudio2005 != null)
                    Install(visualStudio2005);
                // Do we need to perform the task for 2008?
                if (visualStudio2008 != null)
                    Install(visualStudio2008);
            }
        }

        private void Uninstall(DTE visualStudio)
        {
            // Progress has occurred
            OnProgressChanged();

            // Get a handle to the toolbox itself
            EnvDTE.Window window = visualStudio.Windows.Item(EnvDTE.Constants.vsWindowKindToolbox);
            EnvDTE.ToolBox toolbox = (EnvDTE.ToolBox)window.Object;

            // Check to see if the Toolbox tab already exists
            ToolBoxTab CurrentTab = null;
            foreach (ToolBoxTab tab in toolbox.ToolBoxTabs)
            {
                // Is there a match?
                if (tab.Name == _ToolboxTab)
                {
                    // Yes.
                    CurrentTab = tab;
                    break;
                }
            }

            // Is there an existing tab?
            if (CurrentTab != null)
            {
                // Yes.  Get rid of it
                CurrentTab.Activate();
                CurrentTab.Delete();
            }

            // Progress has occurred
            OnProgressChanged();
        }
    }
}
