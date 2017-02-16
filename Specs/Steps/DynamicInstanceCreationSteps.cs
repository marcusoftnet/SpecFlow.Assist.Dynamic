using System;
using System.Collections.Generic;
using Should.Fluent;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Specs.Steps
{
    [Binding]
    public class DynamicInstanceCreationSteps
    {

        [Given(@"I create a dynamic instance from this table")]
        [When(@"I create a dynamic instance from this table")]
        public void CreateDynamicInstanceFromTable(Table table)
        {
            State.OriginalInstance = table.CreateDynamicInstance();
        }

        [Then(@"the Name property should equal '(.*)'")]
        public void NameShouldBe(string expectedValue)
        {
            ((string)State.OriginalInstance.Name).Should().Equal(expectedValue);
        }

        [Then(@"the Age property should equal (\d+)")]
        public void AgeShouldBe(int expectedAge)
        {
            ((int)State.OriginalInstance.Age).Should().Equal(expectedAge);
        }

        [Then(@"the age property should equal (\d+)")]
        public void LowerCaseAgeShouldBe(int expectedAge)
        {
            ((int) State.OriginalInstance.age).Should().Equal(expectedAge);
        }

        [Then(@"the BirthDate property should equal (.*)")]
        public void BirthDateShouldBe(string expectedDate)
        {
            ((DateTime)State.OriginalInstance.BirthDate).Should().Equal(DateTime.Parse(expectedDate));
        }


        [Then]
        public void ThenTheLengthInMetersPropertyShouldEqual_P0(double expectedLenghtInMeters)
        {
            CheckLengthInMeters(expectedLenghtInMeters);

        }

        
        [Then(@"the LengthInMeters property should equal '(\d+\.\d+)'")]
        public void LengthInMeterShouldBe(double expectedLenghtInMeters)
        {
            CheckLengthInMeters(expectedLenghtInMeters);
        }


        [Then(@"the MolecularWeight property should equal '(\d+\.\d+)'")]
        public void MolecularWeightShouldBe(decimal expectedMolecularWeight)
        {
            CheckMolecularWeight(expectedMolecularWeight);
        }



        private static void CheckLengthInMeters(double expectedLenghtInMeters)
        {
            ((double) State.OriginalInstance.LengthInMeters).Should().Equal(expectedLenghtInMeters);
        }


        private static void CheckMolecularWeight(decimal expectedMolecularWeight)
        {
            ((decimal)State.OriginalInstance.MolecularWeight).Should().Equal(expectedMolecularWeight);
        }

        [Then(@"the SATScore should be (\d+)")]
        public void SATTest(int expectedScore)
        {
            ((int) State.OriginalInstance.SATScore).Should().Equal(expectedScore);
        }

        [Then(@"the IsDeveloper property should equal '(.*)'")]
        public void ThenTheIsDeveloperPropertyShouldEqualTrueAndBeOfTypeBool(bool expectedValue)
        {
            ((bool) State.OriginalInstance.IsDeveloper).Should().Equal(expectedValue);
        }

        [Then(@"the CharpNmeWithStrangeChars property should equal '(.*)'")]
        public void ThenTheCharpNmeWithStrangeCharsPropertyShouldEqual(string expectedValue)
        {
            ((string)State.OriginalInstance.CharpNmeWithStrangeChars).Should().Equal(expectedValue);

        }

        [Then(@"the My_Nice_Variable property should equal '(.*)'")]
        public void ThenTheMy_Nice_VariablePropertyShouldEqual(string expectedValue)
        {
            ((string)State.OriginalInstance.My_Nice_Variable).Should().Equal(expectedValue);

        }

        [Then(@"the MyVariableNeedsCleanUp property should equal '(.*)'")]
        public void ThenTheMyVariableNeedsCleanUpPropertyShouldEqual(string expectedValue)
        {
            ((string)State.OriginalInstance.MyVariableNeedsCleanUp).Should().Equal(expectedValue);
        }

        [When(@"I create a dynamic instance with only reserved chars")]
        public void OnlyReservedChars(Table table)
        {
            try
            {
                State.OriginalInstance = table.CreateDynamicInstance();                 
            }
            catch (DynamicInstanceFromTableException ex)
            {
                ScenarioContext.Current.Set(ex);
            }
        }

        [Then(@"an exception with a nice error message about the property only containing reserved chars should be thrown")]
        public void ThenAnExceptionWithANiceErrorMessageAboutThePropertyOnlyContainingReservedCharsShouldBeThrown()
        {
            var ex = ScenarioContext.Current.Get<DynamicInstanceFromTableException>();
            ex.Should().Not.Be.Null();
            ex.Message.Should().Contain("only contains");
        }

        [Given(@"I create a dynamic instance from this table using no type conversion")]
        [When(@"I create a dynamic instance from this table using no type conversion")]
        public void WhenICreateADynamicInstanceFromThisTableUsingNoTypeConversion(Table table)
        {
            State.OriginalInstance = table.CreateDynamicInstance(false);
        }

        [Then(@"the Name value should still be '(.*)'")]
        public void ThenTheNameValueShouldStillBe(string expectedValue)
        {
            ((string)State.OriginalInstance.Name).Should().Equal(expectedValue);
        }

        [Then(@"the Age value should still be '(.*)'")]
        public void ThenTheAgeValueShouldStillBe(string expectedValue)
        {
            ((string)State.OriginalInstance.Age).Should().Equal(expectedValue);
        }

        [Then(@"the birth date should stil be '(.*)'")]
        public void ThenTheBirthDateShouldStilBe(string expectedValue)
        {
            ((string)State.OriginalInstance.BirthDate).Should().Equal(expectedValue);

        }

        [Then(@"length in meter should still be '(.*)'")]
        public void ThenLengthInMeterShouldStillBe(string expectedValue)
        {
            ((string)State.OriginalInstance.LengthInMeters).Should().Equal(expectedValue);
        }

    }
}
