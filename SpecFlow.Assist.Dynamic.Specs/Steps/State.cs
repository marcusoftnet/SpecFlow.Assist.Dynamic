using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Specs.Steps
{
  public class State
  {
    public dynamic OriginalInstance;

    public IList<dynamic> OriginalSet;

    public Exception CurrentException;
  }
}
