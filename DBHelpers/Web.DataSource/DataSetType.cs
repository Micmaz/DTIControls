using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Web.DataSource
{
    [TypeConverter(typeof(DataSetTypeTypeConverter)), Serializable]
    public class DataSetType
    {
        private string m_DataSetType;

        public DataSetType(string dataSetType)
        {
            m_DataSetType = dataSetType;
        }

        public string Name
        {
            get { return m_DataSetType; }
            set { m_DataSetType = value; }
        }
    }
}
