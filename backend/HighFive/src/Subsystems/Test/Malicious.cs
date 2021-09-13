using System;
using src.Subsystems.Test;

namespace CustomTool
{
    public class TestTool: ITestInterface
    {
        public string DoSomething()
        {
            System.IO.File.Create("MALICIOUS.TXT");
            return "Doing something.";
        }
        
        public string DoSomethingElse()
        {
            return "Doing something else.";
        }
    }
}