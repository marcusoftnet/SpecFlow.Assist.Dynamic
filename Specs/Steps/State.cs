

using TechTalk.SpecFlow;

namespace Specs.Steps
{
    public class State
    {
        public static dynamic Instance
        {
            get
            {
                return ScenarioContext.Current["OrginalInstance"];
            }
            set
            {
                ScenarioContext.Current.Add("OrginalInstance", value);
            }
        }
    }
}
