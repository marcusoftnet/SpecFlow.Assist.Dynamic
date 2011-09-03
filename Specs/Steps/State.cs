using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Specs.Steps
{
    public class State
    {
        public static dynamic OriginalInstance
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

        public static IList<dynamic> OriginalSet
        {
            get
            {
                return ScenarioContext.Current["OrginalSet"] as IList<dynamic>;
            }
            set
            {
                ScenarioContext.Current.Add("OrginalSet", value);
            }
        }
    }
}
