using System;

namespace RealTimeCacheApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var startUp = new Startup();

            startUp.Init();
            startUp.Start();

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
