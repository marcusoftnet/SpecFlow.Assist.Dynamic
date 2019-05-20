using System;
using System.Collections.Generic;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Specs.Steps
{
  [Binding]
  public class DynamicInstanceCreationSteps
  {
    private readonly State state;

    public DynamicInstanceCreationSteps(State state) => this.state = state;

    [Given(@"I create a dynamic instance from this table")]
    [When(@"I create a dynamic instance from this table")]
    public void CreateDynamicInstanceFromTable(Table table)
    {
      this.state.OriginalInstance = table.CreateDynamicInstance();
    }

    [Then(@"the Name property should equal '(.*)'")]
    public void NameShouldBe(string expectedValue)
    {
      var actual = ((string)this.state.OriginalInstance.Name);
      Assert.AreEqual(expectedValue, actual);
    }

    [Then(@"the Age property should equal (\d+)")]
    public void AgeShouldBe(int expectedAge)
    {
      var actualAge = ((int)this.state.OriginalInstance.Age);
      Assert.AreEqual(expectedAge, actualAge);
    }

    [Then(@"the age property should equal (\d+)")]
    public void LowerCaseAgeShouldBe(int expectedAge)
    {
      var actualAge = ((int)this.state.OriginalInstance.age);
      Assert.AreEqual(expectedAge, actualAge);
    }

    [Then(@"the BirthDate property should equal (.*)")]
    public void BirthDateShouldBe(string expectedDate)
    {
      var expected = DateTime.Parse(expectedDate);
      var actual = ((DateTime)this.state.OriginalInstance.BirthDate);
      Assert.AreEqual(expected, actual);
    }

    [Then(@"the LengthInMeters property should equal '(\d+\.\d+)'")]
    public void LengthInMeterShouldBe(double expectedLengthInMeters)
    {
      CheckLengthInMeters(expectedLengthInMeters);
    }

    [Then(@"the MolecularWeight property should equal '(\d+\.\d+)'")]
    public void MolecularWeightShouldBe(decimal expectedMolecularWeight)
    {
      CheckMolecularWeight(expectedMolecularWeight);
    }

    private void CheckLengthInMeters(double expectedLengthInMeters)
    {
      var actual = ((double)this.state.OriginalInstance.LengthInMeters);
      Assert.AreEqual(expectedLengthInMeters, actual);
    }

    private void CheckMolecularWeight(decimal expectedMolecularWeight)
    {
      var actual = ((decimal)this.state.OriginalInstance.MolecularWeight);
      Assert.AreEqual(expectedMolecularWeight, actual);
    }

    [Then(@"the SATScore should be (\d+)")]
    public void SATTest(int expectedScore)
    {
      var actual = ((int)this.state.OriginalInstance.SATScore);
      Assert.AreEqual(expectedScore, actual);
    }

    [Then(@"the IsDeveloper property should equal '(.*)'")]
    public void ThenTheIsDeveloperPropertyShouldEqualTrueAndBeOfTypeBool(bool expectedValue)
    {
      var actual = ((bool)this.state.OriginalInstance.IsDeveloper);
      Assert.AreEqual(expectedValue, actual);
    }

    [Then(@"the CharpNmeWithStrangeChars property should equal '(.*)'")]
    public void ThenTheCharpNmeWithStrangeCharsPropertyShouldEqual(string expectedValue)
    {
      var actual = ((string)this.state.OriginalInstance.CharpNmeWithStrangeChars);
      Assert.AreEqual(expectedValue, actual);
    }

    [Then(@"the My_Nice_Variable property should equal '(.*)'")]
    public void ThenTheMy_Nice_VariablePropertyShouldEqual(string expectedValue)
    {
      var actual = ((string)this.state.OriginalInstance.My_Nice_Variable);
      Assert.AreEqual(expectedValue, actual);
    }

    [Then(@"the MyVariableNeedsCleanUp property should equal '(.*)'")]
    public void ThenTheMyVariableNeedsCleanUpPropertyShouldEqual(string expectedValue)
    {
      var actual = ((string)this.state.OriginalInstance.MyVariableNeedsCleanUp);
      Assert.AreEqual(expectedValue, actual);
    }

    [When(@"I create a dynamic instance with only reserved chars")]
    public void OnlyReservedChars(Table table)
    {
      try
      {
        this.state.OriginalInstance = table.CreateDynamicInstance();
      }
      catch (DynamicInstanceFromTableException ex)
      {
        state.CurrentException = ex;
      }
    }

    [Then(@"an exception with a nice error message about the property only containing reserved chars should be thrown")]
    public void ThenAnExceptionWithANiceErrorMessageAboutThePropertyOnlyContainingReservedCharsShouldBeThrown()
    {
      var ex = state.CurrentException as DynamicInstanceFromTableException;
      Assert.NotNull(ex);
      Assert.IsTrue(ex.Message.Contains("only contains"));
    }

    [Given(@"I create a dynamic instance from this table using no type conversion")]
    [When(@"I create a dynamic instance from this table using no type conversion")]
    public void WhenICreateADynamicInstanceFromThisTableUsingNoTypeConversion(Table table)
    {
      this.state.OriginalInstance = table.CreateDynamicInstance(false);
    }

    [Then(@"the Name value should still be '(.*)'")]
    public void ThenTheNameValueShouldStillBe(string expectedValue)
    {
      var actual = ((string)this.state.OriginalInstance.Name);
      Assert.AreEqual(expectedValue, actual);
    }

    [Then(@"the Age value should still be '(.*)'")]
    public void ThenTheAgeValueShouldStillBe(string expectedValue)
    {
      var actual = ((string)this.state.OriginalInstance.Age);
      Assert.AreEqual(expectedValue, actual);
    }

    [Then(@"the birth date should still be '(.*)'")]
    public void ThenTheBirthDateShouldStillBe(string expectedValue)
    {
      var actual = ((string)this.state.OriginalInstance.BirthDate);
      Assert.AreEqual(expectedValue, actual);
    }

    [Then(@"length in meter should still be '(.*)'")]
    public void ThenLengthInMeterShouldStillBe(string expectedValue)
    {
      var actual = ((string)this.state.OriginalInstance.LengthInMeters);
      Assert.AreEqual(expectedValue, actual);
    }
  }
}
