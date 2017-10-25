using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.UI.Design;
using System.Web.Compilation;
using System.Collections;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace Web.DataSource
{    
    public class DataSetDataSourceDesigner : DataSourceDesigner
    {
        private Dictionary<string, DesignerDataSourceView> m_DesignerDataSourceView = new Dictionary<string, DesignerDataSourceView>();

        public override string[] GetViewNames()
        {
            if (DataSource.DataSet != null)
            {
                string[] ret = new string[DataSource.DataSet.Tables.Count];
                DataSource.GetViewNamesInternal().CopyTo(ret, 0);
                return ret;
            }

            return new string[] { };
        }

        public override bool CanConfigure
        {
            get { return true; }
        }

        public override bool CanRefreshSchema
        {
            get { return true; }
        }

        public override void Configure()
        {
            ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ConfigureDataSourceChangeCallback), null, "DataSetDataSource");
        }

        private bool ConfigureDataSourceChangeCallback(object context)
        {
            using (DataSetDataSourceForm form = new DataSetDataSourceForm())
            {
                form.Initialize(this.Component.Site, DataSource);
                return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
            }
        }

        public override DesignerDataSourceView GetView(string viewName)
        {
            if (DataSource.DataSet == null)
            {
                return new DataSetDesignerDataSourceView(this, "");
            }

            DesignerDataSourceView ret = null;
            if (String.IsNullOrEmpty(viewName))
            {
                viewName = DataSource.DefaultView;
            }

            if (!m_DesignerDataSourceView.ContainsKey(viewName))
            {
                if (DataSource.DataSet.Tables[viewName] != null)
                {
                    ret = new DataSetDesignerDataSourceView(this, DataSource.DataSet.Tables[viewName]);
                    m_DesignerDataSourceView.Add(viewName, ret);
                }
            }
            else
            {
                ret = m_DesignerDataSourceView[viewName];
            }
            return ret;
        }

        public override void RefreshSchema(bool preferSilent)
        {
            this.OnDataSourceChanged(EventArgs.Empty);
            this.OnSchemaRefreshed(EventArgs.Empty);
        }
        
        internal DataSetDataSource DataSource
        {
            get
            {
                return (DataSetDataSource) base.Component;
            }
        }
    }
}