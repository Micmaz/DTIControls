using System;
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
using System.Drawing.Design;

namespace Web.DataSource
{
    public sealed class DataSetDataSourceView : DataSourceView
    {
        private string m_DataTableName;
        private DataSetDataSource m_Owner;

        public DataSetDataSourceView(DataSetDataSource owner, string dataTableName)
            : base(owner, dataTableName)
        {
            m_Owner = owner;
            m_DataTableName = dataTableName;
        }

        private DataTable DataTable
        {
            get { return m_Owner.DataSet.Tables[m_DataTableName]; }
        }

        public override bool CanSort
        {
            get { return true; }
        }

        public override bool CanRetrieveTotalRowCount
        {
            get { return false; }
        }

        public override bool CanDelete
        {
            get { return true; }
        }

        public override bool CanPage
        {
            get { return false; }
        }

        public override bool CanInsert
        {
            get { return true; }
        }

        public override bool CanUpdate
        {
            get { return true; } 
        }

        protected override int ExecuteUpdate(IDictionary keys, IDictionary values, IDictionary oldValues)
        {
            DataRow row = DataTable.Rows.Find(GetKeyValue(keys, DataTable));

            if (row != null)
            {
                foreach (string key in values.Keys)
                {
                    if(!row.Table.Columns[key].ReadOnly)
                        row[key] = Convert.ChangeType(values[key], DataTable.Columns[key].DataType);
                }

                this.RaiseChangedEvent();

                return 1;
            }

            return 0;
        }

        protected override int ExecuteDelete(IDictionary keys, IDictionary oldValues)
        {
            DataRow row = DataTable.Rows.Find(GetKeyValue(keys, DataTable));

            if (row != null)
            {
                row.Delete();
                this.RaiseChangedEvent();

                return 1;
            }

            return 0;
        }

        protected override int ExecuteInsert(IDictionary values)
        {
            DataRow row = DataTable.NewRow();

            foreach (string key in values.Keys)
            {
                row[key] = Convert.ChangeType(values[key], DataTable.Columns[key].DataType);
            }

            DataTable.Rows.Add(row);
            this.RaiseChangedEvent();

            return 1;
        }

        protected override IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
        {			
            if (this.CanSort)
            {
                arguments.AddSupportedCapabilities(DataSourceCapabilities.Sort);
            }
            if (this.CanPage)
            {
                arguments.AddSupportedCapabilities(DataSourceCapabilities.Page);
            }
            if (this.CanRetrieveTotalRowCount)
            {
                arguments.AddSupportedCapabilities(DataSourceCapabilities.RetrieveTotalRowCount);
            }
            arguments.RaiseUnsupportedCapabilitiesError(this);                      

            if (!String.IsNullOrEmpty(arguments.SortExpression))
            {
                DataView sortedView = new DataView(DataTable);
                sortedView.Sort = arguments.SortExpression;
                return sortedView;
            }

            return DataTable.DefaultView;
        }

        internal void RaiseChangedEvent()
        {
            OnDataSourceViewChanged(EventArgs.Empty);
        }

        private object[] GetKeyValue(IDictionary oldValues, DataTable table)
        {
            object[] keyValues = new object[table.PrimaryKey.Length];

            int keyIndex = 0;
            foreach (DataColumn column in table.PrimaryKey)
            {
                keyValues[keyIndex] = Convert.ChangeType(oldValues[column.ColumnName], column.DataType);
                keyIndex++;
            }

            return keyValues;
        }

    }
}