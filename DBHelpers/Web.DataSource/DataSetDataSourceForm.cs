using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using EnvDTE;
using System.Windows.Forms;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell.Design;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;

namespace Web.DataSource
{
    public partial class DataSetDataSourceForm : Form
    {
        private DataSetDataSource m_DataSource;
        private DataSetType m_DataSetType;
        
        public DataSetDataSourceForm()
        {
            InitializeComponent();
        }

        public void Initialize(System.IServiceProvider serviceProvider, DataSetDataSource dataSource)        
        {
            m_DataSetType = dataSource.DataSetType;
            m_DataSource = dataSource;
            
            DynamicTypeService typeService = (DynamicTypeService)serviceProvider.GetService(typeof(DynamicTypeService));
            //Debug.Assert(typeService != null, "No dynamic type service registered.");

            IVsHierarchy hier = VsHelper.GetCurrentHierarchy(serviceProvider);
            //Debug.Assert(hier != null, "No active hierarchy is selected.");

            ITypeDiscoveryService discovery = typeService.GetTypeDiscoveryService(hier);
            
            //Project dteProject = VsHelper.ToDteProject(hier);

            this.comboBoxComponent.DataSource = discovery.GetTypes(typeof(DataSet), false);
                this.comboBoxComponent.DisplayMember = "FullName";
                if (dataSource.DataSetType != null)
                {
                    this.comboBoxComponent.SelectedItem = dataSource.DataSetType;
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_DataSource.DataSetType = new DataSetType(((Type)comboBoxComponent.SelectedItem).FullName);         
        }
    }
}