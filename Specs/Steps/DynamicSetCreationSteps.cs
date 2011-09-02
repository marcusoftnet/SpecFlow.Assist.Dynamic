using System;
using System.Collections.Generic;
using NUnit.Framework;
using Should.Fluent;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Specs.Steps
{
    [Binding]
    public class DynamicSetCreationSteps
    {
        private IList<object> _dynamicSet;

        private dynamic GetItem(int itemNumber)
        {
            return _dynamicSet[itemNumber - 1];
        }


        [When(@"I create a set of dynamic instances from this table")]
        public void CreateSetFromTable(Table table)
        {
            _dynamicSet = table.CreateDynamicSet();
        }


        [Then(@"I should have a list of (\d+) dynamic objects")]
        public void ShouldContain(int expectedNumberOfItems)
        {
            _dynamicSet.Count.Should().Equal(expectedNumberOfItems);
        }

        [Then(@"the (\d+) item should have BirthDate equal to '(.*)'")]
        public void ItemInSetShouldHaveExpectedBirthDate(int itemNumber, string expectedBirthDate)
        {
            Assert.AreEqual(DateTime.Parse(expectedBirthDate), GetItem(itemNumber).BirthDate);
        }

        [Then(@"the (\d+) item should have Age equal to '(\d+)'")]
        public void ItemInSetShouldHaveExpectedAge(int itemNumber, int expectedAge)
        {
            Assert.AreEqual(expectedAge, GetItem(itemNumber).Age);
        }

        [Then(@"the (\d+) item should have Name equal to '(.*)'")]
        public void ItemInSetShouldHaveExpectedName(int itemNumber, string expectedName)
        {
            Assert.AreEqual(expectedName, GetItem(itemNumber).Name);
        }

        [Then(@"the (\d+) item should have LengthInMeters equal to '(.*)'")]
        public void ItemInSetShouldHaveExpectedLenghtInMeters(int itemNumber, string expectedLengthInMetersItem)
        {
            var length = double.Parse(expectedLengthInMetersItem);
            Assert.AreEqual(length, GetItem(itemNumber).LengthInMeters);
        }
    }
}
