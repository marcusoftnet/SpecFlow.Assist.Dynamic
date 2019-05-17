using System;

namespace TechTalk.SpecFlow.Assist
{
    public class DynamicInstanceFromTableException : Exception
    {
        public DynamicInstanceFromTableException(string message) : base(message) { }
    }
}