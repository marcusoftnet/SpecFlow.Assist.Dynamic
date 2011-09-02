using System;
using Should.Fluent;
using SpecFlow.Assist.Dynamic;
using TechTalk.SpecFlow;

namespace Specs.Steps
{
    [Binding]
    public class DynamicInstanceCreationSteps
    {

        [Given(@"I create a dynamic instance from this table")]
        [When(@"I create a dynamic instance from this table")]
        public void CreateDynamicInstanceFromTable(Table table)
        {
            State.Instance = table.CreateDynamicInstance();
        }

        [Then(@"the Name property should equal '(.*)'")]
        public void NameShouldBe(string expectedValue)
        {
            ((string)State.Instance.Name).Should().Equal(expectedValue);
        }

        [Then(@"the Age property should equal (\d+)")]
        public void AgeShouldBe(int expectedAge)
        {
            ((int)State.Instance.Age).Should().Equal(expectedAge);
        }

        [Then(@"the BirthDate property should equal (.*)")]
        public void BirthDateShouldBe(string expectedDate)
        {
            ((DateTime)State.Instance.BirthDate).Should().Equal(DateTime.Parse(expectedDate));
        }

        [Then(@"the LengthInMeters property should equal (.*)")]
        public void LengthInMeterShouldBe(string doubleString)
        {
            var expectedDouble = double.Parse(doubleString);
            ((double)State.Instance.LengthInMeters).Should().Equal(expectedDouble);
        }
    }
}
