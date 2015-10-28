using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP_Assigment_2
{
    class Program
    {
        static void test()
        {
            NgramEstimation estimate = new NgramEstimation();
            Console.WriteLine( estimate.Estimate(new string[]{ "作品", "许多","他们","奉献给","自己" }));
        }
        static void Main(string[] args)
        {
            test();
            Console.ReadLine();
        }
    }
}
