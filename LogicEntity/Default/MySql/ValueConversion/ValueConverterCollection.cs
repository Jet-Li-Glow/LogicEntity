using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.ValueConversion
{
    public class ValueConverterCollection
    {
        internal Dictionary<PropertyInfo, ValueConverter> PropertyConvert { get; } = new();

        public void Set<TModel, TProvider>(PropertyInfo property, Func<TProvider, TModel> reader, Func<TModel, TProvider> writer)
        {
            PropertyConvert[property] = new() { Reader = reader, Writer = writer };
        }
    }
}
