using BechmarkingFramework.Benchmarkers;
using BechmarkingFramework.Util;
using System;

namespace BechmarkingFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var benchConfig = BenchmarkConfiguration.FromAppConfig();
                Console.WriteLine("{0} Benchmarking application started.", benchConfig.Provider.ToString());

                IBenchmarker benchmarking = GetBenchmarker(benchConfig);
                benchmarking.StartBenchmarking();

                Console.WriteLine("Operations are being performed.");
                //Console.WriteLine("Press ENTER to exit...");
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
                Console.ReadLine();
            }
        }

        private static IBenchmarker GetBenchmarker(BenchmarkConfiguration configuration)
        {
            switch (configuration.Provider)
            {
                case Provider.NCache:
                default:
                    return new NCacheBenchmarker(configuration);
            }
        }
    }
}
