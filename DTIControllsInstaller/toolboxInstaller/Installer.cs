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
    public sealed class Installer : IOleMessageFilter, IDisposable
    {
        #region Private variables

        // Represents a list of installation or uninstallation tasks
        private ArrayList _Tasks = new ArrayList();
        // Represents a list of solution projects
        private ArrayList _SolutionProjects = new ArrayList();
        // Controls whether Visual Studio 2005 or 2008 is being used
        private bool _IsVisualStudio2005 = false;
        private bool _IsVisualStudio2008 = false;
        // Represents the Visual Studio 2005 design-time environment
        private DTE DesignTimeEnvironment2005;
        // Represents the Visual Studio 2008 design-time environment
        private DTE DesignTimeEnvironment2008;
        // Used to hook into OLE messages during installation
        private IOleMessageFilter _OldOleFilter;

        #endregion

        #region Events

        /// <summary>
        /// Occurs before the first task begins.
        /// </summary>
        public event EventHandler OperationStarted;
        /// <summary>
        /// Occurs when progress has been made on any task.
        /// </summary>
        public event EventHandler TaskProgressOccurred;
        /// <summary>
        /// Occurs after the last task has been completed.
        /// </summary>
        public event EventHandler OperationCompleted;

        #endregion

        #region Constructor / Finalizer

        public Installer()
        {
            // Hook into OLE messages
            CoRegisterMessageFilter(this, out _OldOleFilter);
        }

        ~Installer()
        {
            Dispose();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns a list of install/uninstall tasks for the installer to accomplish.
        /// </summary>
        public ArrayList Tasks
        {
            get
            {
                return _Tasks;
            }
        }

        /// <summary>
        /// Controls whether the Visual Studio 2005 toolbox will be affected.
        /// </summary>
        public bool IsVisualStudio2005
        {
            get
            {
                return _IsVisualStudio2005;
            }
            set
            {
                _IsVisualStudio2005 = value;
            }
        }

        /// <summary>
        /// Controls whether the Visual Studio 2008 toolbox will be affected.
        /// </summary>
        public bool IsVisualStudio2008
        {
            get
            {
                return _IsVisualStudio2008;
            }
            set
            {
                _IsVisualStudio2008 = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs all installation and uninstallation tasks
        /// </summary>
        public void Execute()
        {
            // Signal that tasks have been started
            OnOperationStarted();

            #region Create instances of the Visual Studio IDE

            if (_IsVisualStudio2005)
            {
                // Try to create an instance of Visual Studio 2005
                try
                {
                    DesignTimeEnvironment2005 = (DTE)System.Activator.CreateInstance(Type.GetTypeFromProgID("VisualStudio.DTE.8.0"), true);
                }
                catch
                {
                    // Visual Studio 2005 is not installed
                    DesignTimeEnvironment2005 = null;
                }
            }

            if (_IsVisualStudio2008)
            {
                // Try to create an instance of Visual Studio 2008
                try
                {
                    DesignTimeEnvironment2008 = (DTE)System.Activator.CreateInstance(Type.GetTypeFromProgID("VisualStudio.DTE.9.0"), true);
                }
                catch
                {
                    // Visual Studio 2008 is not installed
                    DesignTimeEnvironment2008 = null;
                }
            }

            #endregion

            // Receive notifications any time a task achieves progress
            foreach (Task task in _Tasks)
            {
                task.ProgressChanged += new EventHandler(task_ProgressChanged);
            }

            // Perform tasks
            foreach (Task task in _Tasks)
            {
                // Execute the task, passing along the instances of VS2005 and VS2008 to work with.
                // We want to recycle instances as much as possible since they are expensive to create.
                task.Execute(DesignTimeEnvironment2005, DesignTimeEnvironment2008);
            }

            // Signal that tasks are complete
            OnOperationCompleted();
        }


        private void OnOperationStarted()
        {
            if (OperationStarted != null)
                OperationStarted(this, EventArgs.Empty);
        }

        void task_ProgressChanged(object sender, EventArgs e)
        {
            OnTaskProgressOccurred();
        }

        private void OnTaskProgressOccurred()
        {
            if (TaskProgressOccurred != null)
                TaskProgressOccurred(this, EventArgs.Empty);
        }

        private void OnOperationCompleted()
        {
            if (OperationCompleted != null)
                OperationCompleted(this, EventArgs.Empty);
        }

        #endregion

        #region Static methods

        [DllImport("ole32.dll")]
        static extern int CoRegisterMessageFilter(IOleMessageFilter newFilter, out IOleMessageFilter oldFilter);

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // We no longer need to finalize
            GC.SuppressFinalize(this);

            // Close all instances of Visual Studio
            if (DesignTimeEnvironment2005 != null)
            {
                // Close the solution
                DesignTimeEnvironment2005.Solution.Close(false);

                // Release the DTE so that it can shut down
                Marshal.ReleaseComObject(DesignTimeEnvironment2005);
            }

            // Close all instances of Visual Studio
            if (DesignTimeEnvironment2008 != null)
            {
                // Close the solution
                DesignTimeEnvironment2008.Solution.Close(false);

                // Release the DTE so that it can shut down
                Marshal.ReleaseComObject(DesignTimeEnvironment2008);
            }

            // Stop listening for OLE messages
            CoRegisterMessageFilter(null, out _OldOleFilter);

            // Perform garbage collection to ensure disposal (?)
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        void IDisposable.Dispose()
        {
            Dispose();
        }

        #endregion

        #region IOleMessageFilter Members

        int IOleMessageFilter.HandleInComingCall(int dwCallType, IntPtr hTaskCaller, int dwTickCount, IntPtr lpInterfaceInfo)
        {
            return 0; //SERVERCALL_ISHANDLED
        }

        int IOleMessageFilter.RetryRejectedCall(IntPtr hTaskCallee, int dwTickCount, int dwRejectType)
        {
            if (dwRejectType == 2) // SERVERCALL_RETRYLATER
                return 50; // wait .5 seconds and try again
            return -1; // cancel call
        }

        int IOleMessageFilter.MessagePending(IntPtr hTaskCallee, int dwTickCount, int dwPendingType)
        {
            return 2; //PENDINGMSG_WAITDEFPROCESS
        }

        #endregion
    }
}
