using System;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Web.Compilation;
using System.Web.UI.Design.WebControls;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Collections.Specialized;
using System.Drawing;
using System.Security.Permissions;

/// <summary>
/// Summary description for DataSetDataSource
/// </summary>
namespace Web.DataSource
{

    [PersistChildren(false),
    Designer("Web.DataSource.DataSetDataSourceDesigner, Web.DataSource"), 
    ParseChildren(true), 
    ToolboxBitmap(typeof(System.Data.DataSet)), 
    DefaultProperty("TypeName"), 
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal), 
    AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class DataSetDataSource : DataSourceControl
    {
        private DataSet m_DataSet;
        private Dictionary<string, DataSourceView> m_DataSourceViews = new Dictionary<string, DataSourceView>();
        private Type m_DataSetType;

        /// <summary>
        /// It is the key that will be used when storing the control information in the session. 
        /// It's always stored in the ViewState
        /// </summary>
        private string m_DataSourceId;

        public DataSetDataSource()
        {
            this.Unload += new EventHandler(DataSetDataSource_Unload);
            this.Load += new EventHandler(DataSetDataSource_Load);         
        }

        private void DataSetDataSource_Load(object sender, EventArgs e)
        {
            if (!DesignMode && !EnableViewState)
            {
                if (m_DataSourceId != null)
                {
                    m_DataSet = (DataSet)Context.Session[m_DataSourceId];
                }
                else
                {
                    m_DataSourceId = Guid.NewGuid().ToString();
                }
            }
        }

        private void DataSetDataSource_Unload(object sender, EventArgs e)
        {
            if (!DesignMode && !EnableViewState)
            {
                this.Context.Session[m_DataSourceId] = m_DataSet;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            Page.RegisterRequiresControlState(this);
            base.OnInit(e);
        }

        
        [Category("DataSet")]
        public DataSetType DataSetType
        {
            set
            {
                if (value == null)
                {
                    m_DataSetType = null;
                }
                else
                {
                    m_DataSetType = System.Web.Compilation.BuildManager.GetType(value.Name, false);
                }
            }
            get
            {
                if (m_DataSetType == null)
                {
                    return null;
                }

                return new DataSetType(m_DataSetType.FullName);
            }
        }

        [Browsable(false)]
        [EditorBrowsable(0)]
        public DataSet DataSet 
        {
            set
            {
                m_DataSet = value;
            }
            get
            {
                if (m_DataSet == null)
                {
                    if (m_DataSetType != null)
                    {
                        m_DataSet = (DataSet) Activator.CreateInstance(m_DataSetType);
                    }
                }

                return m_DataSet;
            }
        }


        internal void RaiseChangedEvent()
        {
            ((DataSetDataSourceView)this.GetView(DefaultView)).RaiseChangedEvent();
        }
        
        [Browsable(false)]
        public string DefaultView
        {
            get
            {
                if (DataSet != null)
                {
                    return Util.GetRootTable(DataSet).TableName;
                }

                return "";
            }
        }

        protected override DataSourceView GetView(string viewName)
        {
            DataSourceView ret = null;

            if (String.IsNullOrEmpty(viewName))
            {
                viewName = DefaultView;
            }

            if (this.DataSet == null)
            {
                return new DataSetDataSourceView(this, null);
            }

            if (!m_DataSourceViews.ContainsKey(viewName))
            {
                if (DataSet.Tables[viewName] != null)
                {
                    ret = new DataSetDataSourceView(this, viewName);
                    m_DataSourceViews.Add(viewName, ret);
                }
            }
            else
            {
                ret = m_DataSourceViews[viewName];
            }

            return ret;
        }

        public ICollection GetViewNamesInternal()
        {
            return GetViewNames();
        }

        protected override ICollection GetViewNames()
        {
            string[] ret = new string[DataSet.Tables.Count];
            int idx = 0;
            foreach (DataTable table in DataSet.Tables)
            {
                ret[idx] = table.TableName;
                idx++;
            }
            return ret;
        }

        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);

            if (EnableViewState)
            {
                DataSet dataSet = (DataSet) ViewState[this.UniqueID];

                if (dataSet != null)
                {
                    m_DataSet = dataSet;
                }
            }
        }

        protected override object SaveControlState()
        {
            if (!EnableViewState)
            {
                return m_DataSourceId;    
            }
            
            return base.SaveControlState();
        }

        protected override void LoadControlState(object savedSate)
        {
            if (savedSate != null)
            {
                if (!EnableViewState)
                {
                    this.m_DataSourceId = (string) savedSate;
                }
            }
        }

        protected override object SaveViewState()
        {
            if (EnableViewState)
            {
                ViewState[this.UniqueID] = m_DataSet;
            }
            else
            {
                if (m_DataSourceId == null)
                    m_DataSourceId = Guid.NewGuid().ToString();

                ViewState["dataSourceId"] = m_DataSourceId;
            }

            return base.SaveViewState();
        }
              
        private void OnParametersChanged(object sender, EventArgs e)
        {
            this.RaiseChangedEvent();
        }
    }
}