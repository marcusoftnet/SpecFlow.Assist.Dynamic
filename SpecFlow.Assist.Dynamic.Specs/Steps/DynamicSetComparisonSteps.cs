using Should.Fluent;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Specs.Steps
{
    [Binding]
    public class DynamicSetComparisonSteps
    {
        private const string EXCEPTION_KEY = "ExceptionKey";

        private static DynamicSetComparisonException GetSetComparisonException()
        {
            return ScenarioContext.Current[EXCEPTION_KEY] as DynamicSetComparisonException;
        }

        private static void CheckForOneDifferenceContaingString(string expectedString)
        {
            var ex = GetSetComparisonException();
            ex.Differences.Should().Contain.One(f => f.Contains(expectedString));
        }

        [When(@"I compare the set to this table")]
        public void CompareSetToInstance(Table table)
        {
            try
            {
                table.CompareToDynamicSet(State.OriginalSet);
            }
            catch (DynamicSetComparisonException ex) 
            {
                ScenarioContext.Current.Add(EXCEPTION_KEY, ex);
            }
        }

        [When(@"I compare the set to this table using no type conversion")]
        public void CompareSetToInstanceNoConversion(Table table)
        {
            try
            {
                table.CompareToDynamicSet(State.OriginalSet, false);
            }
            catch (DynamicSetComparisonException ex)
            {
                ScenarioContext.Current.Add(EXCEPTION_KEY, ex);
            }
        }

        [Then(@"no set comparison exception should have been thrown")]
        public void NoSetExceptionThrown()
        {
            ScenarioContext.Current.ContainsKey(EXCEPTION_KEY).Should().Be.False();
        }

        [Then(@"an set comparison exception should be thrown")]
        public void SetComparisonExceptionThrown()
        {
            GetSetComparisonException().Should().Not.Be.Null();
        }

        [Then(@"an set comparision exception should be thrown with (\d+) differences")]
        [Then(@"an set comparision exception should be thrown with (\d+) difference")]
        public void SetComparisionExceptionWithNumberOfDifferences(int expectedNumberOfDifference)
        {
            SetComparisonExceptionThrown();
            GetSetComparisonException().Differences.Count.Should().Equal(expectedNumberOfDifference);
        }


        [Then(@"the error message for different rows should expect (.*) for table and (.*) for instance")]
        public void ShouldDifferInRowCount(string tableRowCountString, string instanceRowCountString)
        {
            var message = GetSetComparisonException().Message;
            message.Should().Contain(tableRowCountString);
            message.Should().Contain(instanceRowCountString);
        }

        [Then(@"one set difference should be on the (.*) column of the table")]
        public void DifferenceOnTheColumnOfTheTable(string expectedColumnToDiffer)
        {
            CheckForOneDifferenceContaingString(expectedColumnToDiffer);
        }

        [Then(@"one set difference should be on the (.*) field of the instance")]
        public void DifferenceOnFieldOfInstance(string expectedFieldToDiffer)
        {
            CheckForOneDifferenceContaingString(expectedFieldToDiffer);
        }

        [Then(@"(\d+) difference should be on row (\d+) on property '(.*)' for the values '(.*)' and '(.*)'")]
        public void DifferenceOnValue(int differenceNumber, int rowNumber, string expectedProperty, string instanceValue, string tableRowValue)
        {
            var exception = GetSetComparisonException();
            var difference = exception.Differences[differenceNumber -1];
            difference.Contains("'" + rowNumber + "'");
            difference.Contains("'" + expectedProperty + "'");
            difference.Contains("'" + instanceValue + "'");
            difference.Contains("'" + tableRowValue + "'");
        }

    }

}
