using System;
using System.Collections.Generic;
using NUnit.Framework;
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
      Assert.NotNull(ex);
      return ex;
    }

    private static void CheckForOneDifferenceContainingString(string expectedString)
    {
      var ex = GetInstanceComparisonException();
      var diffs = ((List<string>)ex.Differences);
      var diff = diffs.Find(f => f.Contains(expectedString));
      Assert.NotNull(diff);
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
      Assert.False(ScenarioContext.Current.ContainsKey(EXCEPTION_KEY));
    }

    [Then(@"an instance comparison exception should be thrown with (\d+) differences")]
    [Then(@"an instance comparison exception should be thrown with (\d+) difference")]
    public void ExceptionShouldHaveBeenThrown(int expectedNumberOfDifferences)
    {
      Assert.IsTrue(ScenarioContext.Current.ContainsKey(EXCEPTION_KEY));

      var ex = GetInstanceComparisonException();
      Assert.AreEqual(expectedNumberOfDifferences, ex.Differences.Count);
    }

    [Then(@"one difference should be on the (.*) column of the table")]
    public void DifferenceOnTheColumnOfTheTable(string expectedColumnToDiffer)
    {
      CheckForOneDifferenceContainingString(expectedColumnToDiffer);
    }

    [Then(@"one difference should be on the (.*) field of the instance")]
    public void DifferenceOnFieldOfInstance(string expectedFieldToDiffer)
    {
      CheckForOneDifferenceContainingString(expectedFieldToDiffer);
    }

    [Then(@"one message should state that the instance had the value (.*)")]
    public void ExceptionMessageValueOnInstance(string expectedValueOfInstance)
    {
      CheckForOneDifferenceContainingString(expectedValueOfInstance);
    }

    [Then(@"one message should state that the table had the value (.*)")]
    public void ExceptionMessageValueInTable(string expectedValueOfTable)
    {
      CheckForOneDifferenceContainingString(expectedValueOfTable);
    }

    [Then(@"one difference should be on the (.*) property")]
    public void ExceptionMessageValueOnProperty(string expectedPropertyName)
    {
      CheckForOneDifferenceContainingString(expectedPropertyName);
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
