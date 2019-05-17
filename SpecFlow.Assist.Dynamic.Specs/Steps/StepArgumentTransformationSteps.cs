using System.Collections.Generic;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Specs.Steps
{
    [Binding]
    public class StepArgumentTransformationSteps
    {
        [Given(@"I create a set of dynamic instances from this table using step argument transformation")]
        public void a(IList<dynamic> dynamicSet)
        {
            State.OriginalSet = dynamicSet;
        }

        [When(@"I compare the set to this table using step argument transformation")]
        public void b(Table table)
        {
            table.CompareToDynamicSet(State.OriginalSet);
        }

        [Given(@"I create a dynamic instance from this table using step argument transformation")]
        public void c(dynamic instance)
        {
            State.OriginalInstance = instance;
        }

        [When(@"I compare it to this table using step argument transformation")]
        public void d(Table table)
        {
            table.CompareToDynamicInstance((object)State.OriginalInstance);
        }
    }
}
