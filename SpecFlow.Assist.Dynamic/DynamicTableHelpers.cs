using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using ImpromptuInterface;
using TechTalk.SpecFlow;

namespace SpecFlow.Assist.Dynamic
{
    public static class DynamicTableHelpers
    {
        private const string ERRORMESS_NOT_ON_TABLE = "The '{0}' value not present in the table, but on the instance";
        private const string ERRORMESS_NOT_ON_INSTANCE = "The '{0}' value not present on the instance, but in the table";
        private const string ERRORMESS_VALUE_DIFFERS =
            "The '{0}' value differs from table and instance.\n\tInstance: '{1}'.\n\tTable: '{2}'";

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

        /// <summary>
        /// Validates if a dynamic instance <paramref name="instance"/> matches the <paramref name="table"/>
        /// </summary>
        /// <param name="table">the table to compare the instance against</param>
        /// <param name="instance">the instance to compare the table against</param>
        public static void CompareToDynamicInstance(this Table table, dynamic instance)
        {
            // Get all the headers from the table, in PropertyFormat
            var tableHeadersAsPropertyNames = table.Header.Select(CreatePropertyName);

            // Get all the members from the types
            IEnumerable<string> instanceMembers = Impromptu.GetMemberNames(instance);

            var propDiffs = GetPropertyNameDifferences(tableHeadersAsPropertyNames, instanceMembers);
            if (propDiffs.Any())
                throw new DynamicInstanceComparisonException(propDiffs);

            // Kolla värdena
            var valueDiffs = new List<string>();
            foreach (var header in table.Header)
            {
                var propertyName = CreatePropertyName(header);
                var valueFromInstance = Impromptu.InvokeGet(instance, propertyName);
                var valueFromTable = CreateTypedValue(table.Rows[0][header]);

                if (!valueFromInstance.Equals(valueFromTable))
                {
                    var mess = string.Format(ERRORMESS_VALUE_DIFFERS, propertyName, valueFromInstance, valueFromTable);
                    valueDiffs.Add(mess);
                }
            }

            if (valueDiffs.Any())
                throw new DynamicInstanceComparisonException(valueDiffs);
        }

        private static IList<string> GetPropertyNameDifferences(IEnumerable<string> tableHeadersAsPropertyNames, IEnumerable<string> instanceMembers)
        {
            var allMembersInTableButNotInInstance = tableHeadersAsPropertyNames.Except(instanceMembers);
            var allMembersInInstanceButNotInTable = instanceMembers.Except(tableHeadersAsPropertyNames);

            var diffs = new List<string>();

            diffs.AddRange(
                allMembersInInstanceButNotInTable.Select(
                    m => string.Format(ERRORMESS_NOT_ON_TABLE, m)));

            diffs.AddRange(
                allMembersInTableButNotInInstance.Select(
                    m => string.Format(ERRORMESS_NOT_ON_INSTANCE, m)));

            return diffs;
        }

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

    public class DynamicInstanceComparisonException : Exception
    {
        public IList<string> Differences { get; private set; }
        public DynamicInstanceComparisonException(IList<string> diffs) : base("There were some difference between the table and the instance")
        {
            Differences = diffs;
        }
    }

    public class DynamicInstanceFromTableException : Exception
    {
        public DynamicInstanceFromTableException(string message) : base(message) { }
    }
}
