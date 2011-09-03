using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using ImpromptuInterface;

namespace TechTalk.SpecFlow.Assist
{
    public static class DynamicTableHelpers
    {
        private const string ERRORMESS_PROPERTY_DIFF_SET = "Properties differs between the table and the set";
        private const string ERRORMESS_INSTANCETABLE_FORMAT = "Can only create instances of tables with one row, or exactly 2 columns and several rows";
        private const string ERRORMESS_NOT_ON_TABLE = "The '{0}' value not present in the table, but on the instance";
        private const string ERRORMESS_NOT_ON_INSTANCE = "The '{0}' value not present on the instance, but in the table";
        private const string ERRORMESS_VALUE_DIFFERS =
            "The '{0}' value differs from table and instance.\n\tInstance: '{1}'.\n\tTable: '{2}'";

        private const string ERRORMESS_NUMBER_OF_ROWS_DIFFERS =
            "Number of rows for table ({0} rows) and set ({1} rows) differs";

        /// <summary>
        /// Create a dynamic object from the headers and values of the <paramref name="table"/>
        /// </summary>
        /// <param name="table">the table to create a dynamic object from</param>
        /// <returns>the created object</returns>
        public static ExpandoObject CreateDynamicInstance(this Table table)
        {
            if (table.Header.Count == 2 && table.RowCount > 1)
            {
                var horizontalTable = CreateHorizontalTable(table);
                return CreateDynamicInstance(horizontalTable.Rows[0]);
            }

            if (table.RowCount == 1)
            {
                return CreateDynamicInstance(table.Rows[0]);
            }

            throw new DynamicInstanceFromTableException(ERRORMESS_INSTANCETABLE_FORMAT);
        }

        /// <summary>
        /// Creates a set of dynamic objects based of the <paramref name="table"/> headers and values
        /// </summary>
        /// <param name="table">the table to create a set of dynamics from</param>
        /// <returns>a set of dynamics</returns>
        public static IList<dynamic> CreateDynamicSet(this Table table)
        {
            return table.Rows.
                Select(CreateDynamicInstance).ToList<object>();
        }

        /// <summary>
        /// Validates if a dynamic instance <paramref name="instance"/> matches the <paramref name="table"/>
        /// Throws descriptive exception if not
        /// </summary>
        /// <param name="table">the table to compare the instance against</param>
        /// <param name="instance">the instance to compare the table against</param>
        public static void CompareToDynamicInstance(this Table table, dynamic instance)
        {
            IList<string> propDiffs = GetPropertyDifferences(table, instance);
            if (propDiffs.Any())
                throw new DynamicInstanceComparisonException(propDiffs);

            AssertValuesOfRowDifference(table.Rows[0], instance);
        }

        /// <summary>
        /// Validates that the dynamic set <paramref name="set"/> matches the <paramref name="table"/>
        /// Throws descriptive exception if not
        /// </summary>
        /// <param name="table">the table to compare the set against</param>
        /// <param name="set">the set to compare the table against</param>
        public static void CompareToDynamicSet(this Table table, IList<dynamic> set)
        {
            AssertEqualNumberOfRows(table, set);

            IList<string> propDiffs = GetPropertyDifferences(table, set[0]);
            if (propDiffs.Any())
            {
                throw new DynamicSetComparisonException(ERRORMESS_PROPERTY_DIFF_SET, propDiffs);
            }
        }

        private static void AssertValuesOfRowDifference(TableRow tableRow, dynamic instance)
        {
            IList<string> valueDiffs = ValidateValuesOfRow(tableRow, instance);
            if (valueDiffs.Any())
                throw new DynamicInstanceComparisonException(valueDiffs);
        }

        private static IList<string> GetPropertyDifferences(Table table, dynamic instance)
        {
            var tableHeadersAsPropertyNames = table.Header.Select(CreatePropertyName);
            IEnumerable<string> instanceMembers = Impromptu.GetMemberNames(instance);

            return GetPropertyNameDifferences(tableHeadersAsPropertyNames, instanceMembers);
        }

        private static void AssertEqualNumberOfRows(Table table, IList<object> set)
        {
            if (table.RowCount != set.Count)
            {
                var mess = string.Format(ERRORMESS_NUMBER_OF_ROWS_DIFFERS, table.RowCount, set.Count);
                throw new DynamicSetComparisonException(mess);
            }
        }

        private static IList<string> ValidateValuesOfRow(TableRow tableRow, dynamic instance)
        {
            var valueDiffs = new List<string>();

            foreach (var header in tableRow.Keys)
            {
                var propertyName = CreatePropertyName(header);
                var valueFromInstance = Impromptu.InvokeGet(instance, propertyName);
                var valueFromTable = CreateTypedValue(tableRow[header]);

                if (!valueFromInstance.Equals(valueFromTable))
                {
                    var mess = string.Format(ERRORMESS_VALUE_DIFFERS, propertyName, valueFromInstance, valueFromTable);
                    valueDiffs.Add(mess);
                }
            }
            return valueDiffs;
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
                            Rows.ToDictionary(row => row[0], row => row[1]);

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
}
