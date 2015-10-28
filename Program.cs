using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NLP_Assigment_2
{
    class Program
    {
        static void test()
        {
            NgramEstimation estimate = new NgramEstimation();
            StreamReader sr = new StreamReader("input.txt");
            StreamWriter sw = new StreamWriter("output.txt");
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] words = line.Split(new char[] { ' ' });
                if (words.Count() > 10)
                {
                    Console.WriteLine("请输入至多10个词");
                    continue;
                }
                string result = estimate.Estimate(words);
                if (result != null) sw.WriteLine(result);
                else sw.WriteLine("***无可用结果***");
            }
            sr.Close();
            sw.Close();
        }
        static void Main(string[] args)
        {
            test();
            Console.WriteLine("按回车结束...");
            Console.ReadLine();
        }
    }
}
