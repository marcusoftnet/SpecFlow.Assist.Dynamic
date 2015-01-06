# SpecFlow.Assist.Dynamic

SpecFlow.Assist.Dynamic is a couple of simple extension methods for the SpecFlow Table object that helps you to write less code. 

What would you rather write? 
This:
```c#
[Binding]
public class StepsUsingStaticType
{
    private Person _person;

    [Given(@"I create an instance from this table")]
    public void GivenICreateAnInstanceFromThisTable(Table table)
    {
        _person = table.CreateInstance<Person>();
    }

    [Then(@"the Name property on Person should equal '(.*)'")]
    public void PersonNameShouldBe(string expectedValue)
    {
        Assert.AreEqual(expectedValue, _person.Name);
    }

}
```
And then not forget to define a separate Person class for testing, since you don't want to reuse the one your system under test is using - that's bad practice.

```c#
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public DateTime BirthDate { get; set; }
    public double LengthInMeters { get; set; }
}
```
    
Or this:  
```c#
[Binding]
public class StepsUsingDynamic
{
    private dynamic _instance;

    [Given(@"I create a dynamic instance from this table using step argument transformation")]
    public void c(dynamic instance) { _instance = instance; }

    [Then(@"the Name property should equal '(.*)'")]
    public void NameShouldBe(string expectedValue)
    {
        Assert.AreEqual(expectedValue, _instance.Name);
    }
}
```
The later version uses SpecFlow.Assist.Dynamic. Shorter, sweater and more fun!

Full documentation at: https://github.com/marcusoftnet/SpecFlow.Assist.Dynamic/wiki/Documentation
