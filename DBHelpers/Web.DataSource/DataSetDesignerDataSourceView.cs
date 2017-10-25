using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.Design;
using System.Data;
using System.Collections;

namespace Web.DataSource
{
    internal sealed class DataSetDesignerDataSourceView : DesignerDataSourceView
    {
        private DataTable m_DataTable;

        public DataSetDesignerDataSourceView(DataSetDataSourceDesigner owner, string dataTableName)
            : base(owner, dataTableName)
        {
        }

        public DataSetDesignerDataSourceView(DataSetDataSourceDesigner owner, DataTable dataTable)
            : this(owner, dataTable.TableName)
        {
            m_DataTable = dataTable;
        }

        public override IEnumerable GetDesignTimeData(int minimumRows, out bool isSampleData)
        {

            if (m_DataTable == null)
            {
                return base.GetDesignTimeData(minimumRows, out isSampleData);
            }

            isSampleData = true;

            using (DataView view = new DataView(m_DataTable))
            {
                return DesignTimeData.GetDesignTimeDataSource(DesignTimeData.CreateSampleDataTable(view, true), minimumRows);
            }
        }

        public override IDataSourceViewSchema Schema
        {
            get
            {
                if (m_DataTable == null)
                    return null;

                TypeSchema ts = new TypeSchema(m_DataTable.GetType());
                return ts.GetViews()[0];
            }
        }
    }
}
