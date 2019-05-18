using Should;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using NUnit.Framework;
using System.Collections.Generic;

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

    private static void CheckForOneDifferenceContainingString(string expectedString)
    {
      var ex = GetSetComparisonException();
      var diffs = ((List<string>)ex.Differences);
      var diff = diffs.Find(f => f.Contains(expectedString));
      Assert.NotNull(diff);
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
      Assert.IsFalse(ScenarioContext.Current.ContainsKey(EXCEPTION_KEY));
    }

    [Then(@"an set comparison exception should be thrown")]
    public void SetComparisonExceptionThrown()
    {
      Assert.NotNull(GetSetComparisonException());
    }

    [Then(@"an set comparision exception should be thrown with (\d+) differences")]
    [Then(@"an set comparision exception should be thrown with (\d+) difference")]
    public void SetComparisionExceptionWithNumberOfDifferences(int expectedNumberOfDifference)
    {
      SetComparisonExceptionThrown();
      Assert.AreEqual(expectedNumberOfDifference, GetSetComparisonException().Differences.Count);
    }


    [Then(@"the error message for different rows should expect (.*) for table and (.*) for instance")]
    public void ShouldDifferInRowCount(string tableRowCountString, string instanceRowCountString)
    {
      var message = GetSetComparisonException().Message;
      Assert.IsTrue(message.Contains(tableRowCountString));
      Assert.IsTrue(message.Contains(instanceRowCountString));
    }

    [Then(@"one set difference should be on the (.*) column of the table")]
    public void DifferenceOnTheColumnOfTheTable(string expectedColumnToDiffer)
    {
      CheckForOneDifferenceContainingString(expectedColumnToDiffer);
    }

    [Then(@"one set difference should be on the (.*) field of the instance")]
    public void DifferenceOnFieldOfInstance(string expectedFieldToDiffer)
    {
      CheckForOneDifferenceContainingString(expectedFieldToDiffer);
    }

    [Then(@"(\d+) difference should be on row (\d+) on property '(.*)' for the values '(.*)' and '(.*)'")]
    public void DifferenceOnValue(int differenceNumber, int rowNumber, string expectedProperty, string instanceValue, string tableRowValue)
    {
      var exception = GetSetComparisonException();
      var difference = exception.Differences[differenceNumber - 1];
      // I ADDED Assert.IsTrue() here 190518
      Assert.IsTrue(difference.Contains("'" + rowNumber + "'"));
      Assert.IsTrue(difference.Contains("'" + expectedProperty + "'"));
      Assert.IsTrue(difference.Contains("'" + instanceValue + "'"));
      Assert.IsTrue(difference.Contains("'" + tableRowValue + "'"));
    }
  }
}
