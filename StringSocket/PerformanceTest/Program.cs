using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GradingTester;

namespace PerformanceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            StringSocketTest test = new StringSocketTest();
            for (int i = 0; i < 20; i++)
            {
                test.Test7();
            }
        }
    }
}
