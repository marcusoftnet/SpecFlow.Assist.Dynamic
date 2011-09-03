using System;
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

        [Then(@"the error message for different rows should expect (.*) for table and (.*) for instance")]
        public void ShouldDifferInRowCount(string tableRowCountString, string instanceRowCountString)
        {
            var message = GetSetComparisonException().Message;
            message.Should().Contain(tableRowCountString);
            message.Should().Contain(instanceRowCountString);
        }

    }

}
