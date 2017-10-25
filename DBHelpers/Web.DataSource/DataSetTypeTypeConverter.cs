using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.ComponentModel.Design;
using System.Globalization;

namespace Web.DataSource
{
    internal class DataSetTypeTypeConverter : TypeConverter
    {
        public DataSetTypeTypeConverter()
        {
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context != null)
            {
                ITypeDiscoveryService service1 = (ITypeDiscoveryService)context.GetService(typeof(ITypeDiscoveryService));
                if (service1 != null)
                {
                    List<string> ret = new List<string>();

                    foreach (Type type in service1.GetTypes(typeof(System.Data.DataSet), true))
                    {
                        ret.Add(type.FullName);
                    }
                    return new StandardValuesCollection(ret.ToArray());
                }

            }
            return new StandardValuesCollection(new string[] { });
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
            if (destinationType == typeof(InstanceDescriptor))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }


        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string valueString = value as string;
            if (valueString != null)
            {
                return new DataSetType(valueString);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            DataSetType parameterDataSetTypeName = value as DataSetType;

            if (destType == typeof(InstanceDescriptor))
            {
                string parameter = (parameterDataSetTypeName != null ? parameterDataSetTypeName.Name : (string)value);
                return new InstanceDescriptor(typeof(DataSetType).GetConstructor(new Type[] { typeof(string) }), new object[] { parameter });
            }

            if (destType == typeof(string))
            {
                if (value is string)
                {
                    return value;
                }
                else if (parameterDataSetTypeName != null)
                {
                    return parameterDataSetTypeName.Name;
                }
            }

            return base.ConvertTo(context, culture, value, destType);
        }
    }

}
