using Benchmarker;
using Benchmarker.Framework;
using System;

namespace Bechmarker
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Get the instance of IBenchmarkableCache interface implemention
                var benchmarkableCache = GetBenchmarableCache(BenchmarkConfiguration.ProviderFQN);
                // Run the benchmarking 
                var benchmarker = new Benchmarker.Benchmarker(benchmarkableCache);
                benchmarker.Run(BenchmarkConfiguration.CacheName, BenchmarkConfiguration.NumberOfItems, BenchmarkConfiguration.NumberOfThreads, BenchmarkConfiguration.Payload, BenchmarkConfiguration.FetchRatio, BenchmarkConfiguration.UpdateRatio);

                Console.WriteLine("Operations are being performed.");
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
                Console.ReadLine();
            }
            //Console.WriteLine("Press ENTER to exit...");
        }

        private static IBenchmarkableCache GetBenchmarableCache(string fqn)
        {
            // Get type using provided F.Q.N
            var type = Type.GetType(fqn);

            if(type == null)
                throw new Exception("Unable to find the required class using provided FQN: " + fqn);

            //Instantiate the provided implementation
            return Activator.CreateInstance(Type.GetType(fqn)) as IBenchmarkableCache;
        }

    }
}
