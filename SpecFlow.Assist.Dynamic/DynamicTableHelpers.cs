using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using TechTalk.SpecFlow;

namespace SpecFlow.Assist.Dynamic
{
    public static class DynamicTableHelpers
    {
        private const string FIELD = "Field";
        private const string VALUE = "Value";

        /// <summary>
        /// Create a dynamic object from the headers and values of the <paramref name="table"/>
        /// </summary>
        /// <param name="table">the table to create a dynamic object from</param>
        /// <returns>the created object</returns>
        public static ExpandoObject CreateDynamicInstance(this Table table)
        {
            if (table.Header.Count == 2 &&
                    table.Header.Contains(FIELD) &&
                    table.Header.Contains(VALUE))
            {
                var horizontalTable = CreateHorizontalTable(table);
                return CreateDynamicInstance(horizontalTable.Rows[0]);
            }

            if (table.RowCount == 1)
            {
                return CreateDynamicInstance(table.Rows[0]);
            }

            throw new DynamicInstanceFromTableException(
                string.Format("Can only create instances of tables with one row, or with the {0} and {1} columns", FIELD, VALUE));
        }

        /// <summary>
        /// Creates a set of dynamic objects based of the <paramref name="table"/> headers and values
        /// </summary>
        /// <param name="table">the table to create a set of dynamics from</param>
        /// <returns>a set of dynamics</returns>
        public static IList<object> CreateDynamicSet(this Table table)
        {
            return table.Rows.
                Select(CreateDynamicInstance).Cast<object>().ToList<object>();
        }

        //public static void CompareToDynamicInstance(Table table, dynamic instance)
        //{
        //    var expandoDic = instance as IDictionary<string, object>;

        //    var diffs = new List<string>();

        //    foreach (var header in table.Header)
        //    {
        //        var propName = CreatePropertyName(header);
        //        var propValue = CreateTypedValue(table.Rows[0][header]);

        //        if (!expandoDic.ContainsKey(propName))
        //        {
        //            diffs.Add("No prop");
        //        }
        //        if (!expandoDic.Values.Contains(propValue))
        //        {
        //            diffs.Add("No value");
        //        }
        //    }

        //}

        private static Table CreateHorizontalTable(Table verticalFieldValueTable)
        {

            var dic = verticalFieldValueTable.
                            Rows.ToDictionary(row => row[FIELD], row => row[VALUE]);

            var horizontalTable = new Table(dic.Keys.ToArray());
            horizontalTable.AddRow(dic);
            return horizontalTable;
        }

        private static ExpandoObject CreateDynamicInstance(TableRow tablerow)
        {
            dynamic expando = new ExpandoObject();
            var dicExpando = expando as IDictionary<string, object>;

            foreach (var header in tablerow.Keys)
            {
                var propName = CreatePropertyName(header);
                var propValue = CreateTypedValue(tablerow[header]);
                dicExpando.Add(propName, propValue);
            }

            return expando;
        }

        private static object CreateTypedValue(string valueFromTable)
        {
            // TODO: More types here?
            DateTime dt;
            if (DateTime.TryParse(valueFromTable, out dt))
                return dt;

            int i;
            if (int.TryParse(valueFromTable, out i))
                return i;

            double d;
            if (Double.TryParse(valueFromTable, out d))
                return d;

            return valueFromTable;
        }

        private static string CreatePropertyName(string header)
        {
            var arr = header.Split(' ');
            var propName = arr[0]; // leave the first element as is, since it might be correct cased...

            string s;
            for (var i = 1; i < arr.Length; i++)
            {
                s = arr[i];
                propName += s[0].ToString().ToUpperInvariant() +
                            s.Substring(1).ToLowerInvariant();
            }

            return propName;
        }
    }

    public class DynamicInstanceFromTableException : Exception
    {
        public DynamicInstanceFromTableException(string message) : base(message) { }
    }
}
