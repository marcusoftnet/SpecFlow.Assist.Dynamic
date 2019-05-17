using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpecFlow.Assist.Dynamic
{
    [Binding]
    public class DynamicStepArgumentTransformations
    {
        
        [StepArgumentTransformation]
        public IEnumerable<object> TransformToEnumerable(Table table)
        {
            return table.CreateDynamicSet();
        }

        [StepArgumentTransformation]
        public IList<object> TransformToList(Table table)
        {
            return table.CreateDynamicSet().ToList<object>();
        }

        [StepArgumentTransformation]
        public dynamic TransformToDynamicInstance(Table table)
        {
            return table.CreateDynamicInstance();
        }
    }
}
