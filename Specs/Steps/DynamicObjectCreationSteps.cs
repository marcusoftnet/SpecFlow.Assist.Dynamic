using System;
using Should.Fluent;
using SpecFlow.Assist.Dynamic;
using TechTalk.SpecFlow;

namespace Specs.Steps
{
    [Binding]
    public class DynamicObjectCreationSteps
    {
        private dynamic _person;

        [When(@"I create a dynamic instance from this table")]
        public void CreateDynamicInstanceFromTable(Table table)
        {
            _person = table.CreateDynamicInstance();
        }

        [Then(@"the Name property should equal '(.*)'")]
        public void NameShouldBe(string expectedValue)
        {
            ((string)_person.Name).Should().Equal(expectedValue);
        }

        [Then(@"the Age property should equal (\d+)")]
        public void AgeShouldBe(int expectedAge)
        {
            ((int)_person.Age).Should().Equal(expectedAge);
        }

        [Then(@"the BirthDate property should equal (.*)")]
        public void BirthDateShouldBe(string expectedDate)
        {
            ((DateTime)_person.BirthDate).Should().Equal(DateTime.Parse(expectedDate));
        }

        [Then(@"the LengthInMeters property should equal (.*)")]
        public void LengthInMeterShouldBe(string doubleString)
        {
            var expectedDouble = double.Parse(doubleString);
            ((double)_person.LengthInMeters).Should().Equal(expectedDouble);
        }

        [Then(@"the dynamic instance should match this table")]
        public void DynamicInstanceShouldMatch(Table table)
        {
            ScenarioContext.Current.Pending();
            //DynamicTableHelpers.CompareToDynamicInstance(table, _person);
        }

        

    }
}
