using NipahData;
using System;
using System.IO;

namespace NipahDataTest
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                string code = File.ReadAllText("Data.miidat");
                var data = Data.ReadDataFrom(code);
                Console.WriteLine(data);
                Console.ReadKey(true);
                Console.Clear();
            }
        }
    }
}
