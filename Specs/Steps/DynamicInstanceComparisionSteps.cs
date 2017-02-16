using System;
using Should.Fluent;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Specs.Steps
{
    [Binding]
    public class DynamicInstanceComparisionSteps
    {

        private const string EXCEPTION_KEY = "ExceptionKey";

        private static DynamicInstanceComparisonException GetInstanceComparisonException()
        {
            var ex = ScenarioContext.Current[EXCEPTION_KEY] as DynamicInstanceComparisonException;
            ex.Should().Not.Be.Null();
            return ex;
        }

        private static void CheckForOneDifferenceContaingString(string expectedString)
        {
            var ex = GetInstanceComparisonException();
            ex.Differences.Should().Contain.One(f => f.Contains(expectedString));
        }

        [When("I compare it to this table")]
        public void ComparingAgainstDynamicInstance(Table table)
        {
            try
            {
                table.CompareToDynamicInstance((object)State.OriginalInstance);
            }
            catch (DynamicInstanceComparisonException ex)
            {
                ScenarioContext.Current.Add(EXCEPTION_KEY, ex);
            }
        }



        [Then("no instance comparison exception should have been thrown")]
        public void NoException()
        {
            ScenarioContext.Current.ContainsKey(EXCEPTION_KEY).Should().Be.False();
        }

        [Then(@"an instance comparison exception should be thrown with (\d+) differences")]
        [Then(@"an instance comparison exception should be thrown with (\d+) difference")]
        public void ExceptionShouldHaveBeenThrown(int expectedNumberOfDifferences)
        {
            ScenarioContext.Current.ContainsKey(EXCEPTION_KEY).Should().Be.True();

            var ex = GetInstanceComparisonException();
            ex.Differences.Should().Count.Exactly(expectedNumberOfDifferences);
        }

        [Then(@"one difference should be on the (.*) column of the table")]
        public void DifferenceOnTheColumnOfTheTable(string expectedColumnToDiffer)
        {
            CheckForOneDifferenceContaingString(expectedColumnToDiffer);
        }

        [Then(@"one difference should be on the (.*) field of the instance")]
        public void DifferenceOnFieldOfInstance(string expectedFieldToDiffer)
        {
            CheckForOneDifferenceContaingString(expectedFieldToDiffer);
        }

        [Then(@"one message should state that the instance had the value (.*)")]
        public void ExceptionMessageValueOnInstance(string expectedValueOfInstance)
        {
            CheckForOneDifferenceContaingString(expectedValueOfInstance);
        }

        [Then(@"one message should state that the table had the value (.*)")]
        public void ExceptionMessageValueInTable(string expectedValueOfTable)
        {
            CheckForOneDifferenceContaingString(expectedValueOfTable);
        }

        [Then(@"one difference should be on the (.*) property")]
        public void ExceptionMessageValueOnProperty(string expectedPropertyName)
        {
            CheckForOneDifferenceContaingString(expectedPropertyName);
        }

        [When(@"I compare it to this table using no type conversion")]
        public void WhenICompareItToThisTableUsingNoTypeConversion(Table table)
        {
            try
            {
                table.CompareToDynamicInstance((object)State.OriginalInstance, false);
            }
            catch (DynamicInstanceComparisonException ex)
            {
                ScenarioContext.Current.Add(EXCEPTION_KEY, ex);
            }
        }

    }
}
